using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace IOS_MCL
{
	public partial class SimpleBackgroundTransferViewController : UIViewController
	{

		const string Identifier = "com.SimpleBackgroundTransfer.BackgroundSession";
		const string DownloadUrlString = "https://upload.wikimedia.org/wikipedia/commons/9/97/The_Earth_seen_from_Apollo_17.jpg";

		public NSUrlSessionDownloadTask downloadTask;
		public NSUrlSession session;

		public SimpleBackgroundTransferViewController()
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Start();
		}

		public void Start()
		{
			if (downloadTask != null)
				return;
			if (session == null)
            {
				session = InitBackgroundSession();
            }
				

			using (var url = NSUrl.FromString(DownloadUrlString))
			using (var request = NSUrlRequest.FromUrl(url))
			{
				downloadTask = session.CreateDownloadTask(request);
				downloadTask.Resume();
			}

		}

		public NSUrlSession InitBackgroundSession()
		{
			Console.WriteLine("InitBackgroundSession");
			using (var configuration = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration(Identifier))
			{
				return NSUrlSession.FromConfiguration(configuration, new UrlSessionDelegate(this), null);
			}
		}
	}

	public class UrlSessionDelegate : NSObject, INSUrlSessionDownloadDelegate
	{
		public SimpleBackgroundTransferViewController controller;

		public UrlSessionDelegate(SimpleBackgroundTransferViewController controller)
		{
			this.controller = controller;
		}

		public void DidWriteData(NSUrlSession session, NSUrlSessionDownloadTask downloadTask, long bytesWritten, long totalBytesWritten, long totalBytesExpectedToWrite)
		{
			Console.WriteLine("Set Progress");
			if (downloadTask == controller.downloadTask)
			{
				float progress = totalBytesWritten / (float)totalBytesExpectedToWrite;
				Console.WriteLine(string.Format("DownloadTask: {0}  progress: {1}", downloadTask, progress));
			}
		}

		public void DidFinishDownloading(NSUrlSession session, NSUrlSessionDownloadTask downloadTask, NSUrl location)
		{
			Console.WriteLine("Finished");
			Console.WriteLine("File downloaded in : {0}", location);
			NSFileManager fileManager = NSFileManager.DefaultManager;

			var URLs = fileManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User);
			NSUrl documentsDictionry = URLs[0];

			NSUrl originalURL = downloadTask.OriginalRequest.Url;
			NSUrl destinationURL = documentsDictionry.Append("image1.png", false);
			NSError removeCopy;
			NSError errorCopy;

			fileManager.Remove(destinationURL, out removeCopy);
			bool success = fileManager.Copy(location, destinationURL, out errorCopy);

			if (success)
			{
				// we do not need to be on the main/UI thread to load the UIImage
			}
			else
			{
				Console.WriteLine("Error during the copy: {0}", errorCopy.LocalizedDescription);
			}
		}

		public void DidCompleteWithError(NSUrlSession session, NSUrlSessionTask task, NSError error)
		{
			Console.WriteLine("DidComplete");
			if (error == null)
				Console.WriteLine("Task: {0} completed successfully", task);
			else
				Console.WriteLine("Task: {0} completed with error: {1}", task, error.LocalizedDescription);

			controller.downloadTask = null;
		}

		public void DidResume(NSUrlSession session, NSUrlSessionDownloadTask downloadTask, long resumeFileOffset, long expectedTotalBytes)
		{
			Console.WriteLine("DidResume");
		}

		public void DidFinishEventsForBackgroundSession(NSUrlSession session)
		{
			using (AppDelegate appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate)
			{
				var handler = appDelegate.BackgroundSessionCompletionHandler;
				if (handler != null)
				{
					appDelegate.BackgroundSessionCompletionHandler = null;
					handler();
				}
			}

			Console.WriteLine("All tasks are finished");
		}
	}
}
