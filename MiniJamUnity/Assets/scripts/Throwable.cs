using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

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

    [Header("Particles")]

    [SerializeField]
    public Transform notResetingBackground;

    [SerializeField]
    private Color particleColor;

    [SerializeField]
    private ParticleSystem particles;

    [SerializeField]
    private GameObject particlesPrefab;

    private bool runsCoroutine = false;
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

        if (curveTime > 1 && !runsCoroutine)
        {
            runsCoroutine = true;

            StartCoroutine(PotionParticles());
            Explode();
        }

    }

    private IEnumerator PotionParticles()
    {

        GetComponent<SpriteRenderer>().enabled = false;


        particles = GameObject.Instantiate(particlesPrefab,transform.position,transform.rotation,notResetingBackground).GetComponent<ParticleSystem>();
        var main = particles.main;
        main.startColor = new MinMaxGradient(particleColor);
        particles.Play();

        yield return new WaitForSeconds(particles.main.duration);
        //yield return new WaitForSeconds(0.2f);

        particles.Stop();
        GameObject.Destroy(particles.gameObject);

        GameObject.Destroy(gameObject);

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
