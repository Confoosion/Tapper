using UnityEngine;
using UnityEngine.EventSystems;

public class Snowman_EE : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject snowHead;
    [SerializeField] private Vector2 head_Start;
    [SerializeField] private Vector2 head_End;

    [SerializeField] private float animTime;
    [SerializeField] private Vector3 headRotation;
    private bool wasTapped = false;

    void OnEnable()
    {
        snowHead.transform.localPosition = head_Start;
        snowHead.transform.rotation = Quaternion.Euler(Vector3.zero);
        wasTapped = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.activeSelf && !wasTapped)
        {
            LeanTween.moveLocal(snowHead, head_End, animTime);
            LeanTween.rotate(snowHead, headRotation, animTime);

            wasTapped = true;
        }
    }
}
