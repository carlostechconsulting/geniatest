namespace GeniaWebApp.Extentions;

public static class DictionaryExtension
{
	public static R? GetAndMap<K, V, R>(this Dictionary<K, V> data, K key, Func<V, R> func) where R : new()
	{
		if (data.TryGetValue(key, out _))
			return func.Invoke(data[key]);

		return new R();
	}
}