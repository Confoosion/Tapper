using UnityEngine;
using System.Linq;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Singleton { get; private set; }
    public Sprite[] goodCircle_Sprites;
    public Sprite[] badCircle_Sprites;

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    void RetrieveResources()
    {
        goodCircle_Sprites = Resources.LoadAll("Good_Circles", typeof(Sprite)).Cast<Sprite>().ToArray();
        badCircle_Sprites = Resources.LoadAll("Bad_Circles", typeof(Sprite)).Cast<Sprite>().ToArray();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RetrieveResources();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
