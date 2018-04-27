using System;
using Akka.Actor;

namespace AkkaLifecycleAndSupervision.Actors
{
    public class OneForOneSupervisorActor : VerboseReceiveActor
    {
        private readonly IActorRef _actorWithChildren1;
        private readonly IActorRef _actorWithChildren2;
        private readonly Directive _supervisionDirective;

        public OneForOneSupervisorActor(IActorRef consoleWriter, Directive supervisionDirective) : base(consoleWriter)
        {
            _supervisionDirective = supervisionDirective;
            
            _actorWithChildren1 = Context.ActorOf(Props.Create(() => new ActorWithChildren(consoleWriter)), "actorWithChildren1");
            _actorWithChildren2 = Context.ActorOf(Props.Create(() => new ActorWithChildren(consoleWriter)), "actorWithChildren2");
            
            Receive(
                (Predicate<string>) (msg => msg.Equals("crashachild")),
                msg =>
                {
                    _actorWithChildren1.Tell("crashcustom");
                });
            
            Receive<string>(msg => WriteInfo($"## Received message : {msg}"));
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                maxNrOfRetries: 2,
                withinTimeMilliseconds: 2000,
                localOnlyDecider: LocalDecider);
        }

        private Directive LocalDecider(Exception exception)
        {
            switch (exception)
            {
                case MyCustomException myException:
                    return _supervisionDirective;
                default: return Directive.Escalate;
            }
        }
        
        protected override ConsoleColor ConsoleColor => ConsoleColor.Magenta;
    }
}