namespace RPG.Utility.UI
{
    public interface IDragDestination<T> where T : class
    {

        /// <returns>Max accentable number of item that can be added into this destination</returns>
        int GetMaxAcceptable(T item);

        /// <summary>
        /// Add a given number of items into this destination
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        void AddItems(T item, int quantity);
    }
}
