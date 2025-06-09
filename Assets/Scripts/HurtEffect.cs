using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HurtEffect : MonoBehaviour
{
    public static HurtEffect Instance { get; private set; }

    [Header("UI Image References")]
    [SerializeField] private Image redFlashImage;
    [SerializeField] private Image vignetteImage;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.4f;
    [SerializeField] private float maxRedAlpha = 0.6f;
    [SerializeField] private float maxVignetteAlpha = 0.4f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Call this from anywhere using: HurtEffect.Instance.PlayHurtEffect();
    public void PlayHurtEffect()
    {
        Debug.Log("Play Hurt Effect");

        // Kill any ongoing tweens on the images
        redFlashImage.DOKill();
        vignetteImage.DOKill();

        // Reset alpha to 0 instantly
        SetAlpha(redFlashImage, 0f);
        SetAlpha(vignetteImage, 0f);

        // Fade in both images
        redFlashImage.DOFade(maxRedAlpha, fadeDuration);
        vignetteImage.DOFade(maxVignetteAlpha, fadeDuration);

        // Then fade out after short delay
        redFlashImage.DOFade(0f, fadeDuration).SetDelay(fadeDuration + 0.1f);
        vignetteImage.DOFade(0f, fadeDuration).SetDelay(fadeDuration + 0.1f);
    }

    private void SetAlpha(Image img, float alpha)
    {
        if (img != null)
        {
            Color color = img.color;
            color.a = alpha;
            img.color = color;
        }
    }
}
