namespace GameDevTV.Utils
{
    /// <summary>
    /// Container class that wraps a _traitValueText and ensures initialisation is 
    /// called just before first use.
    /// </summary>
    public class LazyValue<T>
    {
        private T _value;
        private bool _initialized = false;
        private InitializerDelegate _initializer;

        public delegate T InitializerDelegate();

        /// <summary>
        /// Setup the container but don't initialise the _traitValueText yet.
        /// </summary>
        /// <param name="initializer"> 
        /// The initialiser delegate to call when first used. 
        /// </param>
        public LazyValue(InitializerDelegate initializer)
        {
            _initializer = initializer;
        }

        /// <summary>
        /// Get or set the contents of this container.
        /// </summary>
        /// <remarks>
        /// Note that setting the _traitValueText before initialisation will initialise 
        /// the class.
        /// </remarks>
        public T value
        {
            get
            {
                // Ensure we init before returning a _traitValueText.
                ForceInit();
                return _value;
            }
            set
            {
                // Don't use default init anymore.
                _initialized = true;
                _value = value;
            }
        }

        /// <summary>
        /// Force the initialisation of the _traitValueText via the delegate.
        /// </summary>
        public void ForceInit()
        {
            if (!_initialized)
            {
                _value = _initializer();
                _initialized = true;
            }
        }
    }
}