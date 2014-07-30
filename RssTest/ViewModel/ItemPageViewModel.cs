using RssTest.Model;

namespace RssTest.ViewModel
{
    public class ItemPageViewModel : PageViewModel
    {
        private RssItem _item;
        public RssItem Item
        {
            get { return _item; }

            set
            {
                if (_item != value)
                {
                    _item = value;
                    OnPropertyChanged();
                    Title = _item.Title;
                }
            }
        }
    }
}

