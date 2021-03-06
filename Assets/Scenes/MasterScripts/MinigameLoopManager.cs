﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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


[System.Serializable]
public struct OverallGameRulesAndConditions
{

    public int currentMinigameCompletedCounter;
    [Tooltip("Set the amount of minigames we need completed.")]
    public int totalMinigameCompletionCounter; //Find a way to set this at runtime and base it off how many scenes we have active in the build? L O L.

    [Tooltip("In case it's not obvious, this bool being set to true is what triggers the wonderful Culmination DDR scene huzzah")]
    public bool superduperfuckingcompleted;

}

#endregion

public class MinigameLoopManager : MonoBehaviour
{
    private enum MinigameTypes { notMinigame, Dig, chewMinigame, ChaseTail, DDRfinalGame, couchGame }
    private MinigameTypes curMinigame;
    private enum MinigameResult { meh, goodDoggo, bestDoggo };
    private MinigameResult curMinigameResult = MinigameResult.meh;

    [Header("Individual win conditions for each minigame.")]
    [Tooltip("Meant to be set from the prefab editor")]
    public MinigameRulesAndConditions digMinigame, chewMinigame, chaseTailMinigame, DDRminigame, CouchGame;

    [Header("Overall win conditions for the game.")]
    [Tooltip("Meant to be set from the editor")]
    public OverallGameRulesAndConditions gameConditions;

    private float curMinigameDuration = 0.0f;
    private int /*curMehThreshold,*/ curGoodDogThreshold, curBestestDoggoThreshold;
    private int curScore = 0;

    private bool doMinigameCountdown = false;
    private AudioSource myAudioSource = null;
    [SerializeField]
    private AudioClip loseSound, winSound;


    private Text currentcountDownText = null;
    private Slider currentbottomSlider = null;
    private pawClicker pawClicker = null;

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
            case (2):
                curMinigame = MinigameTypes.Dig;
                curMinigameDuration = digMinigame.myDuration;
                //   curMediumThreshold = digMinigame.mehThreshold;
                curGoodDogThreshold = digMinigame.goodDoggoThreshold;
                curBestestDoggoThreshold = digMinigame.bestDoggohreshold;
                break;


            case (3):
                curMinigame = MinigameTypes.chewMinigame;
                curMinigameDuration = chewMinigame.myDuration;
                // curMediumThreshold = chewMinigame.mediumScoreThreshold;
                curGoodDogThreshold = chewMinigame.goodDoggoThreshold;
                curBestestDoggoThreshold = chewMinigame.bestDoggohreshold;
                break;
            //case (3):
            //    curMinigame = MinigameTypes.ChaseTail;
            //    curMinigameDuration = chaseTailMinigame.myDuration;
            //    //  curMediumThreshold = chaseTailMinigame.mediumScoreThreshold;
            //    curGoodDogThreshold = chaseTailMinigame.goodDoggoThreshold;
            //    curBestestDoggoThreshold = chaseTailMinigame.bestDoggohreshold;

            //    break;
            case (4):
                curMinigame = MinigameTypes.DDRfinalGame;
                curMinigameDuration = DDRminigame.myDuration;
                //  curMediumThreshold = chaseTailMinigame.mediumScoreThreshold;
                curGoodDogThreshold = DDRminigame.goodDoggoThreshold;
                curBestestDoggoThreshold = DDRminigame.bestDoggohreshold;
                break;
            case (5):
                curMinigame = MinigameTypes.couchGame;
                curMinigameDuration = CouchGame.myDuration;

