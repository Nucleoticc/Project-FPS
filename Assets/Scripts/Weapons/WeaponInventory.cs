using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerAnimationHandler playerAnimationHandler;
    PlayerUIManager playerUIManager;

    [Header("HealthKit Inventory")]
    [SerializeField] HealthKitItem healthKitItem;
    GameObject healthKitModel;
    GameObject healthKitFPSModel;
    public Animator healthKitAnimator { get; private set; }

    [Header("Weapon Inventory")]
    [SerializeField] List<Item> weapons = new List<Item>();
    List<GameObject> weaponModels = new List<GameObject>();
    List<GameObject> fpsWeaponModels = new List<GameObject>();
    public Animator CurrentWeaponAnimator { get; private set; }
    [HideInInspector] public Weapon currentWeapon;
    [HideInInspector] public Item currentWeaponItem;

    [Header("Weapon UI")]
    [SerializeField] Image weaponImage;

    // Weapon Index
    int currentWeaponIndex;
    int nextWeaponIndex;

    // Grenade Index
    int currentGrenadeIndex = 0;
    int[] grenadeIndexes = new int[3] { 6, 7, 8 };

    Transform weaponHolderSlot;
    public Transform fpsWeaponHolderSlot { get; private set; }

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        playerUIManager = GetComponent<PlayerUIManager>();
        weaponHolderSlot = GameObject.FindGameObjectWithTag("WeaponHolder").transform;
        fpsWeaponHolderSlot = GameObject.FindGameObjectWithTag("FPSWeaponHolder").transform;
        LoadAllWeapons();
        LoadHealthKit();
    }

    void Update()
    {
        LoadWeapon();
    }

    //Load All Weapons
    void LoadAllWeapons()
    {
        foreach (Item weapon in weapons)
        {
            GameObject model = Instantiate(weapon.modelPrefab, weaponHolderSlot);
            GameObject fpsModel = Instantiate(weapon.FpsWeaponPrefab, fpsWeaponHolderSlot);
            Transform currentWeaponMuzzleFlashPosition = fpsModel.transform.GetChild(0).Find("MuzzleFlashPosition");
            if (weapon is GunItem)
            {
                GunWeapon thisWeapon = model.GetComponent<GunWeapon>();
                GunItem gunItem = weapon as GunItem;
                thisWeapon.UpdateGunSettings(weapon as GunItem, currentWeaponMuzzleFlashPosition, gunItem.gunType);
            }
            else if (weapon is KnifeItem)
            {
                MeleeWeapon thisWeapon = model.GetComponent<MeleeWeapon>();
                MeleeDamage meleeDamage = fpsModel.GetComponent<MeleeDamage>();
                thisWeapon.UpdateMeleeSettings(weapon as KnifeItem, meleeDamage);
            }
            else if (weapon is GrenadeItem)
            {
                GrenadeWeapon thisWeapon = model.GetComponent<GrenadeWeapon>();
                Transform grenadePosition = fpsModel.transform.GetChild(0);
                thisWeapon.UpdateGrenadeSettings(weapon as GrenadeItem, grenadePosition);
            }
            model.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            model.SetActive(false);
            weaponModels.Add(model);
            fpsModel.SetActive(false);
            fpsWeaponModels.Add(fpsModel);
        }
        nextWeaponIndex = 5;
        ActivateWeapon();
    }

    void LoadHealthKit()
    {
        //3rd person model
        healthKitModel = Instantiate(healthKitItem.modelPrefab, weaponHolderSlot);
        healthKitModel.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        healthKitModel.transform.GetChild(0).gameObject.SetActive(false);

        //FPS model
        healthKitFPSModel = Instantiate(healthKitItem.FpsWeaponPrefab, fpsWeaponHolderSlot);
        healthKitFPSModel.SetActive(false);

        //Animator
        healthKitAnimator = healthKitFPSModel.GetComponent<Animator>();

        //Set HealthKit Settings
        HealthKit healthKit = healthKitModel.GetComponent<HealthKit>();
        healthKit.UpdateHealthKitSettings(healthKitItem);
    }

    void LoadWeapon()
    {
        if (CurrentWeaponAnimator.GetBool("IsInteracting"))
        {
            return;
        }

        if (healthKitFPSModel.activeSelf)
        {
            return;
        }

        if (inputHandler.switchRifleInput)
        {
            currentGrenadeIndex = 0;
            nextWeaponIndex = 0;
            inputHandler.switchRifleInput = false;
            if (currentWeaponIndex != nextWeaponIndex)
            {
                SwitchWeaponWithAnimation();
                playerAnimationHandler.PlayTargetAnimation("Rifle Empty");
            }
        }
        else if (inputHandler.switchPistolInput)
        {
            currentGrenadeIndex = 0;
            nextWeaponIndex = 1;
            inputHandler.switchPistolInput = false;
            if (currentWeaponIndex != nextWeaponIndex)
            {
                SwitchWeaponWithAnimation();
                playerAnimationHandler.PlayTargetAnimation("Pistol Empty");
            }
        }
        else if (inputHandler.switchShotgunInput)
        {
            currentGrenadeIndex = 0;
            nextWeaponIndex = 2;
            inputHandler.switchShotgunInput = false;
            if (currentWeaponIndex != nextWeaponIndex)
            {
                SwitchWeaponWithAnimation();
                playerAnimationHandler.PlayTargetAnimation("Rifle Empty");
            }
        }
        else if (inputHandler.switchSMGInput)
        {
            currentGrenadeIndex = 0;
            nextWeaponIndex = 3;
            inputHandler.switchSMGInput = false;
            if (currentWeaponIndex != nextWeaponIndex)
            {
                SwitchWeaponWithAnimation();
                playerAnimationHandler.PlayTargetAnimation("Rifle Empty");
            }
        }
        else if (inputHandler.switchSniperInput)
        {
            currentGrenadeIndex = 0;
            nextWeaponIndex = 4;
            inputHandler.switchSniperInput = false;
            if (currentWeaponIndex != nextWeaponIndex)
            {
                SwitchWeaponWithAnimation();
                playerAnimationHandler.PlayTargetAnimation("Rifle Empty");
            }
        }
        else if (inputHandler.switchMeleeInput)
        {
            currentGrenadeIndex = 0;
            nextWeaponIndex = 5;
            inputHandler.switchMeleeInput = false;
            if (currentWeaponIndex != nextWeaponIndex)
            {
                SwitchWeaponWithAnimation();
            }
        }
        else if (inputHandler.switchGrenadeInput)
        {
            nextWeaponIndex = grenadeIndexes[currentGrenadeIndex++];
            if (currentGrenadeIndex >= grenadeIndexes.Length)
            {
                currentGrenadeIndex = 0;
            }
            inputHandler.switchGrenadeInput = false;
            if (currentWeaponIndex != nextWeaponIndex)
            {
                SwitchWeaponWithAnimation();
            }
        }
    }

    public void ActivateWeapon()
    {
        if (nextWeaponIndex >= weaponModels.Count)
        {
            return;
        }

        if (currentWeapon is SniperWeapon)
        {
            SniperWeapon sniperWeapon = currentWeapon as SniperWeapon;
            sniperWeapon.CloseScope();
        }

        for (int i = 0; i < weaponModels.Count; i++)
        {
            if (i == nextWeaponIndex)
            {
                weaponModels[i].SetActive(true);
                fpsWeaponModels[i].SetActive(true);
                currentWeapon = weaponModels[i].GetComponent<Weapon>();
                currentWeaponItem = weapons[i];
                if (currentWeapon is GunWeapon)
                {
                    weaponImage.sprite = currentWeaponItem.itemIcon;
                    playerUIManager.CurrentWeapon = currentWeapon as GunWeapon;
                }
                else if (currentWeapon is GrenadeWeapon)
                {
                    playerUIManager.CurrentWeapon = null;
                }
                else if (currentWeapon is MeleeWeapon)
                {
                    weaponImage.sprite = currentWeaponItem.itemIcon;
                    playerUIManager.CurrentWeapon = null;
                }
                CurrentWeaponAnimator = fpsWeaponModels[i].GetComponent<Animator>();
                currentWeaponIndex = nextWeaponIndex;
            }
            else
            {
                weaponModels[i].SetActive(false);
                fpsWeaponModels[i].SetActive(false);
            }
        }
    }

    public void DeactivateWeaponAndActivateHealthKit()
    {
        // Deactivate current weapon
        SwitchWeaponWithAnimation();
        weaponModels[currentWeaponIndex].SetActive(false);
        fpsWeaponModels[currentWeaponIndex].SetActive(false);

        // Activate health kit
        healthKitModel.transform.GetChild(0).gameObject.SetActive(true);
        healthKitFPSModel.SetActive(true);
    }

    public void ReactivateWeapon()
    {
        // Deactivate health kit
        healthKitModel.transform.GetChild(0).gameObject.SetActive(false);
        healthKitFPSModel.SetActive(false);

        // Activate current weapon
        weaponModels[currentWeaponIndex].SetActive(true);
        fpsWeaponModels[currentWeaponIndex].SetActive(true);
    }

    public void CallWeaponAnimation(string animationName, bool isInteracting)
    {
        playerAnimationHandler.PlayTargetAnimation(animationName, isInteracting, CurrentWeaponAnimator);
    }

    void SwitchWeaponWithAnimation()
    {
        if (weapons[currentWeaponIndex] is GunItem)
        {
            CallWeaponAnimation("Hide", true);
        }
        else if (weapons[currentWeaponIndex] is KnifeItem)
        {
            CallWeaponAnimation("Hide", true);
        }
        else if (weapons[currentWeaponIndex] is GrenadeItem)
        {
            if (CurrentWeaponAnimator.GetBool("IsCooking"))
            {
                DestroyGrenade();
                ActivateWeapon();
                inputHandler.primaryFireInput = false;
            }
            else
            {
                CallWeaponAnimation("Hide", true);
            }
        }

    }

    void DestroyGrenade()
    {
        GrenadeWeapon grenadeWeapon = currentWeapon as GrenadeWeapon;
        if (grenadeWeapon != null)
        {
            Destroy(grenadeWeapon.instantiatedGrenade);
        }
    }
}
