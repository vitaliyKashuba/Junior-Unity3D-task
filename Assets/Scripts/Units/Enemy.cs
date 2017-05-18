using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit {

	protected static float basicSpeed = 0.3f;
	protected static float speedIncreaser = basicSpeed / 20; // 5 %
	protected EnemyState state = EnemyState.RANDOM_WALK;
	protected Direction direction;
    private MazePoint[,] maze = DungeonMaster.getMaze();
    private int[,] way = new int[SimpleMazeGenerator.MAZE_SIZE, SimpleMazeGenerator.MAZE_SIZE];

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
        //Debug.Log("n" + nearby.Count);
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
        //Debug.Log("nc" + nearbyChecked.Count);
        return nearbyChecked;
    }

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
