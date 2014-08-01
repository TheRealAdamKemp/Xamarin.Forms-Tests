using Xamarin.Forms;

namespace RssTest.View.Pages
{
    [Navigation.RegisterViewModel(typeof(RssTest.ViewModel.Pages.ItemPageViewModel))]
    public partial class ItemPage : ContentPage
    {    
        public ItemPage ()
        {
            InitializeComponent ();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _webView.Source.BindingContext = BindingContext;
        }
    }
}

