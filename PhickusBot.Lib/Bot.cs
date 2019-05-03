using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;

namespace PhickusBot.Lib
{
    public class Bot
    {
        private ITelegramBotClient _botClient;
        private string _audioPath = @"C:\dev\dotnet\voices";
        private List<FileInfo> _thingsToSay;


        public Bot(string token = "")
        {
            _botClient = new TelegramBotClient(token);
            var me = _botClient.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            _botClient.OnMessage += Bot_OnMessage;
            _botClient.StartReceiving();

            var directory = new DirectoryInfo(_audioPath);
            _thingsToSay = directory.EnumerateFiles("sample*.wav").ToList();



        }
        public async void Bot_OnMessage(object sender, MessageEventArgs e)
        {


            if (e.Message.Text != null)
            {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

                
                if (e.Message.Text == "kek")
                {
                    var randomFile = _thingsToSay[new Random().Next(0, _thingsToSay.Count - 1)];
                    using (var voiceStream = randomFile.OpenRead())
                    {
                    InputOnlineFile voiceToSend = new InputOnlineFile(voiceStream);

                        await _botClient.SendVoiceAsync(
                            chatId: e.Message.Chat,
                            voice: voiceToSend
                            );
                    }

                }
                else
                {
                    await _botClient.SendTextMessageAsync(
                  chatId: e.Message.Chat,
                  text: "You sent:\n" + e.Message.Text + " Send 'kek' for sound"
                );

                }
            }

           
        }
    }
}
