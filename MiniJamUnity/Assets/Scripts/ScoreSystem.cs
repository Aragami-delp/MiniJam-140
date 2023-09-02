using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private ShowText m_scoreDisplay;
    [SerializeField] private ShowText m_gameOverDisplay;

    public static ScoreSystem Instance;

    [ReadOnly] private int m_score;

    private void Awake()
    {
        if (Instance != null )
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        m_gameOverDisplay.ShowTextUI(false);
        m_scoreDisplay.ShowTextUI(true);
    }

    public void IncreaseScore(int _amount = 1)
    {
        m_score += _amount;
        m_scoreDisplay.SetText(m_score.ToString());
    }
}
