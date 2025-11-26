using UnityEngine;
using System.Collections;

public class NewTextAnimation : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveOffset;
    private bool isLooping = false;
    private Vector2 startPos;

    void Start()
    {
        startPos = this.transform.localPosition;
    }

    IEnumerator DoAnimation()
    {
        while(isLooping)
        {
            LeanTween.moveLocal(this.gameObject, startPos + new Vector2(0f, moveOffset), moveSpeed);

            yield return new WaitForSeconds(moveSpeed);

            LeanTween.moveLocal(this.gameObject, startPos - new Vector2(0f, moveOffset), moveSpeed);

            yield return new WaitForSeconds(moveSpeed);
        }

        yield return null;
    }

    void OnEnable()
    {
        isLooping = true;
        StartCoroutine(DoAnimation());
    }

    void OnDisable()
    {
        isLooping = false;
        StopCoroutine(DoAnimation());
    }
}
