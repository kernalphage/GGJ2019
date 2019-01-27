using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public struct Paw
{
   public GameObject target;
   public float t;
   public Vector3 startPos;
   public Vector3 endPos;
   public string axis;
    public float deadzone;
    public dig lastState;
    public List<Sprite> sprites;
    public float audioDelay;
    public float curaudio;
    public enum dig
    {
        Down, 
        Move,
        Up,
    }

    public bool update(float moveSpeed, float snapSpeed)
    {
        var audio = target.GetComponent<AudioSource>();
        curaudio -= Time.deltaTime;
        if(audio.isPlaying && curaudio < 0)
        {
            audio.Stop();
        }
        t += Mathf.Sin(Time.time) * Time.deltaTime;
        t += Input.GetAxis(axis) * moveSpeed * Time.deltaTime;
        if (Input.GetAxis(axis) < .1)
        {
            t = Mathf.Lerp(t, 0, Time.deltaTime * snapSpeed);
        }
        t = Mathf.Clamp01(t);
        target.transform.position = Vector3.Lerp(endPos, startPos, 1-t);
        if (t < deadzone)
        {
            bool finishedDig = lastState != dig.Up;
            lastState = dig.Up;

            if (finishedDig)
            {
                Debug.Log("Finished dig");
                audio.time = audioDelay * Random.value;
                audio.pitch = .85f + Random.value * .3f;
                audio.PlayDelayed(0);
                curaudio = audioDelay;
            }
            return finishedDig;
        }
        else if (t > 1.0f - deadzone)
        {
            lastState = dig.Down;
        }

        target.transform.Find("Pawb").GetComponent<SpriteRenderer>().sprite = sprites[Mathf.FloorToInt(kp.RangeMap(t, 0, 1, 0, sprites.Count - 1, true))];
        return false;
    }
    
}

public class DigController : MonoBehaviour
{
    [Header("Paws")]
    public Paw left;
    public Paw right;
    public float moveSpeed;
    public float snapSpeed;

    [Header("Bone")]
    public GameObject bone;
    public Vector3 endBonePos;
    public Vector3 startBonePos;
    public Vector3 flyBonePos;
    public float spinSpeed;
    public int animFrames;
    [Header("dirt")]
    public GameObject dirt;
    public Vector3 dirtStart;
    public Vector3 dirtEnd;
    public float jiggle;
    [Header("clod")]
    public GameObject clod;
    public float clodX;
    public float clodY;
    public float clodAngle;
    [Header("Gamestate")]
    public int maxDigs = 40;
    public GameObject score;
    private UnityEngine.UI.Text t;
    int digs = 0;
    float curAudioTime;

    public state curState;
    public enum state
    {
        digging, 
        flying,
    }

    public int Digs
    {
        get
        {
            return digs;
        }

        set
        {
            digs = value;
        }
    }

    // Use this for initialization
    void Start()
    {

        right.sprites = left.sprites;
        t = score.GetComponent<UnityEngine.UI.Text>();
        curState= state.digging;
    }

    void dug()
    {
        Digs++;
        bone.transform.position = Vector3.Lerp(startBonePos, endBonePos, ((float)digs) / maxDigs);
        dirt.transform.position = Vector3.Lerp(dirtStart,dirtEnd,  ((float)digs) / maxDigs) + Vector3.right * Mathf.Sin(6.28f * Random.value) * jiggle;
        if (Digs >= maxDigs)
        {
            StartCoroutine(FlyAnim());
        }
        var d = Instantiate(clod);
        Vector3 pos = Random.insideUnitCircle;
        pos.x *= clodX;
        pos.y *= clodY;
        d.transform.position = pos;
        d.transform.rotation = Quaternion.Euler(0, 0, (Random.value * 2 - 1) * clodAngle );
    }

    IEnumerator FlyAnim()
    {
        Debug.Log("Starting fly anim");
        curState = state.flying;
        for (int i=0; i < animFrames; i++)
        {
            bone.transform.rotation = Quaternion.Euler(0, 0, i * spinSpeed);
            bone.transform.position = Vector3.Lerp(endBonePos, flyBonePos, ((float)i) / animFrames);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Ending fly anim");
        Digs = 0;
        bone.transform.rotation = Quaternion.identity;
        curState = state.digging;
    }
    // Update is called once per frame
    void Update()
    {
        if(curState == state.flying)
        {
            return;
        }

        if (left.update(moveSpeed, snapSpeed))
        {
            dug();
        }
        if(right.update(moveSpeed, snapSpeed))
        {
            dug();
        }
        //Un commenting this will put the "Diggs Dugged" text + score on screen
        //  t.text = "Diggs Dugged: " + Digs;

    }
}
