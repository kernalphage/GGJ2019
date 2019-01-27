using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingGame : MonoBehaviour
{

    public float waitingTime;
    public UnityEngine.UI.Text t;
    public GameObject winScreen;
    public GameObject doge;
    public float rotx = 0;
    public Vector3 moveStrength;
    public float rotSpeed;
    public float decay = .25f;
    [Header("Wag")]
    public float wagSpeed;
    public float wagAmount;
    public float fuckupAmount;
    public float wagStart;
    public GameObject tail;

    [Header("Sweat")]
    public GameObject sweat;
    public Vector3 sweatPos;
    public Vector3 sweatArea;
    public float sweatProbability;

    /// <summary>
    /// 
    /// </summary>
    private int fuckupCounter = 0;

    public int FuckupCounter
    {
        get
        {
            return fuckupCounter;
        }

        set
        {
            fuckupCounter = value;
        }
    }


    // Use this for initialization
    void Start()
    {
        (winScreen.GetComponent<SpriteRenderer>()).color = Color.clear;
        StartCoroutine(WaitForIt());
    }
    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(waitingTime);
        (winScreen.GetComponent<SpriteRenderer>()).color = Color.white;
        // Win goes here
    }
    // Update is called once per frame
    void Update()
    {
        string[] axes = { "Horizontal", "Vertical", "Fire1", "Fire2" };
        bool pressed = Input.anyKeyDown;
        foreach (var axis in axes)
        {
            //   rotz += Mathf.Abs(Input.GetAxis(axis)) * Time.deltaTime * rotSpeed;
            pressed |= Mathf.Abs(Input.GetAxis(axis)) > .5f;
        }

        if (pressed)
        {
            rotx += Time.deltaTime * rotSpeed;
            FuckupCounter++;

            if(Random.value < sweatProbability)
            {
                var s = Instantiate(sweat);
                s.transform.position = Vector3.Scale(Random.insideUnitSphere, sweatArea) + sweatPos; 
            }
        }
        rotx -= rotx * Time.deltaTime * decay;
        rotx = Mathf.Clamp01(rotx);
        doge.transform.localScale = Vector3.one + (moveStrength * rotx);

        float wag = wagStart + Mathf.Sin(wagSpeed * Time.time) * wagAmount;
        tail.transform.rotation = Quaternion.Euler(0, 0, fuckupCounter * fuckupAmount + wag);

    }
}
