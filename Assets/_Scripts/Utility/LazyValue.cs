namespace RPG.Utility
{
    /// <summary>
    /// Container class that wraps a value and ensures initialisation is called just before first use.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazyValue<T>
    {
        private T _value;
        private bool hasInitialized = false;
        private InitializerDelegate initializer;

        public delegate T InitializerDelegate();

        public LazyValue(InitializerDelegate initializer)
        {
            this.initializer = initializer;
        }

        public T Value
        {
            get
            {
                ForceInit();
                return _value;
            }
            set
            {
                hasInitialized = true;
                _value = value;
            }
        }

        public void ForceInit()
        {
            if (!hasInitialized)
            {
                _value = initializer();
                hasInitialized = true;
            }
        }
    }
}
