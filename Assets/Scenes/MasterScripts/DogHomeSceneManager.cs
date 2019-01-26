/*Purpose: Be the single game manager persistent throughout every single scene.
 * 
 * 
 * 
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DogHomeSceneManager : MonoBehaviour
{
    public static DogHomeSceneManager instance = null;
    private MinigameLoopManager minigameLoopManager = null;

    public delegate void MinigameSceneLoaded(bool _minigameStarted);
    public static event MinigameSceneLoaded minigameSceneLoad;


    /// <summary>
    /// According to https://docs.unity3d.com/Manual/ExecutionOrder.html, Awake is called before OnEnable
    /// </summary>
    void Awake()
    {

        //Make sure I'm the only one in the scene :)
        if (null == instance)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //Keep it throughout every scene transition.
        DontDestroyOnLoad(gameObject);




        ///Last thing we do...in theory once it's done it should call some sort of event dispatcher that signals that we are done loading the game.
        InitializeGameThings();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        minigameSceneLoad += minigameLoopManager.MinigameSceneTransition;

    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        //   minigameSceneLoad -= minigameLoopManager.MinigameSceneTransition;
    }


    private void InitializeGameThings()
    {
        if (null != GetComponent<MinigameLoopManager>())
        {
            minigameLoopManager = GetComponent<MinigameLoopManager>();
        }
    }
    private void Update()
    {
#if UNITY_EDITOR //Disable in builds.
        TestSceneTransitionsWIthBasicInput();
#endif
    }

    private void TestSceneTransitionsWIthBasicInput()
    {

        //Better way to do this in the long run.
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            HandleSceneTransition(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HandleSceneTransition(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HandleSceneTransition(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HandleSceneTransition(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            HandleSceneTransition(4);
        }
    }

    public void HandleSceneTransition(int _nextSceneIndex)
    {
        SceneManager.LoadScene(_nextSceneIndex);
        // Debug.Log("Loading sceneindex: " + _nextSceneIndex);
    }


    //
    public void BackToMainScene()
    {
        ///Probably need some form of evaluation to determine if we go to DDR screen.
        ///
        //If we have played the required amount of minigames, just simply go to DDR scene.
        if (minigameLoopManager.CheckOverallGameProgression())
        {
            SceneManager.LoadScene("DDRGame");
        }

        //If we are not finished with every minigame, go back to home scene.
        else
        {
            SceneManager.LoadScene("DreamHomeScene");
        }

        
    }


    void OnLevelFinishedLoading(Scene _scene, LoadSceneMode _mode)
    {
        minigameLoopManager.NotifyMinigameManagerOfNewMinigame(_scene.buildIndex);
        //  Debug.Log("FinishedLoading: " + _scene.ToString());
        ///K so this is going to have to line up with the build order. Be super careful about this BUCKO.
        if (_scene.buildIndex >= 1)
        {

            //Call this event if we in fact did load up a minigame scene. True == minigame scene, set countdowntimer active to true.
            minigameSceneLoad(true);

        }
        else
        {
            minigameSceneLoad(false);
        }
    }
}
