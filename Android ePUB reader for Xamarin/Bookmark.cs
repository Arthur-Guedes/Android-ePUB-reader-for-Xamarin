using Android.Content;
using Android.Net;
using Android.OS;

namespace Com.Dteviot.EpubViewer
{
	public class Bookmark {
		public static readonly string PREFS_NAME = "EpubViewerPrefsFile";
		public static readonly string PREFS_EPUB_NAME = "FileName";
		public static readonly string PREFS_RESOURCE_URI = "ResourceUri";
		public static readonly string PREFS_SCROLLY = "ScrollY";

		public string mFileName;
		public Uri mResourceUri;
		public float mScrollY;

		// Constructor
		public Bookmark(string fileName, Uri resourceUri, float scrollY) {
			mFileName = fileName;
			mResourceUri = resourceUri;
			mScrollY = scrollY;
		}
	    
		// Called when user changed screen orientation
		public Bookmark(Bundle savedInstanceState) {
			mFileName = savedInstanceState.GetString(PREFS_EPUB_NAME);
			deserializeUri(savedInstanceState.GetString(PREFS_RESOURCE_URI));
			mScrollY = savedInstanceState.GetFloat(PREFS_SCROLLY);
		}

		// Retrieve from the Shared Preferences
		public Bookmark(Context context) {
			ISharedPreferences settings = context.GetSharedPreferences(PREFS_NAME, FileCreationMode.Private);
			mFileName = settings.GetString(PREFS_EPUB_NAME, "");
			deserializeUri(settings.GetString(PREFS_RESOURCE_URI, ""));
			mScrollY = settings.GetFloat(PREFS_SCROLLY, 0.0f);
		}

		private void deserializeUri(string uri) {
			if ((uri != null) && uri.Length != 0) {
				mResourceUri = Uri.Parse(uri);
			}
		}
	    
		// return true if bookmark is "empty", i.e. doesn't hold a useful value
		public bool isEmpty() {
			return ((mFileName == null) || (mFileName.Length <= 0) || (mResourceUri == null));
		}

		// Write the bookmark into a bundle (normally used when screen orientation changing)
		public void save(Bundle outState) {
			if (!isEmpty()) {
				outState.PutString(PREFS_EPUB_NAME, mFileName);
				outState.PutString(PREFS_RESOURCE_URI, mResourceUri.ToString());
				outState.PutFloat(PREFS_SCROLLY, mScrollY);
			}
		}

		// Write to persistent storage
		public void saveToSharedPreferences(Context context) {
			if (!isEmpty()) {
				ISharedPreferences settings = context.GetSharedPreferences(PREFS_NAME, FileCreationMode.Private);
				ISharedPreferencesEditor editor = settings.Edit();
				editor.PutString(PREFS_EPUB_NAME, mFileName);
				editor.PutString(PREFS_RESOURCE_URI, mResourceUri.ToString());
				editor.PutFloat(PREFS_SCROLLY, mScrollY);
				editor.Commit();
			}
		}

		// The epub that has been bookmarked
		public string getFileName() {
			return mFileName;
		}

		// Chapter of book
		public Uri getResourceUri() {
			return mResourceUri;
		}

		// Position of chapter
		public float getScrollY() {
			return mScrollY;
		}
	}
}
