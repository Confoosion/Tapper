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
    [SerializeField] private Image animal_Bad;
    [SerializeField] private Image animal_Small;
    [SerializeField] private Image animal_Fast;
    [SerializeField] private Image animal_Good;

    [SerializeField] private AudioClip[] animal_Sounds = new AudioClip[4];

    [SerializeField] private AnimalSet_SO[] animalSets;
    private int equippedAnimalIndex;

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

    // ========== CHANGED: Updated Start() ==========
    void Start()
    {
        // Load shop data and get equipped animal index
        equippedAnimalIndex = ShopSaveSystem.LoadShopData(animalSets);
        
        // Equip the loaded animal set (ScriptableObject never modified!)
        ThemeManager.Singleton.EquipAnimalSet(animalSets[equippedAnimalIndex]);
        
        // Initialize shop view
        // currentShopIndex = equippedAnimalIndex;
        // UpdateShopCategoryVisuals(currentShopIndex);
    }
    
    // Helper method to get runtime data for current animal
    private ShopItemData GetCurrentAnimalData()
    {
        return ShopSaveSystem.GetItemData(animalSets[currentShopIndex].name);
    }

    // ========== NEW: Save on quit ==========
    void OnApplicationQuit()
    {
        ShopSaveSystem.SaveShopData(animalSets);
    }

    // ========== NEW: Mobile-safe save (OPTIONAL but recommended) ==========
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) // App going to background
        {
            ShopSaveSystem.SaveShopData(animalSets);
        }
    }

    // SHOP CATEGORY
    private void UpdateShopCategoryVisuals(int index)
    {
        switch(currentShopType)
        {
            case ShopCategory.Animals:
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

                    shopPrice = animalSets[index].price;
                    shopCost.SetText(shopPrice.ToString());

                    // Get runtime data instead of reading from ScriptableObject
                    ShopItemData itemData = ShopSaveSystem.GetItemData(animalSets[index].name);
                    
                    if(itemData.isUnlocked)
                    {
                        if(itemData.isEquipped)
                            currentShopState = ShopButtonState.Equipped;
                        else
                            currentShopState = ShopButtonState.Unlocked;
                    }
                    else
                        currentShopState = ShopButtonState.Locked;

                    MAX_ShopIndex = animalSets.Length;

                    break;
                }
            case ShopCategory.Backgrounds:
                {
                    break;
                }
            case ShopCategory.Taps:
                {
                    break;
                }
        }

        UpdateShopButton();
    }

    private void SwitchShopCategory(ShopCategory shopType, int shopIndex)
    {
        currentShopType = shopType;
        currentShopIndex = shopIndex;
        UpdateShopCategoryVisuals(shopIndex);
    }

    public void GoToAnimalsCategory()
    {
        SwitchShopCategory(ShopCategory.Animals, equippedAnimalIndex);
    }

    public void GoToBackgroundsCategory()
    {
        SwitchShopCategory(ShopCategory.Backgrounds, 0);
    }

    public void GoToTapsCategory()
    {
        SwitchShopCategory(ShopCategory.Taps, 0);
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
                    ShopItemData itemData = ShopSaveSystem.GetItemData(animalSets[currentShopIndex].name);
                    itemData.isUnlocked = true;
                    
                    // Save immediately
                    equippedAnimalIndex = currentShopIndex;
                    ShopSaveSystem.SaveShopData(animalSets);
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
                        ShopItemData data = ShopSaveSystem.GetItemData(animal.name);
                        data.isEquipped = false;
                    }
                    
                    // Equip selected in runtime data
                    ShopItemData selectedData = ShopSaveSystem.GetItemData(animalSets[currentShopIndex].name);
                    selectedData.isEquipped = true;
                    
                    // Apply theme visually
                    ThemeManager.Singleton.EquipAnimalSet(animalSets[currentShopIndex]);
                    
                    // Save immediately
                    ShopSaveSystem.SaveShopData(animalSets);
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
        ShopSaveSystem.SaveShopData(animalSets);
    }
    
    // ========== NEW: Optional reset for testing ==========
    public void ResetShop()
    {
        ShopSaveSystem.DeleteSaveData();
        
        // Reset runtime data
        foreach(AnimalSet_SO animal in animalSets)
        {
            ShopItemData data = ShopSaveSystem.GetItemData(animal.name);
            data.isUnlocked = false;
            data.isEquipped = false;
        }
        
        // First animal unlocked and equipped
        ShopItemData firstData = ShopSaveSystem.GetItemData(animalSets[0].name);
        firstData.isUnlocked = true;
        firstData.isEquipped = true;
        
        currentShopIndex = 0;
        UpdateShopCategoryVisuals(0);
    }
}