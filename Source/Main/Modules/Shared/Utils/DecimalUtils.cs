using GeniaWebApp.Extentions;

namespace GeniaWebApp.Source.Main.Modules.Shared.Utils;

public static class DecimalUtils
{
	public static decimal? Round(decimal? value)
	{
		return value.Map(value => decimal.Round(
			value,
			2,
			MidpointRounding.AwayFromZero));
	}
}