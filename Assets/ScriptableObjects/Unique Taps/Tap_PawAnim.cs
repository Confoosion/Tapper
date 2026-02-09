using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tap_PawAnim : MonoBehaviour
{
    void Awake()
    {
        PlayAnim();    
    }

    [SerializeField] private Image tapImage;
    [SerializeField] private float radius;
    [SerializeField] private float animTime;
    [SerializeField] private float fadeOutTime;
    private Vector3 centerPos;
    private Coroutine animRoutine;
    public void PlayAnim()
    {
        if(animRoutine == null)
            animRoutine = StartCoroutine(Anim());
    }

    IEnumerator Anim()
    {
        centerPos = transform.position;

        Vector3 randomPoint = Random.insideUnitCircle.normalized * radius;
        transform.position = centerPos + randomPoint; // Randomize position

        Vector3 direction = centerPos - transform.position; // Change rotation towards center
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        tapImage.enabled = true;

        LeanTween.move(this.gameObject, centerPos, animTime).setEase(LeanTweenType.easeOutCirc);

        yield return new WaitForSeconds(animTime * 0.5f);

        yield return StartCoroutine(FadeOut());

        tapImage.enabled = false;
        animRoutine = null;
    }

    IEnumerator FadeOut()
    {
        float currentTime = 0f;
        while(currentTime < fadeOutTime)
        {
            currentTime += Time.deltaTime;

            float newAlpha = Mathf.Lerp(1f, 0f, currentTime / fadeOutTime);
            tapImage.color = new Color(tapImage.color.r, tapImage.color.g, tapImage.color.b, newAlpha);

            yield return null;
        }

        tapImage.color = new Color(tapImage.color.r, tapImage.color.g, tapImage.color.b, 0f);
        yield return null;
    }
}
