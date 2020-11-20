using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using CoreGraphics;
using System.Collections.Generic;

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
            
            UIScreen main = UIScreen.MainScreen;
            nfloat w = main.Bounds.Size.Width;
            nfloat h = main.Bounds.Size.Height;
            Globals.DataTypes.PopulateData();
            View.BackgroundColor = Globals.Colors.Backdrop;

            var appLbl = new UILabel();
            appLbl.Frame = new CGRect(w * .10, h * .05, w * .80, h * .15);
            appLbl.Text = "Midlakes Cleaner Log";
            appLbl.TextAlignment = UITextAlignment.Center;
            appLbl.Font = Globals.SizeLabelToRect(appLbl);
            View.AddSubview(appLbl);

            var submitButton = UIButton.FromType(UIButtonType.RoundedRect);
            submitButton.Frame = new CGRect(w*.10, h*.3, w*.80, h*.15);
            submitButton.SetTitle("Start Cleaning", UIControlState.Normal);
            submitButton.BackgroundColor = UIColor.White;
            submitButton.Layer.CornerRadius = 5f;
            submitButton.Font = Globals.SizeLabelToRect(submitButton);
            RoomSelectView RSV = new RoomSelectView();
            RSV.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            submitButton.TouchUpInside += delegate 
            {
                Console.WriteLine("Submit button pressed");
                PresentViewController(RSV, true, null);
            };
            View.AddSubview(submitButton);

            var userButton = UIButton.FromType(UIButtonType.RoundedRect);
            userButton.Frame = new CGRect(w * .10, h * .5, w * .80, h * .15);
            userButton.SetTitle("Select User", UIControlState.Normal);
            userButton.BackgroundColor = UIColor.White;
            userButton.Layer.CornerRadius = 5f;
            userButton.Font = Globals.SizeLabelToRect(userButton);
            UserSelectView USV = new UserSelectView();
            USV.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            userButton.TouchUpInside += delegate
            {
                Console.WriteLine("Select user button pressed");
                PresentViewController(USV, true, null);
            };
            View.AddSubview(userButton);

            Console.WriteLine("Bounds returned: " + w + " H: " + h);
            // Perform any additional setup after loading the view
        }
    }
}