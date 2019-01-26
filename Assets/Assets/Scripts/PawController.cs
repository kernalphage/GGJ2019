using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawController : MonoBehaviour {


    private Vector2 pos = Vector2.zero;
    public float moveSpeed = 1;
	public string XAxis = "Horizontal";
	public string YAxis = "Horizontal";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var left = new Vector2(Input.GetAxis(XAxis), Input.GetAxis(YAxis)) * moveSpeed * Time.deltaTime;
		pos += left;
		transform.position = pos;
	}
}
