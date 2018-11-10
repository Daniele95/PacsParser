using rzdcxLib;
using System;
using System.Collections.Generic;
using System.Threading;
using static PacsParser.Utilities;

namespace PacsParser
{
    class Association
    {
        public String TargetIp;
        public ushort TargetPort;
        public String TargetAET;
        public String myAET;
        public ushort myPort;
    }

    class QuerySettings
    {
        Association serverino;

        public QuerySettings()
        {
            // create server association
            serverino = new Association();
            serverino.TargetIp = "localhost";
            serverino.TargetPort = 11112;
            serverino.TargetAET = "MIOSERVER";
            serverino.myAET = "USER";
            serverino.myPort = 11115;
        }

        public void findAllUsers()
        {
            FindSCU mySearch = new FindSCU();
            // define query map
            mySearch.addToMap("QueryRetrieveLevel", "PATIENT");
            mySearch.addToMap("patientName", "");
            mySearch.addToMap("patientID", "");

            // use it to query and print results according to input query map
            if (mySearch.tryQueryServer(serverino, mySearch.getSearchMap(), "find"))
                if (mySearch.tryReadResults())
                    mySearch.printResults();
            logOutput("------------------------------------------------------------------");
        }

        public void findStudy()
        {
            FindSCU mySearch = new FindSCU();
            // define query map
            mySearch.addToMap("QueryRetrieveLevel", "STUDY");
            mySearch.addToMap("studyInstanceUID", "");
            mySearch.addToMap("patientName", "Doe^Pierre");

            // use it to query and print results according to input query map
            if (mySearch.tryQueryServer(serverino, "find"))
                if (mySearch.tryReadResults())
                    mySearch.printResults();
            logOutput("------------------------------------------------------------------");
        }

        public void downloadStudy (string studyInstanceUID)
        {
            MoveSCU myMove = new MoveSCU();
            myMove.addToMap("QueryRetrieveLevel", "STUDY");
            myMove.addToMap("studyInstanceUID", studyInstanceUID);

            // qui accendo il listener
            Thread listener = new Thread(() =>
            {
                StoreSCP asd = new StoreSCP(this.serverino);
            });
            Thread main = new Thread(() =>
            {
                myMove.tryQueryServer(serverino, "retrieve");
                Thread.Sleep(1000);
            });
            listener.Start();
            Thread.Sleep(1000);
            main.Start();

            listener.Join();

        }

    }
}
