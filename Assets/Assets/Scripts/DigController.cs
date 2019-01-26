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
    public enum dig
    {
        Down, 
        Move,
        Up,
    }

    
    public bool update(float moveSpeed, float snapSpeed)
    {
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
            return finishedDig;
        }
        else if (t > 1.0f - deadzone)
        {
            lastState = dig.Down;
        }
        return false;
    }
    
}

public class DigController : MonoBehaviour
{
    [SerializeField]
    public Paw left;
    [SerializeField]
    public Paw right;
    public float moveSpeed;
    public float snapSpeed;
    public GameObject score;
    private UnityEngine.UI.Text t;

    int digs = 0;

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
        t = score.GetComponent<UnityEngine.UI.Text>();

    }
    
    // Update is called once per frame
    void Update()
    {
        
        if(left.update(moveSpeed, snapSpeed))
        {
            Digs++;
        }

        if(right.update(moveSpeed, snapSpeed))
        {
            Digs++;
        }
        t.text = "Score: " + Digs;
    }
}
