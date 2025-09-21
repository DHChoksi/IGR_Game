using UnityEngine;

public class HUDObjectiveMarker : MonoBehaviour
{
    public Transform playerCamera;   // VR headset camera
    public Transform objective;
    public RectTransform markerUI;
    public Transform canvas;// UI Image on Canvas (Screen Space - Camera)

    private void Start()
    {
        markerUI.SetParent(canvas);
    }

    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(objective.position);

        if (screenPos.z > 0) // In front of camera
        {
            markerUI.anchorMin = screenPos;
            markerUI.anchorMax = screenPos;
            markerUI.gameObject.SetActive(true);
        }
        else
        {
            markerUI.gameObject.SetActive(false);
        }
    }
}
