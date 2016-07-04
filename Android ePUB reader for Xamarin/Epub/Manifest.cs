using System.Collections.Generic;

// The manifest section of the epub's metadata
namespace Com.Dteviot.EpubViewer.Epub
{
	public class Manifest {
		private List<ManifestItem> mItems;
		private Dictionary<string, ManifestItem> idIndex;

		public Manifest() {
			mItems = new List<ManifestItem>();
			idIndex = new Dictionary<string, ManifestItem>();
		}

		public void add(ManifestItem item) {
			mItems.Add(item);
			idIndex.Add(item.getID(), item);
		}

		public void clear() {
			mItems.Clear();
		}

		public ManifestItem findById(string id) {
			return idIndex[id];
		}

		public ManifestItem findByResourceName(string resourceName) {
			for(int i = 0; i < mItems.Count; ++i) {
				ManifestItem item = mItems[i];
				if (resourceName.Equals(item.getHref())) {
					return item;
				}
			}
			return null;
		}

		// For Unit Testing
		public List<ManifestItem> getItems() { return mItems; }
	}
}
