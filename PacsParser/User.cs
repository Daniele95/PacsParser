using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PacsParser.Utilities;

namespace PacsParser
{
    class User : Publisher, Listener
    {


        public void find()
        {
            Subscribe(this);

            string query = "";
            query = " -k PatientName=\"Doe*\" " + query;
            query = " -k QueryRetrieveLevel=\"PATIENT\" " + query;
            string fullQuery =
                 " -P  -aec MIOSERVER " + query + " localhost 11112  -od . -v ";

            // launch query
            initProcess("findscu",fullQuery);

            // read results
            //initProcess("dcmdump", "-v rsp0002.dcm");
        }

        public void Subscribe(Publisher publisher)
        {
            Event += HeardEvent;
        }
        public virtual void HeardEvent(object sender, string s)
        {
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
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                if(line.Contains("PatientName"))
                    RaiseEvent(this,line);
            }
        }

    }


}
