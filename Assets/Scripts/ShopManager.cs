using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Singleton;

    void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }
    }

    public enum ShopCategory { Animals, Backgrounds, Taps }
    public enum ShopButtonState { Locked, Unlocked, Equipped }

    public ShopCategory currentShopType;
    public ShopButtonState currentShopState;

    [Space]

    [Header("Animals Category")]
    [SerializeField] private GameObject animal_HOLDER;
    [SerializeField] private Image animal_Bad;
    [SerializeField] private Image animal_Small;
    [SerializeField] private Image animal_Fast;
    [SerializeField] private Image animal_Good;

    [SerializeField] private AudioClip[] animal_Sounds = new AudioClip[4];

    [SerializeField] private AnimalSet_SO[] animalSets;
    private AnimalSoundSettings animal_soundSettings;
    private int equippedAnimalIndex;

    [Space]

    [Header("Backgrounds Category")]

    [SerializeField] private GameObject background_HOLDER;
    [SerializeField] private Transform background_DETAILS;
    [SerializeField] private Image background_BG;

    [SerializeField] private Background_SO[] backgroundSets;
    private GameObject background_Detail;
    private int equippedBackgroundIndex;

    [Space]

    [Header("Taps Category")]
    [SerializeField] private GameObject taps_HOLDER;
    [SerializeField] private Transform taps_PreviewPosition;
    [SerializeField] private Taps_SO[] tapSets;
    private ParticleSystem currentTapPreview;
    private GameObject currentTapPreviewObj;
    private int equippedTapIndex;

    [Space]

    [Header("General Shop UI")]
    [SerializeField] private Image shopButton;
    [SerializeField] private Sprite[] shopButtonSprites;
    [SerializeField] private TextMeshProUGUI shopTitle;
    [SerializeField] private TextMeshProUGUI shopCost;

    [Space]

    [SerializeField] private int currentShopIndex = 0;
    private int MAX_ShopIndex;
    private int shopPrice;

    void Start()
    {
        // ResetShop();
        // Debug.Log(Application.persistentDataPath);
        if (!ShopSaveSystem.SaveExists())
        {
            Debug.Log("First time player - Setting up default shop items");
            SetupDefaultShop();
        }

        int _animalIndex = ShopSaveSystem.LoadAnimalData(animalSets);
        ThemeManager.Singleton.EquipAnimalSet(animalSets[_animalIndex]);
        equippedAnimalIndex = _animalIndex;
        
        int _backgroundIndex = ShopSaveSystem.LoadBackgroundData(backgroundSets);
        ThemeManager.Singleton.EquipBackgroundSet(backgroundSets[_backgroundIndex]);
        equippedBackgroundIndex = _backgroundIndex;

        int _tapIndex = ShopSaveSystem.LoadTapData(tapSets);
        ThemeManager.Singleton.EquipTap(tapSets[_tapIndex]);
        equippedTapIndex = _tapIndex;
    }

    private void SetupDefaultShop()
    {
        ShopSaveSystem.LoadAnimalData(animalSets);
        ShopSaveSystem.LoadBackgroundData(backgroundSets);
        ShopSaveSystem.LoadTapData(tapSets);

        ShopItemData firstAnimal = ShopSaveSystem.GetAnimalData(animalSets[0].name);
        firstAnimal.isEquipped = true;
        
        ShopItemData firstBG = ShopSaveSystem.GetBackgroundData(backgroundSets[0].name);
        firstBG.isEquipped = true;

        ShopItemData firstTap = ShopSaveSystem.GetTapData(tapSets[0].name);
        firstTap.isEquipped = true;

        // Save the initial state
        ShopSaveSystem.SaveShopData(animalSets, backgroundSets, tapSets);
    }
    
    // Helper method to get runtime data
    private ShopItemData GetCurrentAnimalData()
    {
        return ShopSaveSystem.GetAnimalData(animalSets[currentShopIndex].name);
    }
    
    private ShopItemData GetCurrentBackgroundData()
    {
        return ShopSaveSystem.GetBackgroundData(backgroundSets[currentShopIndex].name);
    }

    private ShopItemData GetCurrentTapData()
    {
        return ShopSaveSystem.GetTapData(tapSets[currentShopIndex].name);
    }

    // ========== NEW: Save on quit ==========
    void OnApplicationQuit()
    {
        ShopSaveSystem.SaveShopData(animalSets, backgroundSets, tapSets);
    }

    // ========== NEW: Mobile-safe save (OPTIONAL but recommended) ==========
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) // App going to background
        {
            ShopSaveSystem.SaveShopData(animalSets, backgroundSets, tapSets);
        }
    }

    // SHOP CATEGORY
    private void UpdateShopCategoryVisuals(int index)
    {
        switch(currentShopType)
        {
            case ShopCategory.Animals:
                {
                    UpdateAnimalShop(index);
                    break;
                }
            case ShopCategory.Backgrounds:
                {
                    UpdateBackgroundShop(index);
                    break;
                }
            case ShopCategory.Taps:
                {
                    UpdateTapShop(index);
                    break;
                }
        }

        UpdateShopButton();
    }

    private void UpdateAnimalShop(int index)
    {
        animal_Bad.sprite = animalSets[index].preview_Set.badTarget;
        animal_Small.sprite = animalSets[index].preview_Set.goodTargets[0];
        animal_Fast.sprite = animalSets[index].preview_Set.goodTargets[1];
        animal_Good.sprite = animalSets[index].preview_Set.goodTargets[2];

        int clipIndex = 0;
        foreach(AudioClip audio in animalSets[index].preview_Sounds)
        {
            animal_Sounds[clipIndex] = audio;
            clipIndex++;
        }
        
        animal_soundSettings = animalSets[index].soundSettings;

        shopTitle.SetText(animalSets[index].name);

        // Get runtime data instead of reading from ScriptableObject
        ShopItemData animal_itemData = ShopSaveSystem.GetAnimalData(animalSets[index].name);
        
        if(animal_itemData.isUnlocked)
        {
            shopCost.SetText("OWNED");
            if(animal_itemData.isEquipped)
                currentShopState = ShopButtonState.Equipped;
            else
                currentShopState = ShopButtonState.Unlocked;
        }
        else
        {
            shopPrice = animalSets[index].price;
            shopCost.SetText(shopPrice.ToString());
            currentShopState = ShopButtonState.Locked;
        }

        MAX_ShopIndex = animalSets.Length;
    }

    private void UpdateBackgroundShop(int index)
    {
        background_BG.sprite = backgroundSets[index].preview_BG;

        background_Detail?.SetActive(false);

        if(!string.IsNullOrEmpty(backgroundSets[index].preview_ObjectName))
        {
            Transform foundDetails = background_DETAILS.Find(backgroundSets[index].preview_ObjectName);
            if(foundDetails != null)
            {
                background_Detail = foundDetails.gameObject;
                background_Detail.SetActive(true);
            }   
        }

        shopTitle.SetText(backgroundSets[index].name);

        ShopItemData bg_itemData = ShopSaveSystem.GetBackgroundData(backgroundSets[index].name);

        // Debug.Log("Looking at: " + backgroundSets[index].name);
        // Debug.Log("This item is " + (bg_itemData.isUnlocked ? "unlocked" : "locked"));
        if(bg_itemData.isUnlocked)
        {
            shopCost.text = "OWNED";
            if(bg_itemData.isEquipped)
                currentShopState = ShopButtonState.Equipped;
            else
                currentShopState = ShopButtonState.Unlocked;
        }
        else
        {
            shopPrice = backgroundSets[index].price;
            shopCost.SetText(shopPrice.ToString());
            currentShopState = ShopButtonState.Locked;
        }
        
        MAX_ShopIndex = backgroundSets.Length;
    }

    private void UpdateTapShop(int index)
    {        
        shopTitle.SetText(tapSets[index].name);

        ShopItemData tap_itemData = ShopSaveSystem.GetTapData(tapSets[index].name);

        if(tap_itemData.isUnlocked)
        {
            shopCost.SetText("OWNED");
            if(tap_itemData.isEquipped)
                currentShopState = ShopButtonState.Equipped;
            else
                currentShopState = ShopButtonState.Unlocked;
        }
        else
        {
            shopPrice = tapSets[index].price;
            shopCost.SetText(shopPrice.ToString());
            currentShopState = ShopButtonState.Locked;
        }

        MAX_ShopIndex = tapSets.Length;
    }

    private void SwitchShopCategory(ShopCategory shopType, int shopIndex)
    {
        HideAllCategories();
        currentShopType = shopType;
        currentShopIndex = shopIndex;
        ShowCategory(shopType);
        UpdateShopCategoryVisuals(shopIndex);
    }

    public void GoToAnimalsCategory()
    {
        SwitchShopCategory(ShopCategory.Animals, equippedAnimalIndex);
    }

    public void GoToBackgroundsCategory()
    {
        SwitchShopCategory(ShopCategory.Backgrounds, equippedBackgroundIndex);
    }

    public void GoToTapsCategory()
    {
        SwitchShopCategory(ShopCategory.Taps, equippedTapIndex);
    }

    private void HideAllCategories()
    {
        // Hiding Animals Category
        animal_HOLDER.SetActive(false);

        // Hiding Backgrounds Category
        background_HOLDER.SetActive(false);

        // Hiding Taps Category
        taps_HOLDER.SetActive(false);
    }

    private void ShowCategory(ShopCategory shopType)
    {
        switch(shopType)
        {
            case ShopCategory.Animals:
                {
                    animal_HOLDER.SetActive(true);
                    break;
                }
            case ShopCategory.Backgrounds:
                {
                    background_HOLDER.SetActive(true);
                    break;
                }
            case ShopCategory.Taps:
                {
                    taps_HOLDER.SetActive(true);
                    break;
                }
        }
    }

    // SHOP MOVEMENT
    public void UpdateShopButton()
    {
        switch(currentShopState)
        {
            case ShopButtonState.Equipped:
                {
                    shopButton.sprite = shopButtonSprites[2];
                    break;
                }
            case ShopButtonState.Unlocked:
                {
                    shopButton.sprite = shopButtonSprites[1];
                    break;
                }
            case ShopButtonState.Locked:
                {
                    shopButton.sprite = shopButtonSprites[0];
                    break;
                }
        }
    }

    public void ShopLeft()
    {
        currentShopIndex--;
        if(currentShopIndex < 0)
        {
            currentShopIndex = MAX_ShopIndex - 1;
        }

        UpdateShopCategoryVisuals(currentShopIndex);
    }

    public void ShopRight()
    {
        currentShopIndex++;
        if(currentShopIndex >= MAX_ShopIndex)
        {
            currentShopIndex = 0;
        }

        UpdateShopCategoryVisuals(currentShopIndex);
    }

    // BUYING/EQUIPING
    public void ShopButtonPressed()
    {
        switch(currentShopState)
        {
            case ShopButtonState.Locked:
                {
                    if(ScoreManager.Singleton.GetGems() >= shopPrice)
                    {
                        Debug.Log("Buying item");
                        BuyItem();   
                    }
                    else
                        Debug.Log("Too broke to buy!");
                    break;
                }
            case ShopButtonState.Unlocked:
                {
                    Debug.Log("Equipping item");
                    EquipItem();
                    break;
                }
        }

        UpdateShopCategoryVisuals(currentShopIndex);
    }

    // ========== CHANGED: Now updates runtime data instead of ScriptableObject ==========
    private void BuyItem()
    {
        switch(currentShopType)
        {
            case ShopCategory.Animals:
                {
                    // Still call BuyItem to deduct gems
                    animalSets[currentShopIndex].BuyItem();
                    
                    // Update runtime data (NOT ScriptableObject)
                    ShopItemData itemData = ShopSaveSystem.GetAnimalData(animalSets[currentShopIndex].name);
                    itemData.isUnlocked = true;
                    
                    // Save immediately
                    ShopSaveSystem.SaveShopData(animalSets, backgroundSets, tapSets);
                    break;
                }
            case ShopCategory.Backgrounds:
                {
                    backgroundSets[currentShopIndex].BuyItem();

                    ShopItemData itemData = ShopSaveSystem.GetBackgroundData(backgroundSets[currentShopIndex].name);
                    itemData.isUnlocked = true;

                    ShopSaveSystem.SaveShopData(animalSets, backgroundSets, tapSets);
                    break;
                }
            case ShopCategory.Taps:
                {
                    tapSets[currentShopIndex].BuyItem();

                    ShopItemData itemData = ShopSaveSystem.GetTapData(tapSets[currentShopIndex].name);
                    itemData.isUnlocked = true;

                    ShopSaveSystem.SaveShopData(animalSets, backgroundSets, tapSets);
                    break;
                }
        }
    }

    // ========== CHANGED: Now updates runtime data instead of ScriptableObject ==========
    private void EquipItem()
    {
        switch(currentShopType)
        {
            case ShopCategory.Animals:
                {
                    // Unequip all in runtime data
                    foreach(AnimalSet_SO animal in animalSets)
                    {
                        ShopItemData data = ShopSaveSystem.GetAnimalData(animal.name);
                        data.isEquipped = false;
                    }
                    
                    // Equip selected in runtime data
                    ShopItemData selectedData = ShopSaveSystem.GetAnimalData(animalSets[currentShopIndex].name);
                    selectedData.isEquipped = true;
                    
                    // Apply theme visually
                    ThemeManager.Singleton.EquipAnimalSet(animalSets[currentShopIndex]);
                    
                    // Save immediately
                    equippedAnimalIndex = currentShopIndex;
                    ShopSaveSystem.SaveShopData(animalSets, backgroundSets, tapSets);
                    break;
                }
            case ShopCategory.Backgrounds:
                {
                    foreach(Background_SO background in backgroundSets)
                    {
                        ShopItemData data = ShopSaveSystem.GetBackgroundData(background.name);
                        data.isEquipped = false;
                    }

                    ShopItemData selectedData = ShopSaveSystem.GetBackgroundData(backgroundSets[currentShopIndex].name);
                    selectedData.isEquipped = true;

                    ThemeManager.Singleton.EquipBackgroundSet(backgroundSets[currentShopIndex]);

                    equippedBackgroundIndex = currentShopIndex;
                    ShopSaveSystem.SaveShopData(animalSets, backgroundSets, tapSets);
                    break;
                }
            case ShopCategory.Taps:
                {
                    foreach(Taps_SO tap in tapSets)
                    {
                        ShopItemData data = ShopSaveSystem.GetTapData(tap.name);
                        data.isEquipped = false;
                    }

                    ShopItemData selectedData = ShopSaveSystem.GetTapData(tapSets[currentShopIndex].name);
                    selectedData.isEquipped = true;

                    ThemeManager.Singleton.EquipTap(tapSets[currentShopIndex]);

                    equippedTapIndex = currentShopIndex;
                    ShopSaveSystem.SaveShopData(animalSets, backgroundSets, tapSets);
                    break;
                }
        }
    }

    // ANIMAL SOUNDS
    public void PreviewAnimalSound(TargetType _targetType)
    {
        int targetIndex = (int)_targetType;
        SoundManager.Singleton.PlaySoundWithPitch(animal_Sounds[targetIndex], animal_soundSettings.soundPitches[targetIndex]);
    }

    // TAP PREVIEW
    public void PreviewTapEffect()
    {
        // Clean up previous preview if still playing
        if (currentTapPreview != null)
        {
            Destroy(currentTapPreview.gameObject);
            currentTapPreview = null;
        }

        if (currentTapPreviewObj != null)
        {
            Destroy(currentTapPreviewObj);
            currentTapPreviewObj = null;
        }
        
        Taps_SO currentTap = tapSets[currentShopIndex];

        if (currentTap.effectType == TapEffectType.Particle)
        {
            PreviewParticleEffect(currentTap);
        }
        else if (currentTap.effectType == TapEffectType.UniqueEffect)
        {
            PreviewUniqueEffect(currentTap);
        }

        SoundManager.Singleton.PlaySoundWithRandomPitch(currentTap.tapSFX);
    }

    private void PreviewParticleEffect(Taps_SO tap)
    {
        GameObject previewObj = Instantiate(
            tap.tapPrefab,
            taps_PreviewPosition.position,
            Quaternion.identity,
            taps_PreviewPosition
        );
        
        currentTapPreview = previewObj.GetComponent<ParticleSystem>();
        if (currentTapPreview != null)
        {
            currentTapPreview.Play();
            
            float duration = currentTapPreview.main.duration + currentTapPreview.main.startLifetime.constantMax;
            Destroy(previewObj, duration + 0.5f);
        }
    }

    private void PreviewUniqueEffect(Taps_SO tap)
    {
        currentTapPreviewObj = Instantiate(
            tap.tapPrefab,
            taps_PreviewPosition.position,
            Quaternion.identity,
            taps_PreviewPosition
        );
        
        Tap_PawAnim pawAnim = currentTapPreviewObj.GetComponent<Tap_PawAnim>();
        if (pawAnim != null)
        {
            pawAnim.PlayAnim();
        }
        
        // Auto-destroy after animation
        Destroy(currentTapPreviewObj, 2f);
    }
    
    // ========== Optional manual save method ==========
    public void SaveShopProgress()
    {
        ShopSaveSystem.SaveShopData(animalSets, backgroundSets, tapSets);
    }
    
    // ========== Optional reset for testing ==========
    public void ResetShop()
    {
        ShopSaveSystem.DeleteSaveData();
        
        // Reset animals
        foreach(AnimalSet_SO animal in animalSets)
        {
            ShopItemData data = ShopSaveSystem.GetAnimalData(animal.name);
            data.isUnlocked = false;
            data.isEquipped = false;
        }
        
        // Reset backgrounds
        foreach(Background_SO bg in backgroundSets)
        {
            ShopItemData data = ShopSaveSystem.GetBackgroundData(bg.name);
            data.isUnlocked = false;
            data.isEquipped = false;
        }
        
        // Reset taps
        foreach(Taps_SO tap in tapSets)
        {
            ShopItemData data = ShopSaveSystem.GetTapData(tap.name);
            data.isUnlocked = false;
            data.isEquipped = false;
        }

        // First animal and background unlocked and equipped
        ShopItemData firstAnimal = ShopSaveSystem.GetAnimalData(animalSets[0].name);
        firstAnimal.isUnlocked = true;
        firstAnimal.isEquipped = true;
        equippedAnimalIndex = 0;
        
        ShopItemData firstBG = ShopSaveSystem.GetBackgroundData(backgroundSets[0].name);
        firstBG.isUnlocked = true;
        firstBG.isEquipped = true;
        equippedBackgroundIndex = 0;

        ShopItemData firstTap = ShopSaveSystem.GetTapData(tapSets[0].name);
        firstTap.isUnlocked = true;
        firstTap.isEquipped = true;
        equippedTapIndex = 0;
        
        currentShopIndex = 0;
        UpdateShopCategoryVisuals(0);
    }
}