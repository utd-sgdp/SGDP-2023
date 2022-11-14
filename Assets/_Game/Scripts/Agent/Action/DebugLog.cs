using Game.Agent.Tree;

namespace Game.Agent.Action
{
    public class DebugLog : ActionNode
    {
        public string Message;
        
        protected override void OnStart()
        {
            print($"OnStart{ Message }");
        }

        protected override void OnStop()
        {
            print($"OnStop{ Message }");
        }

        protected override State OnUpdate()
        {
            print($"OnUpdate{ Message }");
            return State.Success;
        }
    }
}
