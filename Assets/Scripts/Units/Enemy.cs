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
    private int finalStep;

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
	        convertMap();
	        
	        Thread thread = new Thread(waveSearchInThread);
	        thread.Start();
        }
    }

    void convertMap()
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
        int[] target = new int[] { 13, 13 };
        int[] current = new int[] { 1, 1 };

        way[1, 1] = -2;
        //way[13, 13] = -3;

        waveSearch(current, 1, ref way, target);

        for (int i = 0; i < SimpleMazeGenerator.MAZE_SIZE; i++)
        {
            Debug.Log(way[i,0] + " " + way[i, 1] + " " + way[i, 2] + " " + way[i, 3] + " " + way[i, 4] + " " + way[i, 5] + " " + way[i, 6] + " " + way[i, 7] + " " + way[i, 8] + " " + way[i, 9] + " " + way[i,10] + " " + way[i, 11] + " " + way[i,12] + " " + way[i, 13] + " " + way[i, 14]);
        }

        waveSearch2(target, ref way);
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

    ArrayList waveSearch2(int[] finish, ref int[,] way)
    {
        ArrayList route = new ArrayList();
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
            route.Add(current);
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
        foreach (int[] wp in route)
        {
            Debug.Log(wp[0] + " " + wp[1]);
        }
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

	protected void pursuit() //TODO add pathfinding logic
	{
		Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
		float moveCorrection = 0.05f; //to avoid Flip() calling every frame

		if (Mathf.Abs( playerPosition.x - this.transform.position.x ) < moveCorrection) 
		{
			moveHorizontal = 0;
		} 
		else 
		{
			if (playerPosition.x < this.transform.position.x) 
			{
				moveHorizontal = -1;
			}
			else 
			{
				moveHorizontal = 1;
			}
		}

		if (playerPosition.y < this.transform.position.y) 
		{
			moveVertical = -1;
		} 
		else 
		{
			moveVertical = 1;
		}
 
	}

    /*  TODO: find out where error is
    /// <summary>
    /// see: https://habrahabr.ru/post/264189/
    /// </summary>
    void findRoute()
    {
        int[] playerPosition = getPosition(GameObject.FindGameObjectWithTag("Player").transform.position);
        int[] myPosition = getPosition(this.transform.position);
        Debug.Log(myPosition[0] + " " + myPosition[1] + " " + playerPosition[0] + " " + playerPosition[1]);
        way[myPosition[0], myPosition[1]] = 1;
        ArrayList l = new ArrayList();
        l.Add(myPosition);
        waveSearch(l, playerPosition);
    }

    void waveSearch(ArrayList points, int[] target)
    {
        if (points.Count == 0)
        {
            return;
        }
        ArrayList nextStep = new ArrayList();
        foreach (int[] point in points)
        {
            way[point[0], point[1]] = 2;
            Debug.Log(point[0] + " ," + point[1]);
            if (point[0]==target[0] && point[1]==target[1])
            {
                Debug.Log("found");
                return;
            }
            nextStep = getNearby(point);
            //Debug.Log("next step" + nextStep.Count);
        }
        waveSearch(nextStep, target);
    }

    ArrayList getNearby(int[] point)
    {
        ArrayList nearby = new ArrayList();
        nearby.Add(new int[] {point[0] - 1, point[1]});
        nearby.Add(new int[] { point[0] + 1, point[1] });
        nearby.Add(new int[] { point[0], point[1] - 1 });
        nearby.Add(new int[] { point[0], point[1] + 1 });
        return checkNearby(nearby);
    }

    ArrayList checkNearby(ArrayList nearby)
    {
        ArrayList nearbyChecked = new ArrayList();
        foreach (int[] point in nearby)
        {
            if (maze[point[0], point[1]] != MazePoint.WALL)
            {
                nearbyChecked.Add(point);
            }
        }
        return nearbyChecked;
    }*/

    /// <summary>
    /// get position coordinates on map
    /// </summary>
    /// <returns></returns>
    int[] getPosition(Vector3 position)
    {
        return new int[]{ (int)(position.x / DungeonMaster.WALL_SIZE), (int)(position.y / DungeonMaster.WALL_SIZE) };
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
