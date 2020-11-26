using System;

using UIKit;
using Foundation;
using CoreGraphics;
using System.Collections.Generic;

using static IOS_MCL.Globals;

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
            View.BackgroundColor = Colors.Backdrop;
           
            UIScreen main = UIScreen.MainScreen;
            double w = main.Bounds.Size.Width;
            double h = main.Bounds.Size.Height;

            base.ViewDidLoad();

            UIButton GoBack = UIButton.FromType(UIButtonType.RoundedRect);
            GoBack.Frame = new CGRect(w * .40, h * .02, w * .20, h * .08);
            GoBack.SetTitle("Back a menu", UIControlState.Normal);
            GoBack.BackgroundColor = UIColor.White;
            GoBack.Layer.CornerRadius = 5f;
            GoBack.Font = SizeLabelToRect(GoBack);
            GoBack.TouchUpInside += delegate
            {
                DismissViewController(true, null);
            };
            View.AddSubview(GoBack); 

            UILabel Title = new UILabel();
            Title.Frame = new CGRect(w * .10, h * .1, w * .80, h * .1);
            Title.Text = "Select your name:";
            Title.Font = SizeLabelToRect(Title);
            Title.TextAlignment = UITextAlignment.Center;
            View.AddSubview(Title);

            for (int i = 0; i< DataTypes.Users.users.Count; i++)
            {
                var userBtn = UIButton.FromType(UIButtonType.RoundedRect);
                userBtn.Frame = new CGRect(w * .10, (h * .2) + (i * (h * .11)), w * .80, h * .1);
                userBtn.SetTitle(DataTypes.Users.users[i].fullname, UIControlState.Normal);
                userBtn.BackgroundColor = UIColor.White;
                userBtn.Layer.CornerRadius = 5f;
                userBtn.Font = SizeLabelToRect(userBtn);
                int index = i;
                userBtn.TouchUpInside += delegate {
                    if (ActiveUser == null)
                    {
                        ActiveUser = DataTypes.Users.users[index];
                        ActiveUser.CompletedRooms = new Dictionary<string, List<string>>();
                        ActiveUser.RemainingRooms = new List<string>();
                    }
                    else
                    {
                        ActiveRoom = null;
                        ActiveRecord = null;
                        ActiveUser = DataTypes.Users.users[index];
                        ActiveUser.CompletedRooms = new Dictionary<string, List<string>>();
                        ActiveUser.RemainingRooms = new List<string>();
                    }
                    MainView.infoLabel.Text = "Welcome, " + ActiveUser.fullname + ". You are logged in.";
                    MainView.infoLabel.Font = SizeLabelToRect(MainView.infoLabel);
                    foreach (string AssignedRoom in ActiveUser.rooms.Split(';'))
                    {
                        ActiveUser.RemainingRooms.Add(AssignedRoom);
                    }
                    if (ActiveUser != null)
                    {
                        foreach (DataTypes.PendingRecord rec in DataTypes.Records.pending_records)
                        {
                            if (ActiveUser.uid.Equals(rec.userid))
                            {
                                ActiveRecord = rec;
                                if (!rec.data.Equals("0"))
                                {
                                    string[] ExplodeRecords = rec.data.Split('>');
                                    foreach (string expRoom in ExplodeRecords)
                                    {
                                        string[] ExplodeRoom = expRoom.Split(';');
                                        ActiveUser.CompletedRooms.Add(ExplodeRoom[0], new List<string>(ExplodeRoom));
                                        if (ActiveUser.RemainingRooms.Contains(ExplodeRoom[0]))
                                        {
                                            ActiveUser.RemainingRooms.Remove(ExplodeRoom[0]);
                                        }
                                    }
                                }
                                if (rec.lastroom != "NA")
                                {
                                    foreach(DataTypes.Room room in DataTypes.Rooms.rooms)
                                    {
                                        if(room.rid == rec.lastroom)
                                        {
                                            ActiveRoom = room;
                                        }
                                    }
                                    ActiveUser.RemainingRooms.Remove(rec.lastroom);
                                }
                            }
                        }
                        foreach(string room in ActiveUser.CompletedRooms.Keys)
                        {
                            Console.WriteLine("Completed room added. ID: " + room);
                        }
                        foreach(string room in ActiveUser.RemainingRooms)
                        {
                            Console.WriteLine("Remaining room added. ID: " + room);
                        }
                    }
                    if (ActiveRecord != null)
                    {
                        Console.WriteLine("User " + ActiveUser.fullname + " Selected. Has record with date of: " + ActiveRecord.date);
                    }
                    else
                    {
                        Console.WriteLine("User " + ActiveUser.fullname + " Selected. Does not have record on file.");
                    }
                    DismissViewController(true, null);
                };
                View.AddSubview(userBtn);
            }

            // Perform any additional setup after loading the view
        }
    }
}