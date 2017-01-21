using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	protected static float basicSpeed = 0.3f;
	protected static float speedIncreaser = 0.05f;
	protected EnemyState state = EnemyState.RANDOM_WALK;
	protected bool isFacingRight = false;
	protected float moveHorizontal, moveVertical = 0;

	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	protected void FixedUpdate()
	{
		if (moveHorizontal > 0 && !isFacingRight) 
		{
			Flip ();
		} 
		else if (moveHorizontal < 0 && isFacingRight) 
		{
			Flip ();
		}
	}

	protected void Flip()
	{
		//Debug.Log ("flip");
		isFacingRight = !isFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	//findPlayer(), goToPlayer..
}
