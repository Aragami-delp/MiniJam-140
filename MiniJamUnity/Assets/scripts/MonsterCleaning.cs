using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCleaning : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") 
        {
            GameObject.Destroy(collision.gameObject);
        }

    }
}
