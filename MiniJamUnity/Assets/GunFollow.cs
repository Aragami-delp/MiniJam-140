using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        angle = Mathf.Clamp(angle,-75f,75f);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle ));
    }
}
