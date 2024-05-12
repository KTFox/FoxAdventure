namespace RPG.Utility.UI
{
    public interface IDragSource<T> where T : class
    {
        T InventoryItemSO { get; }
        int ItemQuantity { get; }

        void RemoveItems(int quantity);
    }
}

