using UnityEngine;
using UnityEngine.UI;

public class FarmlandTile : MonoBehaviour
{
    [SerializeField] private Image plantImage;
    private bool isPlanted = false;
    
    public void PlantSeed()
    {
        if (!isPlanted)
        {
            SeedData current = SeedInventory.Instance.GetCurrentSeed();
            if (current != null && SeedInventory.Instance.UseSeed())
            {
                plantImage.sprite = current.seedSprite;
                plantImage.enabled = true;
                isPlanted = true;
                
                FindFirstObjectByType<SeedSlot>().UpdateDisplay();
                
                Debug.Log($"Planted {current.seedName}!");
            }
        }
    }
}