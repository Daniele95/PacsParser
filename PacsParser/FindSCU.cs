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

        public FindSCU()
        {
            searchMap = new Dictionary<int, string>();
            queryResults = new DCXOBJIterator();
            Thread.Sleep(1000);
        }


        public bool tryQueryServer(Association serverino, Dictionary<int, string> searchMap)
        {
            this.searchMap = searchMap;
            leggiCampiQuery(searchMap, "find");

            // write query map into DCXOBJ
            DCXOBJ query = encodeQuery(searchMap);

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
                                     query);
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
                logOutput("Ottenuto un risultato");
                //logOutput(results);
            }

            catch (System.Runtime.InteropServices.COMException exc)
            { errorMessage("La ricerca non ha prodotto risultati."); }
        }


        public void addToMap(string dicomTagName, string value)
        {
            int _dicomTagNumber = dicomTagNumber(dicomTagName);
            if (_dicomTagNumber != 0) searchMap.Add(_dicomTagNumber, value);
        }

        public Dictionary<int, string> getSearchMap()
        {
            return searchMap;
        }

    }
}
