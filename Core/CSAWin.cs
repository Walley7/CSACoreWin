using CSACoreWin.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CSACoreWin.Core {

    public static class CSAWin {
        //================================================================================
        private static ApplicationData          sApplicationData;

        private static TutorialSystem           sTutorialSystem = new TutorialSystem();


        //================================================================================
        //--------------------------------------------------------------------------------
        public static void Initialise(string applicationDataCompanyName = "", string applicationDataProgramApplicationName = "") {
            // Application data
            if (!string.IsNullOrWhiteSpace(applicationDataCompanyName) && !string.IsNullOrWhiteSpace(applicationDataProgramApplicationName))
                sApplicationData = new ApplicationData(applicationDataCompanyName, applicationDataProgramApplicationName);
        }

        //--------------------------------------------------------------------------------
        public static void Shutdown() { }


        // APPLICATION DATA ================================================================================
        //--------------------------------------------------------------------------------
        public static ApplicationData ApplicationData { get { return sApplicationData; } }


        // TUTORIAL SYSTEM ================================================================================
        //--------------------------------------------------------------------------------
        public static TutorialSystem TutorialSystem { get { return sTutorialSystem; } }
    }

}
