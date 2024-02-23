using RPG.Inventory;

namespace RPG.Utility.UI
{
    public interface IDragContainer<T> : IDragDestination<T>, IDragSource<T> where T : class
    {

    }
}