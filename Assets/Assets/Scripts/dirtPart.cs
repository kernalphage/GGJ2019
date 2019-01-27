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
    public float endAlpha = 0;

    public bool keep = false;
    public float maxRotateSpeed = 400;
    float rotateSpeed;

	// Use this for initialization
	void Start () {
        dust = transform.Find("dustCloud").gameObject;
        startColor = dust.GetComponent<SpriteRenderer>().color;
        endColor = dust.GetComponent<SpriteRenderer>().color;
        endColor.a = endAlpha;
        t = 0;
        rotateSpeed = (Random.value * 2 - 1) * maxRotateSpeed;
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
            dust.transform.localRotation *= Quaternion.Euler(0, 0, Time.deltaTime * rotateSpeed);
            dust.GetComponent<SpriteRenderer>().color = Color.Lerp(startColor, endColor, mapt);
        }
        else if(!keep)
        {
            Destroy(gameObject);
        }
    }
}
