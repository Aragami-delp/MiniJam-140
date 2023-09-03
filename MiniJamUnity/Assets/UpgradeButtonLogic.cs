using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeButtonLogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Image icon;

    private Button button;

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }



    // Start is called before the first frame update
    void Awake()
    {
        icon = transform.GetComponentInChildren<Image>();
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
