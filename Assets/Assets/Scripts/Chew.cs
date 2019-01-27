using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chew : MonoBehaviour
{
    public GameObject mouth;
    public GameObject bone;
    public Sprite[] anims;
    public string[] sequence;
    public float[] rotations;
    public int idx;
    private float currot;
    private float targetRot;
    public float rotSpeed;


    private int score = 0;

    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        idx = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(sequence[idx]))
        {
            mouth.GetComponent<SpriteRenderer>().sprite = anims[idx];
            targetRot = rotations[idx];
            idx = (idx + 1) % sequence.Length;          
            Debug.Log("now press " + idx);
        }
        currot = Mathf.LerpAngle(currot, targetRot, Time.deltaTime * rotSpeed); ;
        mouth.transform.rotation = Quaternion.Euler(0, 0, currot);
        //mouth.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(mouthMin, mouthMax, mouthT));
        //bone.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(boneMin, boneMax, boneT));
    }
}
