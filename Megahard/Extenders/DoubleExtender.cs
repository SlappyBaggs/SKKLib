namespace System
{
	public static class mhDoubleExtender
	{
		public static bool IsNaN(this double d)
		{
			return d.ToString() == "NaN";
		}
	}
}
