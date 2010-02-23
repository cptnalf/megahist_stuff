
namespace StarTree.Plugin.Adapter.Converters
{
	internal class Revision
	{
		private Contracts.Database.Revision _cr;
		private StarTree.Plugin.Database.Revision _pr;
		
		internal Revision(Contracts.Database.Revision cr, Plugin.Database.Revision pr)
		{
			_cr = cr;
			_pr = pr;
		}
		
		public static implicit operator Contracts.Database.Revision(Revision revcon)
		{ return revcon._cr; }
		public static implicit operator Plugin.Database.Revision(Revision revcon)
		{ return revcon._pr; }
		
		public static implicit operator Revision(Contracts.Database.Revision cr)
		{
			Plugin.Database.Revision pr = new StarTree.Plugin.Database.Revision
				{
					ID = cr.ID,
					Branch = cr.Branch,
					Author = cr.Author,
					Date = new System.DateTime(cr.Date.Ticks, cr.Date.Kind),
					Log = cr.Log,
				};
			foreach(string parent in cr.Parents) { pr.addParent(parent); }
			
			return new Revision(cr, pr);
		}
		
		public static implicit operator Revision(Plugin.Database.Revision pr)
		{
			Contracts.Database.Revision cr = new StarTree.Contracts.Database.Revision
				{
					ID = pr.ID,
					Branch = pr.Branch,
					Author = pr.Author,
					Date = new System.DateTime(pr.Date.Ticks, pr.Date.Kind),
					Log = pr.Log
				};
			foreach(string parent in pr.Parents) { cr.addParent(parent); }
			
			return new Revision(cr, pr);
		}
	}
}
