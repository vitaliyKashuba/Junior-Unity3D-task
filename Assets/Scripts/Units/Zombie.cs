using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy 
{
	protected override void Start ()
	{
		animator = GetComponent<Animator>(); // can't be moved to base (super) constructor ?
		speed = basicSpeed + speedIncreaser;
		//state = EnemyState.PURSUIT;
	}				
}
