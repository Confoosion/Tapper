using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Unique Tap", menuName = "Scriptable Objects/Themes/Unique Tap")]
public class UniqueTap : ScriptableObject
{
    [System.Serializable]
    public class TapEffectEvent : UnityEvent<Vector2> { }

    public TapEffectEvent tapEffect;
    public float duration;
    public void PlayTap(Vector2 position)
    {
        tapEffect.Invoke(position);
    }
}
