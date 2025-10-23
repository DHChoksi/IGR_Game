using UnityEngine;

public class Platform : MonoBehaviour, ITargetable
{
    [SerializeField] Outline outline;
    bool isHighlighted = false;

    public CrosshairTargeting crosshair { get => crosshairTargeting; set => crosshairTargeting = value; }
    public CrosshairTargeting crosshairTargeting;

    void Awake()
    {
        outline.enabled = false;
    }

    public void OnHoverEnter(CrosshairTargeting crosshairTargeting, RaycastHit hit)
    {
        if (!isHighlighted)
        {
            outline.enabled = true;
            isHighlighted = true;
        }
        crosshair = crosshairTargeting;
        crosshair.webShooter.raycastHit = hit;
        // hook haptics / audio here if desired
    }

    public void OnHoverExit()
    {
        if (isHighlighted)
        {
            outline.enabled = false;
            isHighlighted = false;
        }
        crosshair = null;
    }
}
