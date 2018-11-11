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

        // todo after server answered

        public bool tryReadResults()
        {
            bool ret = false;
            try { queryResults.Get(); ret = true; }
            catch (Exception) { errorMessage("Query effettuata, ma il risultato della query è vuoto"); }
            return ret;
        }

        public Dictionary<int, string> printResults()
        {
            DCXOBJ querySingleResult = new DCXOBJ();
            logOutput("Tutti i risultati:");
            for (; !queryResults.AtEnd(); queryResults.Next())
            {
                querySingleResult = queryResults.Get();
                // stampo il risultato e lo salvo nella mappa:
                string results = displayQuerySingleResult(querySingleResult);
                logOutput(results);
            }
            return searchMap;
        }

        public string displayQuerySingleResult(DCXOBJ currObj)
        {
            string results = "";
            string message = "";
            List<int> keys = new List<int>(searchMap.Keys);
            foreach (int key in keys) // es. patientName, patientID,..)
            {
                // store found values into searchMap
                searchMap[key] = foundValue(currObj, key);

                // print only new found values:
                if (searchMap[key] == "") // es. patientID
                {
                    message += dicomTagName(key) + ": " + foundValue(currObj, key) + " | ";
                }
            }
            results += cutLastChar(message, 3);
            results = results.Replace("/n", System.Environment.NewLine);
            return results;
        }

        public static String foundValue(DCXOBJ currObj, int dicomTagNumber)
        {
            string ret = "";
            DCXELM currElem = new DCXELM();
            try
            {
                currElem = currObj.getElementByTag(dicomTagNumber);
                ret = currElem.Value;
            }
            catch (Exception)
            {
                errorMessage("Nel risultato della query non è contenuto il tag DICOM '"
                + dicomTagName(dicomTagNumber) + "'");
            }
            return ret;
        }

        //



        public override void setCallbackDelegate(DCXREQ req)
        {
            req.OnQueryResponseRecieved += new IDCXREQEvents_OnQueryResponseRecievedEventHandler(OnQueryResponseRecieved);
        }

        void OnQueryResponseRecieved(DCXOBJ queryResult)
        {
            try
            {
                logOutput("Ottenuto un risultato");
                //string results = displayQuerySingleResult(queryResult);
                //logOutput(results);
            }

            catch (Exception)
            { errorMessage("La ricerca non ha prodotto risultati."); }
        }

    }
}
