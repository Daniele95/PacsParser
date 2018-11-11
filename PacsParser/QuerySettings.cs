using System;
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

        public string findPatientID(string patientName)
        {
            FindSCU mySearch = new FindSCU();
            // define query map
            mySearch.addToMap("QueryRetrieveLevel", "PATIENT");
            mySearch.addToMap("patientName", patientName);
            mySearch.addToMap("patientID", "");

            if (mySearch.tryQueryServer(association, "find"))
                if (mySearch.tryReadResults())
                    mySearch.printResults();
            logOutput("------------------------------------------------------------------");

            return mySearch.readFromMap("patientID");
        }

        public string findStudyOfUser(string patientID)
        {
            FindSCU mySearch = new FindSCU();
            // define query map
            mySearch.addToMap("QueryRetrieveLevel", "STUDY");
            mySearch.addToMap("patientID", patientID);
            mySearch.addToMap("studyInstanceUID", "");

            // use it to query and print results according to input query map
            if (mySearch.tryQueryServer(association, "find"))
                if (mySearch.tryReadResults())
                    mySearch.printResults();
            logOutput("------------------------------------------------------------------");

            // supponendo che il paziente abbia un solo studio:
            return mySearch.readFromMap("studyInstanceUID");
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
