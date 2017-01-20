using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBuilderScript : MonoBehaviour 
{
	public Rigidbody2D wall;
	public SpriteRenderer ground;
	float wallSize = 0.9f;

	void Start () 
	{
		MazePoint[,] maze = SimpleMazeGenerator.getMaze ();
		int mazeSize = maze.GetUpperBound(1); //TODO fix to be possible use non-square maze's
		mazeSize++; //upperBound returns not size, but last index
		//Debug.Log(mazeSize);
		for (int i = 0; i < mazeSize; i++) 
		{
			for (int j = 0; j < mazeSize; j++) 
			{
				if (maze [i, j].Equals (MazePoint.WALL))
				{
					Instantiate (wall, new Vector3 (i * wallSize, j * wallSize, 0), Quaternion.identity);
				} else 
				{
					if (maze [i, j].Equals (MazePoint.GROUND)) 
					{
						Instantiate (ground, new Vector3 (i * wallSize, j * wallSize, 0), Quaternion.identity);
					}
				}
			}
		}
		//wall.position.Set (position.position.x + 1.0f, position.position.y + 1.0f, position.position.z);
			//position.position.y = position.position.y + 2;
			//Instantiate (wall, new Vector3(i*wallSize, 0, 0), Quaternion.identity);// as RigidbodyType2D;
		//Instantiate (wall);// as RigidbodyType2D;

	}

	void Update () {
		
	}
}
