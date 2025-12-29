using UnityEngine;

public class FarmTester : MonoBehaviour
{
    [SerializeField] private Sprite carrotSprite;
    [SerializeField] private Sprite wormSprite;
    void Start()
{
    // Add seeds to inventory
    SeedInventory.Instance.AddSeed("Carrot", carrotSprite, 2);
    SeedInventory.Instance.AddSeed("Worm", wormSprite, 1);
    
    // Set initial seed
    SeedInventory.Instance.SetCurrentSeed(
        SeedInventory.Instance.GetAllSeeds()[0]
    );
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
