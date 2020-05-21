using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CSACoreWin.Help {

    public class TutorialSystem {
        //================================================================================
        private Tutorial                        mTutorial;


        // TUTORIAL ================================================================================
        //--------------------------------------------------------------------------------
        public void Update() {
            if (mTutorial != null)
                mTutorial.Update();
        }


        // EVENT NOTIFICATION ================================================================================
        //--------------------------------------------------------------------------------
        public void NotifyEvent(string identifier, params object[] data) {
            if (mTutorial != null)
                mTutorial.NotifyEvent(identifier, data);
        }


        // TUTORIAL ================================================================================
        //--------------------------------------------------------------------------------
        public void StartTutorial(Tutorial tutorial, params object[] arguments) {
            StopTutorial();
            mTutorial = tutorial;
            mTutorial.TutorialSystem = this;
            mTutorial.NotifyStart(arguments);
        }

        //--------------------------------------------------------------------------------
        public void StopTutorial() {
            if (mTutorial != null) {
                mTutorial.NotifyStop();
                mTutorial = null;
            }
        }

        //--------------------------------------------------------------------------------
        public Tutorial Tutorial { set { mTutorial = value; } get { return mTutorial; } }
    }

}
