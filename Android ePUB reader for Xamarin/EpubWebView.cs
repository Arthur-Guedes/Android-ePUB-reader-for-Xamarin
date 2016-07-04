using Android.Content;
using Android.Graphics;
using Android.Speech.Tts;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android_ePUB_reader_for_Xamarin;
using Com.Dteviot.EpubViewer.Epub;
using System;
using System.Collections.Generic;

// Holds the logic for the App's special WebView handling
namespace Com.Dteviot.EpubViewer
{
	public class EpubWebView : WebView {
		private readonly /*static*/ float FLING_THRESHOLD_VELOCITY = 200;

		// The book view will show
		private Book mBook;

		private GestureDetector mGestureDetector;

		// "root" page we're currently showing
		private Android.Net.Uri mCurrentResourceUri;

		// Position of document
		private float mScrollY = 0.0f; 

		// Note that we're loading from a bookmark
		private bool mScrollWhenPageLoaded = false;

		// To speak the text
		private TextToSpeechWrapper mTtsWrapper;

		// The page, as text
		private List<string> mText;

		private WebViewClient mWebViewClient;

		// Current line being spoken
		private int mTextLine;

		// Pick an initial default
		private float mFlingMinDistance = 320;

		// The total available area for drawing on
		private Rect mRawScreenDimensions;

		public EpubWebView(Context context) : this(context, null) {
		}

		public EpubWebView(Context context, IAttributeSet attrs) : base(context, attrs) {
			mGestureDetector = new GestureDetector(context, new CustomGestureDetector(this));
			WebSettings settings = Settings;
			settings.CacheMode = CacheModes.NoCache;
			settings.SetPluginState(WebSettings.PluginState.OnDemand);
			settings.BuiltInZoomControls = true;
			addWebSettings(settings);
			SetWebViewClient(mWebViewClient = createWebViewClient());
			SetWebChromeClient(new WebChromeClient());
		}

		/*
		* @ return Android version appropriate WebViewClient
		*/
		protected WebViewClient createWebViewClient() {
			return new CustomWebViewClient(this);
		}

		// Do any Web settings specific to the derived class
		protected void addWebSettings(WebSettings settings) {
			settings.DisplayZoomControls = true;
		}

		// Book to show
		public void setBook(string fileName) {
			// if book already loaded, don't load again
			if ((mBook == null) || !mBook.getFileName().Equals(fileName)) {
				mBook = new Book(fileName);
			}
		}

		public Book getBook() {
			return mBook;
		}

		protected WebViewClient getWebViewClient() {
			return mWebViewClient;
		}

		// Chapter of book to show
		public void loadChapter(Android.Net.Uri resourceUri) {
			if (mBook != null) {
				// if no chapter resourceName supplied, default to first one.
				if (resourceUri == null) {
					resourceUri = mBook.firstChapter();
				}
				if (resourceUri != null) {
					mCurrentResourceUri = resourceUri;
					// prevent cache, because WebSettings.LOAD_NO_CACHE doesn't always work.
					ClearCache(false);
					LoadUri(resourceUri);
				}
			}
		}

		/*
		* @ return load contents of URI into WebView,
		*   implementation is android version dependent 
		*/
		protected void LoadUri(Android.Net.Uri uri) {
			LoadUrl(uri.ToString());
		}

		public override bool OnTouchEvent(MotionEvent e) {
			return mGestureDetector.OnTouchEvent(e) || base.OnTouchEvent(e);
		}

		private bool changeChapter(Android.Net.Uri resourceUri) {
			if (resourceUri != null) {
				loadChapter(resourceUri);
				return true;
			} else {
				Utility.showToast(Context, Resource.String.end_of_book);
				return false;
			}
		}

		// Store current view into bookmark
		public Bookmark getBookmark() {
			if ((mBook != null) && (mCurrentResourceUri != null)) {
				float contentHeight = (float) ContentHeight;
				contentHeight = (contentHeight == 0.0f) ? 0.0f : ScrollY / contentHeight; 
				return new Bookmark (mBook.getFileName(), mCurrentResourceUri, contentHeight);
			}
			return null;
		}

		public void gotoBookmark(Bookmark bookmark) {
			if (!bookmark.isEmpty()) {
				mScrollY = bookmark.mScrollY;

				// get notify when content height is available, for setting Y scroll position
				mScrollWhenPageLoaded = true;
				setBook(bookmark.getFileName());
				loadChapter(bookmark.getResourceUri());
			}
		}

		public void speak(TextToSpeechWrapper ttsWrapper) {
			mTtsWrapper = ttsWrapper;
			if ((mBook != null) && (mCurrentResourceUri != null)) {
				mText = new List<string>();
				XmlUtil.parseXmlResource(mBook.fetch(mCurrentResourceUri).getData(), new XhtmlToText(mText), null);
				mTextLine = 0;
				mTtsWrapper.setOnUtteranceCompletedListener(new CustomUtteranceProgressListener(this));
				if (0 < mText.Count) {
					mTtsWrapper.speak(mText[0]);
				}
			}
		}

