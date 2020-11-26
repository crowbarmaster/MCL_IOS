using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace IOS_MCL
{
    public static class ViewProvider
    {
        private static MainView localMV;
        private static RoomSelectView localRSV;
        private static UserSelectView localUSV;
        private static CleanView localCV;
        private static NotesView localNV;
        private static AudView localAud;
        private static CamView localCam;
        private static TxtView localTxt;

        public static MainView MainView()
        {
            if(localMV == null)
            {
                localMV = new MainView();
            }
            return localMV;
        }

        public static RoomSelectView RoomSelectView ()
        {
            if(localRSV == null)
            {
                localRSV = new RoomSelectView();
                localRSV.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            }
            return localRSV;
        }

        public static UserSelectView UserSelectView ()
        {
            if(localUSV == null)
            {
                localUSV = new UserSelectView();
                localUSV.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            }
            return localUSV;
        }

        public static CleanView CleanView (bool Renew)
        {
            if(localCV == null || Renew)
            {
                localCV = new CleanView();
                localCV.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            }
            return localCV;
        }

        public static NotesView NotesView ()
        {
            if(localNV == null)
            {
                localNV = new NotesView();
                localNV.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            }
            return localNV;
        }
    }
}