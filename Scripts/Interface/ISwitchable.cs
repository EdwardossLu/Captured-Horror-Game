namespace _Game.Scripts.Interface
{
    public interface ISwitchable
    {
        public bool IsActive { get; }

        public void Activate();
        public void Deactivate();
    }
}