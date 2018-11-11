using System;
using System.Windows;
using System.Windows.Controls;
using static PacsParser.Utilities;

namespace PacsParser
{
    public partial class SearchPage : Window, Listener
    {
        Query query;

        public SearchPage()
        {
            InitializeComponent();
            query = new Query();

            // subscribe to find event
            Subscribe(query.myFind.publisher);
        }

        public void Subscribe(Publisher publisher)
        {
            Publisher.Event += HeardEvent;
        }
        public TextBlock infoBox;


        void HeardEvent(object sender, RaiseArgs e)
        {
            infoBox.Text = e.Message;
        }


        private void textBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            infoBox = processBox;

            string wildCard = campoTesto.Text + "*";

            resultsBox.Text = toString(query.findMatchingNames(wildCard));

            
        }


        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
