using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy 
{

	private float speed = basicSpeed + speedIncreaser;
	private Animator animator;
	private Direction direction;

	void Start () 
	{
		animator = GetComponent<Animator>();
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
				//TODO add seek and destroy logic
				break;
		}

		GetComponent<Rigidbody2D>().velocity = new Vector2 (moveHorizontal * speed, moveVertical * speed);

		if (moveHorizontal != 0 || moveVertical != 0) 
		{
			animator.SetBool ("isWalk", true);
		} else 
		{
			animator.SetBool ("isWalk", false);
		}
	}

	void doWait()
	{
		//Debug.Log ("doWait");
		moveVertical = moveHorizontal = 0;
	}

	void doWalk()
	{
		if (Randomizer.getBooleanRandom (10)) 
		{
			changeDirection ();
		}
		//Debug.Log ("doWalk" + direction);
		//GetComponent<Rigidbody2D>().velocity = new Vector2 (moveHorizontal * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
	}

	void changeDirection()
	{
		direction = Randomizer.getRandomDirection ();
		switch (direction) 
		{
		case Direction.NORTH:
			moveVertical=2;
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

	void doAttack()
	{
	}		

	void OnTriggerEnter2D(Collider2D c) 
	{
		//Debug.Log ("trigger enter");
		if (c.gameObject.CompareTag ("Player")) 
		{
			animator.SetBool ("isAttack", true);
		}
		/*if (c.gameObject.name.Equals ("PursuitRadius"))
		{
			state = EnemyState.PURSUIT;
		}*/
	}

	void OnTriggerExit2D(Collider2D c)
	{
		//Debug.Log ("trigger exit");
		animator.SetBool ("isAttack", false);
	}
}
