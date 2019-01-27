using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flipbook : MonoBehaviour {

    public float fliptime;
    private float curtime;
    public List<Sprite> sprites;
    public string onFinished;
    int idx = 0;

	// Use this for initialization
	void Start () {
        curtime = fliptime;
	}
    void setSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = sprites[idx];
    }

    void StartFlipbook(float time)
    {
        fliptime = 1;
        curtime = 0;
    }
    // Update is called once per frame
    void Update () {
        curtime -= Time.deltaTime;
        if(curtime <= 0)
        {
            curtime += fliptime;
            idx++;
            if (idx == sprites.Count && onFinished.Length > 0)
            {
                transform.SendMessage(onFinished);
            }

            idx = idx % sprites.Count;
            setSprite();
        }
    }
}
