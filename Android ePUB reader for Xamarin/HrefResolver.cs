// Converts relative href to absolute
namespace Com.Dteviot.EpubViewer
{
	public class HrefResolver {
		// path to file holding the href
		private string mParentPath;

		public HrefResolver(string parentFileName) {
			mParentPath = Utility.extractPath(parentFileName); 
		}

		public string ToAbsolute(string relativeHref) {
			return Utility.concatPath(mParentPath, relativeHref);
		}
	}
}
