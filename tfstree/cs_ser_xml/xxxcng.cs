
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace ChangesetLoader
{
	using DateTime = System.DateTime;
	using StringList = System.Collections.Generic.List<string>;
	
	[XmlRoot("xxx_cng")]
	public class Changeset : System.IComparable<Changeset>
	{
		[XmlRoot("ChangeInfo")]
		public class Change
		{
			[XmlAttribute("id")]
			public int ItemID { get; set; }
			[XmlAttribute("deletionid")]
			public int DeletionID { get; set; }
			[XmlAttribute("changetype")]
			public string Type { get; set; }
			[XmlAttribute("path")]
			public string Path { get; set; }
			[XmlAttribute("type")]
			public string ItemType { get; set; }
		}

		[XmlAttribute("id")]
		public int ID { get; set; }
		[XmlAttribute("author")]
		public string Author { get; set; }
		[XmlAttribute("date")]
		public DateTime Date { get; set; }
		[XmlAttribute("comment")]
		public string Comment { get; set; }
		
		[XmlElement("changes")]
		public Change[] Changes { get; set; }
		
		[XmlIgnore]
		public string[] Branches { get; set; }
		
		public void buildBranches()
		{
			int cnt = this.Changes.Length;
			StringList branches = TFSTree.Utils.FindChangesetBranches(this);
			
			this.Branches = new string[branches.Count];
			int i =0;
			foreach(string str in branches)
				{
					this.Branches[i] = str;
					++i;
				}
		}
		
		public int CompareTo(Changeset c2)
		{
			int result = -1;
			object o2 = c2;
			
			if (o2 != null)
				{
					result = this.ID.CompareTo(c2.ID);
				}
			return result;
		}
		
		public int CompareTo(object o2) { return this.CompareTo(o2 as Changeset); }
		
		public static bool operator < (Changeset c1, Changeset c2)
		{ return c1.ID < c2.ID; }
		public static bool operator > (Changeset c1, Changeset c2)
		{ return c1.ID > c2.ID; }
		public static bool operator <= (Changeset c1, Changeset c2)
		{ return c1.ID <= c2.ID; }
		public static bool operator >= (Changeset c1, Changeset c2)
		{ return c1.ID >= c2.ID; }
		public static bool operator == (Changeset c1, Changeset c2)
		{
			bool result = false;
			object o1 = c1;
			object o2 = c2;
			
			if ((o1 == null) || (o2 == null))
				{
					result = (o2 == null) && (o1 == null);
				}
			else
				{
					result = (c1.ID == c2.ID);
				}
			return result;
		}
		
		public static bool operator != (Changeset c1, Changeset c2)
		{ return false == (c1 == c2); }
		
		public override int GetHashCode() { return this.ID.GetHashCode(); }
		public override bool Equals(object two)
		{
			bool result = false;
			Changeset c2 = two as Changeset;
			
			if (two != null)
				{
					result = (this == c2);
				}
			
			return result;
		}
	}
}
