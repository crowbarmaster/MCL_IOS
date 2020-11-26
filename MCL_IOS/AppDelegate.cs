using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace IOS_MCL
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }
        public Action BackgroundSessionCompletionHandler { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // create a new window instance based on the screen size
            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            var controller = ViewProvider.MainView();

            Window.RootViewController = controller;

            // make the window visible
            Window.MakeKeyAndVisible();

            return true;
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background execution this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transition from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }

        public override void HandleEventsForBackgroundUrl(UIApplication application, string sessionIdentifier, Action completionHandler)
        {
            Console.WriteLine("HandleEventsForBackgroundUrl");
            BackgroundSessionCompletionHandler = completionHandler;
        }

        public override void OnActivated(UIApplication application)
        {
            Console.WriteLine("OnActivated");
        }

        public override void OnResignActivation(UIApplication application)
        {
            Console.WriteLine("OnResignActivation");
        }
    }
}


