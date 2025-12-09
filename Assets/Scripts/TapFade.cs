using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TapFade : MonoBehaviour
{
    [SerializeField] private float fadeTime = 0.5f;
    [SerializeField] private bool destroyAfterFade;

    void OnEnable()
    {
        StartCoroutine(Dissipate(fadeTime));
    }

    IEnumerator Dissipate(float duration)
    {
        Image _image = GetComponent<Image>();
        Color originalColor = _image.color;
        float currTime = 0f;

        while (currTime < duration)
        {
            currTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, currTime / duration);
            _image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        _image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        if(destroyAfterFade)
            Destroy(this.gameObject);
        else
            gameObject.SetActive(false);
    }
}
