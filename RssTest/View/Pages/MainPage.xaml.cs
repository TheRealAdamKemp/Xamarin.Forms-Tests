using System.Windows.Input;
using Xamarin.Forms;

namespace RssTest.View.Pages
{
    [Navigation.RegisterViewModel(typeof(RssTest.ViewModel.Pages.MainPageViewModel))]
    public partial class MainPage : ContentPage
    {
        public const string ItemSelectedCommandPropertyName = "ItemSelectedCommand"; 
        public static BindableProperty ItemSelectedCommandProperty = BindableProperty.Create(
            propertyName: "ItemSelectedCommand",
            returnType: typeof(ICommand),
            declaringType: typeof(MainPage),
            defaultValue: null);

        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }

        public MainPage ()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            RemoveBinding(ItemSelectedCommandProperty);
            SetBinding(ItemSelectedCommandProperty, new Binding(ItemSelectedCommandPropertyName));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _listView.SelectedItem = null;
        }

        private void HandleItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            var command = ItemSelectedCommand;
            if (command != null && command.CanExecute(e.SelectedItem))
            {
                command.Execute(e.SelectedItem);
            }
        }
    }
}

