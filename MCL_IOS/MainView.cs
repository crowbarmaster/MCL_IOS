using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using CoreGraphics;

namespace IOS_MCL
{
    public class MainView : UIViewController
    {
        public MainView()
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

            SimpleBackgroundTransferViewController urlHandle = new SimpleBackgroundTransferViewController();
            urlHandle.Start();
            
            UIScreen main = UIScreen.MainScreen;
            nfloat w = main.Bounds.Size.Width;
            nfloat h = main.Bounds.Size.Height;

            View.BackgroundColor = new UIColor(140 / 255, 200 / 255, 1, 1);

            var appLbl = new UILabel();
            appLbl.Text = "Midlakes Cleaner Log";
            appLbl.TextAlignment = UITextAlignment.Center;
            appLbl.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            appLbl.MinimumScaleFactor = 10;
            appLbl.Font = UIFont.FromName("GillSans", 42f);
            appLbl.Frame = new CGRect(-(w/2), h/28, w, h/14);
            View.AddSubview(appLbl);

            var submitButton = UIButton.FromType(UIButtonType.RoundedRect);
            submitButton.Frame = new CGRect(w/32, (h/2) - (h/12), w - (w/16), h/16);
            submitButton.SetTitle("Start Cleaning", UIControlState.Normal);
            submitButton.BackgroundColor = UIColor.White;
            submitButton.Layer.CornerRadius = 5f;
            RoomSelectView RSV = new RoomSelectView();
            RSV.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            submitButton.TouchUpInside += delegate 
            {
                Console.WriteLine("Submit button pressed");
                PresentViewController(RSV, true, null);
            };
            View.AddSubview(submitButton);

            var userButton = UIButton.FromType(UIButtonType.RoundedRect);
            userButton.Frame = new CGRect(w/32, (h / 2) + (h/12), w - (w / 16), h/16);
            userButton.SetTitle("Select User", UIControlState.Normal);
            userButton.BackgroundColor = UIColor.White;
            userButton.Layer.CornerRadius = 5f;
            UserSelectView USV = new UserSelectView();
            USV.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            userButton.TouchUpInside += delegate
            {
                Console.WriteLine("Select user button pressed");
                PresentViewController(new UserSelectView(), true, null);
            };
            View.AddSubview(userButton);

            Console.WriteLine("Bounds returned: " + w + " H: " + h);
            // Perform any additional setup after loading the view
        }
    }
}