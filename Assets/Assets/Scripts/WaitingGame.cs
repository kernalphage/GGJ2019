using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingGame : MonoBehaviour {

    public float waitingTime;
    public UnityEngine.UI.Text t;
    public GameObject winScreen;
    public GameObject doge;
    public float rotx = 0;
    public float rotz = 0;
    public Vector3 rotA;
    public Vector3 rotB;
    public float rotSpeed;
    public float decay = .25f;


    // Use this for initialization
    void Start () {
// StartCoroutine(WaitForIt());
    }
	IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(waitingTime);
        
    }
    // Update is called once per frame
    void Update () {
        string[] axes = { "Horizontal", "Vertical", "Fire1", "Fire2" };
        bool pressed = Input.anyKeyDown;
        foreach (var axis in axes)
        {
            rotz += Mathf.Abs(Input.GetAxis(axis)) * Time.deltaTime * rotSpeed;
        }

        if (pressed)
        {
            rotx += Time.deltaTime * rotSpeed;
        }
        rotx -= rotx * Time.deltaTime * decay;
        rotx = Mathf.Clamp01(rotx);
        rotz -= rotz * Time.deltaTime * decay;
        rotz = Mathf.Clamp01(rotz);
        doge.transform.rotation = Quaternion.Euler(rotA * rotx + rotB * rotz);

    }
}
