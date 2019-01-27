using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPFI : MonoBehaviour
{

    [SerializeField]
    private Sprite toggleSPrite = null;

    public void ActivateSecondSprite()
    {
        GetComponent<SpriteRenderer>().sprite = toggleSPrite;
    }
}
