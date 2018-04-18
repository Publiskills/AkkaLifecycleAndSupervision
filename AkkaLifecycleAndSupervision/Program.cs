using Akka.Actor;

namespace AkkaLifecycleAndSupervision
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("AkkaLifecycleAndSupervisionActorSystem", Program.GetAkkaConfigurationFromHoconFile());
            
            actorSystem.WhenTerminated.Wait();
        }
    }
}