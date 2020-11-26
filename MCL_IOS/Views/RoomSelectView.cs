using System;
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

            UIScreen main = UIScreen.MainScreen;
            nfloat w = main.Bounds.Size.Width;
            nfloat h = main.Bounds.Size.Height;
            View.Frame = new CGRect(0, 0, w, h);
            CleanView CV = new CleanView();
            CV.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

            UIButton GoBack = UIButton.FromType(UIButtonType.RoundedRect);
            GoBack.Frame = new CGRect(w * .40, h * .02, w * .20, h * .08);
            GoBack.SetTitle("Back a menu", UIControlState.Normal);
            GoBack.BackgroundColor = UIColor.White;
            GoBack.Layer.CornerRadius = 5f;
            GoBack.Font = Globals.SizeLabelToRect(GoBack);
            GoBack.TouchUpInside += delegate
            {
                DismissViewController(true, null);
            };
            View.AddSubview(GoBack);

            UILabel headerLabel = new UILabel();
            headerLabel.Frame = new CGRect(w * .10, h * .12, w * .80, h * .08);
            headerLabel.Text = "Please select a room to clean.";
            headerLabel.BackgroundColor = Globals.Colors.Backdrop;
            headerLabel.Font = Globals.SizeLabelToRect(headerLabel);
            View.AddSubview(headerLabel);

            if (Globals.ActiveUser.RemainingRooms.Count > 0)
            {
                int rCount = 0;

                if (Globals.ActiveUser.RemainingRooms.Count > 6)
                {
                    rCount = 6;
                }
                else
                {
                    rCount = Globals.ActiveUser.RemainingRooms.Count;
                }

                for (int i = 0; i < rCount; i++)
                {
                    var btn = UIButton.FromType(UIButtonType.RoundedRect);
                    btn.Frame = new CGRect(w * .10, (h * .22) + (i * (h * .12)), w * .80, h * .1);
                    btn.SetTitle("ROOM " + Globals.ActiveUser.RemainingRooms[i], UIControlState.Normal);
                    btn.BackgroundColor = UIColor.White;
                    btn.Layer.CornerRadius = 5f;
                    btn.Font = Globals.SizeLabelToRect(btn);
                    int index = i;
                    btn.TouchUpInside += delegate
                    {
                        Console.WriteLine("user button pressed");
                        foreach (Globals.DataTypes.Room room in Globals.DataTypes.Rooms.rooms)
                        {
                            if (room.rid.Equals(Globals.ActiveUser.RemainingRooms[index]))
                            {
                                Globals.ActiveRoom = room;
                            }
                        }
                    };
                    View.AddSubview(btn);
                }
            }
            else
            {
                UILabel doneLabel = new UILabel();
                doneLabel.Frame = new CGRect(w * .10, h * .45, w * .80, h * .1);
                doneLabel.Text = "You have completed all rooms today.";
                doneLabel.BackgroundColor = Globals.Colors.Backdrop;
                doneLabel.Font = Globals.SizeLabelToRect(doneLabel);
                View.AddSubview(doneLabel);
            }
        }
    }
}