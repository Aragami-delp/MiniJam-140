using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UpgradeButtonLogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Image icon;
    private Button button;

    private TMP_Text priceText;

    [SerializeField]
    private int price;

    [SerializeField]
    private float priceIncrease = 30f;

    bool startWasRun = false, rerunAwake;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ColorBlock colors = button.colors;
        GrayOutIcon(colors.highlightedColor.a);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GrayOutIcon(1f);
    }

    private void Start()
    {
        startWasRun = true;

        if (rerunAwake) 
        {
            Awake();
        }
    }

    void Awake()
    {
        if (!startWasRun) 
        {
            rerunAwake = true;
        }

        Cursor.visible = true;

        icon = transform.GetChild(2).GetComponent<Image>();

        button = GetComponent<Button>();

        foreach (Transform child in transform.GetChild(0))
        {
            if (child.name == "Price") 
            {
                priceText = child.GetComponent<TMP_Text>();
                break;
            }
        }

        if (UISystem.Instance.GetScore() < price ) 
        {
            priceText.color = Color.red;
            button.interactable = false;

            ColorBlock colors = button.colors;
            icon.color = colors.disabledColor;


        }
        else 
        {
            priceText.color = Color.black;
        }


        priceText.text = price.ToString();

    }

    private void GrayOutIcon(float a)
    {
        Color greyedOutIcon = new Color(icon.color.r, icon.color.g, icon.color.b, a);
        icon.color = greyedOutIcon;
    }

    public void IncreasePrice() 
    {
        price *= (int)(1f + priceIncrease); 
    }

}
