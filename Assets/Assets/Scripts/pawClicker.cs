using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pawClicker : MonoBehaviour {

    public List<Vector3> positions;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        RectTransform paw = GetComponent<RectTransform>();
        paw.localPosition = Vector3.zero;
    }
    void MoveToPos(int pos)
    {
    }
}
