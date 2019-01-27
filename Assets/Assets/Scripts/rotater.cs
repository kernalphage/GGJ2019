using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class rotater : MonoBehaviour {
    public Vector3 s;
    public Vector3 c;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * s.x + s.y) * s.z + Mathf.Sin(Time.time * c.x + c.y) * c.z);
	}
}
