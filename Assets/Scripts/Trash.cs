using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants.Constants;
using DG.Tweening;

public class Trash : MonoBehaviour
{
    [SerializeField]
    private TrashType m_TrashType = TrashType.Normal;

    public string _TrashID
    {
        get { return m_TrashID; }
        set { value = m_TrashID; }
    }

    [SerializeField]
    private string m_TrashID = "";

    [Range(0, 50f)]
    [SerializeField]
    private float m_SpinSpeed = 0.1f;

    private Vector3 m_Direction = Vector3.zero;

    private PlayerType m_PlayerType = PlayerType.MotionSickGamer;

    private Transform m_Player = null; 
    private enum m_ActivateTracking
    {
        None,
        Deactivate,
        Activate
    }

    private m_ActivateTracking m_CurrentState = m_ActivateTracking.None;

    void Start()
    {
        m_PlayerType = (PlayerType)PlayerPrefs.GetInt(CURRENT_PLAYER_TYPE, 1);
        m_Player  = GameObject.FindWithTag("Player").GetComponent<Transform>();
        SetSpinDirections();

        m_CurrentState = m_TrashType == TrashType.Normal ? m_ActivateTracking.Deactivate : m_ActivateTracking.Activate;
    }

    void Update()
    {
        Spin();

        if (m_TrashType == TrashType.Normal)
            return;

        if (m_CurrentState != m_ActivateTracking.Deactivate)
        {
            TrackPlayerDistance();
        }
    }

    void SetSpinDirections()
    { 
        if(m_PlayerType == PlayerType.MotionSickGamer)
        {
            return;
        }

        int[] number = new int[] { -1, 0, 1 };
        m_Direction = new Vector3(number[Random.Range(0, 3)], number[Random.Range(0, 3)], number[Random.Range(0, 3)]);
    }

    void Spin()
    {
        transform.Rotate(m_Direction, m_SpinSpeed * Time.deltaTime);
    }

    public void SetID(int index)
    { 
        m_TrashID = m_TrashType.ToString() + "_" +index.ToString();
    }

    public void AnimateAndDisable(Vector3 position)
    {
        transform.DOMove(position, 0.5f);
        transform.DOScale(transform.localScale * 1.02f, 0.5f)
            .OnComplete(() =>
            {
                transform.DOScale(Vector3.zero, 0.5f)
                .OnComplete(() =>
                {
                    Disable();
                });
            });
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    void ExplodeTrash()
    {
        
    }

    void ExposeToRadioactivity()
    {
        
    }

    void TrackPlayerDistance()
    {
        if (Vector3.Distance(m_Player.position, transform.position) < 2f)
        { 
            m_CurrentState = m_ActivateTracking.Deactivate;
            // Blast it and destroy
        } 
        else if (Vector3.Distance(m_Player.position, transform.position) < 5f) 
        {
            Debug.Log("____________________2f______________________");
            // Warning
        }
    }

  
}
