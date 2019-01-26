using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicColorLerp : MonoBehaviour
{
    [SerializeField]
    private Color start, end;

    [SerializeField]
    private float speed;

    [SerializeField]
    private bool imageActive = false;

    private Image  myImage = null;


    private 
    // Use this for initialization
    void Start()
    {
        speed = 1 / speed;
        if (imageActive)
        {
            myImage = GetComponent<Image>();
        }


        ColorFlash(true);
    }


    IEnumerator ColorFlash(bool fadeAway)
    {
        float timer = 0.0f;
        while (timer < 1.0f)
        {
            timer += Time.deltaTime;
            if (imageActive)
            {
                if (fadeAway)
                {
                    myImage.color = Color.Lerp(start, end, timer * speed);
                }
                else
                {
                    myImage.color = Color.Lerp(end, start, timer * speed);
                }
               
            }
            yield return null;
        }

        //Toggle bool, restart coroutine.
        StartCoroutine(ColorFlash(!fadeAway));
    }   
}
