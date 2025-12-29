using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedSelectionMenu : MonoBehaviour
{
    [SerializeField] private GameObject seedButtonPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private SeedSlot seedSlot;
    
    void OnEnable()
    {
        PopulateMenu();
    }
    
    void PopulateMenu()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        
        var seeds = SeedInventory.Instance.GetAllSeeds();
        foreach (SeedData seed in seeds)
        {
            if (seed.quantity > 0)
            {
                GameObject btn = Instantiate(seedButtonPrefab, contentParent);
                
                Image img = btn.transform.Find("SeedImage").GetComponent<Image>();
                img.sprite = seed.seedSprite;
                
                TextMeshProUGUI txt = btn.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();
                txt.text = $"{seed.seedName} x{seed.quantity}";
                
                Button button = btn.GetComponent<Button>();
                button.onClick.AddListener(() => SelectSeed(seed));
            }
        }
    }
    
    void SelectSeed(SeedData seed)
    {
        SeedInventory.Instance.SetCurrentSeed(seed);
        seedSlot.UpdateDisplay();
        gameObject.SetActive(false);
    }
    
    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
}