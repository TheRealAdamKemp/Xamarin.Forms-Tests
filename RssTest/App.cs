using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

using RssTest.Model;
using RssTest.View.Pages;
using RssTest.ViewModel;

namespace RssTest
{
    public class App
    {
        public static Page GetMainPage()
        {
            var mainPageViewModel = new MainPageViewModel();
            mainPageViewModel.LoadItemsAsync(LoadItemsAsync(new Uri("http://feeds.macrumors.com/MacRumors-All?format=xml")));
            var mainPage = new MainPage()
                {
                    BindingContext = mainPageViewModel
                };
            return mainPage;
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

