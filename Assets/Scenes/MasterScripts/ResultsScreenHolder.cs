using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private GameObject heartParticle = null;

    [SerializeField]
    private Sprite winScreen = null, loseScreen = null;


    //Called from MinigameLoopManager
    public void ToggleResultsScreen(bool _active)
    {
        if (SceneManager.GetActiveScene().name != "DDRGame")
        {
            for (int i = 0; i < starImages.Length; i++)
            {
                if (null != starImages[i])
                    starImages[i].enabled = false; /*_active;*/
            }
            for (int i = 0; i < starImageBgs.Length; i++)
            {
                if (null != starImageBgs[i])
                    starImageBgs[i].enabled = false; /*_active;*/

            }
            if (null != overallBG)
                overallBG.enabled = _active;
            if (null != myText)
                myText.enabled = _active;
        }
        else
        {
            for (int i = 0; i < starImages.Length; i++)
            {
                starImages[i].enabled = false;
            }
            for (int i = 0; i < starImageBgs.Length; i++)
            {
                starImageBgs[i].enabled = false;

            }
        }

    }

    //Also called from MinigameLoopManager... This is where we set the visual feedback for win/losses.
    public void PassInResults(int _numberOfStars)
    {
        if (null != myText)
            myText.text = "";
        if (SceneManager.GetActiveScene().name == "DDRGame")
        {
            GameObject obj;
            obj = Instantiate(heartParticle);


            StartCoroutine(DelayedTransition(15.0f));
        }
        else
        {
            switch (_numberOfStars)
            {
                case (1):
                    //myText.text = "1 star!";
                    overallBG.GetComponent<Image>().sprite = loseScreen;
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
                    //myText.text = "2 stars!";
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
                    overallBG.GetComponent<Image>().sprite = winScreen;
                    //myText.text = "3 stars!";
                    break;
            }
            StartCoroutine(DelayedTransition(5.0f));
        }

    }

    IEnumerator DelayedTransition(float _durationOfDelay)
    {
        yield return new WaitForSeconds(_durationOfDelay);
        FindObjectOfType<DogHomeSceneManager>().BackToMainScene();
    }


}
