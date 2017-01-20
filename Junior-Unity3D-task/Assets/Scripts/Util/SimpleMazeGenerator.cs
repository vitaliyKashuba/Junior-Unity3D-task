using UnityEngine;

public static class SimpleMazeGenerator 
{
	private static int mazeSize;
	private static MazePoint[,] maze;

	public static MazePoint[,] getMaze() //TODO add deadzones check
	{
		mazeSize = 25;
		maze = new MazePoint[mazeSize, mazeSize];
		generateWalls (maze);
		generateBorders (maze);
		return maze;
	}

	private static void generateWalls(MazePoint[,] maze)
	{
		for (int i = 0; i < mazeSize; i++)
		{
			for (int j = 0; j < mazeSize; j++) 
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
		for (int i=0; i<mazeSize; i++) //add borders
		{
			for(int j=0; j<mazeSize; j++)
			{
				if (i==0 || j==0 || i==mazeSize-1 || j==mazeSize-1)
				{
					maze [i, j] = MazePoint.WALL;
				}
			}
		}	
	}
}
