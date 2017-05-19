using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : Unit {

	protected static float basicSpeed = 0.3f;
	protected EnemyState state = EnemyState.RANDOM_WALK;
	protected Direction direction;
    private MazePoint[,] maze = DungeonMaster.getMaze();
    private int[,] way = new int[SimpleMazeGenerator.MAZE_SIZE, SimpleMazeGenerator.MAZE_SIZE];
    private bool found = false;
    private bool timeToMakeWaveSearch = true;
    private bool routingComplete = false;
    private bool timeToPop = true;
    private int finalStep;
    private int[] playerCoordinatesOnMap, myCoordinatesOnMap;
    private Stack route;
    private float[] playerPosition;

    protected virtual void Start ()
	{
	    isFacingRight = false;
	}
	
	void Update () 
	{
		switch (state)
		{
		case EnemyState.RANDOM_WALK:
			if (Randomizer.getBooleanRandom (1)) 
			{
				doWait ();
			} 
			else 
			{
				doWalk ();
			}
			break;
		case EnemyState.PURSUIT:
			pursuit ();
			break;
		}

		GetComponent<Rigidbody2D>().velocity = new Vector2 (moveHorizontal * speed, moveVertical * speed);
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            //animator.SetBool ("isAttack", true);
            GameObject.Find("DungeonMaster").SendMessage("gameOver", this.GetType());
        }
    }

    protected void doWait()
	{
		moveVertical = moveHorizontal = 0;
	}

	protected void doWalk()
	{
		if (Randomizer.getBooleanRandom (5)) 
		{
			changeDirection ();
		}
	
	}

    /// <summary>
    /// calls from DungeonMaster if player collects 20 coins
    /// </summary>
    public void startPursuit()
    {
        state = EnemyState.PURSUIT;
    }
    /// <summary>
    /// calls from DungeonMaster after every next after 20 coins
    /// </summary>
    public void increaseSpeed()
    {
        speed = speed + speed / 20;
    }

    /// <summary>
    /// select random direction
    /// </summary>
	protected void changeDirection()
	{
		direction = Randomizer.getRandomDirection ();
		changeDirection(direction);
	}

    protected void changeDirection(Direction direction)
    {
        moveVertical = moveHorizontal = 0;
        switch (direction)
        {
        case Direction.NORTH:
            moveVertical = 1;
            break;
        case Direction.SOUTH:
            moveVertical = -1;
            break;
        case Direction.EAST:
            moveHorizontal = 1;
            break;
        case Direction.WEST:
            moveHorizontal = -1;
            break;
        }

    }

    /// <summary>
    /// contains two pursuit methods, need to uncomment one
    /// </summary>
    protected void pursuit()
    {
        playerCoordinatesOnMap = getCoordinatesOnMap(GameObject.FindGameObjectWithTag("Player").transform.position);
        playerPosition =  getPosition(playerCoordinatesOnMap);

        //advancedPursuit();
                                //need to choode and uncomment one
        simplePursuit();
    }

    /// <summary>
    /// just move to player. ignoring walls
    /// </summary>
    protected void simplePursuit()
    {
        moveToPlayer(playerPosition);
    }

    /// <summary>
    /// try to find optimal way. sometimes sucessful, but more often no
    /// units stuck in walls or went through whole maze
    /// have no time for improving, but suppose i'm close
    /// </summary>
    protected void advancedPursuit() 
	{
        if (timeToMakeWaveSearch)
	    {
	        // TODO: add coroutine for route rebuilding restart once per 1-2 seconds
	        timeToMakeWaveSearch = false;

	        PrepareMap();
	        myCoordinatesOnMap = getCoordinatesOnMap(this.transform.position);
            Thread thread = new Thread(waveSearchInThread);
	        thread.Start();
        }
	    while (!routingComplete)
	    {
	        doWait();
	    }
	    if (timeToPop)
	    {
	        if (route.Count==0)
	        {
                //Debug.Log("new search");
	            timeToMakeWaveSearch = true;
	            routingComplete = false;
	            return;
	        }
	        timeToPop = false;
	        playerPosition = getPosition((int[])route.Pop());
            //Debug.Log("Pop");
        }
	    
        moveToPlayer(playerPosition);
    }

    void moveToPlayer(float[] playerCoordinates)
    {
        float moveCorrection = 0.1f;

        if (Mathf.Abs(playerCoordinates[0] - this.transform.position.x) < moveCorrection)   //to avoid Flip() calling every frame
        {
            if (Mathf.Abs(playerCoordinates[1] - this.transform.position.y) < moveCorrection)
            {
                timeToPop = true;
            }
            moveHorizontal = 0;
        }
        else
        {
            if (playerCoordinates[0] < this.transform.position.x)
            {
                moveHorizontal = -1;
            }
            else
            {
                moveHorizontal = 1;
            }
        }

        if (playerCoordinates[1] < this.transform.position.y)
        {
            moveVertical = -1;
        }
        else
        {
            moveVertical = 1;
        }
    }

    /// <summary>
    /// convert enum map into numeric. erase previous search
    /// </summary>
    void PrepareMap()
    {
        way = new int[SimpleMazeGenerator.MAZE_SIZE, SimpleMazeGenerator.MAZE_SIZE];
        for (int i = 0; i < SimpleMazeGenerator.MAZE_SIZE; i++)
        {
            for (int j = 0; j < SimpleMazeGenerator.MAZE_SIZE; j++)
            {
                switch (maze[i, j])
                {
                    case MazePoint.GROUND:
                        way[i, j] = 0;
                        break;
                    case MazePoint.WALL:
                        way[i, j] = -1;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// runs wave searxh in thread. because sometimes takes lot of time
    /// </summary>
    void waveSearchInThread()
    {
        found = false;

        int[] target = new int[] { playerCoordinatesOnMap[0], playerCoordinatesOnMap[1] };
        int[] current = new int[] { myCoordinatesOnMap[0], myCoordinatesOnMap[1] };

        way[1, 1] = -2;

        waveSearch(current, 1, ref way, target);
        route = waveSearch2(target, ref way);
        routingComplete = true;
    }

    /// <summary>
    /// first step of wave search
    /// see https://habrahabr.ru/post/264189/
    /// </summary>
    void waveSearch(int[] point, int i, ref int[,] way, int[] target)
    {
        Debug.Log("waveSearch" + point[0] + " " + point[1]);
        ArrayList nearby = getNearby(point);
        if (nearby.Count > 0 && !found)
        {
            foreach (int[] p in nearby)
            {
                if (p[0] == target[0] && p[1] == target[1])
                {
                    Debug.Log("!!!!!!!!!! waveSearch: found " + i);
                    finalStep = i;
                    found = true;
                    return;
                }
                way[p[0], p[1]] = i;
                //i++;
                waveSearch(p, i + 1, ref way, target);
            }
        }
    }

    /// <summary>
    /// second step of wave search. finds way back and put it to stack
    /// </summary>
    /// <param name="finish"></param>
    /// <param name="way"></param>
    /// <returns></returns>
    Stack waveSearch2(int[] finish, ref int[,] way)
    {
        Stack route = new Stack();
        int[] current = finish;
        finalStep--;
        while (finalStep > 1)
        {
            route.Push(current);
            if (way[current[0] - 1, current[1]] == finalStep)
            {
                finalStep--;
                current = new int[] { current[0] - 1, current[1] };
                continue;
            }
            if (way[current[0] + 1, current[1]] == finalStep)
            {
                finalStep--;
                current = new int[] { current[0] + 1, current[1] };
                continue;
            }
            if (way[current[0], current[1] - 1] == finalStep)
            {
                finalStep--;
                current = new int[] { current[0], current[1] - 1 };
                continue;
            }
            if (way[current[0], current[1] + 1] == finalStep)
            {
                finalStep--;
                current = new int[] { current[0], current[1] + 1 };
                continue;
            }
            break;
        }

        return route;
    }

    /// <summary>
    /// get available nearby points
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    ArrayList getNearby(int[] point)
    {
        ArrayList n = new ArrayList();

        if (way[point[0] - 1, point[1]] == 0)     //TODO: make DRY
        {
            n.Add(new int[] { point[0] - 1, point[1] });
        }
        if (way[point[0] + 1, point[1]] == 0)
        {
            n.Add(new int[] { point[0] + 1, point[1] });
        }
        if (way[point[0], point[1] - 1] == 0)
        {
            n.Add(new int[] { point[0], point[1] - 1 });
        }
        if (way[point[0], point[1] + 1] == 0)
        {
            n.Add(new int[] { point[0], point[1] + 1 });
        }
        return n;
    }

    /// <summary>
    /// get position coordinates on map
    /// </summary>
    /// <returns></returns>
    int[] getCoordinatesOnMap(Vector3 position)
    {
        return new int[]{ (int)(position.x / DungeonMaster.WALL_SIZE), (int)(position.y / DungeonMaster.WALL_SIZE) };
    }

    /// <summary>
    /// convert coordinates into position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    float[] getPosition(int[] position)
    {
        return new float[]{position[0]* DungeonMaster.WALL_SIZE , position[1] * DungeonMaster.WALL_SIZE };
    }

//	void OnTriggerExit2D(Collider2D c)
//	{
//	    if (c.gameObject.CompareTag("Player"))
//	    {
//	        animator.SetBool("isAttack", false);
//	        Time.timeScale = 0;
//        }  
//    }

}
