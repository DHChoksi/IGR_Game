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
    [SerializeField] private float outerRadius = 6f;
    [SerializeField] private float innerRadius = 3f;
    [SerializeField] private float deathRadius = 1.5f;

    [Header("Effects & UI")]
    [SerializeField] private GameObject warningArrow;
    [SerializeField] private ParticleSystem radiationParticles;

    private Vector3 m_Direction = Vector3.zero;
    private PlayerType m_PlayerType = PlayerType.MotionSickGamer;
    private Transform m_Player;

    private enum m_ActivateTracking { None, Deactivate, Activate }
    private m_ActivateTracking m_CurrentState = m_ActivateTracking.None;

    public string _TrashID
    {
        get => m_TrashID;
        set => m_TrashID = value;
    }

    private void Start()
    {
        m_PlayerType = (PlayerType)PlayerPrefs.GetInt(CURRENT_PLAYER_TYPE, 1);
        m_Player = GameObject.FindWithTag("Player").transform;

        SetSpinDirections();

        m_CurrentState = m_TrashType == TrashType.Normal
            ? m_ActivateTracking.Deactivate
            : m_ActivateTracking.Activate;

        if (warningArrow != null)
            warningArrow.SetActive(false);

        if (radiationParticles != null)
        {
            radiationParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void Update()
    {
        Spin();

        if (m_TrashType == TrashType.Normal) 
            return;

        if (m_CurrentState != m_ActivateTracking.Deactivate)
            TrackPlayerDistance();
    }

    private void SetSpinDirections()
    {
        if (m_PlayerType == PlayerType.MotionSickGamer) 
            return;

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
                transform.DOScale(Vector3.zero, 0.5f).OnComplete(Disable);
            });
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void TrackPlayerDistance()
    {
        float dist = Vector3.Distance(m_Player.position, transform.position);

        if (dist <= deathRadius)
        {
            ExposeToRadioactivity();
        }
        else if (dist <= innerRadius)
        {
            EnableRadiationEffect(5f);
            ShowWarningArrow(true);
        }
        else if (dist <= outerRadius)
        {
            EnableRadiationEffect(2.5f);
            ShowWarningArrow(true); 
        } 
        else
        {
            EnableRadiationEffect(1f);
            ShowWarningArrow(false);
        }
    }

    private void ShowWarningArrow(bool show)
    {
        if (warningArrow == null) 
            return;

        warningArrow.SetActive(show);

        if (show)
        {
            Vector3 dir = (transform.position - m_Player.position).normalized;
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(m_Player.position + dir * 2f);
            warningArrow.transform.position = screenPoint;
        }
    }

    private void EnableRadiationEffect(float intensity)
    {
        if (radiationParticles == null)
            return;

        var main = radiationParticles.main;
        Color color = main.startColor.color;
        color.a = Mathf.Lerp(color.a, intensity, Time.deltaTime * 5f);
        main.startColor = color;

        if (intensity > 0 && !radiationParticles.isPlaying)
            radiationParticles.Play();
        else if (intensity <= 0 && radiationParticles.isPlaying)
            radiationParticles.Stop();
    }

    private void ExposeToRadioactivity()
    {
        Debug.Log("☢️ Player entered radioactive zone!");

        EnableRadiationEffect(1f);

        if (m_TrashType == TrashType.Explosive)
        {
            ExplodeTrash();
        }
        else
        {
            Disable();
        }
    }

    private void ExplodeTrash()
    {
        Debug.Log("💥 Trash exploded!");
        // Add explosion particles, camera shake, etc. here
        Destroy(gameObject);
    }
}
