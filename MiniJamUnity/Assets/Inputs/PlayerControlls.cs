using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlls : MonoBehaviour
{
    BaseInput inputs;
    BaseInput.CartInputActions cartInput;

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
    private float extraPotionRange;

    [SerializeField]
    [Range(0.25f, 10f)]
    private float cannonCooldown;
    public float CannonColdown { get { return cannonCooldown; } set { OnNewCannonColldown(value); } }

    [SerializeField]
    private bool canBreak,breakReady;
    public bool CanBreak { get { return canBreak; } set { EnableBreak(value); } }
    private float breakCooldown = 30;

    [SerializeField]
    private float BonusAmmo;

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

        Debug.Log("ChangePotion fired");
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
            EnableBreak(true);
            return;
        }

        breakCooldown = Math.Clamp(breakCooldown - cooldownReduction,5f,30f);
    }

    #endregion

    private void EnableBreak(bool enable)
    {
        canBreak = enable;

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
        float originalSpeed = background.GetSpeed();
        background.SetSpeed(5);

        breakReady = false;
        yield return new WaitForSeconds(breakCooldown);
        breakReady = true;

        background.SetSpeed(originalSpeed);

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
        
        Vector3 rayStart =  realCannon.transform.position;

        RaycastHit2D hitData =  Physics2D.Raycast(rayStart,Vector3.forward);

        GameObject newPotion = GameObject.Instantiate(potionPrefabs[(int)selectedPotion] ,gunBarrel.transform.position,transform.rotation);

        Throwable newThrowable = newPotion.GetComponent<Throwable>();
        newThrowable.target = hitData.point;
        newThrowable.potionType = selectedPotion;
        newThrowable.notResetingBackground = notResetingBackground;
        newThrowable.PotionSplashRange += extraPotionRange;

        StartCoroutine(CannonCooldownTimer());
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
