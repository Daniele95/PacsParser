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
        
        public int numResults = 0;

        public Dictionary<int, List<string>> returnMap;

        public FindSCU() : base()
        {
            queryResults = new DCXOBJIterator();
            returnMap = new Dictionary<int, List<string>>();
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

        // also populate return map
        public new void addToMap(string dicomTagName, string value)
        {
            base.addToMap(dicomTagName,value);
            int _dicomTagNumber = dicomTagNumber(dicomTagName);
            List<string> returnValues = new List<string>();
            if (_dicomTagNumber != 0) returnMap.Add(_dicomTagNumber, returnValues);
        }

        // todo after server answered:
        /*
        // if query result object not empty
        public bool tryReadResults()
        {
            bool ret = false;
            try { queryResults.Get(); ret = true; }
            catch (Exception) { errorMessage("Query effettuata, ma il risultato della query è vuoto"); }
            return ret;
        }
            // then save results into map
        public void saveResults()
        {
            DCXOBJ querySingleResult = new DCXOBJ();
            logOutput("Tutti i risultati:");
            for (; !queryResults.AtEnd(); queryResults.Next())
            {
                querySingleResult = queryResults.Get();
            }
        }
         */
         // for each key in the map
        public List<string> storeSingleResult(DCXOBJ currObj)
        {
            List<string> returnValues = new List<string>();
            foreach (int dicomTagNumber in searchMap.Keys) // es. patientName, patientID,..)
            {
                // store found values into returnMap
                returnValues = returnMap[dicomTagNumber];
                returnValues.Add(extractValue(currObj, dicomTagNumber));
                returnMap[dicomTagNumber] = returnValues;
            }
            return returnValues;
        }

        public static String extractValue(DCXOBJ currObj, int dicomTagNumber)
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

        public new List<String> readFromMap(string dicomTagName)
        {
            int _dicomTagNumber = dicomTagNumber(dicomTagName);
            return returnMap[_dicomTagNumber];
        }

        public override void setCallbackDelegate(DCXREQ req)
        {
            req.OnQueryResponseRecieved += new IDCXREQEvents_OnQueryResponseRecievedEventHandler(OnQueryResponseRecieved);
        }



        void OnQueryResponseRecieved(DCXOBJ queryResult)
        {
            
            // raise event for window
            try
            {
                List<string> ret = storeSingleResult(queryResult);
                numResults++;
                logOutput(ret[0]);
                //RaiseEvent(numResults, ret);
            }
            

            catch (Exception)
            { errorMessage("La ricerca non ha prodotto risultati."); }
        }

    }
}
