namespace RPG.Utility.UI
{
    public interface IDragSource<T> where T : class
    {
        /// <summary>
        /// Type of item that currently resides in this source
        /// </summary>
        T Item { get; }

        /// <summary>
        /// Quantity of item currently reside in this source
        /// </summary>
        /// <returns></returns>
        int ItemQuanity { get; }

        /// <summary>
        /// Remove a given number of items from this source
        /// </summary>
        /// <param name="quantity"></param>
        void RemoveItems(int quantity);
    }
}

