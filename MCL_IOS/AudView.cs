using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;

namespace IOS_MCL
{
    [Register("AudUniversalView")]
    public class AudUniversalView : UIView
    {
        public AudUniversalView()
        {
            Initialize();
        }

        public AudUniversalView(RectangleF bounds) : base(bounds)
        {
            Initialize();
        }

        void Initialize()
        {
            BackgroundColor = UIColor.Red;
        }
    }

    [Register("AudView")]
    public class AudView : UIViewController
    {
        public AudView()
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
            View = new AudUniversalView();

            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }
    }
}