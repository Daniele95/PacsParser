using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PacsParser.Utilities;
using System.Xml;
using System.Xml.Linq;
using System.Threading;

namespace PacsParser
{
    class User : Publisher
    {


        public void find()
        {

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path= "./Results";
            watcher.Created += new FileSystemEventHandler(onCreated);
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.EnableRaisingEvents = true;

            string query = "";
            query = " -k PatientName=\"Doe*\" " + query;
            query = " -k PatientID " + query;
            query = " -k QueryRetrieveLevel=\"PATIENT\" " + query;
            string fullQuery =
                 " -P  -aec MIOSERVER " + query + " localhost 11112  -od ./Results -v --extract-xml ";

            // launch query
            initProcess("findscu",fullQuery);

        }
        [STAThread]
        private void onCreated(object sender, FileSystemEventArgs e)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            Thread.Sleep(100);

            XmlDocument doc = new XmlDocument();
            doc.Load(e.FullPath);

            XmlNodeList xnList = doc.SelectNodes("/data-set/element[@name='PatientName']");
            string patientName = xnList[0].InnerText;
             xnList = doc.SelectNodes("/data-set/element[@name='PatientID']");
            string patientID=  xnList[0].InnerText;

            results.Add("patientName", patientID);
            results.Add("patientID", patientID);
            RaiseEvent(this, results);
        }

        public void initProcess(string fileName, string arguments)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {

                    FileName = "C:/Users/daniele/Documents/Visual Studio 2017/Projects/PacsParser/PacsParser/Services/" + fileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            DirectoryInfo di = Directory.CreateDirectory("Results");


            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            proc.Start();
            
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                Console.WriteLine(line);

                Dictionary<string, string> results = new Dictionary<string, string>();
                if (line.Contains("PATIENT"))
                    while (!line.Contains("PatientID"))
                    {
                        line = proc.StandardOutput.ReadLine();
                        if (line.Contains("PatientName"))
                            results.Add("PatientName", line);
                    }
                if (line.Contains("PatientID"))
                    results.Add("PatientID", line);
               // if (results["PatientName"] == null) Console.WriteLine("ciao");
                RaiseEvent(this, results);
            }
        }

    }


}
