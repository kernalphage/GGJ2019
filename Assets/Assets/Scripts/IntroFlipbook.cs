using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IntroFlipbook : flipbook
{

    [SerializeField]
    private SpriteRenderer specialSprite;

    private bool stopThePresses = false;

    public override void Start()
    {
        base.Start();

        if (specialSprite.gameObject.activeInHierarchy && null != specialSprite.gameObject)
        {
            //If it's currently active, turn it off.
            specialSprite.gameObject.SetActive(false);
        }
        Debug.Log("Binding");
        idx = -1;
    }


    ///Need a way so that once we get to the last frame, we hold off on the normal transition that flipbook does; 
    ///this COULD be done by just setting flipbook to 30 while we wait for the motion to play out :3
    ///
    private IEnumerator HandleSpecialIntroLastIndexSprite()
    {
        //do this immediately, no delay.
        stopThePresses = true;
        idx++;
        idx = sprites.Count - 1; //grab last sprite
        reallyFinish = false;
        setSprite();
        //This is all crap but it's going to work so eat my shorts.
        Debug.Log("Shane is a god");
        yield return new WaitForSeconds(3.0f);

        //Do things after  delay yay..like alpha fading out previous sprite, alpha fading in new image.
        if (null != specialSprite) //turn on the doggo.
            specialSprite.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        if (null != specialSprite.gameObject.GetComponent<Animator>())
        {
            if (null != specialSprite.gameObject.GetComponent<Animator>().runtimeAnimatorController)
            {
                specialSprite.gameObject.GetComponent<Animator>().SetBool("initiateTailWag", true);

            }
        }

        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("DreamHomeScene");
        //fliptime = 80;

    }

    public override void Update()
    {
        //base.Update();
        curtime -= Time.deltaTime;
        if (curtime <= 0)
        {
            if (!stopThePresses)
            {
                curtime += fliptime;
                idx++;

            }

            if (idx == sprites.Count)
            {

                StartCoroutine(HandleSpecialIntroLastIndexSprite());
                if (onFinished.Length > 0 && reallyFinish) //way to limit the serious ending here.
                {
                    transform.SendMessage(onFinished);
                }



            }

            idx = idx % sprites.Count;
            setSprite();
        }
    }

}
