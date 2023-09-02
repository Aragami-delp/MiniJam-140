using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionDisplayUI : MonoBehaviour
{
    private void Start()
    {
        PlayerControlls.OnPotionChange += OnPotionChange;
    }

    private void OnDestroy()
    {
        PlayerControlls.OnPotionChange -= OnPotionChange;
    }

    private void OnPotionChange(object sender, PlayerControlls.PotionEventArgs args)
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = args.newPotionPrefab.GetComponent<SpriteRenderer>().sprite;

    }

}
