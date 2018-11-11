using System;
using System.Windows;
using System.Windows.Controls;

namespace PacsParser
{
    public partial class SearchPage : Window
    {
        QuerySettings query;

        public SearchPage()
        {
            InitializeComponent();
            query = new QuerySettings();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void textBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            string wildCard = campoTesto.Text + "*";
            results.Text = query.findFullName(wildCard);
        }
    }
}
