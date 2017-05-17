using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : Unit {

	void Start () 
	{
		animator = GetComponent<Animator>();
		isFacingRight = true;
		speed = 2f;
	}

	override protected void FixedUpdate()
	{
		base.FixedUpdate ();

		moveHorizontal = Input.GetAxis ("Horizontal");
		moveVertical = Input.GetAxis ("Vertical");

		GetComponent<Rigidbody2D>().velocity = new Vector2 (moveHorizontal * speed, moveVertical * speed);
	}	

	void OnTriggerEnter2D(Collider2D c) 
	{
		if (c.gameObject.CompareTag ("Coin")) 
		{
			GameObject.Find("DungeonMaster").SendMessage("grabCoin"); // DungeonMaster.grabCoin() but non-static
			c.gameObject.SetActive(false);
			//Destroy (c.gameObject);
		}
	}

}
