using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GemAnimation : MonoBehaviour
{
    [SerializeField] private float animationTime = 1f;
    [SerializeField] private Image gemImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(AnimateGem());
        StartCoroutine(Dissipate(animationTime));
    }

    IEnumerator AnimateGem()
    {
        Vector3 moveTo = this.transform.position + new Vector3(0f, 100f, 0f);
        LeanTween.move(this.gameObject, moveTo, animationTime).setEase(LeanTweenType.easeOutQuad);
        Debug.Log("Gem moved");
        yield return null;
    }

    IEnumerator Dissipate(float duration)
    {
        Color originalColor = gemImage.color;
        float fadeTime = 0f;

        while (fadeTime < duration)
        {
            fadeTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeTime / duration);
            gemImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        gemImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        Destroy(this.gameObject);
    }
}
