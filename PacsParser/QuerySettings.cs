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
            // define query map
            var searchMap = new Dictionary<int, string>();
            searchMap.Add(dicomTagNumber("QueryRetrieveLevel"), "PATIENT");
            searchMap.Add(dicomTagNumber("patientName"), "");
            searchMap.Add(dicomTagNumber("patientID"), "");
            // write query map into DCXOBJ
            DCXOBJ query = encodeQuery(searchMap, "find");
            // use it to query and print results according to input query map
            FindSCU mySearch = new FindSCU(searchMap);
            if (mySearch.tryQueryServer(serverino, query))
                if (mySearch.tryReadResults())
                    mySearch.printResults();
        }

        public void downloadStudy (string studyInstanceUID)
        {
            var map = new Dictionary<int, string>();
            map.Add(dicomTagNumber("QueryRetrieveLevel"), "STUDY");
            map.Add(dicomTagNumber("studyInstanceUID"), studyInstanceUID);
            DCXOBJ moveQuery = encodeQuery(map, "retrieve");
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
