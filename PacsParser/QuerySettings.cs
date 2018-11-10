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
        Association association;

        public QuerySettings()
        {
            // create server association
            association = new Association();
            association.TargetIp = "localhost";
            association.TargetPort = 11112;
            association.TargetAET = "MIOSERVER";
            association.myAET = "USER";
            association.myPort = 11115;
        }

        public void findAllUsers()
        {
            FindSCU mySearch = new FindSCU();
            // define query map
            mySearch.addToMap("QueryRetrieveLevel", "PATIENT");
            mySearch.addToMap("patientName", "");
            mySearch.addToMap("patientID", "");

            // use it to query and print results according to input query map
            if (mySearch.tryQueryServer(association, "find"))
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
            if (mySearch.tryQueryServer(association, "find"))
                if (mySearch.tryReadResults())
                    mySearch.printResults();
            logOutput("------------------------------------------------------------------");
        }

        public void downloadStudy (string studyInstanceUID)
        {
            MoveSCU myMove = new MoveSCU();
            myMove.startListening(association);
            myMove.addToMap("QueryRetrieveLevel", "STUDY");
            myMove.addToMap("studyInstanceUID", studyInstanceUID);

            if (myMove.tryQueryServer(association, "retrieve"))
                logOutput("inviata richiesta di associazione e invio file");

        }

    }
}
