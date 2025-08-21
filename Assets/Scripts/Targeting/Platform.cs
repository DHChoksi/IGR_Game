using UnityEngine;

public class Platform : MonoBehaviour, ITargetable
{
    [SerializeField] private Renderer[] highlightRenderers;
    [SerializeField] private string keyword = "_EmissionColor";
    [SerializeField] private float intensity = 1.5f;
    private Color[] _baseColors;

    public CrosshairTargeting crosshair { get => crosshairTargeting; set => crosshairTargeting=value; }
    public CrosshairTargeting crosshairTargeting;

    void Awake()
    {
        if (highlightRenderers != null && highlightRenderers.Length > 0)
        {
            _baseColors = new Color[highlightRenderers.Length];
            for (int i = 0; i < highlightRenderers.Length; i++)
                _baseColors[i] = highlightRenderers[i].material.HasProperty(keyword)
                    ? highlightRenderers[i].material.GetColor(keyword)
                    : Color.black;
        }
    }

    public void OnHoverEnter(CrosshairTargeting crosshairTargeting,RaycastHit hit)
    {
        for (int i = 0; i < highlightRenderers.Length; i++)
        {
            var m = highlightRenderers[i].material;
            if (m.HasProperty(keyword)) m.SetColor(keyword, Color.white * intensity);
        }
        crosshair=crosshairTargeting;
        crosshair.webShooter.raycastHit = hit;
        // hook haptics / audio here if desired
    }

    public void OnHoverExit()
    {
        for (int i = 0; i < highlightRenderers.Length; i++)
        {
            var m = highlightRenderers[i].material;
            if (m.HasProperty(keyword)) m.SetColor(keyword, _baseColors[i]);
        }
        crosshair = null;
    }
}
