using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] private GameObject infoPage;
    
    public void ClickStart()
    {
        startClicked = true;
        StartCoroutine(WaitAndLoadScene(1, 3));
    }

    public void GoToMainMenu()
    {
        StartCoroutine(WaitAndLoadScene(0, 0));
    }
    
    public void ClickInfos()
    {
        infoPage.SetActive(!infoPage.activeSelf);
    }

    private void Update()
    {
        if (startClicked && cart != null && target != null)
        {
            float distance =  Vector3.Distance(target.position, cart.transform.position);

            float normalizedDistance = Math.Clamp(distance, 1 , 20) / 20;
        
            cart.transform.position = Vector3.MoveTowards(cart.transform.position, target.position, CalcSpeed(normalizedDistance) * 10 * Time.deltaTime);
        }

    }
    
    private float CalcSpeed(float distance)
    {
        return (float)Math.Sin((distance * Math.PI) / 2);
    }
    
    IEnumerator WaitAndLoadScene(int _sceneIndex = 1, float _waitBeforeChanging = 3) {
        yield return new WaitForSeconds(_waitBeforeChanging);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(_sceneIndex, LoadSceneMode.Single);
    }
}
