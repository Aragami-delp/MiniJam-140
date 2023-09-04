using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControlls : MonoBehaviour
{
    BaseInput inputs;
    public BaseInput.CartInputActions cartInput;

    Camera mainCam;

    public class PotionEventArgs 
    {
        public PotionEventArgs(int newPotion, GameObject newPotionPrefab)
        {
            NewPotion = newPotion;
            this.newPotionPrefab = newPotionPrefab;
        }

        public int NewPotion { get; }
        public GameObject newPotionPrefab { get; }
    }

    public static event EventHandler<PotionEventArgs> OnPotionChange;

    [SerializeField]
    private GameObject crosshair,realCannon,gunBarrel;

    [SerializeField]
    private GameObject[] potionPrefabs;

    [SerializeField]
    private float speed = 10;
    
    [SerializeField]
    private float distanceForHighestSpeed = 10;

    [SerializeField]
    private PotionTypes selectedPotion;

    [SerializeField]
    private AnimationCurve throwingCurve;

    [SerializeField]
    private bool weaponCanFire = true;

    [Header("Upgrade Params")]
    [SerializeField]
    private Slider breakUISlider;

    [SerializeField]
    private float extraPotionRange;

    [SerializeField]
    [Range(0.25f, 10f)]
    private float cannonCooldown;
    public float CannonColdown { get { return cannonCooldown; } set { OnNewCannonColldown(value); } }

    [SerializeField]
    private bool canBreak, breakReady = true;
    public bool CanBreak { get { return canBreak; } set { EnableBreak(value); } }
    
    [SerializeField]
    private float breakCooldown = 30;

    [SerializeField]
    private float BonusAmmo;

    [SerializeField]
    private float bonusRoundInaccuracy = 3f;

    [SerializeField]
    [Range(0f, 1f)]
    private float shadowPotionChance;

    [Header("For Particles")]
    [SerializeField]
    Transform notResetingBackground;

    [SerializeField] 
    private AudioSource cannonShootSound;
    
    [SerializeField] 
    private AudioSource switchPotion;

    // Start is called before the first frame update
    void Start()
    {
        inputs = new();
        cartInput = inputs.CartInput;

        cartInput.Enable();

        cartInput.Shoot.performed += ShootCannon;

        cartInput.SwitchToPotion1.performed += ChangePotion1;
        cartInput.SwitchToPotion2.performed += ChangePotion2;
        cartInput.SwitchToPotion3.performed += ChangePotion3;
        cartInput.SwitchToPotion4.performed += ChangePotion4;
        cartInput.SwitchToNextPotion.performed += ChangeNextPotion;

        mainCam = Camera.main;

        Cursor.visible = false;
        
    }
    #region dont look
    private void ChangeNextPotion(InputAction.CallbackContext obj)
    {

        if (Enum.GetValues(typeof(PotionTypes)).Length == (int) selectedPotion + 1)
        {
            ChangePotion(PotionTypes.WRAITH_POTION);
        }
        else 
        {

            ChangePotion((PotionTypes)((int)selectedPotion + 1));
        }
    }

    private void ChangePotion1(InputAction.CallbackContext obj)
    {
        ChangePotion(PotionTypes.WRAITH_POTION);
    }
    private void ChangePotion2(InputAction.CallbackContext obj)
    {
        ChangePotion(PotionTypes.SLIME_PORTION);
    }
    private void ChangePotion3(InputAction.CallbackContext obj)
    {
        ChangePotion(PotionTypes.ZOMBIE_POTION);
    }
    private void ChangePotion4(InputAction.CallbackContext obj)
    {
        ChangePotion(PotionTypes.GOLEM_POTION);
    }
    #endregion
    private void ChangePotion(PotionTypes newPotion)
    {
        switchPotion.Play();
        selectedPotion = newPotion;
        OnPotionChange?.Invoke(this, new PotionEventArgs((int)selectedPotion, potionPrefabs[(int)selectedPotion]));
    }
    
    #region upgrades
    private void OnNewCannonColldown(float value)
    {
        cannonCooldown = Math.Clamp(value, 0.1f, 2f);
    }
    public void ReduceCannonColldown(float amount) 
    {
        CannonColdown -= amount; 
    }

    public void UpgradeSplashRange(float increaseAmount)
    {
        extraPotionRange += increaseAmount;
    }

    public void UpgradeBreak(float cooldownReduction) 
    {
        if (!canBreak) 
        {
            // fist purchase
            CanBreak = true;
            return;
        }

        breakCooldown = Math.Clamp(breakCooldown - cooldownReduction,5f,30f);
    }

    public void IncreaseBonusAmmo(int increaseAmount) 
    {
        BonusAmmo += increaseAmount;
    }

    public void IncreaseShadowPotionChance(float amount) 
    {
        if (shadowPotionChance <= 0f) 
        {
            shadowPotionChance = 0.1f;
            return;
        }
        
        if (amount >= 1) 
        {
            amount /= 100f;
        }

        shadowPotionChance = Math.Clamp(shadowPotionChance, 0,0.9f);

        shadowPotionChance += amount;
    }

    #endregion

    private void EnableBreak(bool enable)
    {
        canBreak = enable;
        
        breakUISlider.gameObject.SetActive(enable);

        if (enable)
        {
            cartInput.Break.performed += ActivateBreak;
        }
        else
        {
            cartInput.Break.performed -= ActivateBreak;
        }
    }
    private void ActivateBreak(InputAction.CallbackContext obj)
    {
        if (!breakReady) 
        {
            return;
        }

        Scrolling background = notResetingBackground.parent.GetComponent<Scrolling>();
        StartCoroutine(BreakCooldownTimer(background));
        
    }

    private IEnumerator BreakCooldownTimer(Scrolling background)
    {
        breakReady = false;
        background.SetSpeed(5);
        float cooldownDone = 0;
        breakUISlider.value = 0;
        breakUISlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.red;
        while (breakCooldown > cooldownDone) 
        {
            yield return new WaitForSeconds(0.5f);
            cooldownDone += 0.5f;
            breakUISlider.value = cooldownDone / breakCooldown;
        }
        breakUISlider.value = 1;
        breakUISlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.cyan;
        breakReady = true;
    }

    private void ShootCannon(InputAction.CallbackContext obj)
    {
        if (!weaponCanFire)
        {
            return;
        }
        else 
        {
            weaponCanFire = false;
        }

        cannonShootSound.Play();


        //Normal Potion
        CreatePotion(6);


        for (int i = 0; i < BonusAmmo; i++)
        {

            CreatePotion(6,true);
        }


        StartCoroutine(CannonCooldownTimer());
    }

    private void CreatePotion(int recusionDepth, bool addRandomTarget = false, bool randomPotionType = false) 
    {
        if (recusionDepth <= 0) 
        {
            return;
        }

        Vector3 rayStart = realCannon.transform.position;

        RaycastHit2D hitData = Physics2D.Raycast(rayStart, Vector3.forward);

        GameObject newPotion;

        if (randomPotionType)
        {
            newPotion = GameObject.Instantiate(potionPrefabs[UnityEngine.Random.Range(0, potionPrefabs.Length)], gunBarrel.transform.position, transform.rotation);
        }
        else 
        {
            newPotion = GameObject.Instantiate(potionPrefabs[(int)selectedPotion], gunBarrel.transform.position, transform.rotation);
        }

        Throwable newThrowable = newPotion.GetComponent<Throwable>();

        Vector2 randomTarget = Vector2.zero;

        if (addRandomTarget) 
        {
            randomTarget.x = UnityEngine.Random.Range(-bonusRoundInaccuracy, bonusRoundInaccuracy);
            randomTarget.y = UnityEngine.Random.Range(-bonusRoundInaccuracy, bonusRoundInaccuracy);
        }

        newThrowable.target = hitData.point + randomTarget;
        //newThrowable.potionType;
        newThrowable.notResetingBackground = notResetingBackground;
        newThrowable.PotionSplashRange += extraPotionRange;

        if (UnityEngine.Random.value <= shadowPotionChance) 
        {
            CreatePotion(recusionDepth - 1,true,true);
        }

    }

    private IEnumerator CannonCooldownTimer() 
    {
        yield return new WaitForSeconds(cannonCooldown);
        weaponCanFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseScreenPos =  cartInput.MousePosition.ReadValue<Vector2>();
        Vector3 mousePosAsVec3 = new Vector3(mouseScreenPos.x, mouseScreenPos.y,0);

        Vector3 mouseToWorld = mainCam.ScreenToWorldPoint(mousePosAsVec3);

        mouseToWorld.z = 0;

        crosshair.transform.position = mouseToWorld;

        float distance =  Vector3.Distance(mouseToWorld, realCannon.transform.position);

        float normalizedDistance = Math.Clamp(distance, 1 , distanceForHighestSpeed) / distanceForHighestSpeed;

        realCannon.transform.position = Vector3.MoveTowards(realCannon.transform.position, mouseToWorld, CalcSpeed(normalizedDistance) * speed * Time.deltaTime);
    }

    private float CalcSpeed(float distance)
    {
        return (float)Math.Sin((distance * Math.PI) / 2);
    } 

}


public enum PotionTypes
{
    WRAITH_POTION,
    SLIME_PORTION,
    ZOMBIE_POTION,
    GOLEM_POTION

}
