using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionDisplayUI : MonoBehaviour
{
    private void Start()
    {
        InputTest.OnPotionChange += OnPotionChange;
    }

    private void OnPotionChange(object sender, InputTest.PotionEventArgs args)
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = args.newPotionPrefab.GetComponent<SpriteRenderer>().sprite;

    }

}
