using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

	protected bool isFacingRight;
	protected float moveHorizontal, moveVertical = 0;
	protected Animator animator;
	protected float speed;

	protected void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	protected virtual void FixedUpdate()
	{
		directionCheck ();
		animationTrigger ();
	}

	void directionCheck()
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

	void animationTrigger()
	{
		if (moveHorizontal != 0 || moveVertical != 0) 
		{
			animator.SetBool ("isWalk", true);
		} else 
		{
			animator.SetBool ("isWalk", false);
		}
	}
}
