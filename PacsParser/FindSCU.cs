using rzdcxLib;
using System;
using System.Collections.Generic;
using System.Threading;
using static PacsParser.Utilities;

namespace PacsParser
{
    class FindSCU
    {
        Dictionary<int, string> searchMap;
        private DCXOBJIterator queryResults;

        public FindSCU(Dictionary<int, string> searchMap)
        {
            this.searchMap = searchMap;
            queryResults = new DCXOBJIterator();
            Thread.Sleep(1000);
        }

        public bool tryQueryServer(Association serverino, DCXOBJ obj)
        {
            bool ret = false;
            DCXREQ req = new DCXREQ();
            req.AssociationRequestTimeout = 1;
            req.OnQueryResponseRecieved += new IDCXREQEvents_OnQueryResponseRecievedEventHandler(OnQueryResponseRecieved);
            logOutput("Launch query:");
            try
            {
                queryResults = req.Query(serverino.myAET,
                                     serverino.TargetAET,
                                     serverino.TargetIp,
                                     serverino.TargetPort,
                                     "1.2.840.10008.5.1.4.1.2.1.1",
                                     obj);
                ret = true;
            }
            catch (System.Runtime.InteropServices.COMException exc)
            { errorMessage("Impossibile connettersi al server"); }
            return ret;
        }

        public bool tryReadResults()
        {
            bool ret = false;
            try { queryResults.Get(); ret = true; }
            catch (Exception e) { errorMessage("Query effettuata, ma il risultato della query è vuoto"); }
            return ret;
        }

        public void printResults()
        {
            DCXOBJ querySingleResult = new DCXOBJ();
            logOutput("Tutti i risultati:");
            for (; !queryResults.AtEnd(); queryResults.Next())
            {
                querySingleResult = queryResults.Get();
                string results = displayQuerySingleResult(querySingleResult, searchMap);
                logOutput(results);
            }
        }

        void OnQueryResponseRecieved(DCXOBJ queryResult)
        {
            try
            {
                string results = displayQuerySingleResult(queryResult, searchMap);
                logOutput("Arrivato il file: " + results);
            }

            catch (System.Runtime.InteropServices.COMException exc)
            { errorMessage("La ricerca non ha prodotto risultati."); }
        }

    }
}
