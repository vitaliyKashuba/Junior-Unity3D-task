using UnityEngine;

public static class Randomizer
{
	/// <summary>
	/// Gets the boolean random. Returns true with probability of 'chance' parameter.
	/// </summary>
	/// <returns><c>true</c> with probability of chance parameter </returns>
	/// <param name="chance">Chance of TRUE</param>
	public static bool getBooleanRandom(int chance)
	{
		int r = (int)(Random.value * 100);
		return r > 0 && r < chance;
	}
}
