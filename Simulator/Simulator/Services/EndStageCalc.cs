using Simulator.BotControl;
using Simulator.Models;
using System;
using System.Threading.Tasks;

namespace Simulator.Services
{
    internal static class EndStageCalc
    {
        public async static Task<ValueTuple<string, int>> GetResultOfModule(
            CaseStageEndModule descriptor, long userId, string courseName)
        {
            double currentRate;
            var result = new ValueTuple<string, int>(); 

            currentRate = await DataBaseControl.StatsStateTableCommand.GetRate(courseName, userId);
            int attemptsRemain = await DataBaseControl.StatsStateTableCommand.GetAttempts(courseName, userId);

            bool extraAttempt = false;
            if (descriptor.ModuleNumber == 1)
            {
                if(await DataBaseControl.StatsStateTableCommand.GetExtraAttempt(courseName, userId))
                {
                    extraAttempt = true;
                }
            }

            int ratePlace = 0;
            if (descriptor.IsEndOfCase || currentRate < descriptor.Rates[0])
            {
                if (extraAttempt)
                {
                    await DataBaseControl.StatsStateTableCommand.SetExtraAttempt(courseName, userId, false);
                }
                else
                {
                    if (attemptsRemain != 0)
                    {
                        --attemptsRemain;
                    }

                    if (descriptor.IsEndOfCase)
                    {
                        await DataBaseControl.StatsBaseTableCommand.SetAttemptsUsed(courseName, userId);
                    }
                    else
                    {
                        if (attemptsRemain != 0)
                        {
                            await DataBaseControl.StatsBaseTableCommand.SetAttemptsUsed(courseName, userId);
                        }
                    }

                    await DataBaseControl.StatsStateTableCommand.SetAttempts(courseName, userId, attemptsRemain);
                }

                if (descriptor.IsEndOfCase)
                {
                    foreach (double rateGrade in descriptor.Rates)
                    {
                        if (currentRate > rateGrade)
                        {
                            ratePlace++;
                        }
                        else break;
                    }

                    result = GetResult(attemptsRemain, descriptor, ratePlace, true);
                }
                else if(currentRate < descriptor.Rates[0])
                {
                    
                    result = GetResult(attemptsRemain, descriptor, ratePlace, false);
                }    
            }
            else
            {
                ratePlace = 1;
                result = GetResult(attemptsRemain, descriptor, ratePlace, false);
                if (extraAttempt)
                {
                    await DataBaseControl.StatsStateTableCommand.SetExtraAttempt(courseName, userId, false);
                }
            }
            return result;
        }
        private static ValueTuple<string, int> GetResult(
            int attemptsRemain,
            CaseStageEndModule descriptor,
            int ratePlace,
            bool endCase)
        {
            var result = new ValueTuple<string, int>();
            result.Item1 = descriptor.Texts[ratePlace] + "\n"; //Информация о результате

            int textAboutAttempts = descriptor.Rates.Count + 1;
            
            if (attemptsRemain == 0)
            {
                result.Item1 += descriptor.Texts[textAboutAttempts];      
            }
            else
            {
                result.Item1 += descriptor.Texts[textAboutAttempts + 1];
            }

            if (endCase)
            {
                result.Item2 = attemptsRemain == 0 ? -1 : 0;
            }
            else
            {
                result.Item2 = ratePlace == 0 && attemptsRemain != 0 ? 0 : descriptor.NextStage;
            }

            return result;
        }
    }
}
