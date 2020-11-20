using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;

namespace IOS_MCL
{
    [Register("CamUniversalView")]
    public class CamUniversalView : UIView
    {
        public CamUniversalView()
        {
            Initialize();
        }

        public CamUniversalView(RectangleF bounds) : base(bounds)
        {
            Initialize();
        }

        void Initialize()
        {
            BackgroundColor = UIColor.Red;
        }
    }

    [Register("CamView")]
    public class CamView : UIViewController
    {
        public CamView()
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
            View = new CamUniversalView();

            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }
    }
}