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

            // query optionts
            string query = "";
            query = " -k PatientName=\"Doe*\" " + query;
            query = " -k PatientID " + query;
            query = " -k QueryRetrieveLevel=\"PATIENT\" " + query;
            string fullQuery =
                 " -P  -aec MIOSERVER " + query + " localhost 11112  -od ./Results -v --extract-xml ";

            // launch query
            initProcess("findscu",fullQuery);

        }


        public void findStudyByPatientID(string patientID)
        {
            // query optionts
            string query = "";
            query = " -k PatientID=\""+ patientID+"\"" + query;
            query = " -k QueryRetrieveLevel=\"STUDY\" " + query;
            query = " -k StudyInstanceUID" + query;

            string fullQuery =
                 " -P  -aec MIOSERVER " + query + " localhost 11112  -od ./Results -v --extract-xml ";

            // launch query
            initProcess("findscu", fullQuery);

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
                file.Delete();

            proc.Start();

            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                Console.WriteLine(line);
            }
        }

    }


}
