using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dirtPart : MonoBehaviour {

    float t;
    public float animTime;
    public Vector3 jump;
    public Vector3 fade;
    GameObject dust;
    Color startColor;
    Color endColor;

    float rotateSpeed;

	// Use this for initialization
	void Start () {
        dust = transform.Find("dustCloud").gameObject;
        startColor = dust.GetComponent<SpriteRenderer>().color;
        endColor = dust.GetComponent<SpriteRenderer>().color;
        endColor.a = 0;
        t = 0;
        rotateSpeed = (Random.value * 2 - 1) * 400;
	}
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime;
        if(t < animTime)
        {
            dust.transform.localPosition = Vector3.Lerp(Vector3.zero, jump, t / animTime);
            dust.transform.localRotation *= Quaternion.Euler(0, 0, Time.deltaTime * rotateSpeed);
        }
        else if(t < animTime * 2)
        {
            float mapt = kp.RangeMap(t, animTime, animTime * 2, 0, 1);
            dust.transform.localPosition = Vector3.Lerp(jump, fade, mapt);
            dust.GetComponent<SpriteRenderer>().color = Color.Lerp(startColor, endColor, mapt);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
