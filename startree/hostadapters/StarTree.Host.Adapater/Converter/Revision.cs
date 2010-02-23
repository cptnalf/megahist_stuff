
namespace StarTree.Host.Adapter.Converter
{
	internal class Revision
	{
		private Contracts.Database.Revision _cr;
		private Host.Database.Revision _hr;
		
		internal Revision(Contracts.Database.Revision cr, Host.Database.Revision hr)
		{
			_cr = cr;
			_hr = hr;
		}
		
		public static implicit operator Contracts.Database.Revision(Revision revcon)
		{
			return revcon._cr;
		}
		
		public static implicit operator Host.Database.Revision(Revision revcon)
		{
			return revcon._hr;
		}
		
		public static implicit operator Revision(Contracts.Database.Revision cr)
		{
			Host.Database.Revision hr = new StarTree.Host.Database.Revision
				{
					ID = cr.ID,
					Author = cr.Author,
					Date = new System.DateTime(cr.Date.Ticks, cr.Date.Kind),
					Branch = cr.Branch,
					Log = cr.Log,
				};
			foreach(string parent in cr.Parents) { hr.addParent(parent); }
			
			return new Revision(cr, hr);
		}
		
		public static implicit operator Revision(Host.Database.Revision hr)
		{
			Contracts.Database.Revision cr = new StarTree.Contracts.Database.Revision
				{
					ID = hr.ID,
					Branch = hr.Branch,
					Author = hr.Author,
					Date = new System.DateTime(hr.Date.Ticks, hr.Date.Kind),
					Log = hr.Log,
				};
			foreach(string parent in cr.Parents) { hr.addParent(parent); }
			
			return new Revision(cr, hr);
		}
	}
}
