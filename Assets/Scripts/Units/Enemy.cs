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
    private int[] playerPosition, myPosition;
    private Stack route;
    private float[] playerCoordinates;

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

	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        startPursuit();
        }
    }

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

    void waveSearchInThread()
    {
        found = false;

        int[] target = new int[] { playerPosition[0], playerPosition[1] };
        int[] current = new int[] { myPosition[0], myPosition[1]};

        way[1, 1] = -2;
        //way[13, 13] = -3;

        waveSearch(current, 1, ref way, target);

//        for (int i = 0; i < SimpleMazeGenerator.MAZE_SIZE; i++)
//        {
//            Debug.Log(way[i,0] + " " + way[i, 1] + " " + way[i, 2] + " " + way[i, 3] + " " + way[i, 4] + " " + way[i, 5] + " " + way[i, 6] + " " + way[i, 7] + " " + way[i, 8] + " " + way[i, 9] + " " + way[i,10] + " " + way[i, 11] + " " + way[i,12] + " " + way[i, 13] + " " + way[i, 14]);
//        }

        route = waveSearch2(target, ref way);
        routingComplete = true;
    }

    void waveSearch(int[] point, int i, ref int[,] way, int[] target)
    {
        //Debug.Log("waveSearch" + point[0] + " " + point[1]);
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
                way[p[0],p[1]] = i;
                //i++;
                waveSearch(p, i+1, ref way, target);
            }
        }
    }

    Stack waveSearch2(int[] finish, ref int[,] way)
    {
        Stack route = new Stack();
        int[] current = finish;
        finalStep--;
        while (finalStep > 1)
        {
//            Debug.Log("current" + current[0] + " " + current[1] + " " + way[current[0],current[1]]);
//            Debug.Log("f step " + finalStep);
//            Debug.Log(way[current[0] - 1,current[1]]);
//            Debug.Log(way[current[0] + 1,current[1]]);
//            Debug.Log(way[current[0],current[1] - 1]);
//            Debug.Log(way[current[0],current[1] + 1]);
            route.Push(current);
            if (way[current[0] - 1,current[1]] == finalStep)
            {
                finalStep--;
                current = new int[] { current[0] - 1, current[1] };
                continue;
            }
            if (way[current[0] + 1,current[1]] == finalStep)
            {
                finalStep--;
                current = new int[] { current[0] + 1, current[1] };
                continue;
            }
            if (way[current[0],current[1] - 1] == finalStep)
            {
                finalStep--;
                current = new int[] { current[0], current[1] - 1 };
                continue;
            }
            if (way[current[0],current[1] + 1] == finalStep)
            {
                finalStep--;
                current = new int[] { current[0], current[1] + 1 };
                continue;
            }
            break;
        }
//        foreach (int[] wp in route)
//        {
//            Debug.Log(wp[0] + " " + wp[1]);
//        }
        Debug.Log("ws2: stack length " + route.Count);
        return route;
    }

    ArrayList getNearby(int[] point)
    {
        ArrayList n = new ArrayList();
        try
        {
            if (way[point[0] - 1,point[1]] == 0)     //TODO: make DRY
            {
                n.Add(new int[] { point[0] - 1, point[1] });
            }
            if (way[point[0] + 1,point[1]] == 0)
            {
                n.Add(new int[] { point[0] + 1, point[1] });
            }
            if (way[point[0],point[1] - 1] == 0)
            {
                n.Add(new int[] { point[0], point[1] - 1 });
            }
            if (way[point[0],point[1] + 1] == 0)
            {
                n.Add(new int[] { point[0], point[1] + 1 });
            }
        }
        catch
        {
            //System.out.println("ex");
        }
        //Debug.Log("getNearby for " + point[0] + " " + point[1] + ": " + n.Count);
        return n;
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

    public void startPursuit()
    {
        state = EnemyState.PURSUIT;
    }

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

    protected void pursuit()
    {
        playerCoordinates =  getCoordinates(getPosition(GameObject.FindGameObjectWithTag("Player").transform.position));
        //advancedPursuit();
        simplePursuit();
    }

	protected void advancedPursuit() 
	{
        if (timeToMakeWaveSearch)
	    {
	        // TODO: add coroutine
	        timeToMakeWaveSearch = false;

	        PrepareMap();
	        myPosition = getPosition(this.transform.position);
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
                Debug.Log("new search");
	            timeToMakeWaveSearch = true;
	            routingComplete = false;
	            return;
	        }
	        timeToPop = false;
	        playerCoordinates = getCoordinates((int[])route.Pop());
            Debug.Log("Pop");
        }
	    
        moveToPlayer(playerCoordinates);
    }

    protected void simplePursuit()
    {
        moveToPlayer(playerCoordinates);
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

    void moveTo(int[] position)
    {
        float[] targetCoordinates = getCoordinates(position);
    }

    /// <summary>
    /// get position coordinates on map
    /// </summary>
    /// <returns></returns>
    int[] getPosition(Vector3 position)
    {
        return new int[]{ (int)(position.x / DungeonMaster.WALL_SIZE), (int)(position.y / DungeonMaster.WALL_SIZE) };
    }

    float[] getCoordinates(int[] position)
    {
        return new float[]{position[0]* DungeonMaster.WALL_SIZE , position[1] * DungeonMaster.WALL_SIZE };
    }

	void OnTriggerEnter2D(Collider2D c) 
	{
		if (c.gameObject.CompareTag ("Player")) 
		{
            //animator.SetBool ("isAttack", true);
		    GameObject.Find("DungeonMaster").SendMessage("gameOver", this.GetType());
        }
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
