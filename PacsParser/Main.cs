using System;
using System.Threading;
using System.Windows.Threading;

namespace PacsParser
{

    class Startup
    {

        [STAThread]
        public static void Main()
        {
            
            var thread = new Thread(() =>
            {
                SearchPage mainWindow = new SearchPage();
                mainWindow.Show();
                Dispatcher.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            

            //query.downloadStudy(query.findStudyOfUser(query.findPatientID("Doe^Pierre")));

        }

    }

}