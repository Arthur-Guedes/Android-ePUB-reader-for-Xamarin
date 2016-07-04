using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android_ePUB_reader_for_Xamarin;
using Java.IO;
using Java.Util;
using System.Collections.Generic;

namespace Com.Dteviot.EpubViewer
{
    [Activity(Label = "ePUB reader")]
    public class ListEpubActivity : ListActivity {
		public static readonly string FILENAME_EXTRA = "FILENAME_EXTRA";
		public static readonly string PAGE_EXTRA = "PAGE_EXTRA";

		private ListView mListView;
		private EpubListAdapter mEpubListAdapter;
		private string mRootPath;
		private List<string> mFileNames;

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			mListView = ListView; // get the built-in ListView
			listEpubFiles();
			mEpubListAdapter = new EpubListAdapter(this, mFileNames);
			mListView.Adapter = mEpubListAdapter;
		}

		// returns true if SD card storage (or equivalent) is available
		private bool isMediaAvailable() {
			string state = Environment.ExternalStorageState;
			if (Environment.MediaMounted.Equals(state)) {
				return true;
			} else {
				return Environment.MediaMountedReadOnly.Equals(state);
			}
		}

		// populate mFileNames with all files in Downloads directory of SD card
		private void listEpubFiles() {
			mFileNames = new List<string>();
			if (!isMediaAvailable()) {
				Utility.finishWithError(this, Resource.String.sd_card_not_mounted);
			} else {
				File path = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads);
				mRootPath = path.ToString();
				string[] filesInDirectory = path.List();
				if (filesInDirectory != null) {
					foreach (string fileName in filesInDirectory) {
						if (isEpubBookFile(fileName)) {
							mFileNames.Add(fileName);
						}
					}
				}
				if (mFileNames.Count == 0) {
					Utility.finishWithError(this, Resource.String.no_epub_found);
				}
				Collections.Sort(mFileNames);
			}
		}

		// returns true if filename ends in epub
		private bool isEpubBookFile(string fileName) {
			Java.Util.Regex.Pattern pattern = Java.Util.Regex.Pattern.Compile(".*epub$");
			return pattern.Matcher(fileName.ToLower()).Matches();
		}

		private string titleToFileName(string title) {
			return mRootPath + "/" + title;
		}

		// Class implementing the "ViewHolder pattern", for better ListView performance
		private class ViewHolder : Java.Lang.Object {
			public TextView textView; // refers to ListView item's TextView
		}

		// Populates entries on the list
		private class EpubListAdapter : ArrayAdapter<string> {
			private List<string> mTitles;
			private LayoutInflater mInflater;

			public EpubListAdapter(Context context, List<string> titles) : base(context, -1, titles) {
				this.mTitles = titles;
				mInflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);
			}

			public override View GetView(int position, View convertView, ViewGroup parent) {
				ViewHolder viewHolder; // holds references to current item's GUI

				// if convertView is null, inflate GUI and create ViewHolder;
				// otherwise, get existing ViewHolder
				if (convertView == null) {
					convertView = mInflater.Inflate(Resource.Layout.epub_list_item, null);
					viewHolder = new ViewHolder();

					viewHolder.textView = (TextView) convertView.FindViewById(Resource.Id.epub_title);
					convertView.Tag = viewHolder; // store as View's tag
				} else {
					viewHolder = (ViewHolder) convertView.Tag;
				}

				// Populate the list item (view) with the comic's details
				string title = mTitles[position];
				viewHolder.textView.Text = title;
				return convertView;
			}
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id) {
			base.OnListItemClick(l, v, position, id);
			string fileName = titleToFileName(((TextView)v).Text.ToString());
			Intent intent = new Intent();
			intent.PutExtra(FILENAME_EXTRA, fileName);
			// set page to first, because ListChaptersActivity returns page to start at
			intent.PutExtra(PAGE_EXTRA, 0);
			SetResult(Result.Ok, intent);
			Finish();
		}    
	}
}
