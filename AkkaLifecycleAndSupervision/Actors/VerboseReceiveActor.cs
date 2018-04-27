using System;
using System.Linq;
using Akka.Actor;

namespace AkkaLifecycleAndSupervision.Actors
{
    public abstract class VerboseReceiveActor: ReceiveActor
    {
        private readonly IActorRef _consoleWriter;
        private int _msgDepth = 0;
        private readonly Guid _id = Guid.NewGuid();

        protected VerboseReceiveActor(IActorRef consoleWriter)
        {
            _consoleWriter = consoleWriter;
            
            WriteInfo("## Constructor execution");
            
            Receive(
                (Predicate<string>) (msg => msg.Equals("crashcustom")),
                msg =>
                {
                    WriteInfo("==> Throw custom exception");
                    throw new MyCustomException("custom exception");
                });
            
            Receive<Terminated>(msg =>
            {
                var terminatedActor = msg.ActorRef;
                WriteInfo($"## Terminated message received from {terminatedActor}");
            });
        }

        protected override void PreRestart(Exception reason, object message)
        {
            TraceMethod("PreRestart", () => base.PreRestart(reason, message));
        }

        protected override void PostRestart(Exception reason)
        {
            TraceMethod("PostRestart", () => base.PostRestart(reason));
        }

        protected override void PostStop()
        {
            TraceMethod("PostStop", () =>
            {
                base.PostStop();
            });
        }

        protected override void PreStart()
        {
            TraceMethod("PreStart", () => base.PreStart());
        }

        private void TraceMethod(string methodName, Action action)
        {
            WriteInfo(methodName);
            _msgDepth++;
            action();
            _msgDepth--;
        }

        protected virtual ConsoleColor ConsoleColor => ConsoleColor.White;
        
        protected void WriteInfo(string message)
        {
            _consoleWriter.Tell(new ConsoleWriterActor.InfoMessage(FormatMessage(message), ConsoleColor));
        }
        
        protected void WriteError(string message)
        {
            _consoleWriter.Tell(new ConsoleWriterActor.ErrorMessage(FormatMessage(message)));
        }

        private string FormatMessage(string message)
        {
            return
                $"{string.Join('/', Self.Path.Elements.Skip(1))} - {_id} => {string.Empty.PadLeft(_msgDepth, '\t')}{message}";
        }
    }
}