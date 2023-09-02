using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve throwingCurve, speedCurve;

    [SerializeField]
    private float curveAmount;

    [SerializeField]
    public PotionTypes potionType;
    
    [SerializeField]
    [Range(1f,4f)]
    private float potionSplashRange;

    [SerializeField]
    public Vector3 target;

    private Vector3 origin;

    float curveTime;
    float unitsToTravel;

    // Start is called before the first frame update
    void Awake()
    {
        origin = transform.position;
        curveTime = 0;

        unitsToTravel = Vector3.Distance(origin,target) * 2f;
    }

    // Update is called once per frame
    void Update()
    {
        curveTime +=  Time.deltaTime * 14 / unitsToTravel;


        //Move to target
        //transform.position = Vector3.Lerp(origin,target, curveTime);

        //transform.position = Vector3.MoveTowards(transform.position, target, CalcSpeed(curveTime / 1f) * Time.deltaTime * 10);

        transform.position = Vector3.Lerp(origin,target,speedCurve.Evaluate(curveTime));

        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.7f, curveTime);

        // amount to sway to the right
       transform.localPosition = transform.localPosition +  Vector3.left * (throwingCurve.Evaluate(curveTime) * curveAmount * 2 );

        if (curveTime > 1)
        {
            GameObject.Destroy(gameObject);
            Explode();
        }

    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, potionSplashRange);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].tag == "Enemy") 
            {
                hits[i].GetComponent<Monster>().OnPotionHit(potionType);
            }
        }
    }
}
