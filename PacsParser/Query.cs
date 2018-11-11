using System;
using System.Collections.Generic;
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

    class Query
    {
        Association association;
        public FindSCU myFind;

        public Query()
        {
            myFind = new FindSCU();
            // create server association
            association = new Association();
            association.TargetIp = "localhost";
            association.TargetPort = 11112;
            association.TargetAET = "MIOSERVER";
            association.myAET = "USER";
            association.myPort = 11115;
        }


        public List<String> findMatchingNames(string partialName)
        {
            myFind = new FindSCU();
            // define query map
            myFind.addToMap("QueryRetrieveLevel", "PATIENT");
            myFind.addToMap("patientName", partialName);

            myFind.tryQueryServer(association, "find");
          //  if (mySearch.tryQueryServer(association, "find"))
          //      if (mySearch.tryReadResults())
         //           mySearch.saveResults();
            logOutput("------------------------------------------------------------------");

            return myFind.readFromMap("patientName");
        }

        public List<String> findPatientID(string patientName)
        {
            FindSCU mySearch = new FindSCU();
            // define query map
            mySearch.addToMap("QueryRetrieveLevel", "PATIENT");
            mySearch.addToMap("patientName", patientName);
            mySearch.addToMap("patientID", "");

          //  if (mySearch.tryQueryServer(association, "find"))
           //     if (mySearch.tryReadResults())
          //          mySearch.saveResults();
            logOutput("------------------------------------------------------------------");

            return mySearch.readFromMap("patientID");
        }

        public List<String> findStudyOfUser(string patientID)
        {
            FindSCU mySearch = new FindSCU();
            // define query map
            mySearch.addToMap("QueryRetrieveLevel", "STUDY");
            mySearch.addToMap("patientID", patientID);
            mySearch.addToMap("studyInstanceUID", "");

            // use it to query and print results according to input query map
         //   if (mySearch.tryQueryServer(association, "find"))
        //        if (mySearch.tryReadResults())
         //           mySearch.saveResults();
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
                if(myMove.listener.errorCount==0)
                    logOutput("file salvati!");

        }

    }
}
