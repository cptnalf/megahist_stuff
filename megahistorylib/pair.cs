
public class pair<F,S> : System.IComparable<pair<F,S>> where F : System.IComparable<F>
{
	public F first;
	public S second;
	
	public pair(F f, S s) { first = f; second = s; }
	
	private int _cmp(pair<F,S> two)
	{
		int result = -1;
		object o = (object)two;
		
		if (o != null) { result = first.CompareTo(two.first); }
		
		return result;
	}
	
	public int CompareTo(pair<F,S> obj) { return _cmp(obj); }
	
	int System.IComparable<pair<F,S>>.CompareTo(pair<F,S> obj) { return _cmp(obj); }
	
	public override int GetHashCode() { return ((object)first).GetHashCode(); }
}
