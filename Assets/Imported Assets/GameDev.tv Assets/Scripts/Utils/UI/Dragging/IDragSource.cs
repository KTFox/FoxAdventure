using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevTV.Core.UI.Dragging
{
    /// <summary>
    /// Components that implement this interfaces can act as the _source for
    /// dragging a `DragItem`.
    /// </summary>
    /// <typeparam name="T">The type that represents the _inventoryItem being dragged.</typeparam>
    public interface IDragSource<T> where T : class
    {
        /// <summary>
        /// What _inventoryItem type currently resides in this _source?
        /// </summary>
        T GetItem();

        /// <summary>
        /// What is the quantity of items in this _source?
        /// </summary>
        int GetNumber();

        /// <summary>
        /// Remove a given quantity of items from the _source.
        /// </summary>
        /// <param name="number">
        /// This should never exceed the quantity returned by `GetNumber`.
        /// </param>
        void RemoveItems(int number);
    }
}