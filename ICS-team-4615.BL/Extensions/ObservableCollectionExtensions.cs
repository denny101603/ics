using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ICS_team_4615.BL.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}
