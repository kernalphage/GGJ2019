using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainSceneMinigameSelector : MonoBehaviour
{
    private EventSystem myEventSystem = null;

    [SerializeField]
    private Button[] minigameButtons = null;

    //private List<Button> minigameButtons = new List<Button>();
    public delegate void MinigameSelection(int _sceneIndex);
    public static event MinigameSelection onMinigameSelect;

    void Start()
    {
        if (null != GetComponent<EventSystem>())
        {
            myEventSystem = GetComponent<EventSystem>();
        }

    }


    ///Relevant Scene transition event bindings/unbindings
    private void OnEnable()
    {
        if (null != FindObjectOfType<DogHomeSceneManager>())
            onMinigameSelect += FindObjectOfType<DogHomeSceneManager>().HandleSceneTransition;
        if (null != FindObjectOfType<MinigameLoopManager>())
            onMinigameSelect += FindObjectOfType<MinigameLoopManager>().NotifyMinigameManagerOfNewMinigame;
    }
    private void OnDisable()
    {
        if (null != FindObjectOfType<DogHomeSceneManager>())
            onMinigameSelect -= FindObjectOfType<DogHomeSceneManager>().HandleSceneTransition;
        if (null != FindObjectOfType<MinigameLoopManager>())
            onMinigameSelect -= FindObjectOfType<MinigameLoopManager>().NotifyMinigameManagerOfNewMinigame;
    }
    public void StartSceneTransition(int _sceneIndex)
    {
        //Call this event; whatever is currently bound gets called; a little round about because we need to call functions on the Undestroyable-singleton, and buttons can't have a reference
        // to something not in the current scene.
        onMinigameSelect(_sceneIndex);
    }

}
