using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RssTest.Model;

namespace RssTest.ViewModel
{
    public class MainPageViewModel : NotifyPropertyChangedBase
    {
        public async void LoadItemsAsync(Task<IEnumerable<RssItem>> loadTask)
        {
            IsLoading = true;
            try
            {
                Items = await loadTask;
            }
            catch (Exception)
            {
                Items = null;
            }
            IsLoading = false;
        }

        private List<RssItem> _items;
        public IEnumerable<RssItem> Items
        {
            get
            {
                return _items;
            }

            private set
            {
                if (_items != value)
                {
                    _items = value.ToList();
                    OnPropertyChanged();
                }
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }

            private set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}

