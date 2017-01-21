using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBuilderScript : MonoBehaviour 
{
	public Rigidbody2D wall;
	public Rigidbody2D zombie;
	public SpriteRenderer ground;

	private IEnumerator coroutine;
	bool timeToSpawnZombie = true;
	int mazeSize;
	float wallSize = 0.9f;
	MazePoint[,] maze;

	void Start () 
	{
		maze = SimpleMazeGenerator.getMaze ();
		mazeSize = maze.GetUpperBound(1); //TODO fix to be possible use non-square maze's
		mazeSize++; //upperBound returns not size, but last index
		//Debug.Log(mazeSize);
		for (int i = 0; i < mazeSize; i++) 
		{
			for (int j = 0; j < mazeSize; j++) 
			{
				if (maze [i, j].Equals (MazePoint.WALL))
				{
					Instantiate (wall, new Vector3 (i * wallSize, j * wallSize, 0), Quaternion.identity);
				} else 
				{
					if (maze [i, j].Equals (MazePoint.GROUND)) 
					{
						Instantiate (ground, new Vector3 (i * wallSize, j * wallSize, 0), Quaternion.identity);
					}
				}
			}
		}
			
	}		

	void Update () 
	{
		//spawnZombie ();
		if (timeToSpawnZombie) 
		{
			coroutine = spawnZombie ();
			StartCoroutine (coroutine);
			timeToSpawnZombie = false;
		}
	}

	IEnumerator spawnZombie()
	{
		yield return new WaitForSeconds(1.0f);
		int[] c = Randomizer.getRandomCoordinate (mazeSize, mazeSize);
		while(!maze[c[0],c[1]].Equals(MazePoint.GROUND))
		{
			c = Randomizer.getRandomCoordinate (mazeSize, mazeSize);
		}
		Instantiate (zombie, new Vector3 (c[0] * wallSize, c[1] * wallSize, 0), Quaternion.identity);
		timeToSpawnZombie = true;
		Debug.Log ("spawn zombie " + c[0] + " " + c[1]);
	}
}
