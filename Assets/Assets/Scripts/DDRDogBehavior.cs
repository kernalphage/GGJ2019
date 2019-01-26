using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDRDogBehavior : MonoBehaviour
{
    private Animator myAnimController = null;
    // Use this for initialization
    void Start()
    {
        myAnimController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateAnimationConditions(int _newDogInt)
    {
        myAnimController.SetInteger("DogInt", _newDogInt);
    }
}
