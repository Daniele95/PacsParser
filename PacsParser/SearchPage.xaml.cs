using rzdcxLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using static PacsParser.Utilities;

namespace PacsParser
{
    public partial class SearchPage : Window
    {
        User publisher;
        Dictionary<string, Dictionary<string,string>> results = new Dictionary<string, Dictionary<string, string>>();

        public SearchPage()
        {
            InitializeComponent();
            publisher = new User();
            Subscribe();           

        }
        public void Subscribe()
        {

            // create a listener for incoming results
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = "./Results";
            watcher.Created += new FileSystemEventHandler(onCreated);
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.EnableRaisingEvents = true;
        }

        private void onCreated(object sender, FileSystemEventArgs e)
        {
          //  Dictionary<string, string> singleResult = new Dictionary<string, string>();
            Thread.Sleep(100);

            XmlDocument doc = new XmlDocument();
            doc.Load(e.FullPath);

            XmlNodeList xnList = doc.SelectNodes("/data-set/element[@name='PatientName']");
            string patientName="";
            if (xnList.Count>0)
                patientName = xnList[0].InnerText;
            xnList = doc.SelectNodes("/data-set/element[@name='PatientID']");
            string patientID = xnList[0].InnerText;

            Dictionary<string,string> valoriPaziente = new Dictionary<string, string>();
            valoriPaziente.Add("patientID", patientID);
            
            results.Add(patientName, valoriPaziente);
          

        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            publisher.find();
            Thread.Sleep(1000);
            
            foreach(string paziente in results.Keys )
                addButton(results[paziente]["patientID"]);
        }


        public void addButton(string patientName)
        {
            Button button = new Button();
            button.Click += patientName_Click;
            button.Content = patientName;

            // Add created button to a previously created container.
            myStackPanel.Children.Add(button);
        }
        void patientName_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            publisher.findStudyByPatientID(button.Content.ToString()); // ad esempio 5874345

        }

        private void textBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
