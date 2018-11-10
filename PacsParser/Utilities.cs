using rzdcxLib;
using System;
using System.Collections.Generic;
using System.Windows;

namespace PacsParser
{
    public class Utilities
    {
        // the following is utility because shared by both find and move
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
            if (type=="find") logOutput("Query for:/n" + cutLastChar(queryFor, 2));
            string[] campiQuery = { specifiedFields, queryFor };
            return campiQuery;
        }

        public static string displayQuerySingleResult(DCXOBJ currObj, Dictionary<int, string> searchMap)
        {
            string results = "";
            string message = "";
            foreach (var pair in searchMap)
                if (pair.Value == "")
                    message += stampa(currObj, pair.Key);

            results += cutLastChar(message,3);
            results = results.Replace("/n", System.Environment.NewLine);
            return results;
        }

        public static String stampa(DCXOBJ currObj, int dicomTagNumber)
        {
            string ret = "";
            DCXELM currElem = new DCXELM();
            try
            {
                currElem = currObj.getElementByTag(dicomTagNumber);
                ret = dicomTagName(dicomTagNumber) + ": " + currElem.Value + " | ";
            }
            catch (Exception e)
            {
                errorMessage("Nel risultato della query non è contenuto il tag DICOM '"
                + dicomTagName(dicomTagNumber) + "'");
            }
            return ret;
        }

        public static void logOutput(string s)
        {
            Console.WriteLine(breakLines(s));
        }

        public static void errorMessage(string s)
        {
            MessageBox.Show(s);
            Console.WriteLine(s);
        }

        public static string breakLines(string a)
        {
            return a.Replace("/n", System.Environment.NewLine);
        }

        public static string cutLastChar(string a, int n)
        {
            string ret = "";
            if (a.Length > n) ret = a.Substring(0, a.Length - n);
            else ret = a;
            return ret;
        }

        public static String dicomTagName(int dicomTagNumber)
        {
            string ret = "";
            DICOM_TAGS_ENUM myEnum;
            try { 
                myEnum = (DICOM_TAGS_ENUM)dicomTagNumber;
                ret = myEnum.ToString();
            }
            catch(Exception e) { errorMessage("Al numero " + dicomTagNumber + " non corrisponde alcun tag DICOM"); }
            return ret;
        }

        public static int dicomTagNumber(string dicomTagName)
        {
            int ret = 0;
            DICOM_TAGS_ENUM myEnum;
            try {
                myEnum = (DICOM_TAGS_ENUM)System.Enum.Parse(typeof(DICOM_TAGS_ENUM), dicomTagName);
                ret = (int)myEnum;
            }
            catch(Exception e) { errorMessage("'" + dicomTagName + "' non è un tag DICOM"); }
            return ret;
        }

    }
}
