using Simulator.BotControl;
using Simulator.Models;
using Simulator.Properties;
using Simulator.Services;
using Simulator.TelegramBotLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Simulator.Case
{
    internal static class StagesControl
    {
        public static StageList Stages { get; set; } = new StageList();
        public static bool Make()
        {
            
            string path = ControlSystem.caseDirectory +
                "\\" + ControlSystem.caseInfoFileName;

            if (System.IO.File.Exists(path))
            {
                try
                {
                    CaseConverter.FromFile(path);
                }
                catch
                {
                    DeleteCaseFiles();
                    return false;
                }
                return true;
            }
            return false;
        }

        public static void DeleteCaseFiles()
        {
            ControlSystem.DeleteFilesFromDirectory(ControlSystem.caseDirectory);
        }

        public static double CalculateRatePoll(CaseStagePoll stage, int[] answers)
        {
            double rate = 0;
            
            int count = answers.Length;
            if(stage.Limit < count && stage.Limit != 0)
            {
                //Штраф за превышение кол-ва ответов
                rate -= stage.Fine * (count - stage.Limit);
                count = stage.Limit;
            }
            for (int i = 0; i < count;i++)
            {
                rate += stage.PossibleRate[answers[i]];
            }
            if(stage.WatchNonAnswer)
            {
                foreach (var opt in stage.NonAnswers)
                {
                    if(!answers.Contains(opt.Key))
                    {
                        rate -= opt.Value;
                    }
                }
            }
            return rate;
        }
        public static void SetStageForMove(CaseStagePoll stage, int[] answers)
        {
            if(stage.ConditionalMove)
            {
                stage.NextStage = stage.MovingNumbers[answers[0]];
            } 
            //Если переход безусловный, то NextStage уже установлено
            //Если нет, то на основе свойств и ответа выбираем NextStage
        }
        public static async Task Move(long userId, CaseStage nextStage, ITelegramBotClient botClient)
        {
            switch (nextStage)
            {
                case CaseStagePoll poll:
                    await DataBaseControl.UserFlagsTableCommand.SetStartTime(userId, DateTime.Now);
                    await ShowAdditionalInfo(botClient, poll, userId);
                    await botClient.SendPollAsync(
                        chatId: userId,
                        question: poll.TextBefore,
                        options: poll.Options,
                        isAnonymous: false,
                        allowsMultipleAnswers: poll.ManyAnswers,
                        replyMarkup: new InlineKeyboardMarkup(CommandKeyboard.ToFinishButton)
                        );

                    break;
                case CaseStageMessage message:
                    string answerGuide = GetAnsweGuide(message.MessageTypeAnswer);

                    await DataBaseControl.UserFlagsTableCommand.SetStartTime(userId, DateTime.Now);
                    await ShowAdditionalInfo(botClient, message, userId);
                    await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: message.TextBefore + answerGuide,
                        replyMarkup: new InlineKeyboardMarkup(CommandKeyboard.ToFinishButton)
                        );

                    break;
                case CaseStageNone none:
                    if(await DataBaseControl.UserCaseTableCommand.IsNullAttempts(userId))
                    {
                        await DataBaseControl.UserCaseTableCommand.SetAttempts(userId, Stages.AttemptCount);
                        await DataBaseControl.UserCaseTableCommand.SetExtraAttempt(userId, Stages.ExtraAttempt);
                        await DataBaseControl.UserCaseTableCommand.SetStartCaseTime(userId, DateTime.Now);
                        await CaseJsonCommand.CheckJsonFile($"{ControlSystem.statsDirectory}\\{userId}.json");
                    }
                    await botClient.SendTextMessageAsync(userId,
                        none.TextBefore,
                        replyMarkup: CommandKeyboard.StageMenu);

                    break;
                case CaseStageEndModule endStage:
                    var resultsCallback = await EndStageCalc.GetResultOfModule(endStage, userId);

                    await DataBaseControl.UserCaseTableCommand.SetPoint(userId, resultsCallback.Item2);

                    if (resultsCallback.Item2 == -1) // Последний этап и попыток больше нет
                    {
                        await DataBaseControl.UserCaseTableCommand.SetEndCaseTime(userId, DateTime.Now);
                        IReplyMarkup replyMarkup = new InlineKeyboardMarkup(CommandKeyboard.ToFinishButton);
                        await botClient.SendTextMessageAsync(userId,
                            resultsCallback.Item1,
                            replyMarkup: replyMarkup);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(userId,
                            resultsCallback.Item1);
                        CaseStage newNextStage = Stages[resultsCallback.Item2];
                        if(resultsCallback.Item2 == 0)
                        {
                            await DataBaseControl.UserCaseTableCommand.SetRate(userId, 0);
                        }
                        await Move(userId, newNextStage, botClient);
                    }

                    break;
                default:
                    break;
            }
        }

        private static string GetAnsweGuide(MessageType messageTypeAnswer)
        {
            string answerGuide = $"\n\n{Resources.AnswerGuidePt1}";

            switch (messageTypeAnswer)
            {
                case MessageType.Video:
                    answerGuide += "видео формата mp4";
                    break;
                case MessageType.Audio:
                    answerGuide += "аудио формата mp3";
                    break;
                case MessageType.Text:
                    answerGuide += "текстовое сообщение";
                    break;
                default:
                    answerGuide += "документ pdf";
                    break;
            }
            answerGuide += "\n" + Resources.AnswerGuidePt2;
            return answerGuide;
        }

        private async static Task ShowAdditionalInfo(ITelegramBotClient botClient, CaseStage nextStage, long userId)
        {
            if (nextStage.AdditionalInfoFiles == null) return;
            foreach (string infoType in nextStage.AdditionalInfoFiles.Keys)
            {
                foreach (string fileName in nextStage.AdditionalInfoFiles[infoType])
                {
                    using (Stream fs = new FileStream(ControlSystem.caseDirectory +
                        "\\" + fileName.Trim(), FileMode.Open))
                    {
                        switch (infoType)
                        {
                            case "docs":
                                var inputOnlineFileDoc = new InputOnlineFile(fs, fileName.Trim());
                                await botClient.SendDocumentAsync(userId, inputOnlineFileDoc);
                                break;
                            case "audios":
                                var inputOnlineFileAudio = new InputOnlineFile(fs, fileName.Trim());
                                await botClient.SendAudioAsync(userId, inputOnlineFileAudio);
                                break;
                            case "videos":
                                var inputOnlineFileVideo = new InputOnlineFile(fs, fileName.Trim());
                                await botClient.SendVideoAsync(userId, inputOnlineFileVideo);
                                break;
                            case "photos":
                                var inputOnlineFilePhoto = new InputOnlineFile(fs, fileName.Trim());
                                await botClient.SendPhotoAsync(userId, inputOnlineFilePhoto);
                                break;
                        }
                    }
                }     
            }
        }

        public static Dictionary<int, int> GetTaskCountDictionary()
        {
            return Stages.GetTaskCountDictionary();
        }

        public static List<int> GetStageNumbers(int moduleNumber)
        {
            return Stages.GetStageNumbers(moduleNumber);
        }
    }
}
