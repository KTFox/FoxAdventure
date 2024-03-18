namespace RPG.Utility.UI
{
    public interface IDragDestination<T> where T : class
    {
        int GetMaxAcceptable(T item);

        void AddItems(T item, int quantity);
    }
}
