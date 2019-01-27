using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.anyKeyDown)
        {
            transform.SendMessage("StartFlipbook", 10);
        }
    }
    void StartGame()
    {
        SceneManager.LoadScene("DreamHomeScene");
    }
}
