using rzdcxLib;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using static PacsParser.Utilities;

namespace PacsParser
{
    public partial class SearchPage : Window, Listener
    {

   
        public SearchPage()
        {
            InitializeComponent();
            Startup publisher = new Startup();
            Subscribe(publisher);
            publisher.find();
        }

        public void Subscribe(Publisher publisher)
        {
            publisher.Event += HeardEvent;
        }



        private void textBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            /*
            string wildCard = campoTesto.Text + "*";
            query.findMatchingNames(wildCard);*/
        }



        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void HeardEvent(object sender, string s)
        {
            resultsBox.Text += s;
        }
    }
}
