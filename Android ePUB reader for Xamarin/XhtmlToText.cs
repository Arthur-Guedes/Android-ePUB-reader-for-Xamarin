using Org.Xml.Sax;
using Org.Xml.Sax.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

// Extracts the Text that would be shown to a user from a XHTML document
namespace Com.Dteviot.EpubViewer
{
	public class XhtmlToText : DefaultHandler {
		// Nodes that need their contents to be followed by white space
		private static readonly string[] ADD_WHITE_SPACE_NODES = { "br", "p", "h1", "h2", "h3", "h4", "h5" };

		// chop text into strings of a couple of hundred words or so
		private static readonly int MIN_CHARS_PER_STRING = 6 * 200;

		private StringBuilder mBuilder;
		private List<string> mText;
		private bool mInBody = false;

		public XhtmlToText(List<string> text) {
			mText = text;
			mBuilder = new StringBuilder();
		}

		public override void Characters(char[] ch, int start, int length) {
			base.Characters(ch, start, length);
			// ignore text in head
			if (mInBody) {
				mBuilder.Append(ch, start, length);
			}
		}

		public override void EndElement(string uri, string localName, string name) {
			base.EndElement(uri, localName, name);
			if (isWhiteSpaceNode(localName)) {
				mBuilder.Append(" ");
			}
			if (MIN_CHARS_PER_STRING < mBuilder.Length) {
				flushAccumulator();
			}
		}

		public override void EndDocument() {
			base.EndDocument();
			// we're done, make sure any remaining text is moved
			flushAccumulator();
		}

		public override void StartElement(string uri, string localName, string name, IAttributes attributes) {
			base.StartElement(uri, localName, name, attributes);
			if (localName.Equals("li", StringComparison.InvariantCultureIgnoreCase)) {
				mBuilder.Append(" ");
			} else if (localName.Equals("body", StringComparison.InvariantCultureIgnoreCase)) {
				mInBody = true;
			}
		}

		private void flushAccumulator() {
			if (0 < mBuilder.Length) {
				mText.Add(mBuilder.ToString());
				mBuilder.Length = 0;
			}
		}

		private bool isWhiteSpaceNode(string nodeName) {
			foreach (string s in ADD_WHITE_SPACE_NODES) {
				if (s.Equals(nodeName)) {
					return true;
				}
			}
			// if get here, not found
			return false;
		}
	}
}
