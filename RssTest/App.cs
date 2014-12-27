using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

using Navigation;
using RssTest.Model;
using RssTest.ViewModel.Pages;

namespace RssTest
{
    public class RssTestApplication : Application
    {
        public RssTestApplication()
        {
            MainPage = CreateMainPage();
        }

        private static Page CreateMainPage()
        {
            var mainPageViewModel = new MainPageViewModel();
            mainPageViewModel.LoadItemsAsync(LoadItemsAsync(new Uri("http://feeds.macrumors.com/MacRumors-All?format=xml")));

            var navigationFrame = new NavigationFrame(mainPageViewModel);
            return navigationFrame.Root;
        }

        private static async Task<IEnumerable<RssItem>> LoadItemsAsync(Uri feedUri)
        {
            var client = new HigLabo.Net.Rss.RssClient();
            var tcs = new TaskCompletionSource<HigLabo.Net.Rss.RssFeed>();
            client.GetRssFeed(feedUri, tcs.SetResult);
            var feed = await tcs.Task;
            return feed.Items.Select(item => new RssItem()
                {
                    Title = item.Title,
                    Description = item.Description,
                    Link = item.Link
                });
        }
    }
}
