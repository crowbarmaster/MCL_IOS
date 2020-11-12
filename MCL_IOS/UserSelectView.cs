using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using CoreGraphics;

namespace IOS_MCL
{
    [Register("UserSelectView")]
    public class UserSelectView : UIViewController
    {
        public UserSelectView()
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
            View.BackgroundColor = new UIColor(160 / 255, 1, 1, .5f);

            string[] users = { "test", "test2", "test3", "test4" };
            UIScreen main = UIScreen.MainScreen;
            nfloat w = main.Bounds.Size.Width;
            nfloat h = main.Bounds.Size.Height;

            base.ViewDidLoad();

            for(int i = 0; i<users.Length; i++)
            {
                var submitButton = UIButton.FromType(UIButtonType.RoundedRect);
                submitButton.Frame = new CGRect(w / 32, (h / 2) - (h / 12) + (i * 20), w - (w / 16), h / 16);
                submitButton.SetTitle(users[i], UIControlState.Normal);
                submitButton.BackgroundColor = UIColor.White;
                submitButton.Layer.CornerRadius = 5f;
                submitButton.TouchUpInside += (sender, e) => {
                    Console.WriteLine("Submit button pressed");
                };
                View.AddSubview(new UIView { submitButton });
            }

            // Perform any additional setup after loading the view
        }
    }
}