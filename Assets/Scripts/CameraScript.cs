using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	private float dampTime = 0.15f;
	public Transform target;
	private Vector3 velocity = Vector3.zero;

	void Start () 
	{
        System.Threading.Thread.Sleep(1000);    // TODO: !! add isSpawnedCheck instead of waiting
        target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update () //TODO refactor it
	{
		Vector3 point = GetComponent<Camera>().WorldToViewportPoint(new Vector3(target.position.x, target.position.y+0.75f,target.position.z));
		Vector3 delta = new Vector3(target.position.x, target.position.y+0.5f,target.position.z) - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
		Vector3 destination = transform.position + delta;

		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
	}
}
