namespace PacsParser
{
    class Startup
    {
        
        public static void Main()
        {
            QuerySettings query = new QuerySettings();
            
            query.downloadStudy(query.findStudyOfUser(query.findPatientID("Doe^Pierre")));

        }

    }

}