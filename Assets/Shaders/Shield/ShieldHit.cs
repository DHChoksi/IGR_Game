// Assets/Scripts/ShieldHit.cs
using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class ShieldHit : MonoBehaviour
{
    [SerializeField] private Renderer rend;
    [SerializeField] private string hitCenterProp = "_HitCenterWS";
    [SerializeField] private string hitRadiusProp = "_HitRadius";
    [SerializeField] private string hitStrengthProp = "_HitStrength";

    private int _hitCenterID;
    private int _hitRadiusID;
    private int _hitStrengthID;

    private void Awake()
    {
        _hitCenterID = Shader.PropertyToID(hitCenterProp);
        _hitRadiusID = Shader.PropertyToID(hitRadiusProp);
        _hitStrengthID = Shader.PropertyToID(hitStrengthProp);
    }

    public void Pulse(Vector3 hitPosWS, float startRadius = 0.0f, float endRadius = 1.2f, float duration = 0.4f, float peakStrength = 1.5f)
    {
        StartCoroutine(PulseCR(hitPosWS, startRadius, endRadius, duration, peakStrength));
    }

    private IEnumerator PulseCR(Vector3 pos, float r0, float r1, float t, float peak)
    {
        if (rend == null) yield break;
        var mpb = new MaterialPropertyBlock();
        float timer = 0f;
        while (timer < t)
        {
            float k = timer / t;
            float radius = Mathf.Lerp(r0, r1, k);
            float strength = Mathf.Lerp(peak, 0f, k);
            rend.GetPropertyBlock(mpb);
            mpb.SetVector(_hitCenterID, pos);
            mpb.SetFloat(_hitRadiusID, radius);
            mpb.SetFloat(_hitStrengthID, strength);
            rend.SetPropertyBlock(mpb);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
