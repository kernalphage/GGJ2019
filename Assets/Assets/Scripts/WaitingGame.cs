using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingGame : MonoBehaviour {

    public float waitingTime;
    public UnityEngine.UI.Text t;
    public GameObject winScreen;
    public GameObject doge;
    public float rotx = 0;
    public Vector3 moveStrength;
    public float rotSpeed;
    public float decay = .25f;


    // Use this for initialization
    void Start () {
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
    void Update () {
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
        }
        rotx -= rotx * Time.deltaTime * decay;
        rotx = Mathf.Clamp01(rotx);
        doge.transform.localScale = Vector3.one + (moveStrength * rotx);

    }
}
