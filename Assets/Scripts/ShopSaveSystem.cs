using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ShopItemData
{
    public string itemName;
    public bool isUnlocked;
    public bool isEquipped;
    public int price;
    
    public ShopItemData(string name, ShopItem item)
    {
        itemName = name;
        isUnlocked = item.isUnlocked;
        isEquipped = item.isEquipped;
        price = item.price;
    }
}

[Serializable]
public class ShopSaveData
{
    public List<ShopItemData> animalSets = new List<ShopItemData>();
    public int equippedAnimalSetIndex = 0;
}

public static class ShopSaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "shopData.json");
    
    // Runtime copies of shop data (NOT the ScriptableObjects!)
    private static Dictionary<string, ShopItemData> runtimeShopData = new Dictionary<string, ShopItemData>();
    
    public static void SaveShopData(AnimalSet_SO[] animalSets)
    {
        ShopSaveData saveData = new ShopSaveData();
        
        for (int i = 0; i < animalSets.Length; i++)
        {
            string itemName = animalSets[i].name;
            
            // Save from runtime data, not ScriptableObjects
            if (runtimeShopData.ContainsKey(itemName))
            {
                saveData.animalSets.Add(runtimeShopData[itemName]);
                
                if (runtimeShopData[itemName].isEquipped)
                {
                    saveData.equippedAnimalSetIndex = i;
                }
            }
        }
        
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
        
        Debug.Log($"Shop data saved to: {SavePath}");
    }
    
    public static int LoadShopData(AnimalSet_SO[] animalSets)
    {
        // Initialize runtime data from ScriptableObject defaults
        runtimeShopData.Clear();
        foreach (AnimalSet_SO animal in animalSets)
        {
            runtimeShopData[animal.name] = new ShopItemData(animal.name, animal);
        }
        
        // If save file exists, override with saved data
        if (File.Exists(SavePath))
        {
            try
            {
                string json = File.ReadAllText(SavePath);
                ShopSaveData saveData = JsonUtility.FromJson<ShopSaveData>(json);
                
                foreach (var savedItem in saveData.animalSets)
                {
                    if (runtimeShopData.ContainsKey(savedItem.itemName))
                    {
                        runtimeShopData[savedItem.itemName] = savedItem;
                    }
                }
                
                Debug.Log("Shop data loaded successfully!");
                return saveData.equippedAnimalSetIndex;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load shop data: {e.Message}");
            }
        }
        else
        {
            Debug.Log("No save file found. Using default values.");
            
            // Set defaults in runtime data
            if (animalSets.Length > 0)
            {
                string firstName = animalSets[0].name;
                runtimeShopData[firstName].isUnlocked = true;
                runtimeShopData[firstName].isEquipped = true;
            }
        }
        
        return 0;
    }
    
    // Get runtime data for an item
    public static ShopItemData GetItemData(string itemName)
    {
        return runtimeShopData.ContainsKey(itemName) ? runtimeShopData[itemName] : null;
    }
    
    // Update runtime data
    public static void UpdateItemData(string itemName, bool isUnlocked, bool isEquipped, int price)
    {
        if (runtimeShopData.ContainsKey(itemName))
        {
            runtimeShopData[itemName].isUnlocked = isUnlocked;
            runtimeShopData[itemName].isEquipped = isEquipped;
            runtimeShopData[itemName].price = price;
        }
    }
    
    public static void DeleteSaveData()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Save file deleted.");
        }
    }
    
    public static bool SaveExists()
    {
        return File.Exists(SavePath);
    }
}