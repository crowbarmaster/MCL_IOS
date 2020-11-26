using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using CoreGraphics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IOS_MCL
{
    public class MainView : UIViewController
    {
        public static UIButton userButton;
        public static UILabel infoLabel;
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
            Task.Run(async delegate
            {
                await Globals.DataTypes.PopulateData();
            });

            View.BackgroundColor = Globals.Colors.Backdrop;

            var appLbl = new UILabel();
            appLbl.Frame = new CGRect(w * .10, h * .05, w * .80, h * .15);
            appLbl.Text = "Midlakes Cleaner Log";
            appLbl.TextAlignment = UITextAlignment.Center;
            appLbl.Font = Globals.SizeLabelToRect(appLbl);
            View.AddSubview(appLbl);

            userButton = UIButton.FromType(UIButtonType.RoundedRect);
            userButton.Frame = new CGRect(w * .10, h * .32, w * .80, h * .10);
            userButton.SetTitle("Select User", UIControlState.Normal);
            userButton.BackgroundColor = UIColor.White;
            userButton.Layer.CornerRadius = 5f;
            userButton.Font = Globals.SizeLabelToRect(userButton);
            userButton.TouchUpInside += delegate
            {
                Console.WriteLine("Select user button pressed");
                PresentViewController(ViewProvider.UserSelectView(), true, null);
            };
            View.AddSubview(userButton);

            var submitButton = UIButton.FromType(UIButtonType.RoundedRect);
            submitButton.Frame = new CGRect(w*.10, h*.2, w*.80, h*.10);
            submitButton.SetTitle("Start Cleaning", UIControlState.Normal);
            submitButton.BackgroundColor = Globals.Colors.White;
            submitButton.Layer.CornerRadius = 5f;
            submitButton.Font = Globals.SizeLabelToRect(submitButton);
            submitButton.TouchUpInside += delegate 
            {
                if (Globals.ActiveUser != null)
                {
                    if (Globals.ActiveRoom != null)
                    {
                        Console.WriteLine("Submit button pressed");
                        Globals.SkippedRoomSelect = true;
                        PresentViewController(ViewProvider.CleanView(true), true, null);
                    }
                    else
                    {
                        Console.WriteLine("Submit button pressed");
                        PresentViewController(ViewProvider.RoomSelectView(), true, null);
                    }
                }
                else
                {
                    UIViewPropertyAnimator.CreateRunningPropertyAnimator(.2, 0, UIViewAnimationOptions.CurveEaseOut, delegate { userButton.BackgroundColor = Globals.Colors.Red; }, delegate { UIViewPropertyAnimator.CreateRunningPropertyAnimator(.2, 0, UIViewAnimationOptions.CurveEaseOut, delegate { userButton.BackgroundColor = Globals.Colors.White; }, delegate { UIViewPropertyAnimator.CreateRunningPropertyAnimator(.2, 0, UIViewAnimationOptions.CurveEaseOut, delegate { userButton.BackgroundColor = Globals.Colors.Red; }, delegate { UIViewPropertyAnimator.CreateRunningPropertyAnimator(.2, 0, UIViewAnimationOptions.CurveEaseOut, delegate { userButton.BackgroundColor = Globals.Colors.White; }, null).StartAnimation(); }).StartAnimation(); }).StartAnimation(); }).StartAnimation();
                }
            };
            View.AddSubview(submitButton);

            infoLabel = new UILabel();
            infoLabel.Frame = new CGRect(w * .10, h * .5, w * .80, h * .15);
            infoLabel.Text = "You are not logged in. Please proceed to the User selection screen and login.";
            infoLabel.TextAlignment = UITextAlignment.Center;
            infoLabel.Font = Globals.SizeLabelToRect(infoLabel);
            View.AddSubview(infoLabel);

            Console.WriteLine("Bounds returned: " + w + " H: " + h);
            // Perform any additional setup after loading the view
        }
    }
}