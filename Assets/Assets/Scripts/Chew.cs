using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chew : MonoBehaviour
{
    public GameObject mouth;
    public GameObject bone;
    public GameObject fluff;
    public Sprite[] anims;
    public string[] sequence;
    public float[] rotations;
    public Vector3[] sheepPos;
    public int idx;
    private float currot;
    private float targetRot;
    public float rotSpeed;
    public float tweenSpeed;

    public Vector2 clodSize;

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
    void chewFluff()
    {
        var d = Instantiate(fluff);
        Vector3 pos = Random.insideUnitCircle;
        pos.x *= clodSize.x;
        pos.y *= clodSize.y;
        pos = bone.transform.GetChild(0).TransformPoint(pos);
        d.transform.position = pos;
        d.transform.rotation = Quaternion.Euler(0, 0, (Random.value * 90 + 140));
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

            chewFluff();
            chewFluff();
            chewFluff();
        }

        currot = Mathf.LerpAngle(currot, targetRot, Time.deltaTime * rotSpeed); ;
        mouth.transform.rotation = Quaternion.Euler(0, 0, currot);
        bone.transform.localPosition = Vector3.Lerp(bone.transform.localPosition, sheepPos[idx], Time.deltaTime * tweenSpeed);
        
        //mouth.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(mouthMin, mouthMax, mouthT));
        //bone.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(boneMin, boneMax, boneT));
    }
}
