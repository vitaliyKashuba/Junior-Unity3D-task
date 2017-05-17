using UnityEngine;

public static class SimpleMazeGenerator 
{
	private const int MAZE_SIZE = 25;
	private static MazePoint[,] maze;

    /// <summary>
    /// generate and return maze map as matrix
    /// </summary>
    /// <returns></returns>
	public static MazePoint[,] getMaze() //TODO add deadzones check
	{
		maze = new MazePoint[MAZE_SIZE, MAZE_SIZE];
		generateWalls (maze);
		generateBorders (maze);
		return maze;
	}

	private static void generateWalls(MazePoint[,] maze)
	{
		for (int i = 0; i < MAZE_SIZE; i++)
		{
			for (int j = 0; j < MAZE_SIZE; j++) 
			{
				if (i % 2 == 0 && j % 2 == 0) 
				{
					maze [i, j] = MazePoint.WALL;
				} else 
				{
					maze [i, j] = MazePoint.GROUND;
				}

				if ((i % 2 == 0 && j % 2 == 1) || (i % 2 == 1 && j % 2 == 0))
				{
					if (Randomizer.getBooleanRandom(25)) 
					{
						maze [i, j] = MazePoint.WALL;
					}
				}
			}
		}	
	}

	private static void generateBorders(MazePoint[,] maze)
	{
		for (int i=0; i<MAZE_SIZE; i++) //add borders
		{
			for(int j=0; j<MAZE_SIZE; j++)
			{
				if (i==0 || j==0 || i==MAZE_SIZE-1 || j==MAZE_SIZE-1)
				{
					maze [i, j] = MazePoint.WALL;
				}
			}
		}	
	}
}
