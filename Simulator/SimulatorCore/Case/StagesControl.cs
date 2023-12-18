using Simulator.BotControl;
using Simulator.Models;
using Simulator.Properties;
using Simulator.Services;
using SimulatorCore.Case;
using SimulatorCore.Models.DbModels;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Simulator.Case
{
    internal static class StagesControl
    {
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
            var currentUserFlags = await DataBaseControl.GetEntity<UserFlags>(userId);
            switch (nextStage)
            {
                case CaseStagePoll poll:

                    await DataBaseControl.StatsStateTableCommand.
                        SetStartTime(currentUserFlags.CurrentCourse, userId, DateTime.Now);

                    await ShowAdditionalInfo(botClient, poll, userId);
                    int activePollMessageId = (await botClient.SendPollAsync(
                        chatId: userId,
                        question: poll.TextBefore,
                        options: poll.Options,
                        isAnonymous: false,
                        allowsMultipleAnswers: poll.ManyAnswers,
                        replyMarkup: new InlineKeyboardMarkup(CommandKeyboard.ToFinishButton)
                        )).MessageId;
                    currentUserFlags.ActivePollMessageId = activePollMessageId;
                    await DataBaseControl.UpdateEntity(userId, currentUserFlags);

                    break;
                case CaseStageMessage message:

                    string answerGuide = GetAnswerGuide(message.MessageTypeAnswer);

                    await DataBaseControl.StatsStateTableCommand.
                        SetStartTime(currentUserFlags.CurrentCourse, userId, DateTime.Now);

                    await ShowAdditionalInfo(botClient, message, userId);
                    await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: message.TextBefore + answerGuide,
                        replyMarkup: new InlineKeyboardMarkup(CommandKeyboard.ToFinishButton)
                        );

                    break;
                case CaseStageNone none:

                    // Ставится только если не было поставлено ранее
                    await DataBaseControl.StatsBaseTableCommand.
                        SetStartCaseTime(currentUserFlags.CurrentCourse, userId, DateTime.Now);

                    await botClient.SendTextMessageAsync(userId,
                        none.TextBefore,
                        replyMarkup: CommandKeyboard.StageMenu);

                    break;
                case CaseStageEndModule endStage:

                    var resultsCallback = await EndStageCalc.
                        GetResultOfModule(endStage, userId, currentUserFlags.CurrentCourse);

                    await DataBaseControl.StatsStateTableCommand.
                        SetPoint(currentUserFlags.CurrentCourse, userId, resultsCallback.Item2);

                    if (resultsCallback.Item2 == -1) // Последний этап и попыток больше нет
                    {
                        await DataBaseControl.StatsBaseTableCommand.
                            SetEndCaseTime(currentUserFlags.CurrentCourse, userId, DateTime.Now);

                        IReplyMarkup replyMarkup = new InlineKeyboardMarkup(CommandKeyboard.ToFinishButton);
                        await botClient.SendTextMessageAsync(userId,
                            resultsCallback.Item1,
                            replyMarkup: replyMarkup);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(userId,
                            resultsCallback.Item1);
                        CaseStage newNextStage = CoursesControl.Courses[currentUserFlags.CurrentCourse][resultsCallback.Item2];
                        if(resultsCallback.Item2 == 0)
                        {
                            await DataBaseControl.StatsStateTableCommand.SetRate(currentUserFlags.CurrentCourse, userId, 0);
                        }
                        await Move(userId, newNextStage, botClient);
                    }

                    break;
                default:
                    break;
            }
        }

        private static string GetAnswerGuide(MessageType messageTypeAnswer)
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
                                var inputOnlineFileDoc = new InputFileStream(fs, fileName.Trim());
                                await botClient.SendDocumentAsync(userId, inputOnlineFileDoc);
                                break;
                            case "audios":
                                var inputOnlineFileAudio = new InputFileStream(fs, fileName.Trim());
                                await botClient.SendAudioAsync(userId, inputOnlineFileAudio);
                                break;
                            case "videos":
                                var inputOnlineFileVideo = new InputFileStream(fs, fileName.Trim());
                                await botClient.SendVideoAsync(userId, inputOnlineFileVideo);
                                break;
                            case "photos":
                                var inputOnlineFilePhoto = new InputFileStream(fs, fileName.Trim());
                                await botClient.SendPhotoAsync(userId, inputOnlineFilePhoto);
                                break;
                        }
                    }
                }     
            }
        }
    }
}
