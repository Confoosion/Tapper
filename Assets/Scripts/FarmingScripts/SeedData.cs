using UnityEngine;

// Seed data structure
[System.Serializable]
public class SeedData
{
    public string seedName;
    public Sprite seedSprite;
    public int quantity;
    
    public SeedData(string name, Sprite sprite, int qty)
    {
        seedName = name;
        seedSprite = sprite;
        quantity = qty;
    }
}