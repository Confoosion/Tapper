using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Unique Tap", menuName = "Scriptable Objects/Themes/Unique Tap")]
public class UniqueTap : ScriptableObject
{
    public UnityEvent tapEffect;
    public void PlayTap()
    {
        tapEffect.Invoke();
    }
}
