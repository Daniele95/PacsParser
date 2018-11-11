using rzdcxLib;
using System.Collections.Generic;
using static PacsParser.Utilities;

namespace PacsParser
{
    abstract class SCU
    {
        public DCXREQ req = new DCXREQ() { AssociationRequestTimeout = 1 };
        public Dictionary<int, string> searchMap;

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

            logOutput("Launching " + type+" command:");

            try
            {
                serverConnection(req,serverino,query);
                ret = true;
            }
            catch (System.Runtime.InteropServices.COMException)
            { errorMessage("Impossibile connettersi al server"); }

            return ret;
        }

        // to do before calling server        
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
            if (type == "find")
            {
                logOutput("Set parameters:/n" + cutLastChar(specifiedFields, 2) + "/n");
                logOutput("Query for:/n" + cutLastChar(queryFor, 2) + "/n");
            }
            if (type=="retrieve")
                logOutput("Retrieving:/n" + cutLastChar(specifiedFields, 2)+"/n");

            string[] campiQuery = { specifiedFields, queryFor };
            return campiQuery;
        }
        //

        // getter and setter

        public void addToMap(string dicomTagName, string value)
        {
            int _dicomTagNumber = dicomTagNumber(dicomTagName);
            if (_dicomTagNumber != 0) searchMap.Add(_dicomTagNumber, value);
        }

        public string readFromMap(string dicomTagName)
        {
            int _dicomTagNumber = dicomTagNumber(dicomTagName);
            return searchMap[_dicomTagNumber];
        }

        public Dictionary<int, string> getSearchMap()
        {
            return searchMap;
        }

    }

}
