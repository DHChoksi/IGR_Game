using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ButtonInteraction : MonoBehaviour
{
    public XRRayInteractor rayInteractor;  // Reference to the XR Ray Interactor from the controller
    public Button uiButton;                // Reference to the UI Button
    private bool isHovering = false;        // To track if the button is being pointed at

    [SerializeField]
    private SceneChanger m_SceneChanger;
    void Start()
    {
        // Add listener for button interaction
        if (uiButton != null)
        {
            uiButton.onClick.AddListener(OnButtonClicked);  
        }
    }

    void Update()
    {
        CheckHovering();
    }

    // Method to detect if ray is hovering over the button
    private void CheckHovering()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (hit.transform == uiButton.transform)
            {
                if (!isHovering)
                {
                    isHovering = true;
                    OnHoverStart(); 
                }
            }
            else if (isHovering)
            {
                isHovering = false;
                OnHoverEnd();
            }
        }
        else if (isHovering)
        {
            isHovering = false;
            OnHoverEnd();
        }
    }

    // What happens when button is clicked
    private void OnButtonClicked()
    {
        Debug.Log("Button Clicked in VR!");
        m_SceneChanger.LoadTargetScene();
    }

    // Called when the ray starts hovering over the button
    private void OnHoverStart()
    {
        Debug.Log("Hovering over button");
        // You can add effects like highlighting the button here
    }

    // Called when the ray stops hovering over the button
    private void OnHoverEnd()
    {
        Debug.Log("Stopped hovering over button");
        // Remove any highlight effects here
    }
}
