using UnityEngine;
using UnityEngine.UI;

public class Tap_PawAnim : MonoBehaviour
{
    void Awake()
    {
        PlayAnim();    
    }

    [SerializeField] private Image tapImage;
    [SerializeField] private float radius;
    [SerializeField] private float animTime;
    private Vector3 centerPos;
    public void PlayAnim()
    {
        centerPos = transform.position;

        Vector3 randomPoint = Random.insideUnitCircle.normalized * radius;
        transform.position = centerPos + randomPoint; // Randomize position

        Vector3 direction = centerPos - transform.position; // Change rotation towards center
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        tapImage.enabled = true;

        LeanTween.move(this.gameObject, centerPos, animTime).setEase(LeanTweenType.easeOutCirc);
    }
}
