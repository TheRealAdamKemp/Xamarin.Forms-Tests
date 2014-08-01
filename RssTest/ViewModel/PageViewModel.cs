using Navigation;
using RssTest.Model;

namespace RssTest.ViewModel
{
    public abstract class PageViewModel : NotifyPropertyChangedBase, INavigatingViewModel
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }
            
        #region INavigatedPage implementation

        public IViewModelNavigation ViewModelNavigation { get; set; }

        #endregion
    }
}

