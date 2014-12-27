using MonoTouch.Foundation;
using Xamarin.Forms.Platform.iOS;

namespace RssTest.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        AppDelegate()
        {
            Xamarin.Forms.Forms.Init();
            LoadApplication(new RssTestApplication());
        }
    }
}
