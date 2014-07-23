﻿using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;

namespace RssTest
{    
    public partial class ListPage : ContentPage
    {    
        public ListPage ()
        {
            InitializeComponent ();
        }

        public IEnumerable<object> ItemsSource
        {
            get
            {
                return _listView.ItemsSource.Cast<object>();
            }

            set
            {
                _listView.ItemsSource = value;
            }
        }
    }
}

