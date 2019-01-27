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

    private static int dumbCounter = 0;
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
        if (dumbCounter < 1)
        {

            dumbCounter++;

           

            ///Last thing we do...in theory once it's done it should call some sort of event dispatcher that signals that we are done loading the game.
            InitializeGameThings();
        }




    }




    private void InitializeGameThings()
    {

        Debug.Log("DogHomeScenemanager.InitializeGameThings() is being called."); //Doesn't print because of how Awake just doesn't print shit correctly if called from Awake.

        minigameLoopManager = GetComponent<MinigameLoopManager>();




        //Do initial Bindings. Keep in mind that .sceneLoaded calls after awake, so OnLevelFinishedLoading should call after this.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;

        //Bind scene transition function inside MinigameLoopManager to our event, which we call when level finishes loading.
        minigameSceneLoad += minigameLoopManager.MinigameSceneTransition;



        //So at the very start of the game, we go Awake => OnLevelFInishedLoading (should be bound by this point) => minigameLoopManager.MinigameSceneTransition (also should be bound by this point).
    }


    void OnEnable()
    {

    }

    void OnDisable()
    {
        //Debug.Log("Calling OnDisable event from DogHomeSceneManager.cs");
        //SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        //minigameSceneLoad -= minigameLoopManager.MinigameSceneTransition;
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
        if (_nextSceneIndex != 7)
        {
            SceneManager.LoadScene(_nextSceneIndex);
        }
        else if (_nextSceneIndex == 7)
        {
            Application.Quit();
        }
    
        // Debug.Log("Loading sceneindex: " + _nextSceneIndex);
    }


    //
    public void BackToMainScene()
    {

        //Debug.Log("Current scene name" + SceneManager.GetActiveScene().name);

        //If we're already in DDR game...don't replay it, just go back to home screen.
        if (SceneManager.GetActiveScene().name == "DDRGame")
        {

            minigameLoopManager.gameConditions.currentMinigameCompletedCounter = -1;
        }



        //If we have played the required amount of minigames, just simply go to DDR scene.
        if (minigameLoopManager.CheckOverallGameProgression())
        {


            SceneManager.LoadScene("DDRGame");
        }//If we are not finished with every minigame, go back to home scene.
        else
        {
            SceneManager.LoadScene("DreamHomeScene");
        }


    }

    //private int practicesceneLoadedInt = 0;
    //private int LevelFinishedLoadingInt = 0;

    void OnLevelFinishedLoading(Scene _scene, LoadSceneMode _mode)
    {
        Debug.Log("Calling OnLevelFinished Loading");
        //if (AwakeIntTest == 0)
        //{
        //    //only increment this if awakeIntTest = 0. This should confirm if Awake indeed calls before SceneManager.sceneLoaded.
        //    practicesceneLoadedInt++;
        //}
        //LevelFinishedLoadingInt++; //Increment this everytime OnLevelFinishedLoading is called.


        minigameLoopManager.NotifyMinigameManagerOfNewMinigame(_scene.buildIndex);
        //  Debug.Log("FinishedLoading: " + _scene.ToString());
        ///K so this is going to have to line up with the build order. Be super careful about this BUCKO.
        if (_scene.buildIndex >= 2)
        {

            //Call this event if we in fact did load up a minigame scene. True == minigame scene, set countdowntimer active to true.
            minigameSceneLoad(true);
            Debug.Log("Loading Minigame Scene");
        }
        else 
        {
            minigameSceneLoad(false);
            Debug.Log("Loading Home Scene or Splash Screen");
        }
    }
}
