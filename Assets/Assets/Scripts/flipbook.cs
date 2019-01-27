using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flipbook : MonoBehaviour
{

    public float fliptime;
    protected float curtime;
    public List<Sprite> sprites;
    public string onFinished;
    protected int idx = 0;

    public delegate void HandleLastIndexInFlipbook();
    public static event HandleLastIndexInFlipbook callLastIndex;

    protected bool reallyFinish = true;

    // Use this for initialization
    public virtual void Start()
    {
        curtime = fliptime;
    }
    public virtual void setSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = sprites[idx];
    }

    public virtual void StartFlipbook(float time)
    {
        fliptime = 1;
        curtime = 0;
    }
    // Update is called once per frame
    public virtual void Update()
    {
        curtime -= Time.deltaTime;
        if (curtime <= 0)
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
