using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartBounce : MonoBehaviour
{

    [SerializeField]
    Animator[] cartAnimators;

    [SerializeField]
    Animator[] wheelAnimators;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CartAnimStartDelay());   
    }

    public IEnumerator CartAnimStartDelay()
    {
        for (int i = 0; i < cartAnimators.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            cartAnimators[i].enabled = true;
        }
    }

    public void AdjustWheelSpeed(float _newRelativeSpeed)
    {
        foreach (Animator wheelAnimator in wheelAnimators)
        {
            wheelAnimator.speed = _newRelativeSpeed;
        }
    }
}
