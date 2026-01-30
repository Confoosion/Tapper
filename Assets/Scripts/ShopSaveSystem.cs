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
    public List<ShopItemData> backgrounds = new List<ShopItemData>();
    public int equippedAnimalSetIndex = 0;
    public int equippedBackgroundIndex = 0;
}

public static class ShopSaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "shopData.json");
    
    // Runtime copies of shop data (NOT the ScriptableObjects!)
    private static Dictionary<string, ShopItemData> runtimeAnimalData = new Dictionary<string, ShopItemData>();
    private static Dictionary<string, ShopItemData> runtimeBackgroundData = new Dictionary<string, ShopItemData>();
    
    public static void SaveShopData(AnimalSet_SO[] animalSets, Background_SO[] backgrounds)
    {
        ShopSaveData saveData = new ShopSaveData();
        
        for (int i = 0; i < animalSets.Length; i++)
        {
            string itemName = animalSets[i].name;
            
            // Save from runtime data, not ScriptableObjects
            if (runtimeAnimalData.ContainsKey(itemName))
            {
                saveData.animalSets.Add(runtimeAnimalData[itemName]);
                
                if (runtimeAnimalData[itemName].isEquipped)
                {
                    saveData.equippedAnimalSetIndex = i;
                }
            }
        }
        
        for (int i = 0; i < backgrounds.Length; i++)
        {
            string itemName = backgrounds[i].name;
            
            if (runtimeBackgroundData.ContainsKey(itemName))
            {
                saveData.backgrounds.Add(runtimeBackgroundData[itemName]);
                
                if (runtimeBackgroundData[itemName].isEquipped)
                {
                    saveData.equippedBackgroundIndex = i;
                }
            }
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
        
        Debug.Log($"Shop data saved to: {SavePath}");
    }
    
    // ========== LOAD ANIMALS ==========
    public static int LoadAnimalData(AnimalSet_SO[] animalSets)
    {
        // Initialize runtime data from ScriptableObject defaults
        runtimeAnimalData.Clear();
        foreach (AnimalSet_SO animal in animalSets)
        {
            runtimeAnimalData[animal.name] = new ShopItemData(animal.name, animal);
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
                    if (runtimeAnimalData.ContainsKey(savedItem.itemName))
                    {
                        runtimeAnimalData[savedItem.itemName] = savedItem;
                    }
                }
                
                Debug.Log("Animal data loaded successfully!");
                return saveData.equippedAnimalSetIndex;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load animal data: {e.Message}");
            }
        }
        else
        {
            Debug.Log("No save file found. Using default animal values.");
            
            // Set defaults in runtime data
            if (animalSets.Length > 0)
            {
                string firstName = animalSets[0].name;
                runtimeAnimalData[firstName].isUnlocked = true;
                runtimeAnimalData[firstName].isEquipped = true;
            }
        }
        
        return 0;
    }

    // ========== LOAD BACKGROUNDS ==========
    public static int LoadBackgroundData(Background_SO[] backgrounds)
    {
        // Initialize runtime data from ScriptableObject defaults
        runtimeBackgroundData.Clear();
        foreach (Background_SO background in backgrounds)
        {
            runtimeBackgroundData[background.name] = new ShopItemData(background.name, background);
        }
        
        // If save file exists, override with saved data
        if (File.Exists(SavePath))
        {
            try
            {
                string json = File.ReadAllText(SavePath);
                ShopSaveData saveData = JsonUtility.FromJson<ShopSaveData>(json);
                
                foreach (var savedItem in saveData.backgrounds)
                {
                    if (runtimeBackgroundData.ContainsKey(savedItem.itemName))
                    {
                        runtimeBackgroundData[savedItem.itemName] = savedItem;
                    }
                }
                
                Debug.Log("Background data loaded successfully!");
                return saveData.equippedBackgroundIndex;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load background data: {e.Message}");
            }
        }
        else
        {
            Debug.Log("No save file found. Using default background values.");
            
            // Set defaults in runtime data
            if (backgrounds.Length > 0)
            {
                string firstName = backgrounds[0].name;
                runtimeBackgroundData[firstName].isUnlocked = true;
                runtimeBackgroundData[firstName].isEquipped = true;
            }
        }
        
        return 0;
    }
    
    // ========== GET DATA ==========
    public static ShopItemData GetAnimalData(string itemName)
    {
        return runtimeAnimalData.ContainsKey(itemName) ? runtimeAnimalData[itemName] : null;
    }
    
    public static ShopItemData GetBackgroundData(string itemName)
    {
        return runtimeBackgroundData.ContainsKey(itemName) ? runtimeBackgroundData[itemName] : null;
    }
    
    // ========== UPDATE DATA ==========
    public static void UpdateAnimalData(string itemName, bool isUnlocked, bool isEquipped, int price)
    {
        if (runtimeAnimalData.ContainsKey(itemName))
        {
            runtimeAnimalData[itemName].isUnlocked = isUnlocked;
            runtimeAnimalData[itemName].isEquipped = isEquipped;
            runtimeAnimalData[itemName].price = price;
        }
    }
    
    public static void UpdateBackgroundData(string itemName, bool isUnlocked, bool isEquipped, int price)
    {
        if (runtimeBackgroundData.ContainsKey(itemName))
        {
            runtimeBackgroundData[itemName].isUnlocked = isUnlocked;
            runtimeBackgroundData[itemName].isEquipped = isEquipped;
            runtimeBackgroundData[itemName].price = price;
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