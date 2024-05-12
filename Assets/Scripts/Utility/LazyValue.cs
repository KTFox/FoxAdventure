namespace RPG.Utility
{
   public class LazyValue<T>
    {
        // Constructor
        public LazyValue(InitializerDelegate initializer)
        {
            this._initializer = initializer;
        }

        // Variables

        private T _value;
        private bool _hasInitialized = false;
        private InitializerDelegate _initializer;

        public delegate T InitializerDelegate();

        // Properties

        public T Value
        {
            get
            {
                ForceInit();
                return _value;
            }
            set
            {
                _hasInitialized = true;
                _value = value;
            }
        }


        // Methods

        public void ForceInit()
        {
            if (!_hasInitialized)
            {
                _value = _initializer();
                _hasInitialized = true;
            }
        }
    }
}
