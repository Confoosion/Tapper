using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public enum ShopCategory { Animals, Backgrounds, Taps }
    [SerializeField] ShopCategory currentShopType;

    [Space]
    [SerializeField] AnimalSet_SO[] animalSets; 

    private void UpdateShopVisuals()
    {
        
    }

    private void SwitchShopCategory(ShopCategory shopType)
    {
        currentShopType = shopType;
        UpdateShopVisuals();
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
}
