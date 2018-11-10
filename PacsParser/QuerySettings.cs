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
            if (mySearch.tryQueryServer(serverino, mySearch.getSearchMap()))
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
            if (mySearch.tryQueryServer(serverino, mySearch.getSearchMap()))
                if (mySearch.tryReadResults())
                    mySearch.printResults();
            logOutput("------------------------------------------------------------------");
        }

        public void downloadStudy (string studyInstanceUID)
        {
            var map = new Dictionary<int, string>();
            map.Add(dicomTagNumber("QueryRetrieveLevel"), "STUDY");
            map.Add(dicomTagNumber("studyInstanceUID"), studyInstanceUID);
            leggiCampiQuery(map, "retrieve");
            DCXOBJ moveQuery = encodeQuery(map);
            listenAndMove(moveQuery);
        }

        void listenAndMove(DCXOBJ moveQuery)
        {

            Thread listener = new Thread(() =>
            {
                StoreSCP asd = new StoreSCP(this.serverino);
            });
            Thread main = new Thread(() =>
            {
                MoveSCU mv = new MoveSCU(this.serverino);
                mv.issueMoveCommand(moveQuery);
                Thread.Sleep(1000);
            });
            listener.Start();
            Thread.Sleep(1000);
            main.Start();

            listener.Join();
        }

    }
}
