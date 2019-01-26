using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chew : MonoBehaviour {
    private GameObject mouth;
    private GameObject bone;


    public float mouthMin;
    public float mouthMax;
    public float boneMin;
    public float boneMax;

    public float speed;
    public float offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float mouthT = Mathf.Sin(Time.time * speed);
        float boneT = Mathf.Sin(Time.time * speed + offset);

        mouth.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(mouthMin, mouthMax, mouthT));
        bone.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(boneMin, boneMax, boneT));
    }
}
