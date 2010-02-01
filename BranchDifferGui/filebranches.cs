

using System;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Collections.Generic;

public class branch_order
{
	public struct Options
	{
		public string src_path;
		public int src_id;
		public VersionControlServer server;
	}
	
	public class TreeItem
	{
		private List<TreeItem> _children = new List<TreeItem>();
		private string _name;
		private string _fullPath;
		
		public TreeItem(string name, string fullPath) { _name = name; _fullPath = fullPath; }
		
		public string Name { get { return _name; } }
		public string FullPath { get { return _fullPath; } }
		public List<TreeItem> Children { get { return _children; } }
		public void push_back(TreeItem child) { _children.Add(child); }
		
		public static bool HasChildren(object o)
		{
			bool result = false;
			TreeItem item = o as TreeItem;
			if (item != null) { result = item._children.Count > 0; }
			return result;
		}
		
		public static System.Collections.IEnumerable ChildrenGetter(Object model)
		{
			System.Collections.IEnumerable result = null;
			TreeItem item = model as TreeItem;
			if (item != null) { result = item._children; }
			return result;
		}
	}

	private System.Text.RegularExpressions.Regex _igt0803_re = 
	  new System.Text.RegularExpressions.Regex("/([^/]+)/([^/]+)/EGS/", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
		
	public branch_order() { }
	
	public TreeItem get_branches(Options options)
	{
		TreeItem root = null;
		VersionControlServer vcs = options.server;
		
		/* i want to collect all of the branches which came from main. */
		ItemSpec itm = new ItemSpec(options.src_path, RecursionType.None);
		VersionSpec ver = VersionSpec.Latest;
		BranchHistoryTreeItem[][] branches = vcs.GetBranchHistory(new ItemSpec[] { itm }, ver);
		for (int i=0; i < branches.Length; ++i)
			{
				for(int j=0; j < branches[i].Length; ++j)
					{
						root = walk_tree(branches[i][j]);
					}
			}
		
		return root;
	}

	void print_tree(TreeItem root, int level)
	{
		if (root != null)
			{
				string buffer = string.Empty;
				if (level > 0) { buffer= new string(' ', level); }
				
				Console.WriteLine("{0}{1}", buffer, root.Name);
				
				foreach(TreeItem item in root.Children) { print_tree(item, level +1); }
			}
	}
	
	TreeItem walk_tree(BranchHistoryTreeItem ptr)
	{
		TreeItem root = null;
		if (ptr != null)
			{
				if (ptr.Relative != null && ptr.Relative.BranchToItem != null)
					{
// 						if (ptr.Relative.BranchFromItem != null)
// 							{
// 								Console.WriteLine("{0} => {1}", 
// 																	ptr.Relative.BranchFromItem.ServerItem,
// 																	ptr.Relative.BranchToItem.ServerItem);
// 							}
// 						else
// 							{ Console.WriteLine(ptr.Relative.BranchToItem.ServerItem); }
						
						//root = new TreeItem(ptr.Relative.BranchToItem.ServerItem);
						
						string branchName;
						System.Text.RegularExpressions.Match m = _igt0803_re.Match(ptr.Relative.BranchToItem.ServerItem);
						
						if (m != null)
							{
								branchName = string.Format("{0}-{1}", m.Groups[1].Value, m.Groups[2].Value);
								root = new TreeItem(branchName, ptr.Relative.BranchToItem.ServerItem);
								
								foreach(BranchHistoryTreeItem itm in ptr.Children)
									{
										TreeItem item = walk_tree(itm);
										
										if (item != null) { root.push_back(item); }
									}
							}
					}
			}
		
		return root;
	}
} 