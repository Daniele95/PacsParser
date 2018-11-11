using rzdcxLib;
using System;
using System.Collections.Generic;
using System.Windows;

namespace PacsParser
{

    // object with the event
    public class Publisher
    {
        // declare event handler
        public delegate void EventHandler(Object sender, RaiseArgs e);
        // declare event
        public static event EventHandler Event;
        // raise event, result: "hello"
        public void RaiseEvent(int a)
        {
            RaiseArgs args = new RaiseArgs();
            args.Message = a.ToString();
            Event(this, args);
        }
    }

    // object that listens for the event
    public interface Listener
    {
        // subscribe to event
         void Subscribe(Publisher publisher);
            

    }

    // the arguments to use with an event
    public class RaiseArgs : EventArgs
    {
        public string Message { get; set; }
    }

    public class Utilities
    {

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
            catch(Exception) { errorMessage("Al numero " + dicomTagNumber + " non corrisponde alcun tag DICOM"); }
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
            catch(Exception) { errorMessage("'" + dicomTagName + "' non è un tag DICOM"); }
            return ret;
        }

        public static string toString(List<string> lista)
        {
            string sumOfResults = "";
            foreach (string result in lista)
            {
                sumOfResults += result + "\n";
            }
            return breakLines(sumOfResults);
        }



    }
}
