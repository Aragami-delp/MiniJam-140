using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private PotionTypes takesPotion;

    [SerializeField]
    private bool takesAnyPotion;
    
    [SerializeField]
    private int potionsNeeded = 1,potionsHit;

    [SerializeField]
    private bool hasBeenHit = false;

    [Header("Animations")]
    [SerializeField]
    private float animationIntensity;
    public bool HasBeenHit { get { return hasBeenHit; } private set { hasBeenHit = value; } }
    
    public void OnPotionHit(PotionTypes potionHitBy) 
    {
        if (potionHitBy == takesPotion || takesAnyPotion) 
        {
            if (HasBeenHit) return;

            potionsHit++;

            if (potionsHit >= potionsNeeded) 
            {
                HasBeenHit = true;
                ScoreSystem.Instance.IncreaseScore();
            }
        }
    }

}
