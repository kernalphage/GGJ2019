using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameLoopManager : MonoBehaviour
{
    private enum MinigameTypes { notMinigame, Dig, ChaseTail, ToyDestroy }
    private MinigameTypes curMinigame;

    [SerializeField]
    [Tooltip("Meant to be set from the editor")]
    private float DigDuration = 10.0f, ChaseTailDuration = 10.0f, ToyDestroyDuration = 10.0f;
    private float curMinigameDuration = 0.0f;

    private Text currentcountDownText = null;
    private Slider currentbottomSlider = null;

    private DogHomeSceneManager mySceneManager = null;
    private ResultsScreenHolder resultScreenHolder = null;


    /// <summary>
    /// Gets called when a button for minigame selection is called.
    /// 
    /// Note that the int arg corresponds directly to Build order set in Unity Build settings.
    /// </summary>
    /// <param name="_minigameScene"></param>
    public void NotifyMinigameManagerOfNewMinigame(int _minigameScene)
    {


        switch (_minigameScene)
        {
            ///We should go here if we are re-entering the main scene.
            default:
                curMinigame = MinigameTypes.notMinigame;
                //Reset this bad boy
                curMinigameDuration = 0.0f;
                break;
            case (3):
                curMinigame = MinigameTypes.Dig;
                curMinigameDuration = DigDuration;
                break;
            case (4):
                curMinigame = MinigameTypes.ChaseTail;
                curMinigameDuration = ChaseTailDuration;
                break;
            case (5):
                curMinigame = MinigameTypes.ToyDestroy;
                curMinigameDuration = ToyDestroyDuration;
                break;

        }

        Debug.Log("MinigameLoopManager.NotifyMinigameGamaner reporting for duty. Current Minigame type: " + curMinigame);
    }

    ///Need an event for once we finished loading into the minigame scene to set the duration of that scene + update the relevant UI
    ///Specifically, we only want the minigame countdown timer to appear in the UI if we are entering a minigame scene. And need to de-activate it when we leave.
    public void MinigameSceneTransition(bool _startedNewMinigame)
    {


        //Set reference to these guys first.
        currentcountDownText = FindObjectOfType<CountdownTimer>().GetComponent<Text>();
        currentbottomSlider = FindObjectOfType<MinigameCountdownSlider>().GetComponent<Slider>();
        resultScreenHolder = FindObjectOfType<ResultsScreenHolder>();

        FindObjectOfType<MainSceneButtonTag>().gameObject.SetActive(!_startedNewMinigame);


        resultScreenHolder.gameObject.GetComponentInChildren<Image>().enabled = false;
        resultScreenHolder.gameObject.GetComponentInChildren<Text>().enabled = false;


        currentbottomSlider.gameObject.SetActive(_startedNewMinigame);
        //Set relevant gameobjects on or off based on off it is a minigame scene or not.
        currentcountDownText.gameObject.SetActive(_startedNewMinigame);
        currentcountDownText.enabled = _startedNewMinigame;
        currentcountDownText.text = "Countdown: " + curMinigameDuration;

        StartCoroutine(MinigameHandler());

        //Debug.Log(_startedNewMinigame);

    }

    IEnumerator MinigameHandler()
    {

        float timer = 0.0f;
        float totalTimer = curMinigameDuration;
        while (timer <= totalTimer)
        {
            timer += Time.deltaTime;
            curMinigameDuration -= Time.deltaTime;
            currentcountDownText.text = "Countdown: " + curMinigameDuration;
            currentbottomSlider.value = curMinigameDuration / totalTimer;




            //GetComponent<DogHomeSceneManager>().BackToMainScene();

            yield return null;
        }
        EvaluateResults();


        yield return null;
    }


    private void EvaluateResults()
    {

    }

}
