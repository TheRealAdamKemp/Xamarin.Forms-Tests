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
    public class App
    {
        public static Page GetMainPage()
        {
            var mainPageViewModel = new MainPageViewModel();
            mainPageViewModel.LoadItemsAsync(LoadItemsAsync(new Uri("http://feeds.macrumors.com/MacRumors-All?format=xml")));

            var navigationFrame = new NavigationFrame(mainPageViewModel);
            return navigationFrame.Root;
        }

        public static async Task<IEnumerable<RssItem>> LoadItemsAsync(Uri feedUri)
        {
            var client = new HigLabo.Net.Rss.RssClient();
            var feed = await client.GetRssFeedAsync(feedUri);
            return feed.Items.Select(item => new RssItem()
                {
                    Title = item.Title,
                    Description = item.Description,
                    Link = item.Link
                });
        }
    }
}

