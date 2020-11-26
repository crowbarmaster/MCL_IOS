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

            string[] Tasks = { "Disinfected student desks and chairs", "Disinfected teacher desk and chair", "Cleaned floor", "Handles disinfected", "Vacuumed carpet(s)", "Cleaned bathroom(s)", "Disinfected bathrooms", "Checked for sanitizer" };
            char[] TaskData = new char[Tasks.Length];
            for (int i = 0; i < TaskData.Length; i++)
            {
                TaskData[i] = '0';
            }

            if (Globals.ActiveRecord != null && Globals.ActiveRecord.lastdata != "NA")
            {
                char[] tmp = Globals.ActiveRecord.lastdata.ToCharArray();
                if (tmp.Length.Equals(TaskData.Length))
                {
                    TaskData = Globals.ActiveRecord.lastdata.ToCharArray();
                }
                else
                {
                    Console.WriteLine("ERROR! CleanView: Data string size vs tasks array size mismatch!!");
                }
            }
            else
            {
                Globals.ActiveRecord = new Globals.DataTypes.PendingRecord();
                Globals.ActiveRecord.userid = Globals.ActiveUser.uid;
                Globals.ActiveRecord.date = Globals.GetDate();
                Globals.ActiveRecord.data = Globals.ConcatRecords();
                Globals.ActiveRecord.lastroom = Globals.ActiveRoom.rid;
                Globals.ActiveRecord.lastdata = new string(TaskData);
            }
            Console.WriteLine("Convert.ToString(TaskData) returned: " + new string(TaskData));
            UIScreen main = UIScreen.MainScreen;
            nfloat w = main.Bounds.Size.Width;
            nfloat h = main.Bounds.Size.Height;
            View.BackgroundColor = Globals.Colors.Backdrop;
            View.Frame = new CGRect(0, 0, w, h);

            base.ViewDidLoad();

            UIButton GoBack = UIButton.FromType(UIButtonType.RoundedRect);
            GoBack.Frame = new CGRect(w * .10, h * .02, w * .20, h * .08);
            GoBack.SetTitle("Take a note", UIControlState.Normal);
            GoBack.BackgroundColor = UIColor.White;
            GoBack.Layer.CornerRadius = 5f;
            GoBack.Font = Globals.SizeLabelToRect(GoBack);
            GoBack.TouchUpInside += delegate
            {
                InvokeInBackground(async delegate
                {
                    Globals.ActiveRecord.lastdata = new string(TaskData);
                    await Globals.DataTypes.UpdateTempRecord(Globals.ActiveRecord);
                });
            };
            View.AddSubview(GoBack);

            UIButton Notes = UIButton.FromType(UIButtonType.RoundedRect);
            Notes.Frame = new CGRect(w * .40, h * .02, w * .20, h * .08);
            Notes.SetTitle("Back a menu", UIControlState.Normal);
            Notes.BackgroundColor = UIColor.White;
            Notes.Layer.CornerRadius = 5f;
            Notes.Font = Globals.SizeLabelToRect(Notes);
            Notes.TouchUpInside += delegate
            {
                DismissViewController(true, null);
            };
            View.AddSubview(Notes);

            UIButton Final = UIButton.FromType(UIButtonType.RoundedRect);
            Final.Frame = new CGRect(w * .70, h * .02, w * .20, h * .08);
            Final.SetTitle("Finalize", UIControlState.Normal);
            Final.BackgroundColor = UIColor.White;
            Final.Layer.CornerRadius = 5f;
            Final.Font = Globals.SizeLabelToRect(Final);
            Final.TouchUpInside += delegate
            {
                InvokeInBackground(async delegate
                {
                    await Globals.DataTypes.FinalizeRecord(Globals.ActiveRecord);
                    Globals.ActiveRoom = null;
                    if (Globals.SkippedRoomSelect)
                    {
                        InvokeOnMainThread(delegate
                        {
                            Globals.SkippedRoomSelect = false;
                            DismissViewController(false, delegate
                            {
                                PresentViewController(ViewProvider.RoomSelectView(), true, null);
                            });

                        });
                    }
                    else
                    {
                        InvokeOnMainThread(delegate
                        {
                            DismissViewController(true, null);
                        });
                    }
                });
            };
            View.AddSubview(Final);

            for (int i = 0; i < Tasks.Length; i++)
            {
                int cbIndex = i;
                var cbLabel = UIButton.FromType(UIButtonType.RoundedRect);
                cbLabel.SetTitle(Tasks[i], UIControlState.Normal);
                cbLabel.Frame = new CGRect(w*.1, (h * .3) + (i * (h * 0.07)), (w * 0.8), (h * 0.06));
                if (TaskData[i].Equals('0'))
                {
                    cbLabel.BackgroundColor = Globals.Colors.White;
                }
                else
                {
                    cbLabel.BackgroundColor = Globals.Colors.Green;
                }
                cbLabel.Layer.CornerRadius = 5f;
                cbLabel.Font = Globals.SizeLabelToRect(cbLabel);
                cbLabel.TouchUpInside += delegate
                {
                    if(cbLabel.BackgroundColor.IsEqual(Globals.Colors.White))
                    {
                        cbLabel.BackgroundColor = Globals.Colors.Green;
                        TaskData[cbIndex] = '1';
                    }else
                    {
                        cbLabel.BackgroundColor = Globals.Colors.White;
                        TaskData[cbIndex] = '0';
                    }
                };
                View.AddSubview(cbLabel);

            }
        }
    }
}