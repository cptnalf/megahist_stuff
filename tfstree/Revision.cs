
using System;
using System.Collections.Generic;

using System.Xml.Serialization;

namespace TFSTree
{
	public class IntSorterDesc : IComparer<int>
	{
		public int Compare(int one, int two)
		{
			return two.CompareTo(one);
		}
		
		public int Compare(object one, object two)
		{ return this.Compare((int)one, (int)two); }
	}
	
	public class StringSorterInsensitive : IComparer<string>
	{
		public int Compare(string one, string two)
		{ return string.Compare(one, two, StringComparison.InvariantCultureIgnoreCase); }
		
		public int Compare(object one, object two)
		{ return this.Compare(one as string, two as string); }
	}
	
	/// <summary>A revision in monotone.</summary>
	[XmlRoot("revision")]
	public class Revision : IComparable<Revision>
	{
		/// <summary>Unique identifier.</summary>
		private string _id;
		
		/// <summary>Branch the revision belongs to.</summary>
		private string _branch;

		/// <summary>Author of the revision.</summary>
		private string _author;

		/// <summary>Date and time when the revision was created.</summary>
		private DateTime _date;

		/// <summary>Description of the revision as specified by the author.</summary>
		private string _log;

		/// <summary>All parent IDs of this revision.</summary>
		List<string> _parents = new List<string>();

		/// <summary>
		/// for the xml serializer.
		/// </summary>
		public Revision() { }
		
		/// <summary>Creates a new revision.</summary>
		/// <param name="id">Unique identifier.</param>
		/// <param name="branch">Branch.</param>
		public Revision(string id, string branch)
		{
			this._id = id;
			this._branch = branch;
		}

		/// <summary>Creates a new revision.</summary>
		/// <param name="id">Unique identifier.</param>
		/// <param name="branch">Branch.</param>
		/// <param name="author">Author.</param>
		/// <param name="date">Date.</param>
		/// <param name="log">Log.</param>
		public Revision(string id, string branch, string author, string date, string log)
		{
			this._id = id;
			this._branch = branch;
			this._author = author;
			this._date = DateTime.Parse(date);
			this._log = log;
		}
		
		public Revision(int id, string branch, string author, DateTime date, string log)
		{
			_id = id.ToString();
			_branch = branch;
			_author = author;
			_date = date;
			_log = log;
		}
		
		/// <summary>Gets the unique identifier.</summary>
		/// <value>Gets the unique identifier.</value>
		[XmlAttribute("ID")]
		public string ID { get { return _id; } set { _id = value; } }

		/// <summary>Gets the branch.</summary>
		/// <value>Gets the branch.</value>
		[XmlAttribute("Branch")]
		public string Branch { get { return _branch; } set { _branch = value; } }

		/// <summary>Gets or sets the author.</summary>
		/// <value>Gets or sets the author.</value>
		[XmlAttribute("Author")]
		public string Author
		{
			get { return _author; }
			set { _author = value; }
		}

		/// <summary>Gets or sets the date and time.</summary>
		/// <value>Gets or sets the date and time.</value>
		[XmlElement("Date")]
		public DateTime Date
		{
			get { return _date; }
			set { _date = value; }
		}

		/// <summary>Gets or sets the description.</summary>
		/// <value>Gets or sets the description.</value>
		[XmlAttribute("Log")]
		public string Log
		{
			get { return _log; }
			set { _log = value; }
		}
		
		public void addParent(string id)
		{
			bool found = false;
			foreach(string revid in _parents)
				{ if (revid == id) { found = true; break; } }
			if (!found) { _parents.Add(id); }
		}
		
		/// <summary>Gets the parent IDs.</summary>
		/// <value>Gets the parent IDs.</value>
		[XmlElement("Parent")]
		public List<string> Parents { get { return _parents; } }

#region IComparable<Revision> Members

		public int CompareTo(Revision other)
		{
			int result = -1;
			object o = other;
			
			if (o != null) { result = this.ID.CompareTo(other.ID); }
			
			return result;
		}

#endregion
	}
}
