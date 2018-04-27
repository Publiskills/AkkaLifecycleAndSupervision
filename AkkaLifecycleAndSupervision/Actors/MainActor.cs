using System;
using Akka.Actor;

namespace AkkaLifecycleAndSupervision.Actors
{
    public class MainActor: VerboseReceiveActor
    {
        public MainActor(IActorRef consoleWriter) : base(consoleWriter)
        {
            var allForOneSupervisorActor = Context.ActorOf(
                Props.Create(() => new AllForOneSupervisorActor(consoleWriter, Directive.Restart))
                , "allForOne");
            allForOneSupervisorActor.Tell("crashachild");
            allForOneSupervisorActor.Tell("stopachild");
            
            
            var oneForOneSupervisorActor = Context.ActorOf(
                Props.Create(() => new OneForOneSupervisorActor(consoleWriter, Directive.Stop))
                , "oneForOne");
            oneForOneSupervisorActor.Tell("crashcustom");
        }
        
        protected override ConsoleColor ConsoleColor => ConsoleColor.Yellow;
    }
}