using UnityEngine;

public class GroundAnimation : MonoBehaviour
{
    [SerializeField] private Vector2 Classic_Position;
    [SerializeField] private Vector2 Rain_Position;

    private float animationTime = 1f;

    public void UpdateGroundPosition(GameMode mode)
    {
        if (mode == GameMode.Classic)
        {
            return;
        }
        
        switch (mode)
        {
            case GameMode.Classic:
                {
                    LeanTween.moveLocal(this.gameObject, Classic_Position, animationTime).setEase(LeanTweenType.easeOutCubic);
                    break;
                }
            case GameMode.Rain:
                {
                    LeanTween.moveLocal(this.gameObject, Rain_Position, animationTime).setEase(LeanTweenType.easeOutCubic);
                    break;
                }
        }
    }
}
