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
    
    public static void SaveShopData(AnimalSet_SO[] animalSets)
    {
        ShopSaveData saveData = new ShopSaveData();
        
        for (int i = 0; i < animalSets.Length; i++)
        {
            AnimalSet_SO animal = animalSets[i];
            saveData.animalSets.Add(new ShopItemData(animal.name, animal));
            
            if (animal.isEquipped)
            {
                saveData.equippedAnimalSetIndex = i;
            }
        }
        
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
        
        Debug.Log($"Shop data saved to: {SavePath}");
    }
    
    public static int LoadShopData(AnimalSet_SO[] animalSets)
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("No save file found. Using default values.");
            
            if (animalSets.Length > 0)
            {
                animalSets[0].isUnlocked = true;
                animalSets[0].isEquipped = true;
            }
            
            return 0;
        }
        
        try
        {
            string json = File.ReadAllText(SavePath);
            ShopSaveData saveData = JsonUtility.FromJson<ShopSaveData>(json);
            
            Dictionary<string, ShopItemData> savedItems = new Dictionary<string, ShopItemData>();
            foreach (var item in saveData.animalSets)
            {
                savedItems[item.itemName] = item;
            }
            
            foreach (AnimalSet_SO animal in animalSets)
            {
                if (savedItems.TryGetValue(animal.name, out ShopItemData savedData))
                {
                    animal.isUnlocked = savedData.isUnlocked;
                    animal.isEquipped = savedData.isEquipped;
                    animal.price = savedData.price;
                }
            }
            
            Debug.Log("Shop data loaded successfully!");
            return saveData.equippedAnimalSetIndex;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load shop data: {e.Message}");
            return 0;
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