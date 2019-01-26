using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class Chew : MonoBehaviour {
    public GameObject mouth;
    public GameObject bone;


    public float mouthMin;
    public float mouthMax;
    public float boneMin;
    public float boneMax;

    public float speed;
    public float offset;
    public float lastAngle = 0;
    public float sensitivity = .1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector2 inputPos = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        float angle = Mathf.Atan2(-inputPos.y, inputPos.x);
        if(angle < lastAngle)
        {
            angle += 6.28f;
        }
        if (Mathf.Abs(angle - lastAngle) < sensitivity && inputPos.SqrMagnitude() > .5f)
        {
            lastAngle = angle;
        }
        lastAngle = lastAngle % 6.28f;
        bone.transform.rotation = Quaternion.EulerAngles(0, 0, lastAngle);

        //mouth.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(mouthMin, mouthMax, mouthT));
        //bone.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(boneMin, boneMax, boneT));
    }
}
