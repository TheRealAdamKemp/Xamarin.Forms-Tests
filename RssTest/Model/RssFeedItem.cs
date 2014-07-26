using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HigLabo.Net.Rss;

namespace RssTest.Model
{
    public class RssFeedItem : NotifyPropertyChangedBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }

            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _feedUrl;
        public string FeedUrl
        {
            get { return _feedUrl; }

            set
            {
                if (_feedUrl != value)
                {
                    _feedUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<RssItem> _items;
        public IEnumerable<RssItem> Items
        {
            get { return _items; }

            set
            {
                if (_items != value)
                {
                    _items = value.ToList();
                    OnPropertyChanged();
                }
            }
        }

        public async Task LoadItemsAsync()
        {
            var client = new RssClient();
            var feed = await client.GetRssFeedAsync(new Uri(FeedUrl));
            Items = feed.Items.Select(item => new RssItem()
                {
                    Title = item.Title,
                    Description = item.Description,
                    Link = item.Link
                });
        }
    }
}

