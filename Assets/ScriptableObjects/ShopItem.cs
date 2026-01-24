using UnityEngine;

public class ShopItem : ScriptableObject
{
    public bool isUnlocked;
    public bool isEquipped;
    public int price;
    public void BuyItem()
    {
        ScoreManager.Singleton.AddGems(-price);
        isUnlocked = true;
    }
    
    public virtual void EquipItem()
    {
        return;
    }

    public void UnequipItem()
    {
        isEquipped = false;
    }
}
