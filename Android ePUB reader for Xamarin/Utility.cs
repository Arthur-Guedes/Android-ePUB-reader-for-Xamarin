using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Java.IO;

// Assorted utility functions
namespace Com.Dteviot.EpubViewer
{
	public class Utility {
		public static readonly string ERROR_STRING_ID_EXTRA = "ERROR_STRING_ID_EXTRA";

		public static void showToast(Context context, int stringId) {
			Toast msg = Toast.MakeText(context, stringId, ToastLength.Short);
			msg.SetGravity(GravityFlags.Center, msg.XOffset / 2, msg.XOffset / 2);
			msg.Show();
		}

		public static void finishWithError(Activity activity, int stringId) {
			Intent intent = new Intent();
			intent.PutExtra(ERROR_STRING_ID_EXTRA, stringId);
			activity.SetResult(Result.Canceled, intent);
			activity.Finish();
		}

		public static void showErrorToast(Context context, Intent intent) {
			if (intent != null) {
				int stringId = intent.GetIntExtra(ERROR_STRING_ID_EXTRA, 0);
				if (stringId != 0) {
					showToast(context, stringId);
				}
			}
		}

		// Return path part of a filename
		public static string extractPath(string fileName) {
			try {
				string path = new File(fileName).CanonicalFile.Parent;
				// remove leading '/'
				return path == null ? "" : path.Substring(1);
			} catch (IOException e) {
				throw new Java.Lang.RuntimeException(e);
			}
		}

		public static string concatPath(string basePath, string pathToAdd) {
			string rawPath = basePath + '/' + pathToAdd;
			if ((basePath == null) || basePath.Length == 0 || pathToAdd.StartsWith("/")) {
				rawPath = pathToAdd;
			}
			try {
				return new File(rawPath).CanonicalPath.Substring(1);
			} catch (IOException e) {
				throw new Java.Lang.RuntimeException(e);
			}
		}
	}
}
