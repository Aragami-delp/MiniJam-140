using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private bool startClicked = false;

    [SerializeField] 
    private Transform target;

    [SerializeField] 
    private GameObject cart;
    
    [SerializeField] 
    private Animator transition;
    
    public void ClickStart()
    {
        startClicked = true;
        StartCoroutine(WaitAndLoadScene());
    }

    private void Update()
    {
        if (startClicked)
        {
            float distance =  Vector3.Distance(target.position, cart.transform.position);

            float normalizedDistance = Math.Clamp(distance, 1 , 20) / 20;
        
            cart.transform.position = Vector3.MoveTowards(cart.transform.position, target.position,CalcSpeed(normalizedDistance) * 10 * Time.deltaTime);
        }

    }
    
    private float CalcSpeed(float distance)
    {
        return (float)Math.Sin((distance * Math.PI) / 2);
    }
    
    IEnumerator WaitAndLoadScene() {
        yield return new WaitForSeconds(3);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }
}
