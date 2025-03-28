using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Constants.Constants;

public class CrosshairController : MonoBehaviour
{
    [SerializeField]
    private Transform m_LeftController;

    [SerializeField]
    private Transform m_RightController;

    [SerializeField]
    private Image m_LeftCrosshair;

    [SerializeField]
    private Image m_RightCrosshair;

    [SerializeField]
    private Sprite m_LockTargetImage;

    [SerializeField]
    private Sprite m_OriginalCrosshair;

    [SerializeField]
    private Color m_EnemyTargetColor;

    [SerializeField]
    private Color m_GrabbableTargetColor;

    [SerializeField]
    private Color m_SwingTargetColor;

    [SerializeField]
    private Color m_OriginalColor;

    [SerializeField]
    private Vector3 m_OriginalScale;

    [SerializeField]
    private LayerMask m_SwingingPlatform;

    [SerializeField]
    private LayerMask m_GrabbableObject;

    [SerializeField]
    private LayerMask m_Enemy;

    [SerializeField]
    private LayerMask m_Core;

    [SerializeField]
    private GripAction m_LeftGripActionType = GripAction.None;

    [SerializeField]
    private GripAction m_RightGripActionType = GripAction.None;

    public GripAction _LeftGripActionType
    {
        get { return m_LeftGripActionType; }
        private set { m_LeftGripActionType = value; } 
    }

    public GripAction _RightGripActionType
    {
        get { return m_RightGripActionType; }
        private set { m_RightGripActionType = value; }
    }

    private void Update()
    { 
        UpdateCrosshair(m_LeftController, m_LeftCrosshair);
        UpdateCrosshair(m_RightController, m_RightCrosshair);
    }

    private void UpdateCrosshair(Transform controller, Image crosshair)
    {
        if (DetectRaycast(crosshair, controller.position, controller.forward, m_SwingingPlatform, PLATFORM_DETECT_DISTANCE,8))
            return;

        if (DetectRaycast(crosshair, controller.position, controller.forward, m_GrabbableObject, TRASH_DETECT_DISTANCE,9))
            return;

        if (DetectRaycast(crosshair, controller.position, controller.forward, m_Enemy, ENEMY_DETECT_DISTANCE,13))
            return;

        if (DetectRaycast(crosshair, controller.position, controller.forward, m_Core, CORE_DETECT_DISTANCE,10))
            return;

        ResetTargetWithDelay(crosshair);
        //Disabled by Lokesh
        //if (DetectRaycast(crosshair, controller.position, controller.forward, 0, MAX_HIT_DETECT_DISTANCE))
        //    return;

    }

    //LOKESH IMPLEMENTATION
    RaycastHit hit;
    private bool DetectRaycast(Image crosshair, Vector3 startPosition, Vector3 direction, LayerMask layer, float distance,int type)
    {
        if (Physics.Raycast(startPosition, direction,distance, layer))
        {
            ChangeCrosshair(crosshair, type);
            return true;
        }
        return false;
    }

    //private void DetectRaycast(Image crosshair, Vector3 startPosition, Vector3 direction, LayerMask layer, float distance)
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(startPosition, direction, out hit, distance, layer))
    //    {
    //        ChangeCrosshair(crosshair, hit.transform.gameObject.layer); 
    //    }
    //}
    
    private void ChangeCrosshair(Image crosshair, int layerMask)
    {
        ChangeSprite(crosshair);
        AnimateCrosshair(crosshair);

        switch (layerMask)
        {
            case 8: // Grabbable
                ChangeColor(crosshair, m_GrabbableTargetColor);

                /*m_LeftGripActionType = crosshair.gameObject == m_LeftCrosshair ? GripAction.HyperHook : GripAction.None;
                m_RightGripActionType = crosshair.gameObject == m_RightCrosshair ? GripAction.HyperHook : GripAction.None;  */

                break;

            case 9: // Swinging Platform
                ChangeColor(crosshair, m_SwingTargetColor);
                  
                /*m_LeftGripActionType = crosshair.gameObject == m_LeftCrosshair ? GripAction.WebSwinging : GripAction.None;  
                m_RightGripActionType = crosshair.gameObject == m_RightCrosshair ? GripAction.WebSwinging : GripAction.None;
                Debug.Log(m_LeftGripActionType.ToString() + " | " + (crosshair.gameObject == m_LeftCrosshair));  */

                break; 

            case 10: // Core 
                ChangeColor(crosshair, m_GrabbableTargetColor);
                break;

            case 13: // Enemy 
                ChangeColor(crosshair, m_EnemyTargetColor);
                break;
        }
    }

    private void AnimateCrosshair(Image crosshair)
    {
        if (DOTween.IsTweening(crosshair.transform))
            return;

        crosshair.transform.DOScale(m_OriginalScale * 1.2f, 0.25f).OnComplete(() =>
        {
            crosshair.transform.DOScale(m_OriginalScale, 0.3f);
        });
    }

    private void ChangeSprite(Image crosshair)
    {
        crosshair.sprite = m_LockTargetImage;
    }

    private void ChangeColor(Image crosshair, Color color)
    {
    /*  Debug.Log("Change Color "+color.ToString());*/
        crosshair.color = color;
    }

    private void ResetTargetWithDelay(Image crosshair)
    {
        /*  Debug.Log("Reset it !!");*/
        crosshair.sprite = m_OriginalCrosshair;
        crosshair.color = m_OriginalColor;
        if(DOTween.IsTweening(crosshair.transform))
        {
            DOTween.Kill(crosshair.transform);
        }

        crosshair.transform.localScale = m_OriginalScale;
    }

    //Disabled by Lokesh
    //private IEnumerator ResetTargetWithDelay(Image crosshair)
    //{
    //    yield return new WaitForEndOfFrame();
         
    ///*  Debug.Log("Reset it !!");*/
    //    crosshair.sprite = m_OriginalCrosshair;
    //    crosshair.color = m_OriginalColor;
    //    crosshair.transform.localScale = m_OriginalScale;
    //}
}   