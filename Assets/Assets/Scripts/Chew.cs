using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chew : MonoBehaviour
{
    public GameObject mouth;
    public GameObject bone;


    public string[] sequence;
    public int idx;

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
            idx = (idx + 1) % sequence.Length;
          
        Debug.Log("now press " + idx);
        }
        //mouth.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(mouthMin, mouthMax, mouthT));
        //bone.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(boneMin, boneMax, boneT));
    }
}
