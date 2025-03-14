using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants.Constants;
using TMPro;
using Random = UnityEngine.Random;

public class TrainingManager : MonoBehaviour
{
    [SerializeField]
   

    [Serializable]
    public class TrainingDialogesManager
    {
        [SerializeField] 
        public SpeakerName _SpeakerName; 

        [SerializeField]
        public string _Dialoges;

        [SerializeField]
        public TaskDone _TaskDone = TaskDone.NotDone;
    }

    [SerializeField]
    private GameObject m_RedCube1, m_RedCube2;

    [SerializeField]
    private GameObject m_BlueSphere;

    [SerializeField]
    private GameObject m_GreenSphere;

    [SerializeField]
    private TextMeshProUGUI m_KaaraTextMeshPro = null;

    [SerializeField]
    private List<TrainingDialogesManager> m_Dialoges = new List<TrainingDialogesManager>();

    [SerializeField] 
    private AudioSource m_AudioSource;

    public static Action<bool> PlayClip;

    [SerializeField]
    private List<AudioClip> m_KaaraAudio = new List<AudioClip>();
    
    private Vector3 originalPosition;

    int m_Index = 0; 

    private GameObject m_Player = null;

    private bool m_Positionreset = false;   

    private void Start()
    {
        m_GreenSphere.SetActive(false);
        m_BlueSphere.SetActive(false);  
        m_RedCube1.SetActive(false);
        m_RedCube2.SetActive(false);    
        m_Player = GameObject.FindGameObjectWithTag("Player");
        originalPosition = m_Player.transform.position;
        GetDialoges(m_Index);
    }

    void Update()
    {
        
        switch (m_Index) 
        {
            case 0:
                if (m_Dialoges[m_Index]._TaskDone == TaskDone.NotDone)
                {
                    m_RedCube1.SetActive(true);
                    m_RedCube2.SetActive(true);

                    if (Vector3.Distance(m_RedCube1.transform.position, m_Player.transform.position) < 1f ||
                        Vector3.Distance(m_RedCube2.transform.position, m_Player.transform.position) < 1f)
                    {
                        m_Dialoges[m_Index]._TaskDone = TaskDone.Done;
                        StartCoroutine(PlayAudio());
                        m_Index++;
                        GetDialoges(m_Index);
                    }
                }
                break;
            case 1:
                if (m_Dialoges[m_Index]._TaskDone == TaskDone.NotDone)
                {
                    m_RedCube1.SetActive(false);
                    m_RedCube2.SetActive(false);
                    m_BlueSphere.SetActive(true);
                    if (Vector3.Distance(m_BlueSphere.transform.position, m_Player.transform.position) < 1f)
                    {
                        m_Dialoges[m_Index]._TaskDone = TaskDone.Done;
                        StartCoroutine(PlayAudio());
                        m_Index++;
                        GetDialoges(m_Index);
                    }
                }
                break; 
            case 2:
                if (Vector3.Distance(originalPosition, m_Player.transform.position) > 0.0f && !m_Positionreset)
                {
                    Vector3.Lerp(m_Player.transform.position, originalPosition, Time.deltaTime * 1.2f);
                }
                else if (Vector3.Distance(originalPosition, m_Player.transform.position) <= 0.0f && !m_Positionreset)
                {
                    m_Positionreset = true;
                }
                else if (m_Dialoges[m_Index]._TaskDone == TaskDone.NotDone)
                {
                    m_BlueSphere.SetActive(false); 
                    m_GreenSphere.SetActive(true);  
                    if (Vector3.Distance(m_GreenSphere.transform.position, m_Player.transform.position) < 1f)
                    {
                        m_Dialoges[m_Index]._TaskDone = TaskDone.Done;
                        StartCoroutine(PlayAudio());
                        m_Index++;
                        GetDialoges(m_Index);   
                    }
                }
                break;
            
        }
    }

    private void GetDialoges(int index)
    {
        if (index < m_Dialoges.Count)
        {
            m_KaaraTextMeshPro.text = m_Dialoges[index]._Dialoges;
        }
    }

    private IEnumerator PlayAudio()
    {
       m_AudioSource.clip =  m_KaaraAudio[Random.Range(0, 1)];
       m_AudioSource.Play();
        yield return new WaitForSeconds(m_AudioSource.clip.length);

    }
}
