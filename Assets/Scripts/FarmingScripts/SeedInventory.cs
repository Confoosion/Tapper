using UnityEngine;
using System.Collections.Generic;

public class SeedInventory : MonoBehaviour
{
    public static SeedInventory Instance;
    
    [SerializeField] private List<SeedData> availableSeeds = new List<SeedData>();
    [SerializeField] private SeedData currentSeed;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    public void AddSeed(string name, Sprite sprite, int quantity)
    {
        SeedData existing = availableSeeds.Find(s => s.seedName == name);
        if (existing != null)
            existing.quantity += quantity;
        else
            availableSeeds.Add(new SeedData(name, sprite, quantity));
    }
    
    public void SetCurrentSeed(SeedData seed)
    {
        currentSeed = seed;
    }
    
    public SeedData GetCurrentSeed()
    {
        return currentSeed;
    }
    
    public List<SeedData> GetAllSeeds()
    {
        return availableSeeds;
    }
    
    public bool UseSeed()
    {
        if (currentSeed != null && currentSeed.quantity > 0)
        {
            currentSeed.quantity--;
            return true;
        }
        return false;
    }
}