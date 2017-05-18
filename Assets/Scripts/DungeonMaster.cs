using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private bool timeToSpawnCoin = true, alive = true;
    private static int coinsCount = 0, score = 0;
    int mazeSize;
	const float WALL_SIZE = 0.9f;
    private const int COIN_SPAWN_DELAY = 1; //TODO 5
	MazePoint[,] maze;
    private Stopwatch time;

    void resetVariables()
    {
        coinsCount = 0;
        score = 0;
        Time.timeScale = 1;
        time = new Stopwatch();
        time.Start();
    }

	void Start ()
	{
        resetVariables();
	    
		maze = SimpleMazeGenerator.getMaze ();
		mazeSize = maze.GetUpperBound(1);
		mazeSize++; //upperBound returns not size, but last index
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
            Scoresheet.Node game = new Scoresheet.Node(Static.getName(), score, time.Elapsed, DateTime.Now,
                ExitStatus.ESCAPED);
            Static.appendResult(game);
           
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
    }

    /// <summary>
    /// spawn object, use layer from prefab
    /// </summary>
    void spawn(GameObject spawner)
    {
        spawn(spawner, spawner.transform.position.z);
    }

    /// <summary>
    /// coroutine ued to delay coin spawn
    /// </summary>
    /// <returns></returns>
    IEnumerator spawnCoin()
    {
        yield return new WaitForSeconds(COIN_SPAWN_DELAY);
        spawn(coin);
        coinsCount++;
        timeToSpawnCoin = true;
    }

    /// <summary>
    /// called when player collides the coin
    /// enemy spawn logic here
    /// </summary>
    public void grabCoin()
    {
        coinsCount--;
        score++;
        if (score > 20)
        {
            //increase speed
        } else
        {
            switch (score)  //TODO spawn logic here
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
    /// called whep player collides enemy
    /// write results to xml, tps game and propose go to main menu
    /// </summary>
    /// <param name="killedBy"></param>
    public void gameOver(Type killedBy)
    {
        ExitStatus exitStatus = ExitStatus.KILLED_BY_ZOMBIE; // because need to be initialized
        time.Stop();
        if (killedBy.ToString() == "Mummy") //TODO: find how compare 'Type'
        {
            exitStatus = ExitStatus.KILLED_BY_MUMMY;
            score = 0;
        }
        Debug.Log(killedBy);
        Scoresheet.Node game = new Scoresheet.Node(Static.getName(), score, time.Elapsed, DateTime.Now, exitStatus);
        Static.appendResult(game);
        alive = false;
        Time.timeScale = 0;
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
        GUI.Box(new Rect(0, 0, 100, 25), "Score: " + score);
        if (!alive)
        {
            GUI.Box(new Rect(Screen.width / 2, Screen.height / 2 - 50, 100, 25), "Game over");
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, MainMenu.BUTTON_LENGTH, MainMenu.BUTTON_HEIGTH), "Main menu"))
            {
                SceneManager.LoadScene("menu");
            }
        }
    }
}
