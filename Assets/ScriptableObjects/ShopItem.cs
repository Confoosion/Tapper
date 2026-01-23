using UnityEngine;

public class ShopItem : ScriptableObject
{
    public bool isUnlocked;
    public bool isEquipped;
    public int price;
    public virtual void BuyItem()
    {
        return;
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
