using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDR : MonoBehaviour
{
    [SerializeField]
    private Transform dogTransform;
    private Transform finalTransform;

    private Vector3 DogpositionWithOffset;

   

    [SerializeField]
    private Vector3 offset;

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

    private DDRDogBehavior DDRDogBehavior = null;
    private SquashAndStretchSprite DogSquashAndStretch = null;


    public enum state
    {
        startDude,
        dude,
        dog,
        hooray,

    }


    [SerializeField]
    private GameObject heartParticle = null;
    // Use this for initialization
    void Start()
    {
        DogpositionWithOffset = dogTransform.position + offset;


        curState = state.startDude;
        dudeButtons = new List<GameObject>();
        dogButtons = new List<GameObject>();

        DDRDogBehavior = FindObjectOfType<DDRDogBehavior>();
        DogSquashAndStretch = DDRDogBehavior.gameObject.GetComponentInChildren<SquashAndStretchSprite>();
    }


    void clear()
    {
        for (int i = 0; i < dudeButtons.Count; i++)
        {
            Destroy(dudeButtons[i]);
        }
        for (int j = 0; j < dogButtons.Count; j++)
        {
            Destroy(dogButtons[j]);
        }
        dudeButtons = new List<GameObject>();
        dogButtons = new List<GameObject>();
    }
    IEnumerator fill()
    {
        curState = state.dude;
        clear();

        List<int> idxes = new List<int> { 0, 1, 2, 3 };
        curidx = 0;
        int len = sizes.RandomSample();
        //  Debug.Log("sequence len " + len);
        curSequence = new List<int>();
        for (int i = 0; i < len; i++)
        {
            var index = idxes.RandomSample();
            curSequence.Add(index);
            var dudeb = Instantiate(buttons[index % buttons.Length] );
            dudeb.transform.position = dudePos + Vector3.right * i * 1.2f;
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
    void Update()
    {
        if (curState == state.startDude)
        {
            StartCoroutine(fill());
        }
        if (curState == state.dog)
        {

            // TODO: add wait time before accepting input 
            if (Input.GetButtonDown(axis[curSequence[curidx]]))
            {
                //var dogb = Instantiate(buttons[curSequence[curidx]]);
                //dogb.transform.position = dogpos + Vector3.right * curidx;
                //dogButtons.Add(dogb);
                dudeButtons[curidx].GetComponent<ButtonPFI>().ActivateSecondSprite();

                //Basic feedback particle
                HandleParticleFeedback(curidx);

                HandleAnimationFeedback(axis[curSequence[curidx]]);

                if (curidx >= curSequence.Count - 1)
                {
                    //  Debug.Log("Finished sequence ");
                    StartCoroutine(DoHooray());
                    return;
                }
                curidx++;
            }
            else if (Input.anyKeyDown)
            {
                //   Debug.Log("anykey returning true");
                HandleAnimationFeedback("sad");
            }
        }

    }

    private void HandleParticleFeedback(int _curidx)
    {
        if (null != heartParticle)
        {
            GameObject obj;
            obj = Instantiate(heartParticle, dogTransform);

            obj.GetComponent<HeartParticleComponent>().UpdateBurstAmount(_curidx, false, 0.0f);
        }

    }

    private int curAnimInt = 0;
    private int lastAnimInt = 0;

    private void HandleAnimationFeedback(string _Input)
    {

        Debug.Log("Is this calling");

        switch (_Input)
        {
            case ("ex"):

                curAnimInt = 1;
                break;
            case ("be"):

                curAnimInt = 4;
                break;
            case ("ay"):

                curAnimInt = 3;
                break;
            case ("wy"):

                curAnimInt = 2;

                break;
            case ("sad"):
                curAnimInt = -1;
                break;
            default:

                curAnimInt = 0;
                break;

        }


        if (curAnimInt != lastAnimInt)
        {
            //Only call if it's different
            DDRDogBehavior.UpdateAnimationConditions(curAnimInt);

        }
        else if (curAnimInt == lastAnimInt)
        {

            //If it's the same as the last input, we should restart the squash and stretch
            DogSquashAndStretch.ResetSquashAndStretch();


        }
        //And lastly, update lastAnimInt;
        curAnimInt = lastAnimInt;

        // Debug.Log("Update Animation Feedback");

    }


}
