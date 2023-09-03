using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;
using UnityEngine.UI;
using UnityEngine.Windows;

public class UISystem : MonoBehaviour
{
    [SerializeField] private ShowText m_scoreDisplay;
    [SerializeField] private ShowText m_gameOverDisplay;
    [SerializeField] private RectTransform m_gameOverButton;
    [SerializeField] private int m_maxHealth = 50;
    [SerializeField] private Image m_healthBar;
    [SerializeField] private PlayerControlls m_actions;
    [SerializeField] private LevelLoader m_levelLoader;
    [SerializeField] private RectTransform m_upgradeScreen;
    [SerializeField] private int m_timeBetweenUpgrades = 60; // int to more easily adjust in inspector
#if UNITY_EDITOR
    [SerializeField]
#endif 
    private bool m_godmode = false;

    public static UISystem Instance;

    private int score = 0;
    private int health;
    private float timeSinceLastUpgrade = 0;
    private bool isGameOver;

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
        m_gameOverButton.gameObject.SetActive(false);
        m_scoreDisplay.ShowTextUI(true);
        m_upgradeScreen.gameObject.SetActive(false);
        health = m_maxHealth;
        MonsterCleaning.OnMonsterDestroyed += OnMonsterCleaned;
        timeSinceLastUpgrade = 0f;
    }

    private void Update()
    {
        if (isGameOver) return;

        timeSinceLastUpgrade += Time.deltaTime;
        if (timeSinceLastUpgrade >= m_timeBetweenUpgrades)
        {
            OpenUpgradeScreen();
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void OpenUpgradeScreen()
    {
        m_upgradeScreen.gameObject.SetActive(true);
        m_actions.cartInput.Disable();
        Time.timeScale = 0.0001f;
    }

    [ContextMenu("CloseUpgradeScreen")]
    public void CloseUpgradeScreen()
    {
        timeSinceLastUpgrade = 0f;
        Time.timeScale = 1f;
        m_upgradeScreen.gameObject.SetActive(false);
        m_actions.cartInput.Enable();
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
        score += _amount;
        m_scoreDisplay.SetText(score.ToString());
        AddHealth(1); // Heal for every enemy hit until full
        Scrolling.Instance.SpeedUp();
    }
    public int GetScore() 
    {
        return score;
    }

    public void LoseGame()
    {
        if (isGameOver) return;
        m_gameOverButton.gameObject.SetActive(true);
        m_gameOverDisplay.ShowTextUI(true);
        m_gameOverDisplay.SetText(score.ToString());
        m_actions.cartInput.Disable();
        isGameOver = true;
        Cursor.visible = true;
    }

    public void GoToMainMenu()
    {
        m_actions.cartInput.Enable();
        m_levelLoader.GoToMainMenu();
    }

    public void AddHealth(int _amount = -1)
    {
        health = Mathf.Clamp(health + _amount, 0, m_maxHealth);
        m_healthBar.fillAmount = health / (float)m_maxHealth;
        if (health <= 0 && !m_godmode)
        {
            LoseGame();
        }
    }
}
