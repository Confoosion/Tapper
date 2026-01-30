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
        int _animalIndex = ShopSaveSystem.LoadAnimalData(animalSets);
        ThemeManager.Singleton.EquipAnimalSet(animalSets[_animalIndex]);
        
        int _backgroundIndex = ShopSaveSystem.LoadBackgroundData(backgroundSets);
        ThemeManager.Singleton.EquipBackgroundSet(backgroundSets[_backgroundIndex]);
        
        // // Load shop data and get equipped animal index
        // equippedAnimalIndex = ShopSaveSystem.LoadShopData(animalSets);
        
        // // Equip the loaded animal set (ScriptableObject never modified!)
        // ThemeManager.Singleton.EquipAnimalSet(animalSets[equippedAnimalIndex]);
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

    // ========== NEW: Save on quit ==========
    void OnApplicationQuit()
    {
        ShopSaveSystem.SaveShopData(animalSets, backgroundSets);
    }

    // ========== NEW: Mobile-safe save (OPTIONAL but recommended) ==========
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) // App going to background
        {
            ShopSaveSystem.SaveShopData(animalSets, backgroundSets);
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
        
        shopTitle.SetText(animalSets[index].name);

        // Get runtime data instead of reading from ScriptableObject
        ShopItemData itemData = ShopSaveSystem.GetAnimalData(animalSets[index].name);
        
        if(itemData.isUnlocked)
        {
            shopCost.SetText("OWNED");
            if(itemData.isEquipped)
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
            background_Detail = background_DETAILS.Find(backgroundSets[index].preview_ObjectName).gameObject;
            background_Detail.SetActive(true);   
        }

        ShopItemData itemData = ShopSaveSystem.GetBackgroundData(backgroundSets[index].name);

        if(itemData.isUnlocked)
        {
            if(itemData.isEquipped)
                currentShopState = ShopButtonState.Equipped;
            else
                currentShopState = ShopButtonState.Unlocked;
        }
        else
            shopPrice = backgroundSets[index].price;
            shopCost.SetText(shopPrice.ToString());
            currentShopState = ShopButtonState.Locked;
        
        MAX_ShopIndex = backgroundSets.Length;
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
        SwitchShopCategory(ShopCategory.Taps, 0);
    }

    private void HideAllCategories()
    {
        // Hiding Animals Category
        animal_HOLDER.SetActive(false);

        // Hiding Backgrounds Category
        background_HOLDER.SetActive(false);
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
        }
    }

    // SHOP MOVEMENT
    public void UpdateShopButton()
    {
        switch(currentShopState)
        {
            case ShopButtonState.Locked:
                {
                    shopButton.sprite = shopButtonSprites[0];
                    break;
                }

            case ShopButtonState.Unlocked:
                {
                    shopButton.sprite = shopButtonSprites[1];
                    break;
                }

            case ShopButtonState.Equipped:
                {
                    shopButton.sprite = shopButtonSprites[2];
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
                    // equippedAnimalIndex = currentShopIndex;
                    ShopSaveSystem.SaveShopData(animalSets, backgroundSets);
                    break;
                }
            case ShopCategory.Backgrounds:
                {
                    backgroundSets[currentShopIndex].BuyItem();

                    ShopItemData itemData = ShopSaveSystem.GetBackgroundData(backgroundSets[currentShopIndex].name);
                    itemData.isUnlocked = true;

                    // equippedBackgroundIndex = currentShopIndex;
                    ShopSaveSystem.SaveShopData(animalSets, backgroundSets);
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
                    ShopSaveSystem.SaveShopData(animalSets, backgroundSets);
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
                    ShopSaveSystem.SaveShopData(animalSets, backgroundSets);
                    break;
                }
        }
    }

    // ANIMAL SOUNDS
    public void PreviewAnimalSound(int targetIndex)
    {
        SoundManager.Singleton.PlaySound(animal_Sounds[targetIndex]);
    }
    
    // ========== NEW: Optional manual save method ==========
    public void SaveShopProgress()
    {
        ShopSaveSystem.SaveShopData(animalSets, backgroundSets);
    }
    
    // // ========== NEW: Optional reset for testing ==========
    // public void ResetShop()
    // {
    //     ShopSaveSystem.DeleteSaveData();
        
    //     // Reset runtime data
    //     foreach(AnimalSet_SO animal in animalSets)
    //     {
    //         ShopItemData data = ShopSaveSystem.GetItemData(animal.name);
    //         data.isUnlocked = false;
    //         data.isEquipped = false;
    //     }
        
    //     // First animal unlocked and equipped
    //     ShopItemData firstData = ShopSaveSystem.GetItemData(animalSets[0].name);
    //     firstData.isUnlocked = true;
    //     firstData.isEquipped = true;
        
    //     currentShopIndex = 0;
    //     UpdateShopCategoryVisuals(0);
    // }
}