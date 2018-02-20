using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace UILibrary
{
    public class SortableBindingList<T> : BindingList<T>
    {
        public SortableBindingList(IList<T> list) : base(list)
        {
        }

        private bool isSorted = false;
        private ListSortDirection sortDirection = ListSortDirection.Ascending;
        private PropertyDescriptor sortProperty;

        protected override bool SupportsSortingCore
        {
            get
            {
                return true;
            }
        }

        protected override bool IsSortedCore
        {
            get
            {
                return isSorted;
            }
        }

        protected override bool SupportsChangeNotificationCore
        {
            get
            {
                return true;
            }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirection; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortProperty; }
        }

        protected override void RemoveSortCore()
        {
            sortDirection = ListSortDirection.Ascending;
            sortProperty = null;
        }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            sortProperty = prop;
            sortDirection = direction;
            isSorted = true;

            var items = this.Items;

            switch (direction)
            {
                case ListSortDirection.Ascending:
                    items = this.Items.OrderBy(x => prop.GetValue(x)).ToList();
                    break;
                case ListSortDirection.Descending:
                    items = this.Items.OrderByDescending(x => prop.GetValue(x)).ToList();
                    break;
            }

            this.ClearItems();
            items.ToList().ForEach(a => this.Add(a));

            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
    }
}