		// Send next piece of text to Text to speech
		[Obsolete]
		public class CustomUtteranceProgressListener : Java.Lang.Object , TextToSpeech.IOnUtteranceCompletedListener {
			EpubWebView epubWebView;

			public CustomUtteranceProgressListener(EpubWebView epubWebView) {
				this.epubWebView = epubWebView;
			}

			public void OnUtteranceCompleted(string utteranceId) {
				if (epubWebView.mTextLine < epubWebView.mText.Count - 1) {
					epubWebView.mTtsWrapper.speak(epubWebView.mText[++epubWebView.mTextLine]);
				}
			}
		}

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh) {
			base.OnSizeChanged(w, h, oldw, oldh);
			mRawScreenDimensions = new Rect(0, 0, w, h);
			mFlingMinDistance = w / 2;
		}

		/*
		* Called when page is loaded,
		* if we're scrolling to a bookmark, we need to set the
		* page size listener here.  Otherwise it can be called
		* for pages other than the one we're interested in 
		*/
		#pragma warning disable 618
		protected void onPageLoaded() {
			if (mScrollWhenPageLoaded) {
				SetPictureListener(new CustomPictureListener(this));
				mScrollWhenPageLoaded = false;
			}
		}
		#pragma warning restore 618

		/*
		* Need to wait until view has figured out how big web page is
		* Otherwise, we can't scroll to last position because 
		* ContentHeight returns 0. At current time, there is no replacement for PictureListener 
		*/
		[Obsolete]
		public class CustomPictureListener : Java.Lang.Object , IPictureListener {

			EpubWebView epubWebView;

			public CustomPictureListener(EpubWebView epubWebView) {
				this.epubWebView = epubWebView;
			}

			public void OnNewPicture(WebView view, Picture picture) {
				// stop listening
				epubWebView.SetPictureListener(null);

				epubWebView.ScrollTo(0, (int)(epubWebView.ContentHeight * epubWebView.mScrollY));
				epubWebView.mScrollY = 0.0f;
			}
		}

		public class CustomGestureDetector : GestureDetector.SimpleOnGestureListener {
			EpubWebView epubWebView;

			public CustomGestureDetector(EpubWebView epubWebView) {
				this.epubWebView = epubWebView;
			}

			public override bool OnFling(MotionEvent event1, MotionEvent event2, float velocityX, float velocityY) {
				// if no book, nothing to do
				if (epubWebView.mBook == null) {
					return false;
				}

				// also ignore swipes that are vertical, too slow, or too short.
				float deltaX = event2.GetX() - event1.GetX();
				float deltaY = event2.GetY() - event1.GetY();

				if ((Math.Abs(deltaX) < Math.Abs(deltaY)) || (Math.Abs(deltaX) < epubWebView.mFlingMinDistance) || (Math.Abs(velocityX) < epubWebView.FLING_THRESHOLD_VELOCITY)) {
					return false;
				} else {
					if (deltaX < 0) {
						return epubWebView.changeChapter(epubWebView.mBook.nextResource(epubWebView.mCurrentResourceUri));
					} else {
						return epubWebView.changeChapter(epubWebView.mBook.previousResource(epubWebView.mCurrentResourceUri));
					}
				}
			}

			// If double tap at top/bottom fifth of screen, scroll page up/down
			public override bool OnDoubleTap(MotionEvent e) {
				float y = e.GetY();
				if (y <= epubWebView.mRawScreenDimensions.Height() / 5) {
					epubWebView.PageUp(false);
					return true;
				} else if (4 * epubWebView.mRawScreenDimensions.Height() / 5 <= y) {
					epubWebView.PageDown(false);
					return true;
				} else {
					return false;
				}
			}
		}

		/*
		* Called when Android 3.0 webview wants to load a URL.
		* @return the requested resource from the ebook
		*/
		private WebResourceResponse onRequest(string url) {
			Android.Net.Uri resourceUri = Android.Net.Uri.Parse(url);
			WebResourceResponse webResponse = new WebResourceResponse("", "UTF-8", null);
			ResourceResponse response = getBook().fetch(resourceUri);

			// if don't have resource, give error
			if (response == null) {
				getWebViewClient().OnReceivedError(this, ClientError.FileNotFound, "Unable to find resource in epub", url);
			} else {
				webResponse.Data = response.getData();
				webResponse.MimeType = response.getMimeType();
			}
			return webResponse;
		}

		public class CustomWebViewClient : WebViewClient {
			EpubWebView epubWebView;

			public CustomWebViewClient(EpubWebView epubWebView) {
				this.epubWebView = epubWebView;
			}

			public override WebResourceResponse ShouldInterceptRequest(WebView view, string url) {
				return epubWebView.onRequest(url);
			}

			public override void OnPageFinished(WebView view, string url) {
				base.OnPageFinished(view, url);
				epubWebView.onPageLoaded();
			}
		}
	}
}
