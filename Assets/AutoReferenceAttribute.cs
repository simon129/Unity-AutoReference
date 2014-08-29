[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
public class AutoReference : System.Attribute
{
	public string Alias;
	
	public AutoReference(string alias)
	{
		Alias = alias;
	}
	
	public AutoReference() { }
}