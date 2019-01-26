using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsScreenHolder : MonoBehaviour
{
    [Header("Must be set in prefab editor.")]
    [SerializeField]
    private Image[] starImages;
    [SerializeField]
    private Image[] starImageBgs;
    [SerializeField]
    private Image overallBG = null;
    [SerializeField]
    private Text myText = null;

    


    //Called from MinigameLoopManager
    public void ToggleResultsScreen(bool _active)
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].enabled = _active;
        }
        for (int i = 0; i < starImageBgs.Length; i++)
        {
            starImageBgs[i].enabled = _active;
            
        }
        overallBG.enabled = _active;
        myText.enabled = _active;
    }

    //Also called from MinigameLoopManager
    public void PassInResults(int _numberOfStars)
    {
        switch (_numberOfStars)
        {
            case (1):
                myText.text = "1 star!";
                for (int i = 1; i < starImages.Length; i++)
                {
                    starImages[i].enabled = false;
                }
                for (int i = 1; i < starImageBgs.Length; i++)
                {
                    starImageBgs[i].enabled = false;
                }
                break;
            case (2):
                myText.text = "2 stars!";
                for (int i = 2; i < starImages.Length; i++)
                {
                    starImages[i].enabled = false;
                }
                for (int i = 2; i < starImageBgs.Length; i++)
                {
                    starImageBgs[i].enabled = false;
                }
                break;
            case (3):
                myText.text = "3 stars!";
                break;
        }

        StartCoroutine(DelayedTransition());
    }

    IEnumerator DelayedTransition()
    {
        yield return new WaitForSeconds(5.0f);
        FindObjectOfType<DogHomeSceneManager>().BackToMainScene();
    }
}
