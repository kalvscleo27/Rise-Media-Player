﻿using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Rise.App.Common
{
    public static class VisualTreeExtensions
    {
        /// <summary>
        /// Tries to find the specified visual child.
        /// </summary>
        /// <typeparam name="ChildItem">The kind of item to find.</typeparam>
        /// <param name="obj">Object where search will happen.</param>
        /// <returns>The item if it's found, null otherwise.</returns>
        public static ChildItem FindVisualChild<ChildItem>(this DependencyObject obj)
            where ChildItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is ChildItem item)
                {
                    return item;
                }
                else
                {
                    ChildItem childOfChild = child.FindVisualChild<ChildItem>();

                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Tries to find all children of specified kind.
        /// </summary>
        /// <param name="parent">Where to find the item.</param>
        /// <param name="recurse">Whether or not the search should be recursive.</param>
        /// <returns>All child items found.</returns>
        public static IEnumerable<ChildItem> GetChildren<ChildItem>
            (this DependencyObject parent, bool recurse = false)
                where ChildItem : DependencyObject
        {
            if (parent != null)
            {
                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    // Retrieve child visual at specified index value.
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                    if (child != null && child is ChildItem item)
                    {
                        yield return item;

                        if (recurse)
                        {
                            foreach (var grandChild in child.GetChildren<ChildItem>(true))
                            {
                                yield return grandChild;
                            }
                        }
                    }
                }
            }
        }
    }
}
