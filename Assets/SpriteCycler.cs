using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteCycler : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Image _dirtImage;

    [Header("Target Sprites")]
    [SerializeField] private Sprite[] inSprites;
    [SerializeField] private Sprite[] outSprites;
    [SerializeField] private Sprite[] idleSprites;

    [Header("Dirt Sprites")]
    [SerializeField] private Sprite[] dirtSprites;

    [Space]
    [SerializeField] private float cycleInterval = 0.25f;

    [SerializeField] public bool canTap = false;

    private Coroutine _animation;
    private Coroutine _idleAnimation;

    // TESTING ANIMS
    // void Start()
    // {
    //     AnimateImage(false, true);
    // }

    public void AnimateIn()
    {
        AnimateImage(true);
    }

    public void AnimateOut()
    {
        AnimateImage(false);
    }

    public void AnimateHit()
    {
        AnimateImage(false, true);
    }

    private void AnimateImage(bool animateIn, bool gotHit = false)
    {
        if (_animation == null)
        {
            if (animateIn)  // Animal is out of the hole and shown
            {
                _animation = StartCoroutine(CycleSprites(inSprites, true));
            }
            else
            {
                if (_idleAnimation != null)
                    _idleAnimation = null;

                if (gotHit) // Hit animation plays
                    _animation = StartCoroutine(CycleSprites(outSprites, false, true));
                else        // Animal goes back into hiding (reversed animation)
                    _animation = StartCoroutine(CycleSprites(inSprites, false, true));
            }
        }
    }

    private IEnumerator CycleSprites(Sprite[] sprites, bool cycleIn, bool backwards = false)
    {
        if (!backwards)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                _image.sprite = sprites[i];
                int dirtIndex = i - 1;
                if (dirtIndex >= 0 && dirtIndex < dirtSprites.Length)
                {
                    _dirtImage.gameObject.SetActive(true);
                    _dirtImage.sprite = dirtSprites[i - 1];
                }
                else
                    _dirtImage.gameObject.SetActive(false);
                yield return new WaitForSeconds(cycleInterval);
            }
        }
        else
        {
            for (int i = sprites.Length - 1; i >= 0; i--)
            {
                _image.sprite = sprites[i];
                int dirtIndex = i - 1;
                if (dirtIndex >= 0 && dirtIndex < dirtSprites.Length)
                {
                    _dirtImage.gameObject.SetActive(true);
                    _dirtImage.sprite = dirtSprites[i - 1];
                }
                else
                    _dirtImage.gameObject.SetActive(false);
                yield return new WaitForSeconds(cycleInterval);
            }
        }

        if (cycleIn)
        {
            canTap = true;
            if (idleSprites.Length > 0)
            {
                _idleAnimation = StartCoroutine(IdleAnimate(idleSprites));
            }
        }
        else
        {
            Destroy(this.gameObject);
        }

        _animation = null;
    }

    private IEnumerator IdleAnimate(Sprite[] sprites)
    {
        while (true)
        {
            foreach (Sprite _sprite in sprites)
            {
                _image.sprite = _sprite;
                yield return new WaitForSeconds(cycleInterval);
            }
        }
    }

}