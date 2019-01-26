using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class Chew : MonoBehaviour {
    public GameObject mouth;
    public GameObject bone;


    public string[] sequence = { "ex","ay","be","ay" };
    public int idx;

	// Use this for initialization
	void Start () {
        idx = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetButtonDown("joystick button " + sequence[idx]))
        {
            idx = (idx + 1) % sequence.Length;
            Debug.Log("now press " + idx);
        }
        //mouth.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(mouthMin, mouthMax, mouthT));
        //bone.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(boneMin, boneMax, boneT));
    }
}