                curGoodDogThreshold = CouchGame.goodDoggoThreshold;
                curBestestDoggoThreshold = CouchGame.bestDoggohreshold;
                break;

        }

        // Debug.Log("MinigameLoopManager.NotifyMinigameGamaner reporting for duty. Current Minigame type: " + curMinigame);
    }

    ///Need an event for once we finished loading into the minigame scene to set the duration of that scene + update the relevant UI
    ///Specifically, we only want the minigame countdown timer to appear in the UI if we are entering a minigame scene. And need to de-activate it when we leave.
    public void MinigameSceneTransition(bool _startedNewMinigame)
    {


        //Set reference to these guys first. We need to find them everytime we enter a new scene because the UI is different in each scene.
        if (null != FindObjectOfType<CountdownTimer>())
            currentcountDownText = FindObjectOfType<CountdownTimer>().GetComponent<Text>();
        if (null != FindObjectOfType<MinigameCountdownSlider>())
            currentbottomSlider = FindObjectOfType<MinigameCountdownSlider>().GetComponent<Slider>();
        if (null != FindObjectOfType<ResultsScreenHolder>())
            resultScreenHolder = FindObjectOfType<ResultsScreenHolder>();
        if (null != FindObjectOfType<DreamHomeScreenTag>())
            dreamHomeHolder = FindObjectOfType<DreamHomeScreenTag>();
        if (null != FindObjectOfType<ScoreTrackerTag>())
            scoreTracker = FindObjectOfType<ScoreTrackerTag>().gameObject;
        if (null != FindObjectOfType<MainSceneButtonTag>())
            FindObjectOfType<MainSceneButtonTag>().gameObject.SetActive(!_startedNewMinigame);
        if (null != FindObjectOfType<pawClicker>())
            pawClicker = FindObjectOfType<pawClicker>();


        //Setting individual components inactive because we need the original parent holder active at the start of a scene in order to set references correctly.
        resultScreenHolder.ToggleResultsScreen(false);


        //Set relevant gameobjects/components on or off based on off it is a minigame scene or not.
        if (null != dreamHomeHolder)
            dreamHomeHolder.gameObject.SetActive(!_startedNewMinigame);
        if (null != currentbottomSlider)
            currentbottomSlider.gameObject.SetActive(_startedNewMinigame);

        if (null != currentcountDownText)
        {
            ///If we want countdown text back, un-comment these things.
            currentcountDownText.gameObject.SetActive(false);/*SetActive(_startedNewMinigame);*/
            currentcountDownText.enabled = false; /*_startedNewMinigame;*/
            currentcountDownText.text = "";/*"Countdown: " + curMinigameDuration.ToString("0");*/
        }

        if (null != scoreTracker)
        {
            scoreTracker.GetComponent<Text>().text = "";
        }
        if (null != pawClicker)
        {
            if (SceneManager.GetActiveScene().name == "DreamHomeScene")
            {
                pawClicker.gameObject.SetActive(true);
            }
            else
                pawClicker.gameObject.SetActive(false);
        }


        //Only run countdown timer + results screen stuff if we're in a minigame.
        if (_startedNewMinigame)
        {

            //Reset timer things
            timer = 0.0f;
            totalTimer = curMinigameDuration;
            doMinigameCountdown = true;
        }




    }


    private float totalTimer = 0.0f;
    float timer = 0.0f;

    private void Update()
    {
        if (doMinigameCountdown)
        {
            MinigameCountdownHandler();
        }
    }

    private void MinigameCountdownHandler()
    {


        //Debug.Log("totalTimer: " + totalTimer);
        //Debug.Log("curMinigameDuration: " + curMinigameDuration);
        if (timer <= totalTimer)
        {
            //  Debug.Log("Timer" + timer);

            timer += Time.deltaTime;
            curMinigameDuration -= Time.deltaTime;
            if (null != currentcountDownText)
                currentcountDownText.text = "Countdown: " + curMinigameDuration;
            if (null != currentbottomSlider)
                currentbottomSlider.value = curMinigameDuration / totalTimer;




            //GetComponent<DogHomeSceneManager>().BackToMainScene();


        }
        else if (timer > totalTimer)
        {

            //This keeps getting called twice, which is a problem.
            EvaluateMinigameResults();
            Debug.Log("calling evaluateMinigameResults");


        }




    }


    /// <summary>
    /// Meant to be called at the end of every minigame scene.
    /// </summary>
    private void EvaluateMinigameResults()
    {
        //Stop update calling
        doMinigameCountdown = false;

        if (myAudioSource == null)
        {
            myAudioSource = GetComponent<AudioSource>();
        }
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

                curScore = FindObjectOfType<Tailchase>().NumSpins;
                CheckScoreThreshold();

                break;
            case (MinigameTypes.chewMinigame):

                curScore = FindObjectOfType<Chew>().Score;
                CheckScoreThreshold();
                break;
            case (MinigameTypes.DDRfinalGame):
                curScore = FindObjectOfType<DDR>().curidx;
                CheckScoreThreshold();
                break;
            case (MinigameTypes.couchGame):

                //Invert this
                curScore = 3 - FindObjectOfType<WaitingGame>().FuckupCounter;
                if (null != GameObject.Find("StayCanvas"))
                    GameObject.Find("StayCanvas").SetActive(false);
                CheckScoreThreshold();
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
            if (null != myAudioSource && null != loseSound)
                myAudioSource.PlayOneShot(loseSound, 0.8f);
        }
        //2 star
        else if (curScore >= curGoodDogThreshold && curScore < curBestestDoggoThreshold)
        {
            curMinigameResult = MinigameResult.goodDoggo;
            resultScreenHolder.PassInResults(2);
            if (null != myAudioSource && null != winSound)
            {
                myAudioSource.PlayOneShot(winSound, 0.8f);
                myAudioSource.PlayOneShot(winSound, 0.8f);
            }

        }
        //3 stars
        else if (curScore >= curBestestDoggoThreshold)
        {
            curMinigameResult = MinigameResult.bestDoggo;
            resultScreenHolder.PassInResults(3);

            if (null != myAudioSource && null != winSound)
            {
                myAudioSource.PlayOneShot(winSound, 0.8f);
                myAudioSource.PlayOneShot(winSound, 0.8f);
                myAudioSource.PlayOneShot(winSound, 0.8f);
            }

        }
        return curMinigameResult;

    }

    public bool CheckOverallGameProgression()
    {
        //We call this at the end of every minigame, so we should increment current completion counter.
        gameConditions.currentMinigameCompletedCounter++;

        if (gameConditions.currentMinigameCompletedCounter >= gameConditions.totalMinigameCompletionCounter)
        {
            gameConditions.superduperfuckingcompleted = true;

        }
        else
        {
            gameConditions.superduperfuckingcompleted = false;

        }

        return gameConditions.superduperfuckingcompleted;


    }
}
