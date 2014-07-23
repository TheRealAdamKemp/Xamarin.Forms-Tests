using System;
using System.Linq;
using Xamarin.Forms;

using HigLabo.Net.Rss;

namespace RssTest
{
    public class App
    {
        private static ListPage _listPage;

        public static Page GetMainPage()
        {
            _listPage = new ListPage()
                {
                    ItemsSource = Enumerable.Repeat("Loading...", 1),
                };
            LoadItemsAsync(new Uri("http://feeds.macrumors.com/MacRumors-All?format=xml"));
            return _listPage;
        }

        public static async void LoadItemsAsync(Uri feedUri)
        {
            var client = new RssClient();
            var feed = await client.GetRssFeedAsync(feedUri);
            _listPage.ItemsSource = feed.Items.Select(item => item.Title);
        }
    }
}

