using rzdcxLib;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static PacsParser.Utilities;

namespace PacsParser
{
    class StoreSCP
    {
        static private int _filecount = 0;
        string storageDirectory = "C:/Users/daniele/Desktop/moveAndStore/";
        public int errorCount { get; private set; }

        DCXAPP app;
        DCXACC acc;
        public StoreSCP()
        {
            errorCount = 0;
            app = new DCXAPP();
            app.StartLogging("C:/Users/daniele/Desktop/dicomLog.txt");

            // creo l'accettore
            acc = new DCXACC();

            // connetto gli eventi alle callback
            acc.OnConnection += new IDCXACCEvents_OnConnectionEventHandler(OnConnectionEventHandler);
            acc.OnStoreSetup += new IDCXACCEvents_OnStoreSetupEventHandler(OnStoreSetupEventHandler);
            acc.OnTimeout += new IDCXACCEvents_OnTimeoutEventHandler(OnTimeoutEventHandler);
            acc.OnStoreDone += new IDCXACCEvents_OnStoreDoneEventHandler(OnStoreDoneEventHandler);
            acc.OnCommitResult += new IDCXACCEvents_OnCommitResultEventHandler(OnCommitResultEventHandler);

        }
        
        public void startListening(Association ass)
        {

            //Console.WriteLine("StoreSCP listening...");
            if (acc.WaitForConnection(ass.myAET, ass.myPort, 2))
            {
                // Can go to new thread
                bool res;
                do
                {
                    if (acc.WaitForCommand(2))
                    {
                        res = true;
                    }
                    else
                    {
                        res = false;
                    }
                } while (res);
            }

            ReleaseComObject(acc);
            app.StopLogging();
            ReleaseComObject(app);
        }

        // evento: ricevuta richiesta associazione
        private void OnConnectionEventHandler(string callingTitle, string calledTitle,
                                       string callingHost, ref bool acceptConnection)
        {
            acceptConnection = true;
        }

        // evento: ricevuto comando move
        private void OnStoreSetupEventHandler(ref string filename)
        {
            logOutput("incoming file");
            filename = storageDirectory + (++_filecount) + ".dcm";
            // saving file as _filecount.dcm
        }

        // cosa fare in caso finisca il tempo
        private void OnTimeoutEventHandler()
        {
            MessageBox.Show("A timeout occurred");
        }

        // evento: arrivato il file (lo controllo e rispondo se è ok)
        private void OnStoreDoneEventHandler(string filename, bool status, ref bool accept)
        {
            // if status, file saved!
            if (!status)
            {
                errorCount++;
                errorMessage("errore nel salvataggio del file!");
            }

            // check if file already exists
            string sop_instance_uid = null;
            DCXOBJ o = new DCXOBJ();
            o.openFile(filename);
            DCXELM e = o.getElementByTag((int)DICOM_TAGS_ENUM.sopInstanceUID);

            if (e != null)
            {
                sop_instance_uid = e.Value.ToString();
            }
            ReleaseComObject(e);
            ReleaseComObject(o);

            if (sop_instance_uid != null)
            {
                String destinationFilename = storageDirectory + sop_instance_uid + ".dcm";
                if (!File.Exists(destinationFilename))
                {
                    File.Move(filename, destinationFilename);
                }
                else
                {
                    errorMessage("file gia esiste");
                    File.Delete(filename);
                }
            }
            accept = true;
        }

        // evento: risultato di storage commit
        private void OnCommitResultEventHandler(bool status, string transactionUID,
                                                string succeededInstances, string failedInstances)
        {
            MessageBox.Show("Commit result: Status: " + status + "\n" +
                            "TransactionUID: " + transactionUID + "\n" +
                            "SucceededInstances: " + succeededInstances + "\n" +
                            "FailedInstances: " + failedInstances);
        }

        // libera la memoria
        private void ReleaseComObject(object o)
        {
            if (o != null)
                Marshal.ReleaseComObject(o);
        }

 
    }
}
