using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBuilderScript : MonoBehaviour 
{
	public GameObject wall;
	//public GameObject zombie;
	public GameObject ground;
	//public GameObject coin;
	//public GameObject mummy;

	private IEnumerator coroutine;
	bool timeToSpawnZombie = true, timeToSpawnCoin = true, timeToSpawnMummy = true;
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

	/*void Update ()  //still not good, but better then before
	{
		if (timeToSpawnZombie) //no need to refactor, because will be rewritten after spawn conditions change
		{
			coroutine = spawn (zombie, 5.0f);
			StartCoroutine (coroutine);
			timeToSpawnZombie = false;
		}
		if (timeToSpawnCoin) 
		{
			coroutine = spawn (coin, 2.0f);
			StartCoroutine (coroutine);
			timeToSpawnCoin = false;
		}
		if (timeToSpawnMummy) 
		{
			coroutine = spawn (mummy, 10.0f);
			StartCoroutine (coroutine);
			timeToSpawnMummy = false;
		}

	}		

	IEnumerator spawn(GameObject spawner, float delay) //TODO add spawn-near-player-check
	{
		yield return new WaitForSeconds(delay);
		int[] c = getRandomGroundPoint ();
		Instantiate (spawner, new Vector3 (c[0] * wallSize, c[1] * wallSize, 0), Quaternion.identity);
		switch (spawner.name) //TODO do something with this shit
		{
		case "Coin":
			timeToSpawnCoin = true;
			break;
		case "Zombie":
			timeToSpawnZombie = true;
			break;
		case "Mummy":
			timeToSpawnMummy = true;
			break;
		}

		Debug.Log ("spawn" + spawner.ToString() + " " + spawner.name + " " + spawner.tag);
	}

	/// <summary>
	/// Gets the random ground point.
	/// used to avoid spawning at walls
	/// </summary>
	int[] getRandomGroundPoint()
	{
		int[] c;
		do 
		{
			c = Randomizer.getRandomCoordinate (mazeSize, mazeSize);
		} 
		while (!maze [c [0], c [1]].Equals (MazePoint.GROUND));
		return c;
	}*/
}
