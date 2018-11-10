using static PacsParser.Utilities;

namespace PacsParser
{
    class Startup
    {
        
        public static void Main()
        {
            QuerySettings findQuery = new QuerySettings();
            findQuery.findAllUsers();

            logOutput("------------------------------------------------------------------");

           /* string studyInstanceUID = "1.3.6.1.4.1.5962.99.1.2786334768.1849416866.1385765836848.3.0";
            QuerySettings moveQuery = new QuerySettings();
            moveQuery.downloadStudy(studyInstanceUID);
            */
        }
        
    }

}