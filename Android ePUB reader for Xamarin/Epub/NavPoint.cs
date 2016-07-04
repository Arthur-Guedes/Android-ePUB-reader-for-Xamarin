using Android.Net;
using Android.OS;
using Java.Interop;

// Navpoint entry in a Table of Contents
namespace Com.Dteviot.EpubViewer.Epub
{
	public class NavPoint : Java.Lang.Object, IParcelable {
		private int mPlayOrder;
		private string mNavLabel;
		private string mContent;

		public int getPlayOrder() { return mPlayOrder; }
		public string getNavLabel() { return mNavLabel; }
		public string getContent() { return mContent; }

		[ExportField("CREATOR")]
		static MyParcelableCreator InititalizeCreator() {
			return new MyParcelableCreator();
		}

		// Sometimes the content (resourceName) contains a tag into the HTML.
		public Uri getContentWithoutTag() {
			int indexOf = mContent.IndexOf('#');
			string temp = mContent;
			if (0 < indexOf) {
				temp = mContent.Substring(0, indexOf);
			}
			return Book.resourceName2Url(temp);
		}

		public void setPlayOrder(int playOrder) { mPlayOrder = playOrder; }
		public void setNavLabel(string navLabel) { mNavLabel = navLabel; }
		public void setContent(string content) { mContent = content; }

		// Construct as part of reading from XML
		public NavPoint(string playOrder) {
			mPlayOrder = int.Parse(playOrder);
		}

		public NavPoint(Parcel parcel) {
			mPlayOrder = parcel.ReadInt();
			mNavLabel = parcel.ReadString();
			mContent = parcel.ReadString();
		}

		public int DescribeContents() {
			return 0;
		}

		public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags) {
			dest.WriteInt(mPlayOrder);
			dest.WriteString(mNavLabel);
			dest.WriteString(mContent);
		}

		public sealed class MyParcelableCreator : Java.Lang.Object, IParcelableCreator {
			public Java.Lang.Object CreateFromParcel(Parcel source) {
				return new NavPoint(source);
			}

			public Java.Lang.Object[] NewArray(int size) {
				return new NavPoint[size];
			}
		}
	}
}
