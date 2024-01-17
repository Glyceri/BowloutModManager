namespace BowloutModManager.BowloutModded.Interfaces
{
    public abstract class SingletonBase<T> where T : SingletonBase<T>
    {
        public SingletonBase()
        {
            if (_instance != null) return;
            _instance = (T)this;
        }
        protected static T _instance = null;
        public static T Instance
        {
            get => _instance;
        }
    }
}
