using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit {

	protected static float basicSpeed = 0.3f;
	protected static float speedIncreaser = basicSpeed / 20; // 5 %
	protected EnemyState state = EnemyState.RANDOM_WALK;

	protected Direction direction;

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

	protected void changeDirection()
	{
		direction = Randomizer.getRandomDirection ();
		switch (direction) 
		{
		case Direction.NORTH:
			moveVertical=1;
			break;
		case Direction.SOUTH:
			moveVertical=-1;
			break;
		case Direction.EAST:
			moveHorizontal=1;
			break;
		case Direction.WEST:
			moveHorizontal=-1;
			break;
		}
	}

	protected void pursuit() //TODO add pathfinding logic
	{
		Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
		//Debug.Log ("player" + playerPosition.x + " " + playerPosition.y);
		//Debug.Log ("mummy " + this.transform.position.x + " " + this.transform.position.y);
		//Debug.Log ("move  " + moveHorizontal + " " + moveVertical);
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

	void OnTriggerEnter2D(Collider2D c) 
	{
		//Debug.Log ("trigger enter");
		if (c.gameObject.CompareTag ("Player")) 
		{
			animator.SetBool ("isAttack", true);
		}
	}

	void OnTriggerExit2D(Collider2D c)
	{
		//Debug.Log ("trigger exit");
		animator.SetBool ("isAttack", false);
	}

	//findPlayer(), goToPlayer..
}
