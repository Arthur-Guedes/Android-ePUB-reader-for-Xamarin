using Android.Content;
using Android.OS;
using Android.Sax;
using Org.Xml.Sax;
using System.Collections.Generic;

// The TableOfContents part of the epub
namespace Com.Dteviot.EpubViewer.Epub
{
	public class TableOfContents {
		private static readonly string XML_NAMESPACE_TABLE_OF_CONTENTS = "http://www.daisy.org/z3986/2005/ncx/";
		private static readonly string XML_ELEMENT_NCX = "ncx";
		private static readonly string XML_ELEMENT_NAVMAP = "navMap";
		private static readonly string XML_ELEMENT_NAVPOINT = "navPoint";
		private static readonly string XML_ELEMENT_NAVLABEL = "navLabel";
		private static readonly string XML_ELEMENT_TEXT = "text";
		private static readonly string XML_ELEMENT_CONTENT = "content";
		private static readonly string XML_ATTRIBUTE_PLAYORDER = "playOrder";
		private static readonly string XML_ATTRIBUTE_SCR = "src";

		private List<NavPoint> mNavPoints;

		private int mCurrentDepth = 0;
		private int mSupportedDepth = 1;
		private HrefResolver mHrefResolver = null;

		public TableOfContents() {
			mNavPoints = new List<NavPoint>();
		}

		// Unpacks contents from a bundle
		public TableOfContents(Intent intent, string key) {
			mNavPoints = (List<NavPoint>) intent.GetParcelableArrayListExtra(key);
		}

		public void add(NavPoint navPoint) {
			mNavPoints.Add(navPoint);
		}

		public void clear() {
			mNavPoints.Clear();
		}

		public NavPoint Get(int index) {
			return mNavPoints[index];
		}

		// Used to fetch the last item we're building
		public NavPoint getLatestPoint() {
			return mNavPoints[mNavPoints.Count - 1];
		}

		public int size() {
			return mNavPoints.Count;
		}

		// Packs this into an intent
		public void pack(Intent intent, string key) {
            intent.PutParcelableArrayListExtra(key, (IList<IParcelable>)mNavPoints);
        }

		/*
		* build parser to parse the "Table of Contents" file,
		* @return parser
		*/
		public IContentHandler constructTocFileParser(HrefResolver resolver) {
			RootElement root = new RootElement(XML_NAMESPACE_TABLE_OF_CONTENTS, XML_ELEMENT_NCX);
			Element navMap = root.GetChild(XML_NAMESPACE_TABLE_OF_CONTENTS, XML_ELEMENT_NAVMAP);
			Element navPoint = navMap.GetChild(XML_NAMESPACE_TABLE_OF_CONTENTS, XML_ELEMENT_NAVPOINT);
			mHrefResolver = resolver;
			AddNavPointToParser(navPoint);
			return root.ContentHandler;
		}

		// Build up code to parse a ToC NavPoint
		private void AddNavPointToParser(Element navPoint) {
			Element navLabel = navPoint.GetChild(XML_NAMESPACE_TABLE_OF_CONTENTS, XML_ELEMENT_NAVLABEL);
			Element text = navLabel.GetChild(XML_NAMESPACE_TABLE_OF_CONTENTS, XML_ELEMENT_TEXT);
			Element content = navPoint.GetChild(XML_NAMESPACE_TABLE_OF_CONTENTS, XML_ELEMENT_CONTENT);

			navPoint.StartElement += (object sender, StartElementEventArgs e) => {
				add(new NavPoint(e.Attributes.GetValue(XML_ATTRIBUTE_PLAYORDER)));
				// extend parser to handle another level of nesting if required
				if (mSupportedDepth == ++mCurrentDepth) {
					Element child = navPoint.GetChild(XML_NAMESPACE_TABLE_OF_CONTENTS, XML_ELEMENT_NAVPOINT);
					AddNavPointToParser(child);
					++mSupportedDepth;
				}
			};

			text.EndTextElement += (object sender, EndTextElementEventArgs e) => {
				getLatestPoint().setNavLabel(e.Body);
			};

			content.StartElement += (object sender, StartElementEventArgs e) => {
				getLatestPoint().setContent(mHrefResolver.ToAbsolute(e.Attributes.GetValue(XML_ATTRIBUTE_SCR)));
			};

			navPoint.EndElement += (object sender, System.EventArgs e) => {
				--mCurrentDepth;
			};
		}

	}
}
