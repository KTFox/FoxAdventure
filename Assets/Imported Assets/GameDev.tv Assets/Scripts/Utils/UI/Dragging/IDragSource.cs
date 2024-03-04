﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevTV.Core.UI.Dragging
{
    /// <summary>
    /// Components that implement this interfaces can act as the source for
    /// dragging a `DragItem`.
    /// </summary>
    /// <typeparam name="T">The type that represents the _item being dragged.</typeparam>
    public interface IDragSource<T> where T : class
    {
        /// <summary>
        /// What _item type currently resides in this source?
        /// </summary>
        T GetItem();

        /// <summary>
        /// What is the quantity of items in this source?
        /// </summary>
        int GetNumber();

        /// <summary>
        /// Remove a given quantity of items from the source.
        /// </summary>
        /// <param name="number">
        /// This should never exceed the quantity returned by `GetNumber`.
        /// </param>
        void RemoveItems(int number);
    }
}