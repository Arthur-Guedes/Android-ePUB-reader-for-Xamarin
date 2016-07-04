using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android_ePUB_reader_for_Xamarin;
using Com.Dteviot.EpubViewer.Epub;

namespace Com.Dteviot.EpubViewer
{
    [Activity(Label = "ePUB reader")]
    public class ListChaptersActivity : ListActivity {
		public static readonly string CHAPTERS_EXTRA = "CHAPTERS_EXTRA";
		public static readonly string CHAPTER_EXTRA = "CHAPTER_EXTRA";

		private static TableOfContents mToc;

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			mToc = new TableOfContents(Intent, CHAPTERS_EXTRA);
			ListView.Adapter = new EpubChapterAdapter(this);
		}

		// Class implementing the "ViewHolder pattern", for better ListView performance
		private class ViewHolder : Java.Lang.Object {
			public TextView textView; // refers to ListView item's TextView
			public NavPoint chapter;
		}

		// Populates entries on the list
		private class EpubChapterAdapter : BaseAdapter {
			private LayoutInflater mInflater;

			public override int Count { get { return mToc.size(); } }

			public EpubChapterAdapter(Context context) {
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

				// Populate the list item (view) with the chapters details
				viewHolder.chapter = (NavPoint) GetItem(position);
				string title = viewHolder.chapter.getNavLabel();
				viewHolder.textView.Text = title;

				return convertView;
			}

			public override Java.Lang.Object GetItem(int position) {
				return mToc.Get(position);
			}

			public override long GetItemId(int position) {
				return position;
			}

		}

		protected override void OnListItemClick(ListView l, View v, int position, long id) {
			base.OnListItemClick(l, v, position, id);

			// return URI of selected chapter
			Intent intent = new Intent();
			Uri uri = ((ViewHolder)v.Tag).chapter.getContentWithoutTag(); 
			intent.PutExtra(CHAPTER_EXTRA, uri);
			SetResult(Result.Ok, intent);
			Finish();
		}    
	}
}
