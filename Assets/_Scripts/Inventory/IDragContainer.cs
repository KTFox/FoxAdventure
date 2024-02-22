using RPG.UI.Inventory;

namespace RPG.Inventory {
    public interface IDragContainer<T> : IDragDestination<T>, IDragSource<T> where T : class {

    }
}