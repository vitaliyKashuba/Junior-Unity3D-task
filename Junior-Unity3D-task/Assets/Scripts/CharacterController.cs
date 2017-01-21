using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

	public float maxSpeed = 2f; 
	private Animator animator;
	bool isFacingRight = true;

	void Start () 
	{
		animator = GetComponent<Animator>();
	}

	void Update () 
	{
		
	}

	private void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		//animator.SetFloat("Speed", Mathf.Abs(moveHorizontal+moveVertical));
		if (moveHorizontal != 0 || moveVertical != 0) 
		{
			animator.SetBool ("isRun", true);
		} else 
		{
			animator.SetBool ("isRun", false);
		}

		GetComponent<Rigidbody2D>().velocity = new Vector2 (moveHorizontal * maxSpeed, moveVertical * maxSpeed);
		//GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x, moveVertical * maxSpeed);

		if (moveHorizontal > 0 && !isFacingRight) 
		{
			Flip ();
		} 
		else if (moveHorizontal < 0 && isFacingRight) 
		{
			Flip ();
		}

	}	
		
	private void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
