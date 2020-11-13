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

            string[] users = { "test", "test2", "test3", "test4" };
            UIScreen main = UIScreen.MainScreen;
            nfloat w = main.Bounds.Size.Width;
            nfloat h = main.Bounds.Size.Height;
            View.BackgroundColor = new UIColor(140 / 255, 200 / 255, 1, 1);
            View.Frame = new CGRect(0, 0, w, h);

            base.ViewDidLoad();

            for (double i = 0; i < users.Length; i++)
            {
                var checkBox = new UISwitch();
                checkBox.Frame = new CGRect((nfloat)(w*0.03), (h * .5) - (i * (h * 0.055)), (w * 0.3), (h*0.05));
                checkBox.BackgroundColor = new UIColor(140 / 255, 200 / 255, 1, 1);
                checkBox.Layer.CornerRadius = 5f;
                int cbIndex = (int)i;
                checkBox.ValueChanged += delegate
                {
                    Console.WriteLine("Slider value changed on index: " + cbIndex);
                };

                var cbLabel = new UILabel();
                cbLabel.Text = users[(int)i];
                cbLabel.Frame = new CGRect(60, -(h*.015), (w * 0.3), (h * 0.04));
                cbLabel.BackgroundColor = new UIColor(140 / 255, 200 / 255, 1, 1);
                cbLabel.Layer.CornerRadius = 5f;
                cbLabel.Font = cbLabel.Font.WithSize((nfloat)(w * 0.05));
                checkBox.AddSubview(cbLabel);
                View.AddSubview(checkBox);

            }
        }
    }
}