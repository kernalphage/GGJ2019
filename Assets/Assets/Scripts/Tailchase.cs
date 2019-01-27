using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tailchase : MonoBehaviour {

    public float speed;
    public float offset;
    public float lastAngle = 0;
    public float sensitivity = .1f;
    public Vector2 mousepos;
    public GameObject bone;
    public GameObject meter;
    public Sprite superSprite;
    public Sprite originalSprite;
    public float rpm = 0;
    public float rotations = 0;
    public float superspeed = 3;
    public float maxRotations = 100;
    public float friction = .2f;
    public float acceleration = 3;
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
        originalSprite = bone.GetComponent<SpriteRenderer>().sprite;
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 inputPos = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        float angle = Mathf.Atan2(-inputPos.y, inputPos.x);
        if (angle < lastAngle)
        {
            angle += 6.28f;
        }
        if (Mathf.Abs(angle - lastAngle) < sensitivity && inputPos.magnitude > .5f)
        {
            rpm += Vector3.Distance(mousepos, inputPos) * Time.deltaTime * acceleration;
            lastAngle = angle;
            mousepos = inputPos;
        }

        float curAngle = lastAngle;
        lastAngle = lastAngle % 6.28f;
        if(lastAngle < curAngle)
        {
            NumSpins++;
            Debug.Log("NumSpins: " + NumSpins);
        }


        rpm -= rpm * friction * Time.deltaTime;
        rotations += rpm * Time.deltaTime;

        bone.transform.rotation = Quaternion.EulerAngles(0, 0, rotations);
        if (rpm > superspeed)
        {
            bone.GetComponent<SpriteRenderer>().sprite = superSprite;
        }
        else
        {
            bone.GetComponent<SpriteRenderer>().sprite = originalSprite;
        }
        meter.transform.localScale = new Vector3(rotations / maxRotations, 1, 1);

        //mouth.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(mouthMin, mouthMax, mouthT));
        //bone.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(boneMin, boneMax, boneT));
    }
}
