using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

	public float maxSpeed = 5f; 
	private Animator animator;

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

		animator.SetFloat("Speed", Mathf.Abs(moveHorizontal+moveVertical));

		GetComponent<Rigidbody2D>().velocity = new Vector2 (moveHorizontal * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
		GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x, moveVertical * maxSpeed);

		//если нажали клавишу для перемещения вправо, а персонаж направлен влево
		/*if(move > 0 && !isFacingRight)
			//отражаем персонажа вправо
			Flip();
		//обратная ситуация. отражаем персонажа влево
		else if (move < 0 && isFacingRight)
			Flip();*/
	}		
}
