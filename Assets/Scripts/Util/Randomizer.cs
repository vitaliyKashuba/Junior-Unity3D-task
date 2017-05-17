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
		
	public static Direction getRandomDirection()
	{
		if (getBooleanRandom (50)) 
		{
			if (getBooleanRandom (50)) 
			{
				return Direction.NORTH;
			} 
			else 
			{
				return Direction.SOUTH;
			}
		} 
		else 
		{
			if (getBooleanRandom (50)) 
			{
				return Direction.EAST;
			} 
			else 
			{
				return Direction.WEST;
			}
		}
	}

	public static int[] getRandomCoordinate(int maxX, int maxY)
	{
		return new int[] {(int)(Random.value * maxX), (int)(Random.value * maxY)};
	}
}
