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
    public GameObject player;

	private IEnumerator coroutine;
	//bool timeToSpawnZombie = true, timeToSpawnCoin = true, timeToSpawnMummy = true;
	int mazeSize;
	const float WALL_SIZE = 0.9f;
	MazePoint[,] maze;

	void Start () 
	{
		maze = SimpleMazeGenerator.getMaze ();
		mazeSize = maze.GetUpperBound(1);
		mazeSize++; //upperBound returns not size, but last index
		//Debug.Log(mazeSize);
		for (int i = 0; i < mazeSize; i++) 
		{
			for (int j = 0; j < mazeSize; j++) 
			{
				if (maze [i, j].Equals (MazePoint.WALL))
				{
					Instantiate (wall, new Vector3 (i * WALL_SIZE, j * WALL_SIZE, 0), Quaternion.identity);
				} else 
				{
					if (maze [i, j].Equals (MazePoint.GROUND)) 
					{
						Instantiate (ground, new Vector3 (i * WALL_SIZE, j * WALL_SIZE, 0), Quaternion.identity);
					}
				}
			}
		}

        spawn(player, -1);
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

    */

        /// <summary>
        /// spawn object
        /// </summary>
        /// <param name="spawner">what to spawn</param>
        /// <param name="layer">visibility layer, less = closer </param>
    void spawn(GameObject spawner, int layer) 
    {
        int[] c = getRandomGroundPoint();
        Instantiate(spawner, new Vector3(c[0] * WALL_SIZE, c[1] * WALL_SIZE, layer), Quaternion.identity);

        Debug.Log("spawn" + spawner.ToString() + " " + spawner.name + " " + spawner.tag);
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
	}
}
