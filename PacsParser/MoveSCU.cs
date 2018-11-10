using rzdcxLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using static PacsParser.Utilities;

namespace PacsParser
{
    class MoveSCU : SCU
    {
        DCXACC accepter = new DCXACC();

        StoreSCP listener = new StoreSCP();

        public MoveSCU() : base()
        {
            accepter.StoreDirectory = "C:/Users/daniele/Desktop/moveAndStore";
            Directory.CreateDirectory(accepter.StoreDirectory);

        }

        public void startListening(Association serverino)
        {
            // qui accendo il listener
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                listener.startListening(serverino);
            }).Start();
        }

        public override void printResults(){ }
        public override bool tryReadResults() { return true; }

        public override void serverConnection(DCXREQ req, Association serverino, DCXOBJ moveQuery)
        {
            req.MoveAndStore(
                serverino.myAET,          // The AE title that issue the C-MOVE
                serverino.TargetAET,      // The PACS AE title
                serverino.TargetIp,       // The PACS IP address
                serverino.TargetPort,     // The PACS listener port
                serverino.myAET,          // The AE title to send the
                moveQuery,              // The matching criteria
                serverino.myPort,         // The port to receive the results
                accepter);          // The accepter to handle the results
        }

        public override void setCallbackDelegate( DCXREQ req)
        {
            req.OnMoveResponseRecievedEx += new IDCXREQEvents_OnMoveResponseRecievedExEventHandler(OnMoveResponseRecievedEx);
        }

        void OnMoveResponseRecievedEx(
           ushort status,
           ushort remaining,
           ushort completed,
           ushort failed,
           ushort warning)
        {
            logOutput("eccezione");
        }

    }
}
