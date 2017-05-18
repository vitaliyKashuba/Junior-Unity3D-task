using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// generate maze, spawn player, enemies, coins
/// </summary>
public class DungeonMaster : MonoBehaviour 
{
	public GameObject wall;
	public GameObject zombie;
	public GameObject ground;
    public GameObject coin;
    public GameObject mummy;
    public GameObject player;

    private bool timeToSpawnCoin = true;
    private static int coinsCount = 0;
	int mazeSize;
	const float WALL_SIZE = 0.9f;
    private const int COIN_SPAWN_DELAY = 1; //TODO 5
	MazePoint[,] maze;
    private Stopwatch time;

	void Start () 
	{
        time = new Stopwatch();
        time.Start();
        Debug.Log(Static.getName());
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
        spawn(zombie);
	}

    void Update () 
	{
	    if (Input.GetKeyDown(KeyCode.Escape))
	    {
            time.Stop();
            Debug.Log("exit " + time.Elapsed + " " + DateTime.Now);
            Scoresheet.Node game = new Scoresheet.Node(Static.getName(), Static.getScore(), time.Elapsed, DateTime.Now,
                ExitStatus.ESCAPED);
//	        Scoresheet.Node game2 = new Scoresheet.Node("111", 100, time.Elapsed, DateTime.Now,
//	            ExitStatus.KILLED_BY_MUMMY);
//            Scoresheet scoresheet = new Scoresheet();
            Static.getScoresheet().addGame(game);
            //scoresheet.addGame(game2);
	        try
	        {
	            XMLUtil.writeData(Static.getScoresheet());
	        }
	        catch (IOException e)
	        {
	            Debug.Log("Error during saving results, resuls will not be saved");
                Debug.Log(e.Message);
	        }
           
            Application.Quit();
	    }
	    if (coinsCount < 10 && timeToSpawnCoin)
	    {
	        IEnumerator coroutine = spawnCoin();
	        StartCoroutine(coroutine);
	        timeToSpawnCoin = false;
	    }
	}	

    /// <summary>
    /// spawn object
    /// </summary>
    /// <param name="spawner">what to spawn</param>
    /// <param name="layer">visibility layer, less = closer </param>
    void spawn(GameObject spawner, float layer) 
    {
        int[] c = getRandomGroundPoint();
        Instantiate(spawner, new Vector3(c[0] * WALL_SIZE, c[1] * WALL_SIZE, layer), Quaternion.identity);

        Debug.Log("spawn" + spawner.ToString() + " " + spawner.name + " " + spawner.tag);
    }

    /// <summary>
    /// spawn object, use layer from prefab
    /// </summary>
    void spawn(GameObject spawner)
    {
        spawn(spawner, spawner.transform.position.z);
    }

    IEnumerator spawnCoin()
    {
        yield return new WaitForSeconds(COIN_SPAWN_DELAY);
        spawn(coin);
        coinsCount++;
        timeToSpawnCoin = true;
    }

    public void grabCoin()
    {
        int score = Static.getScore();
        coinsCount--;
        Static.increaseScore();
        if (score > 20)
        {
            //increase speed
        } else
        {
            switch (Static.getScore())  //TODO spawn logic here
            {
                case 5:
                    spawn(zombie);
                    break;
                case 10:
                    spawn(mummy);
                    break;
                case 20:
                    //pursuit
                    break;
                default:
                    break;
            }
        }
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

    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 100, 25), "Score: " + Static.getScore());
    }
}
