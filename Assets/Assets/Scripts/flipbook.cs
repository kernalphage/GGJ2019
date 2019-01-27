using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flipbook : MonoBehaviour {

    public float fliptime;
    private float curtime;
    public List<Sprite> sprites;
    int idx = 0;

	// Use this for initialization
	void Start () {
        curtime = fliptime;
	}
    void setSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = sprites[idx];
    }

    // Update is called once per frame
    void Update () {
        curtime -= Time.deltaTime;
        if(curtime <= 0)
        {
            curtime += fliptime;
            idx = (idx + 1) % sprites.Count;
            setSprite();
        }
    }
}
