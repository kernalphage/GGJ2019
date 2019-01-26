using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tailchase : MonoBehaviour {

    public float speed;
    public float offset;
    public float lastAngle = 0;
    public float sensitivity = .1f;
    private int numSpins;

    public int NumSpins
    {
        get
        {
            return numSpins;
        }

        set
        {
            numSpins = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 inputPos = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        float angle = Mathf.Atan2(-inputPos.y, inputPos.x);
        if (angle < lastAngle)
        {
            angle += 6.28f;
        }
        if (Mathf.Abs(angle - lastAngle) < sensitivity && inputPos.SqrMagnitude() > .5f)
        {
            lastAngle = angle;
        }

        float curAngle = lastAngle;
        lastAngle = lastAngle % 6.28f;
        if(lastAngle < curAngle)
        {
            NumSpins++;
        }

        Debug.Log("NumSpins: " + NumSpins);
        //bone.transform.rotation = Quaternion.EulerAngles(0, 0, lastAngle);

        //mouth.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(mouthMin, mouthMax, mouthT));
        //bone.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(boneMin, boneMax, boneT));
    }
}
