using rzdcxLib;
using System.Collections.Generic;
using static PacsParser.Utilities;

namespace PacsParser
{
    abstract class SCU
    {
        DCXREQ req = new DCXREQ() { AssociationRequestTimeout = 1 };
        Dictionary<int, string> searchMap;

        public SCU()
        {
            searchMap = new Dictionary<int, string>();
        }

        public abstract void serverConnection(DCXREQ req, Association serverino, DCXOBJ query);

        public abstract void setCallbackDelegate(DCXREQ req);

        public bool tryQueryServer(Association serverino, string type)
        {
            bool ret = false;
            
            leggiCampiQuery(searchMap, type);
            DCXOBJ query = encodeQuery(searchMap);

            setCallbackDelegate(req);

            logOutput("Launching " + type+" command: ");

            try
            {
                serverConnection(req,serverino,query);
                ret = true;
            }
            catch (System.Runtime.InteropServices.COMException exc)
            { errorMessage("Impossibile connettersi al server"); }

            return ret;
        }


        public static DCXOBJ encodeQuery(Dictionary<int, string> searchMap)
        {
            DCXOBJ obj = new DCXOBJ();
            DCXELM el = new DCXELM();

            foreach (var pair in searchMap)
            {
                el.Init(pair.Key); // ad esempio (int)QueryRetrieveLevel
                if (pair.Value != "")
                {
                    el.Value = pair.Value; // ad esempio "STUDY"
                }
                obj.insertElement(el);
            }
            return obj;
        }

        public static string[] leggiCampiQuery(Dictionary<int, string> searchMap, string type)
        {
            string specifiedFields = "";
            string queryFor = "";
            // ad es "Query for: patientName, patientID"

            foreach (var pair in searchMap)
            {
                if (pair.Value != "")
                {
                    // pair.Key ad esempio (int)QueryRetrieveLevel
                    // pair.Value ad esempio "STUDY"
                    specifiedFields += dicomTagName(pair.Key) + "   " + pair.Value + "/n";
                }
                else
                    queryFor += dicomTagName(pair.Key) + ", ";
            }
            logOutput("Retrieving:/n" + cutLastChar(specifiedFields, 2));
            if (type == "find") logOutput("Query for:/n" + cutLastChar(queryFor, 2));
            string[] campiQuery = { specifiedFields, queryFor };
            return campiQuery;
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
