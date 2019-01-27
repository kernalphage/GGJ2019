using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashAndStretchSprite : MonoBehaviour {

    [SerializeField]
    [Tooltip("Set the start scale size for lerping purposes")]
    private Vector3 startScale;
    public Vector3 GetSetStartScale { get { return startScale; } set { startScale = value; } }

    [SerializeField]
    [Tooltip("Set the end size for lerping purposes")]
    private Vector3 endScale;
    public Vector3 GetSetEndScale { get { return endScale; } set { endScale = value; } }

    public float Timer
    {
        get
        {
            return timer;
        }

        set
        {
            timer = value;
        }
    }

    private enum ScaleStates { scalingUp, scalingDown }
    private ScaleStates state = ScaleStates.scalingUp;

    private float timer;

    [SerializeField]
    private float timerMultipler = 1;
    private float ogMultiplier = 0.0f;

    private void Start()
    {
        ogMultiplier = timerMultipler; //Store this og
    }
    void Update()
    {
        switch (state)
        {
            default:
            case (ScaleStates.scalingUp):
                ScaleUp();
                if (Timer > 1.0)
                {
                    Timer = 0;
                    state = ScaleStates.scalingDown;
                }
                break;
            case (ScaleStates.scalingDown):
                ScaleDown();
                if (Timer > 1.0)
                {
                    Timer = 0;
                    state = ScaleStates.scalingUp;
                }
                break;
        }
    }

    private void ScaleUp()
    {
        Timer += Time.deltaTime * timerMultipler;
        transform.localScale = Vector3.Lerp(startScale, endScale, Timer);
    }

    private void ScaleDown()
    {
        Timer += Time.deltaTime * timerMultipler;
        transform.localScale = Vector3.Lerp(endScale, startScale, Timer);
    }

    private void ResetScale()
    {
        Timer += Time.deltaTime;
        transform.localScale = Vector3.Lerp(startScale, endScale, Timer);
    }


    public IEnumerator ResetSquashAndStretch()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        timerMultipler = 0.0f;
        yield return new WaitForSeconds(2.0f);
        timerMultipler = ogMultiplier;
        //yield return null;
    }
}
