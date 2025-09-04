using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Target_SO", menuName = "Scriptable Objects/Target_SO")]
public class Target_SO : ScriptableObject
{
    public Sprite[] inSprites;
    public Sprite[] outSprites;
    public AudioClip sfx;
}
