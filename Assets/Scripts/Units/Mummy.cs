using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mummy : Enemy 
{
	protected override void Start ()
	{
		animator = GetComponent<Animator>(); // can't be moved to base (super) constructor ?
		speed = (basicSpeed * 2) + speedIncreaser;
		state = EnemyState.PURSUIT;
	}
		
}
