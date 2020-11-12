using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;

namespace IOS_MCL
{
    [Register("UIViewController1")]
    public class RoomSelectView : UIViewController
    {
        public RoomSelectView()
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {

            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }
    }
}