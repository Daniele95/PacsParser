using rzdcxLib;
using System;
using System.IO;

namespace PacsParser
{
    class MoveSCU
    {
        Association ass;

        public MoveSCU(Association ass )
        {
            this.ass = ass;
        }

        public void issueMoveCommand(DCXOBJ moveQuery)
        {
            DCXREQ requester = new DCXREQ();
            requester.OnMoveResponseRecievedEx += new IDCXREQEvents_OnMoveResponseRecievedExEventHandler(req_OnMoveResponseRecievedEx);

            // Create an accepter to handle the incomming association
            DCXACC accepter = new DCXACC();
            accepter.StoreDirectory = "C:/Users/daniele/Desktop/moveAndStore";
            Directory.CreateDirectory(accepter.StoreDirectory);

            // Create a requester and run the query
            requester.MoveAndStore(
                ass.myAET,          // The AE title that issue the C-MOVE
                ass.TargetAET,      // The PACS AE title
                ass.TargetIp,       // The PACS IP address
                ass.TargetPort,     // The PACS listener port
                ass.myAET,          // The AE title to send the
                moveQuery,              // The matching criteria
                ass.myPort,         // The port to receive the results
                accepter);          // The accepter to handle the results
        }

        void req_OnMoveResponseRecievedEx(
           ushort status,
           ushort remaining,
           ushort completed,
           ushort failed,
           ushort warning)
        {
            Console.Write("pronto a muovere");
        }
        
    }
}
