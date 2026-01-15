using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public enum ShopCategory { Animals, Backgrounds, Taps }
    public enum ShopButtonState { Locked, Unlocked, Equipped }

    public ShopCategory currentShopType;
    public ShopButtonState currentShopState;

    [Space]

    [Header("Shop UI")]
    [SerializeField] private Image animal_BG;
    [SerializeField] private Image animal_Set;

    [SerializeField] private Image shopButton;
    [SerializeField] private Sprite[] shopButtonSprites;
    [SerializeField] private TextMeshProUGUI shopTitle;
    [SerializeField] private TextMeshProUGUI shopCost;

    [Space]
    [SerializeField] private int currentShopIndex = 0;
    private int MAX_ShopIndex;
    [SerializeField] private AnimalSet_SO[] animalSets;

    // SHOP CATEGORY
    private void UpdateShopCategoryVisuals(int index)
    {
        string title;
        int price;

        switch(currentShopType)
        {
            case ShopCategory.Animals:
                {
                    animal_BG.sprite = animalSets[index].preview_BG;
                    animal_Set.sprite = animalSets[index].preview_Set;
                    
                    shopTitle.SetText(animalSets[index].name);
                    shopCost.SetText(animalSets[index].preview_Price.ToString());

                    if(animalSets[index].isUnlocked)
                    {
                        if(animalSets[index].isEquipped)
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

    private void SwitchShopCategory(ShopCategory shopType)
    {
        currentShopType = shopType;
        UpdateShopCategoryVisuals(0);
    }

    public void GoToAnimalsCategory()
    {
        SwitchShopCategory(ShopCategory.Animals);
    }

    public void GoToBackgroundsCategory()
    {
        SwitchShopCategory(ShopCategory.Backgrounds);
    }

    public void GoToTapsCategory()
    {
        SwitchShopCategory(ShopCategory.Taps);
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
        if(currentShopIndex > MAX_ShopIndex)
        {
            currentShopIndex = 0;
        }

        UpdateShopCategoryVisuals(currentShopIndex);
    }

    // BUYING/EQUIPING
    public void BuyItem()
    {
        
    }

    public void EquipItem()
    {
        
    }
}
