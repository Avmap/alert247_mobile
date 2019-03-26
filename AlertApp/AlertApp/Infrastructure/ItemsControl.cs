using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlertApp.Infrastructure
{
    public class ItemsControl : StackLayout
    {
        #region BindAble        
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(object), typeof(ItemsControl), null, BindingMode.TwoWay, null, propertyChanged: ItemsSourceChanged);
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create<ItemsControl, object>(p => p.SelectedItem, default(object), BindingMode.TwoWay, null, OnSelectedItemChanged);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create<ItemsControl, DataTemplate>(p => p.ItemTemplate, default(DataTemplate));

        public event EventHandler<SelectedItemChangedEventArgs> SelectedItemChanged;

        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public bool ItemsClickable { get; set; }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        private static void ItemsSourceChanged<TPropertyType>(BindableObject bindable, TPropertyType oldValue, TPropertyType newValu)
        {
            var itemsLayout = (ItemsControl)bindable;
            itemsLayout.SetItems();
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemsView = (ItemsControl)bindable;
            if (newValue == oldValue)
                return;

            itemsView.SetSelectedItem(newValue);
        }
        #endregion

        #region item rendering
        protected readonly ICommand ItemSelectedCommand;

        protected virtual void SetItems()
        {
            Children.Clear();

            if (ItemsSource is INotifyCollectionChanged)
                ((INotifyCollectionChanged)ItemsSource).CollectionChanged += CollectionChanged;

            foreach (var item in ItemsSource)
                Children.Add(GetItemView(item));
        }

        protected virtual View GetItemView(object item)
        {
            var content = ItemTemplate.CreateContent();

            var view = content as View;
            if (view == null)
                return null;

            view.BindingContext = item;
            if (ItemsClickable)
            {
                var gesture = new TapGestureRecognizer
                {
                    Command = ItemSelectedCommand,
                    CommandParameter = item
                };

                AddGesture(view, gesture);
            }


            return view;
        }

        protected void AddGesture(View view, TapGestureRecognizer gesture)
        {
            view.GestureRecognizers.Add(gesture);

            var layout = view as Layout<View>;

            if (layout == null)
                return;

            foreach (var child in layout.Children)
                AddGesture(child, gesture);
        }

        protected virtual void SetSelectedItem(object selectedItem)
        {
            var handler = SelectedItemChanged;
            if (handler != null)
                handler(this, new SelectedItemChangedEventArgs(selectedItem));
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var source = new List<object>(((IEnumerable)sender).Cast<object>());
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        int index = e.NewStartingIndex;
                        foreach (var item in e.NewItems)
                            Children.Insert(index++, GetItemView(item));
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    {
                        var item = source[e.OldStartingIndex];
                        Children.RemoveAt(e.OldStartingIndex);
                        Children.Insert(e.NewStartingIndex, GetItemView(item));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        Children.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    {
                        Children.RemoveAt(e.OldStartingIndex);
                        Children.Insert(e.NewStartingIndex, GetItemView(source[e.NewStartingIndex]));
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Children.Clear();
                    foreach (var item in ItemsSource)
                        Children.Add(GetItemView(item));
                    break;
            }
        }
        #endregion

        public ItemsControl()
        {

            ItemSelectedCommand = new Command<object>(item =>
            {
                SelectedItem = item;
            });
        }
    }
}
