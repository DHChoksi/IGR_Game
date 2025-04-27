using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

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
    //PlayerHurtEffect.Instance.PlayHurtEffect();   
    public void PlayHurtEffect()
    {
        StopAllCoroutines();
        StartCoroutine(HurtSequence());
    }

    private IEnumerator HurtSequence()
    {
        float timer = 0f;

        // Fade in
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);

            SetAlpha(redFlashImage, alpha * maxRedAlpha);
            SetAlpha(vignetteImage, alpha * maxVignetteAlpha);
            yield return null;
        }

        // Hold for a split second
        yield return new WaitForSeconds(0.1f);

        // Fade out
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

            SetAlpha(redFlashImage, alpha * maxRedAlpha);
            SetAlpha(vignetteImage, alpha * maxVignetteAlpha);
            yield return null;
        }

        SetAlpha(redFlashImage, 0f);
        SetAlpha(vignetteImage, 0f);
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
