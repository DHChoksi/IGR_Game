using System.Collections;
using UnityEngine;
using DG.Tweening;
using static Constants.Constants;

public class Trash : MonoBehaviour
{
    [Header("Trash Settings")]
    [SerializeField] private TrashType m_TrashType = TrashType.Normal;
    [SerializeField] private string m_TrashID = "";
    [SerializeField][Range(0, 50f)] private float m_SpinSpeed = 0.1f;

    [Header("Radiation Settings")]
    [SerializeField] private float m_RadiationRadius = 10f;
    [SerializeField] private float m_CloseThreshold = 0.5f; // Threshold to disable effect when very close

    [Header("Core Distance Reset")]
    [SerializeField][Range(0f, 10000f)] private float m_MaxCoreDistance = 10000f;

    private Vector3 m_Direction = Vector3.zero;
    private Transform m_Player;
    private Transform m_Core;
    private Vector3 m_OriginalPosition;
    private bool m_IsEffectActive = false;

    private enum m_ActivateTracking { None, Deactivate, Activate }
    private m_ActivateTracking m_CurrentState = m_ActivateTracking.None;

    public string _TrashID
    {
        get => m_TrashID;
        set => m_TrashID = value;
    }

    private void Start()
    {
        m_Player = GameObject.FindWithTag("Player").transform;
        m_Core = GameObject.FindWithTag("Core")?.transform;
        m_OriginalPosition = transform.position;

        SetSpinDirections();

        m_CurrentState = m_TrashType == TrashType.Normal
            ? m_ActivateTracking.Deactivate
            : m_ActivateTracking.Activate;
    }

    private void Update()
    {
        Spin();

        if (m_TrashType != TrashType.Normal && m_CurrentState != m_ActivateTracking.Deactivate)
        {
            TrackPlayerDistance();
        }

        CheckCoreDistance();
    }

    private void SetSpinDirections()
    {
        int[] number = { -1, 0, 1 };
        m_Direction = new Vector3(
            number[Random.Range(0, 3)],
            number[Random.Range(0, 3)],
            number[Random.Range(0, 3)]
        );
    }

    private void Spin()
    {
        transform.Rotate(m_Direction, m_SpinSpeed * Time.deltaTime);
    }

    public void SetID(int index)
    {
        m_TrashID = m_TrashType + "_" + index;
    }

    public void AnimateAndDisable(Vector3 position)
    {
        transform.DOMove(position, 0.5f);
        transform.DOScale(transform.localScale * 1.02f, 0.5f)
            .OnComplete(() =>
            {
                Disable();
            });
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void TrackPlayerDistance()
    {
        if (m_Player == null) return;

        float dist = Vector3.Distance(m_Player.position, transform.position);

        if (dist <= m_RadiationRadius && dist > m_CloseThreshold)
        {
            if (!m_IsEffectActive)
            {
                m_IsEffectActive = true;
                GeneralEvents.OnHurtEffect?.Invoke(true); //  Enable hurt effect
            }
        }
        else
        {
            if (m_IsEffectActive)
            {
                m_IsEffectActive = false;
                GeneralEvents.OnHurtEffect?.Invoke(false); //  Disable hurt effect
            }
        }
    }

    private void ExplodeTrash()
    {
        Destroy(gameObject);
    }

    private void CheckCoreDistance()
    {
        if (m_Core == null) return;

        float distFromCore = Vector3.Distance(transform.position, m_Core.position);
        if (distFromCore > m_MaxCoreDistance)
        {
            Debug.Log($"📦 {name} too far from Core. Resetting position.");
            transform.position = m_OriginalPosition;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
