using System;
using Akka.Actor;

namespace AkkaLifecycleAndSupervision.Actors
{
    public class SimpleActor : VerboseReceiveActor
    {
        public SimpleActor(IActorRef consoleWriter) : base(consoleWriter)
        {
            Receive<string>(msg => WriteInfo($"## Received message : {msg}"));
        }
        
        protected override ConsoleColor ConsoleColor => ConsoleColor.DarkCyan;
    }
}