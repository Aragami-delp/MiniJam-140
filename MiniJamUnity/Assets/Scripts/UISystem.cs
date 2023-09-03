using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    [SerializeField] private ShowText m_scoreDisplay;
    [SerializeField] private ShowText m_gameOverDisplay;
    [SerializeField] private int m_maxHealth = 50;
    [SerializeField] private Image m_healthBar;
#if UNITY_EDITOR
    [SerializeField]
#endif 
    private bool m_godmode = false;

    public static UISystem Instance;

    
    private int m_score = 0;
    private int m_health = 0;

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
        m_health = m_maxHealth;
        MonsterCleaning.OnMonsterDestroyed += OnMonsterCleaned;
    }

    private void OnMonsterCleaned(object sender, bool _wasHit)
    {
        if (!_wasHit)
        {
            AddHealth(-1);
        }
    }

    public void IncreaseScore(int _amount = 1)
    {
        m_score += _amount;
        m_scoreDisplay.SetText(m_score.ToString());
        AddHealth(1); // Heal for every enemy hit until full
        Scrolling.Instance.SpeedUp();
    }
    public int GetScore() 
    {
        return m_score;
    }

    public void LoseGame()
    {
        m_gameOverDisplay.ShowTextUI(true);
        m_gameOverDisplay.SetText(m_score.ToString());
    }

    public void AddHealth(int _amount = -1)
    {
        m_health = Mathf.Clamp(m_health + _amount, 0, m_maxHealth);
        m_healthBar.fillAmount = m_health / (float)m_maxHealth;
        if (m_health <= 0 && !m_godmode)
        {
            LoseGame();
            // TODO: Disable controls
        }
    }
}
