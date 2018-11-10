using rzdcxLib;
using System;
using System.Collections.Generic;
using System.Threading;
using static PacsParser.Utilities;

namespace PacsParser
{
    class FindSCU : SCU
    {
        private DCXOBJIterator queryResults;

        public FindSCU() : base()
        {
            queryResults = new DCXOBJIterator();
            Thread.Sleep(1000);
        }

        public override void serverConnection(DCXREQ req, Association serverino, DCXOBJ query)
        {
            queryResults = req.Query(serverino.myAET,
                                    serverino.TargetAET,
                                    serverino.TargetIp,
                                    serverino.TargetPort,
                                    "1.2.840.10008.5.1.4.1.2.1.1",
                                    query);
        }

        public override bool tryReadResults()
        {
            bool ret = false;
            try { queryResults.Get(); ret = true; }
            catch (Exception e) { errorMessage("Query effettuata, ma il risultato della query è vuoto"); }
            return ret;
        }

        public override void printResults()
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

        public override void setCallbackDelegate(DCXREQ req)
        {
            req.OnQueryResponseRecieved += new IDCXREQEvents_OnQueryResponseRecievedEventHandler(OnQueryResponseRecieved);
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

    }
}
