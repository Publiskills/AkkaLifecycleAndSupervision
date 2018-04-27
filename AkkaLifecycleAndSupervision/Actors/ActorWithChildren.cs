using System;
using Akka.Actor;

namespace AkkaLifecycleAndSupervision.Actors
{
    public class ActorWithChildren: VerboseReceiveActor
    {
        public ActorWithChildren(IActorRef consoleWriter) : base(consoleWriter)
        {
            var simpleActor1 = Context.ActorOf(Props.Create(() => new SimpleActor(consoleWriter)), "simpleActor1");
            var simpleActor2 = Context.ActorOf(Props.Create(() => new SimpleActor(consoleWriter)), "simpleActor2");
            
            Receive<string>(msg => WriteInfo($"## Received message : {msg}"));
        }
        
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new AllForOneStrategy(Decider.From(Directive.Restart));
        }
        
        protected override ConsoleColor ConsoleColor => ConsoleColor.Blue;
    }
}