
namespace StarTree.Plugin.Adapter.Converters
{
	internal class DisplayNames
	{
		private Contracts.Database.DisplayNames _cdn;
		private StarTree.Plugin.Database.DisplayNames _pdn;

		internal DisplayNames(Contracts.Database.DisplayNames cdn, 
													StarTree.Plugin.Database.DisplayNames pdn)
		{
			_cdn = cdn;
			_pdn = pdn;
		}

		public static implicit operator Contracts.Database.DisplayNames(DisplayNames dn)
		{
			return dn._cdn;
		}
		
		public static implicit operator StarTree.Plugin.Database.DisplayNames(DisplayNames dn)
		{ return dn._pdn; }

		public static implicit operator DisplayNames(Contracts.Database.DisplayNames cdn)
		{
			StarTree.Plugin.Database.DisplayNames pdn = 
				new StarTree.Plugin.Database.DisplayNames();
			pdn.name = cdn.name;
			pdn.parent = cdn.parent;
			pdn.id = cdn.id;
			pdn.author = cdn.author;
			pdn.date = cdn.date;
			pdn.log = cdn.log;
			
			return new DisplayNames(cdn, pdn);
		}
		
		public static implicit operator DisplayNames(StarTree.Plugin.Database.DisplayNames pdn)
		{
			Contracts.Database.DisplayNames cdn = new Contracts.Database.DisplayNames();
			cdn.name = pdn.name;
			cdn.parent = pdn.parent;
			cdn.id = pdn.id;
			cdn.author = pdn.author;
			cdn.date = pdn.date;
			cdn.log = pdn.log;
			
			return new DisplayNames(cdn, pdn);
		}
	}
}
