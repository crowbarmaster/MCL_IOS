using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;

namespace IOS_MCL
{
    [Register("CleanUniversalView")]
    public class CleanUniversalView : UIView
    {
        public CleanUniversalView()
        {
            Initialize();
        }

        public CleanUniversalView(RectangleF bounds) : base(bounds)
        {
            Initialize();
        }

        void Initialize()
        {
            BackgroundColor = UIColor.Red;
        }
    }

    [Register("UIViewController1")]
    public class CleanView : UIViewController
    {
        public CleanView()
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
            View = new CleanUniversalView();

            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }
    }
}