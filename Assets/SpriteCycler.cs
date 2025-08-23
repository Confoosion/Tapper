using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteCycler : MonoBehaviour
{
    [Header("Image Iteration")]
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float cycleInterval = 0.5f;

    [Header("Scale Object")]
    [SerializeField] private Transform rewardObject; // The object to scale in/out
    [SerializeField] private float scaleDuration = 0.5f;
    [SerializeField] private float stayDuration = 2.0f;

    [Header("Dissipate (Fade Out)")]
    [SerializeField] private float dissipateDelayAfterScale = 0.25f; // wait after scale-out before fading
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private bool disableOnFinish = true;
    [SerializeField] private bool destroyOnFinish = false;

    private int currentIndex;
    private float timer;
    private bool finished;

    private CanvasGroup cg;

    void Awake()
    {
        if (!targetImage) targetImage = GetComponent<Image>();

        // CanvasGroup for clean fading (affects the whole Image GO, including its children)
        if (targetImage)
        {
            cg = targetImage.GetComponent<CanvasGroup>();
            if (!cg) cg = targetImage.gameObject.AddComponent<CanvasGroup>();
            cg.alpha = 1f;
        }
    }

    void OnEnable()
    {
        timer = 0f;
        finished = false;

        if (sprites != null && sprites.Length > 0 && targetImage)
        {
            targetImage.sprite = sprites[0];
            currentIndex = 1; // index 0 already shown
        }
        else
        {
            currentIndex = 0;
        }

        if (rewardObject)
            rewardObject.localScale = Vector3.zero;

        if (cg) cg.alpha = 1f;
    }

    void Update()
    {
        if (finished || sprites.Length == 0 || targetImage == null) return;

        timer += Time.deltaTime;
        if (timer >= cycleInterval)
        {
            timer = 0f;

            if (currentIndex < sprites.Length)
            {
                targetImage.sprite = sprites[currentIndex];
                currentIndex++;
            }

            if (currentIndex >= sprites.Length) // reached end
            {
                finished = true;
                StartCoroutine(SequenceRoutine());
            }
        }
    }

    private IEnumerator SequenceRoutine()
    {
        // 1) Scale sequence (in -> stay -> out)
        if (rewardObject)
        {
            // scale in
            yield return StartCoroutine(ScaleOverTime(rewardObject, Vector3.zero, Vector3.one, scaleDuration));
            // stay
            if (stayDuration > 0f) yield return new WaitForSeconds(stayDuration);
            // scale out
            yield return StartCoroutine(ScaleOverTime(rewardObject, Vector3.one, Vector3.zero, scaleDuration));
        }

        // 2) Delay before dissipating the Image
        if (dissipateDelayAfterScale > 0f)
            yield return new WaitForSeconds(dissipateDelayAfterScale);

        // 3) Dissipate (fade out) the Image
        if (cg)
            yield return StartCoroutine(FadeCanvasGroup(cg, cg.alpha, 0f, fadeDuration));
        else
            yield return StartCoroutine(FadeImageColor(targetImage, 1f, 0f, fadeDuration));

        // 4) Cleanup
        if (destroyOnFinish)
            Destroy(gameObject);
        else if (disableOnFinish)
            gameObject.SetActive(false);
    }

    private IEnumerator ScaleOverTime(Transform t, Vector3 from, Vector3 to, float duration)
    {
        float el = 0f;
        if (duration <= 0f)
        {
            t.localScale = to;
            yield break;
        }
        while (el < duration)
        {
            el += Time.deltaTime;
            float k = Mathf.Clamp01(el / duration);
            t.localScale = Vector3.Lerp(from, to, k);
            yield return null;
        }
        t.localScale = to;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration)
    {
        float el = 0f;
        if (duration <= 0f)
        {
            group.alpha = to;
            yield break;
        }
        while (el < duration)
        {
            el += Time.deltaTime;
            float k = Mathf.Clamp01(el / duration);
            group.alpha = Mathf.Lerp(from, to, k);
            yield return null;
        }
        group.alpha = to;
    }

    // Fallback if you’d rather fade the Image color (used only if CanvasGroup is missing)
    private IEnumerator FadeImageColor(Image img, float fromA, float toA, float duration)
    {
        float el = 0f;
        Color c = img.color;
        if (duration <= 0f)
        {
            c.a = toA;
            img.color = c;
            yield break;
        }
        while (el < duration)
        {
            el += Time.deltaTime;
            float k = Mathf.Clamp01(el / duration);
            c.a = Mathf.Lerp(fromA, toA, k);
            img.color = c;
            yield return null;
        }
        c.a = toA;
        img.color = c;
    }
}
