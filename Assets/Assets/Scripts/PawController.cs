using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawController : MonoBehaviour {


	private Vector2 pos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		pos = new Vector2(Mathf.Sin(Time.time), Mathf.Cos(Time.time) * 4);
		transform.position = pos;
	}
}
