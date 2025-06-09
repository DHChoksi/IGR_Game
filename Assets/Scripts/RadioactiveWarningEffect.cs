using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RadioactiveWarningEffect : MonoBehaviour
{
    [SerializeField] private Image m_RadiationImage;

    private void OnEnable()
    {
        GeneralEvents.OnHurtEffect += HandleHurtEffect;
    }

    private void OnDisable()
    {
        GeneralEvents.OnHurtEffect -= HandleHurtEffect;
    }

    private void HandleHurtEffect(bool isActive)
    {
        if (m_RadiationImage == null) return;

        m_RadiationImage.enabled = isActive;
        if (isActive)
        {
            m_RadiationImage.DOFade(1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            m_RadiationImage.DOKill();
            m_RadiationImage.DOFade(0f, 0.5f);
        }
    }
}
