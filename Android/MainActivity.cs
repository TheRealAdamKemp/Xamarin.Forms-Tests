using Android.App;
using Android.OS;

using Xamarin.Forms.Platform.Android;

namespace RssTest.Android
{
    [Activity(Label = "RssTest.Android.Android", MainLauncher = true)]
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Xamarin.Forms.Forms.Init(activity: this, bundle: savedInstanceState);

            base.OnCreate(savedInstanceState);

            LoadApplication(new RssTestApplication());
        }
    }
}
