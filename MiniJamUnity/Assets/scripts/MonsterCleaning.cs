using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterCleaning : MonoBehaviour
{
    public static event EventHandler<bool> OnMonsterDestroyed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") 
        {
            if (!collision.gameObject.GetComponent<Monster>().HasBeenHit) 
            {
                OnMonsterDestroyed?.Invoke(this, false);
                GameObject.Destroy(collision.gameObject);
                return;
            }

            GameObject.Destroy(collision.gameObject);
            
            OnMonsterDestroyed?.Invoke(this, true);
        }

    }
}
