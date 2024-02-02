namespace RPG.UI.Inventory {
    public interface IDragSource<T> where T : class {

        /// <returns>Item type currently resides in this source</returns>
        T GetItem();

        /// <returns>Quantity of item currently reside in this source</returns>
        int GetItemQuanity();

        /// <summary>
        /// Remove a given number of items from this source
        /// </summary>
        /// <param name="quantity"></param>
        void RemoveItems(int quantity);
    }
}

