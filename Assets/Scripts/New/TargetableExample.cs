using UnityEngine;

public class TargetableExample : MonoBehaviour, ITargetable
{
    [SerializeField] private Renderer[] highlightRenderers;
    [SerializeField] private string keyword = "_EmissionColor";
    [SerializeField] private float intensity = 1.5f;
    private Color[] _baseColors;

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

    public void OnHoverEnter()
    {
        for (int i = 0; i < highlightRenderers.Length; i++)
        {
            var m = highlightRenderers[i].material;
            if (m.HasProperty(keyword)) m.SetColor(keyword, Color.white * intensity);
        }
        // hook haptics / audio here if desired
    }

    public void OnHoverExit()
    {
        for (int i = 0; i < highlightRenderers.Length; i++)
        {
            var m = highlightRenderers[i].material;
            if (m.HasProperty(keyword)) m.SetColor(keyword, _baseColors[i]);
        }
    }
}
