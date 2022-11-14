using Game.Agent.Tree;

namespace Game.Agent.Composite
{
    public class InitializeAndUpdate : CompositeNode
    {
        protected override void OnStart() => Children[0].Update();
        protected override void OnStop() { }
        protected override State OnUpdate() => Children[1].Update();
    }
}