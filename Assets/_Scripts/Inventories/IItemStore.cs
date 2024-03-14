namespace RPG.Inventories
{
    public interface IItemStore
    {
        int AddItems(InventoryItemSO item, int number);
    }
}
