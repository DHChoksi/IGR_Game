using UnityEngine;

public interface ITargetable { void OnHoverEnter(CrosshairTargeting crosshair,RaycastHit hit); void OnHoverExit(); CrosshairTargeting crosshair { get; set; } }

public class CrosshairTargeting : MonoBehaviour
{
    [Header("Crosshair (world-space)")]
    [SerializeField] Transform crosshair;              // small quad/circle in world space
    [SerializeField] SpriteRenderer crosshairSpr;              // small quad/circle in world space
    [SerializeField] float fixedCrosshairDistance = 2.0f; // distance ahead of the controller
    [SerializeField] float baseSize = 0.03f;           // size at 1m

    [SerializeField] bool alwaysShowCrosshair = true;

    [Header("Raycast (from Camera, through Crosshair)")]
    [SerializeField] float maxDistance = 15f;
    [SerializeField] LayerMask hitMask = ~0;

    [Header("Beam (from Controller -> Crosshair)")]
    [SerializeField] bool drawBeam = true;
    [SerializeField] LineRenderer line;

    [Header("Billboard")]
    [SerializeField] bool smoothRotation = true;
    [SerializeField] float rotateLerp = 20f;

    [Header("WebShooter")]
    public WebShooter webShooter;

    // internals
    Camera _cam; Transform _camT;
    bool _hasHit; Transform _lastHitRoot;
    public Ray camRay;

    void Awake()
    {
        _cam = Camera.main; _camT = _cam ? _cam.transform : null;
        if (crosshair) crosshair.gameObject.SetActive(false);
        if (line) line.enabled = drawBeam;
        crosshair.localScale = Vector3.one * baseSize;
    }

    void Update()
    {
        if (!_cam && Camera.main) { _cam = Camera.main; _camT = _cam.transform; }
        if (!_camT) return;

        // 1) Place the crosshair at a FIXED distance in front of the controller
        Vector3 origin = transform.position;
        Vector3 dir = transform.forward;
        Vector3 crosshairPos = origin + dir * fixedCrosshairDistance;

        if (crosshair)
        {
            bool show = alwaysShowCrosshair || _hasHit; // keep visible by default
            if (show && !crosshair.gameObject.activeSelf) crosshair.gameObject.SetActive(true);
            else if (!show && crosshair.gameObject.activeSelf) crosshair.gameObject.SetActive(false);

            if (show)
            {
                crosshair.position = crosshairPos;

                // Face camera
                Quaternion desiredRot = Quaternion.LookRotation((_camT.position - crosshair.position).normalized, Vector3.up);
                crosshair.rotation = smoothRotation
                    ? Quaternion.Slerp(crosshair.rotation, desiredRot, 1f - Mathf.Exp(-rotateLerp * Time.deltaTime))
                    : desiredRot;
            }
        }

        // 2) Ray FROM CAMERA that PASSES THROUGH the crosshair
        Vector3 toCrosshair = crosshair ? (crosshair.position - _camT.position) : _camT.forward;
        if (toCrosshair.sqrMagnitude < 1e-8f) toCrosshair = _camT.forward; // safety
        camRay = new Ray(_camT.position, toCrosshair.normalized);

        bool hitSomething = Physics.Raycast(camRay, out RaycastHit hit, maxDistance, hitMask, QueryTriggerInteraction.Ignore);

        // 3) Hover enter/exit based on camera ray hits
        if (hitSomething)
        {
            _hasHit = true;
            crosshairSpr.color = Color.green;
            Transform root = hit.collider.attachedRigidbody ? hit.collider.attachedRigidbody.transform.root
                                                            : hit.collider.transform.root;
            if (_lastHitRoot != root)
            {
                if (_lastHitRoot)
                    foreach (var t in _lastHitRoot.GetComponentsInChildren<ITargetable>(true)) t.OnHoverExit();
                if (root)
                    foreach (var t in root.GetComponentsInChildren<ITargetable>(true)) t.OnHoverEnter(this, hit);
                _lastHitRoot = root;
            }
        }
        else
        {
            crosshairSpr.color = Color.white;
            if (_hasHit && _lastHitRoot)
            {
                foreach (var t in _lastHitRoot.GetComponentsInChildren<ITargetable>(true)) t.OnHoverExit();
                _lastHitRoot = null;
            }
            _hasHit = false;
        }

        // 4) Optional beam from controller to crosshair
        if (line && drawBeam)
        {
            line.positionCount = 2;
            line.SetPosition(0, origin);
            line.SetPosition(1, crosshair ? crosshair.position : crosshairPos);
        }
    }
}
