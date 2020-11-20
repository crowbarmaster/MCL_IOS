﻿using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using CoreGraphics;

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
            View.BackgroundColor = Globals.Colors.Backdrop;
            base.ViewDidLoad();

            string[] users = { "room", "room2", "room3", "room4" };
            UIScreen main = UIScreen.MainScreen;
            nfloat w = main.Bounds.Size.Width;
            nfloat h = main.Bounds.Size.Height;
            View.Frame = new CGRect(0, 0, w, h);

            base.ViewDidLoad();

            for (int i = 0; i < users.Length; i++)
            {
                var btn = UIButton.FromType(UIButtonType.RoundedRect);
                btn.Frame = new CGRect(w * .10, (h * .3) + (i * (h * .12)), w * .80, h * .1);
                btn.SetTitle(users[i], UIControlState.Normal);
                btn.BackgroundColor = UIColor.White;
                btn.Layer.CornerRadius = 5f;
                btn.Font = Globals.SizeLabelToRect(btn);
                CleanView CV = new CleanView();
                CV.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                btn.TouchUpInside += delegate
                {
                    Console.WriteLine("user button pressed");
                    ShowViewController(CV, this);
                };
                View.AddSubview(btn);
            }
        }
    }
}