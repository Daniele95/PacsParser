using rzdcxLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using static PacsParser.Utilities;

namespace PacsParser
{
    public partial class SearchPage : Window, Listener
    {
        Startup publisher;

        public SearchPage()
        {
            InitializeComponent();
            publisher = new Startup();
            Subscribe(publisher);
            
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

        public void HeardEvent(object sender, Dictionary<string, string> s)
        {
            // Create a button.
            Button button = new Button();
            // Set properties.
           // button.Content = s["PatientName"]; 
            button.Click += myButton_Click;

            // Add created button to a previously created container.
            myStackPanel.Children.Add(button);
        }

        void myButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            button.Content = "asd";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            publisher.find();
        }

        private void listBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
