using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android_ePUB_reader_for_Xamarin;

namespace Com.Dteviot.EpubViewer
{
	public class BookmarkDialog {
		private MainActivity mActivity;
		private Dialog mDlg;

		public BookmarkDialog(MainActivity activity) {
			mActivity = activity;
		}

		public void setSetBookmarkAction() {
			AttachClickListener(Resource.Id.bookmark_dialog_set_button);
		}

		public void setGotoBookmarkAction() {
			AttachClickListener(Resource.Id.bookmark_dialog_goto_button);
		}

		public void setStartSpeechAction() {
			AttachClickListener(Resource.Id.bookmark_dialog_start_speech);
		}

		public void setStopSpeechAction() {
			AttachClickListener(Resource.Id.bookmark_dialog_stop_speech);
		}

		public void show() {
			mDlg = new Dialog(mActivity);
			mDlg.SetContentView(Resource.Layout.options_menu);
			mDlg.SetTitle(Resource.String.options_menu_title);
			mDlg.SetCancelable(true);
			mDlg.Show();
		}

		// When button clicked, dismiss dialog, then perform action
		private void AttachClickListener(int buttonId) {
			Button button = (Button)(mDlg.FindViewById(buttonId));
			button.Click += delegate {
				mDlg.Dismiss();
				if(buttonId == Resource.Id.bookmark_dialog_set_button) {
					mActivity.mSaveBookmark();
				} else if(buttonId == Resource.Id.bookmark_dialog_goto_button) {
					mActivity.mGotoBookmark();
				} else if(buttonId == Resource.Id.bookmark_dialog_start_speech) {
					mActivity.mStartSpeech();
				} else if(buttonId == Resource.Id.bookmark_dialog_stop_speech) {
					mActivity.mStopSpeech();
				}
			};
		}
	}
}
