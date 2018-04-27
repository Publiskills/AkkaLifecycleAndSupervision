using System;
using Akka.Actor;

namespace AkkaLifecycleAndSupervision.Actors
{
    public class ConsoleWriterActor: ReceiveActor
    {
        private const ConsoleColor DefaultConsoleColor = ConsoleColor.White;
        
        public abstract class ConsoleWriterMessage
        {
            internal readonly string Message;

            protected ConsoleWriterMessage(string message)
            {
                Message = message;
            }
        }

        public class InfoMessage : ConsoleWriterMessage
        {
            public readonly ConsoleColor Color;

            public InfoMessage(string message, ConsoleColor color) : base(message)
            {
                Color = color;
            }
        }

        public class ErrorMessage : ConsoleWriterMessage
        {
            public ErrorMessage(string message) : base(message)
            {
            }
        }
        
        public ConsoleWriterActor()
        {
            Receive<ConsoleWriterMessage>(msg =>
            {
                switch (msg)
                {
                    case ErrorMessage error:
                        WriteMessage(error.Message, ConsoleColor.Red);
                        break;
                    case InfoMessage info:
                        WriteMessage(info.Message, info.Color);
                        break;
                    default:
                        WriteMessage(msg.Message);
                        break;
                }
            });
            
            Receive<string>(msg =>
            {
                WriteMessage(msg);
            });
        }

        private static void WriteMessage(string message, ConsoleColor color = DefaultConsoleColor)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = DefaultConsoleColor;
        }
    }
}