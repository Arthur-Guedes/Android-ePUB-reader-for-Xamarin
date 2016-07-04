using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Views;
using Android_ePUB_reader_for_Xamarin;
using Com.Dteviot.EpubViewer.Epub;
using Com.Dteviot.EpubViewer.WebServer;

namespace Com.Dteviot.EpubViewer
{
    [Activity(Label = "ePUB reader", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity , IResourceSource {
		private const int LIST_EPUB_ACTIVITY_ID = 0;
		private const int LIST_CHAPTER_ACTIVITY_ID = 1;
		private readonly static int CHECK_TTS_ACTIVITY_ID = 2;

		public static readonly string BOOKMARK_EXTRA = "BOOKMARK_EXTRA";

		// the app's main view
		private EpubWebView mEpubWebView;

		TextToSpeechWrapper mTtsWrapper;

		private ServerSocketThread mWebServerThread = null;

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			// setContentView(R.layout.activity_main);
			// mEpubWebView = (EpubWebView) findViewById(R.id.webview);
			mEpubWebView = createView();
			SetContentView(mEpubWebView);
			mTtsWrapper = new TextToSpeechWrapper();
			mWebServerThread = createWebServer();
			mWebServerThread.startThread();

			//TestCases.run(this);
			if (savedInstanceState != null) {
				// screen orientation changed, reload
				mEpubWebView.gotoBookmark(new Bookmark(savedInstanceState));
			} else {
				// app has just been started.
				// If a bookmark has been saved, go to it, else, ask user for epub to view
				Bookmark bookmark = new Bookmark(this);
				if (bookmark.isEmpty()) {
					launchBookList();
				} else {
					mEpubWebView.gotoBookmark(bookmark);
				}
			}
		}

		private ServerSocketThread createWebServer() {
			FileRequestHandler handler = new FileRequestHandler(this);
			WebServer.WebServer server = new WebServer.WebServer(handler);
			return new ServerSocketThread(server, Globals.WEB_SERVER_PORT);
		}

		private EpubWebView createView() {
			return new EpubWebView(this);
		}

		public override bool OnCreateOptionsMenu(IMenu menu) {
			// Inflate the menu; this adds items to the action bar if it is present.
			MenuInflater.Inflate(Resource.Menu.activity_main, menu);
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			// Handle item selection
			switch (item.ItemId) {
				case Resource.Id.menu_pick_epub:
					launchBookList();
					return true;
				case Resource.Id.menu_bookmark:
					launchBookmarkDialog();
					return true;
				case Resource.Id.menu_chapters:
					launchChaptersList();
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}

		private void launchBookList() {
			Intent listComicsIntent = new Intent(this, typeof(ListEpubActivity));
			StartActivityForResult(listComicsIntent, LIST_EPUB_ACTIVITY_ID);
		}

		private void launchChaptersList() {
			Book book = getBook(); 
			if (book == null) {
				Utility.showToast(this, Resource.String.no_book_selected);
			} else {
				TableOfContents toc = book.getTableOfContents();
				if (toc.size() == 0) {
					Utility.showToast(this, Resource.String.table_of_contents_missing);
				} else {
					Intent listChaptersIntent = new Intent(this, typeof(ListChaptersActivity));
					toc.pack(listChaptersIntent, ListChaptersActivity.CHAPTERS_EXTRA);
					StartActivityForResult(listChaptersIntent, LIST_CHAPTER_ACTIVITY_ID);
				}
			}
		}

		private void launchBookmarkDialog() {
			BookmarkDialog dlg = new BookmarkDialog(this);
			dlg.show();
			dlg.setSetBookmarkAction();
			dlg.setGotoBookmarkAction();
			dlg.setStartSpeechAction();
			dlg.setStopSpeechAction();
		}

		// Should return with epub or chapter to load
		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);
			if (requestCode == CHECK_TTS_ACTIVITY_ID) {
				//mTtsWrapper.checkTestToSpeechResult(this, resultCode);
				return;
			}
			if (resultCode == Result.Ok) {
				switch (requestCode) {
					case LIST_EPUB_ACTIVITY_ID:
						onListEpubResult(data);
						break;

					case LIST_CHAPTER_ACTIVITY_ID:
						onListChapterResult(data);
						break;

					default:
						Utility.showToast(this, Resource.String.something_is_badly_broken);
						break;
				}
			} else if (resultCode == Result.Canceled) {
				Utility.showErrorToast(this, data);
			}
		}

		private void onListEpubResult(Intent data) {
			string fileName = data.GetStringExtra(ListEpubActivity.FILENAME_EXTRA);
			loadEpub(fileName, null);
		}

		private void onListChapterResult(Intent data) {
			Uri chapterUri = (Uri) data.GetParcelableExtra(ListChaptersActivity.CHAPTER_EXTRA);
			mEpubWebView.loadChapter(chapterUri);
		}

		private void loadEpub(string fileName, Uri chapterUri) {
			mEpubWebView.setBook(fileName);
			mEpubWebView.loadChapter(chapterUri);
		}

		protected void OnSaveInstanceState (Bundle outState) {
			base.OnSaveInstanceState(outState);
			Bookmark bookmark = mEpubWebView.getBookmark();
			if (bookmark != null) {
				bookmark.save(outState);
			}
		}

		public void mSaveBookmark() {
			Bookmark bookmark = mEpubWebView.getBookmark();
			if (bookmark != null) {
				bookmark.saveToSharedPreferences(this);
			}
		}

		public void mGotoBookmark() {
			mEpubWebView.gotoBookmark(new Bookmark(this));
		}

		public void mStartSpeech() {
			mTtsWrapper.checkTextToSpeech(this, CHECK_TTS_ACTIVITY_ID);
			mEpubWebView.speak(mTtsWrapper);
		}

		public void mStopSpeech() {
			mTtsWrapper.stop();
		}

		protected override void OnDestroy() {
			base.OnDestroy();
			mTtsWrapper.onDestroy();
			mWebServerThread.stopThread();
		}

		/*
		* Book currently being used.
		* (Hack to provide book to WebServer.)
		*/
		public Book getBook() { 
			return mEpubWebView.getBook(); 
		}

		public ResourceResponse fetch(Uri resourceUri) {
			return getBook().fetch(resourceUri);
		}
	}
}
