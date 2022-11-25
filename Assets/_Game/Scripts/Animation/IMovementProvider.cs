using UnityEngine;

namespace Game.Animation
{
    public interface IMovementProvider
    {
        public void SubscribeToOnMove(System.Action<Vector2> movement);
        public void UnsubscribeToOnMove(System.Action<Vector2> movement);
    }
}