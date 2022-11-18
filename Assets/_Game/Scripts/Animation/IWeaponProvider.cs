namespace Game.Animation
{
    public interface IWeaponProvider
    {
        public void Subscribe(System.Action<ActionType> callback);
        public void Unsubscribe(System.Action<ActionType> callback);
    }

    public enum ActionType
    {
        None = 0, Attack = 1, Reload = 2,
    }
}