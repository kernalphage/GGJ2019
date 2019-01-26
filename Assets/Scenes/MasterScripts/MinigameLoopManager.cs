using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


#region Struct things
[System.Serializable]
public struct MinigameRulesAndConditions
{


    //public float DigDuration = 10.0f, ChaseTailDuration = 10.0f, ToyDestroyDuration = 10.0f;
    public float myDuration;

    //[Tooltip("If you don't get over meh threshold, player receives 0 star.")]
    //public int mehThreshold;
    [Tooltip("If player does not meet this threshold; return 1 star. If they meet this threshold but not bestDoggo threshold, 2 stars.")]
    public int goodDoggoThreshold;
    [Tooltip("if player exceeds this threshold, 3 stars.")]
    public int bestDoggohreshold;
}
#endregion

public class MinigameLoopManager : MonoBehaviour
{
    private enum MinigameTypes { notMinigame, Dig, ChaseTail, chewMinigame }
    private MinigameTypes curMinigame;
    private enum MinigameResult { meh, goodDoggo, bestDoggo };
    private MinigameResult curMinigameResult = MinigameResult.meh;

    [Tooltip("Meant to be set from the editor")]
    public MinigameRulesAndConditions digMinigame, chaseTailMinigame, chewMinigame;

    private float curMinigameDuration = 0.0f;
    private int /*curMehThreshold,*/ curGoodDogThreshold, curBestestDoggoThreshold;
    private int curScore = 0;



    private Text currentcountDownText = null;
    private Slider currentbottomSlider = null;

    //  private DogHomeSceneManager mySceneManager = null;
    private ResultsScreenHolder resultScreenHolder = null;
    private DreamHomeScreenTag dreamHomeHolder = null;
    private GameObject scoreTracker = null;


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


            ///There's definitely better way to handle this lol.
            case (1):
                curMinigame = MinigameTypes.Dig;
                curMinigameDuration = digMinigame.myDuration;
                //   curMediumThreshold = digMinigame.mehThreshold;
                curGoodDogThreshold = digMinigame.goodDoggoThreshold;
                curBestestDoggoThreshold = digMinigame.bestDoggohreshold;
                break;
            case (2):
                curMinigame = MinigameTypes.ChaseTail;
                curMinigameDuration = chaseTailMinigame.myDuration;
                //  curMediumThreshold = chaseTailMinigame.mediumScoreThreshold;
                curGoodDogThreshold = chaseTailMinigame.goodDoggoThreshold;
                curBestestDoggoThreshold = chaseTailMinigame.bestDoggohreshold;
                break;
            case (3):
                curMinigame = MinigameTypes.chewMinigame;
                curMinigameDuration = chewMinigame.myDuration;
                // curMediumThreshold = chewMinigame.mediumScoreThreshold;
                curGoodDogThreshold = chewMinigame.goodDoggoThreshold;
                curBestestDoggoThreshold = chewMinigame.bestDoggohreshold;
                break;

        }

        Debug.Log("MinigameLoopManager.NotifyMinigameGamaner reporting for duty. Current Minigame type: " + curMinigame);
    }

    ///Need an event for once we finished loading into the minigame scene to set the duration of that scene + update the relevant UI
    ///Specifically, we only want the minigame countdown timer to appear in the UI if we are entering a minigame scene. And need to de-activate it when we leave.
    public void MinigameSceneTransition(bool _startedNewMinigame)
    {


        //Set reference to these guys first. We need to find them everytime we enter a new scene because the UI is different in each scene.
        currentcountDownText = FindObjectOfType<CountdownTimer>().GetComponent<Text>();
        currentbottomSlider = FindObjectOfType<MinigameCountdownSlider>().GetComponent<Slider>();
        resultScreenHolder = FindObjectOfType<ResultsScreenHolder>();
        dreamHomeHolder = FindObjectOfType<DreamHomeScreenTag>();
        scoreTracker = FindObjectOfType<ScoreTrackerTag>().gameObject;

        FindObjectOfType<MainSceneButtonTag>().gameObject.SetActive(!_startedNewMinigame);


        //Setting individual components inactive because we need the original parent holder active at the start of a scene in order to set references correctly.
        resultScreenHolder.ToggleResultsScreen(false);

        //Turn off dream scene holder 

        dreamHomeHolder.gameObject.SetActive(!_startedNewMinigame);
        currentbottomSlider.gameObject.SetActive(_startedNewMinigame);
        //Set relevant gameobjects on or off based on off it is a minigame scene or not.
        currentcountDownText.gameObject.SetActive(_startedNewMinigame);
        currentcountDownText.enabled = _startedNewMinigame;
        if (null != scoreTracker)
        {
            scoreTracker.GetComponent<Text>().text = "";
        }

        currentcountDownText.text = "Countdown: " + curMinigameDuration.ToString("0");

        //Only run countdown timer + results screen stuff if we're in a minigame.
        if (_startedNewMinigame)
            StartCoroutine(MinigameCountdownHandler());

        //Debug.Log(_startedNewMinigame);

    }

    IEnumerator MinigameCountdownHandler()
    {

        float timer = 0.0f;
        float totalTimer = curMinigameDuration;
        //Debug.Log("totalTimer: " + totalTimer);
        //Debug.Log("curMinigameDuration: " + curMinigameDuration);
        while (timer <= totalTimer)
        {
            //  Debug.Log("Timer" + timer);

            timer += Time.deltaTime;
            curMinigameDuration -= Time.deltaTime;
            currentcountDownText.text = "Countdown: " + curMinigameDuration;
            currentbottomSlider.value = curMinigameDuration / totalTimer;




            //GetComponent<DogHomeSceneManager>().BackToMainScene();

            yield return null;
        }
        if (timer > totalTimer)
        {
            EvaluateResults();
        }



        yield return null;
    }


    private void EvaluateResults()
    {
       // Debug.Log("calling EvaluateResults");

        resultScreenHolder.ToggleResultsScreen(true);

        //Which minigame did we just complete?
        switch (curMinigame)
        {
            ///We should go here if we are re-entering the main scene.
            default:
                curMinigame = MinigameTypes.notMinigame;

                //Reset this bad boy
                curMinigameDuration = 0.0f;
                break;
            case (MinigameTypes.Dig):

                curScore = FindObjectOfType<DigController>().Digs;
                CheckScoreThreshold();
                ///
                break;
            case (MinigameTypes.ChaseTail):

                break;
            case (MinigameTypes.chewMinigame):

                break;


        }




    }

    private MinigameResult CheckScoreThreshold()
    {
        //1 star
        if (curScore < curGoodDogThreshold)
        {
            curMinigameResult = MinigameResult.meh;
            resultScreenHolder.PassInResults(1);
        }
        //2 star
        else if (curScore >= curGoodDogThreshold && curScore < curBestestDoggoThreshold)
        {
            curMinigameResult = MinigameResult.goodDoggo;
            resultScreenHolder.PassInResults(2);

        }
        //3 stars
        else if (curScore >= curBestestDoggoThreshold)
        {
            curMinigameResult = MinigameResult.bestDoggo;
            resultScreenHolder.PassInResults(3);

        }
        return curMinigameResult;

    }
}
