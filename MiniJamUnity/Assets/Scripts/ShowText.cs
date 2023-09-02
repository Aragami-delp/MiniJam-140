using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowText : MonoBehaviour
{
    [SerializeField, TextArea] private string m_preText;
    [SerializeField, TextArea] private string m_postText;

    private TMP_Text m_text;

    private void Awake()
    {
        m_text = GetComponent<TMP_Text>();
    }

    public void SetText(string _text)
    {
        m_text.SetText(m_preText + _text + m_postText);
    }

    public void ShowTextUI(bool _show)
    {
        m_text.gameObject.SetActive(_show);
    }
}
