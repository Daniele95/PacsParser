using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace PacsParser
{

    class Startup : User
    {
        public override void HeardEvent(object sender, string s)
        {

        }
        public void ThreadProc()
        {

            SearchPage mainWindow = new SearchPage();
            mainWindow.Show();
            Dispatcher.Run();
        }
        
        public void init()
        {
            Thread t = new Thread(ThreadProc);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }
        
        public static void Main()
            {
                Startup a = new Startup();

                a.init();
        }

        }


    }
