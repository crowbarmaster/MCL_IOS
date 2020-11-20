using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using CoreGraphics;

namespace IOS_MCL
{
    [Register("CleanView")]
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
            base.ViewDidLoad();

            string[] users = { "Disinfected student desks and chairs", "Disinfected teacher desk and chair", "Cleaned floor", "Handles disinfected", "Vacuumed carpet(s)", "Cleaned bathroom(s)", "Disinfected bathrooms", "Checked for sanitizer" };
            UIScreen main = UIScreen.MainScreen;
            nfloat w = main.Bounds.Size.Width;
            nfloat h = main.Bounds.Size.Height;
            View.BackgroundColor = Globals.Colors.Backdrop;
            View.Frame = new CGRect(0, 0, w, h);

            base.ViewDidLoad();

            for (double i = 0; i < users.Length; i++)
            {
                int cbIndex = (int)i;
                var cbLabel = UIButton.FromType(UIButtonType.RoundedRect);
                cbLabel.SetTitle(users[(int)i], UIControlState.Normal);
                cbLabel.Frame = new CGRect(w*.1, (h * .3) + (i * (h * 0.07)), (w * 0.8), (h * 0.06));
                cbLabel.BackgroundColor = Globals.Colors.White;
                cbLabel.Layer.CornerRadius = 5f;
                cbLabel.Font = Globals.SizeLabelToRect(cbLabel);
                cbLabel.TouchUpInside += delegate
                {
                    if(cbLabel.BackgroundColor.IsEqual(Globals.Colors.White))
                    {
                        cbLabel.BackgroundColor = Globals.Colors.Green;
                    }else
                    {
                        cbLabel.BackgroundColor = Globals.Colors.White;
                    }
                };
                View.AddSubview(cbLabel);

            }
        }
    }
}