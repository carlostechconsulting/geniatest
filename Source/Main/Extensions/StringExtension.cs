namespace GeniaWebApp.Extentions;

public static class StringExtension
{
	public static void IfHasContentOrElse(this string value, Action<string> action, Action actionElse)
	{
		if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
		{
			action.Invoke(value);
		}
		else
		{
			actionElse.Invoke();
		}
	}
}