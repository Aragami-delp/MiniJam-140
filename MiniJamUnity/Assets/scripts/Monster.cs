using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator),typeof(Collider2D))]
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
    private Sprite onHitSpride;

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
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = onHitSpride;
            }
        }
    }

}
