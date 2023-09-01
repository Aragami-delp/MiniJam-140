using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoPool<T> : MonoBehaviour where T : Transform
{
    [SerializeField] protected T poolPrefab;
    private List<T> poolItems = new();

    protected T GetNewPoolItem()
    {
        if (poolPrefab == null)
        {
            T nextItem = null;
            for (int i = 0; i<poolItems.Count; i++)
            {
                if (!poolItems[i].gameObject.activeInHierarchy)
                {
                    nextItem = poolItems[i];
                    nextItem.gameObject.SetActive(true);
                    break;
                }
            }
            if (nextItem == null)
            {
                nextItem = Instantiate(poolPrefab);
                poolItems.Add(nextItem);
            }
            return nextItem;
        }
        return null;
    }
}
