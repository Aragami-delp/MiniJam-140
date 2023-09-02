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
    private bool hasBeenHit = false;

    public bool HasBeenHit { get { return hasBeenHit; } private set { hasBeenHit = value; } }
    
    public void OnPotionHit(PotionTypes potionHitBy) 
    {
        if (potionHitBy == takesPotion || takesAnyPotion) 
        {
            if (HasBeenHit) return;

            HasBeenHit = true;
            //Give Points
        }
    }

}