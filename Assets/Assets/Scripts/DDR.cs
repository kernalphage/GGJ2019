using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDR : MonoBehaviour {

    
    public List<string> axis;
    public GameObject[] buttons;
    public Vector3 dudePos;
    public List<GameObject> dudeButtons;
    public List<GameObject> dogButtons;

    public float animDelay = .2f;
    public float hoorayDelay = .5f;
    public Vector3 dogpos;
    public int[] sizes = { 3, 3, 3, 3, 4, 4, 4, 4, 5, 1 };
    public List<int> curSequence;
    public int curidx;
    public state curState = state.dude;
    public enum state
    {
        startDude,
        dude,
        dog,
        hooray,

    }
    // Use this for initialization
    void Start () {

        curState = state.startDude;
        dudeButtons = new List<GameObject>();
        dogButtons = new List<GameObject>();
	}


    void clear()
    {
        for(int i=0; i < dudeButtons.Count;i++)
        {
            Destroy(dudeButtons[i]);
            Destroy(dogButtons[i]);
        }
        dudeButtons = new List<GameObject>();
        dogButtons = new List<GameObject>();
    }
    IEnumerator fill()
    {
        curState = state.dude;    
        clear();

        List<int> idxes = new List<int>{ 0,1,2,3};
        curidx = 0;
        int len = sizes.RandomSample();
        Debug.Log("sequence len " + len);
        curSequence = new List<int>();
        for(int i=0; i <len; i++)
        {
            curSequence.Add(idxes.RandomSample());
            var dudeb = Instantiate(buttons[curSequence[i]]);
            dudeb.transform.position = dudePos + Vector3.right * i;
            dudeButtons.Add(dudeb);
            yield return new WaitForSeconds(animDelay);
        }
        curState = state.dog;
    }
    IEnumerator DoHooray()
    {
        yield return new WaitForSeconds(hoorayDelay);
        curState = state.startDude;
    }

    // Update is called once per frame
    void Update ()
    {
        if( curState == state.startDude)
        {
            StartCoroutine(fill());
        }
        if (curState == state.dog)
        {

            // TODO: add wait time before accepting input 
            if (Input.GetButtonDown(axis[curSequence[curidx]]))
            {
                var dogb = Instantiate(buttons[curSequence[curidx]]);
                dogb.transform.position = dogpos + Vector3.right * curidx;
                dogButtons.Add(dogb);

                if (curidx >= curSequence.Count - 1)
                {
                    Debug.Log("Finished sequence ");
                    StartCoroutine(DoHooray());
                    return;
                }
                curidx++;
            }
        }

    }
}
