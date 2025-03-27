using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants.Constants;
using TMPro;

public class Introduction : MonoBehaviour
{
    [Serializable]
    public class DialogesManager
    {
        [SerializeField]
        public SpeakerName _SpeakerName; 

        [SerializeField]
        public string _Dialoges;

        [SerializeField]
        public AudioClip _DialogAudioClip;
    }

    [SerializeField]
    private TextMeshProUGUI m_KaaraTextMeshPro = null;

    [SerializeField]
    private TextMeshProUGUI m_RangerTextMeshPro = null;

    [SerializeField]
    private List<DialogesManager> m_Dialoges = new List<DialogesManager>();

    [SerializeField]
    private AudioSource m_AudioSource;

    public static Action<bool> PlayClip;

    private void Start()
    {
        StartCoroutine(WaitTimer());
    }

    IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(GetDialoges());
    }
     
    private IEnumerator GetDialoges() 
    {
        for (int i = 0; i < m_Dialoges.Count; i++)
        {
            m_AudioSource.clip = m_Dialoges[i]._DialogAudioClip;
            m_AudioSource.Play();

            float time = m_Dialoges[i]._DialogAudioClip.length + 1f;
            m_KaaraTextMeshPro.text = "";
            if (m_Dialoges[i]._SpeakerName == SpeakerName.Kaara)
            {
                m_KaaraTextMeshPro.text = m_Dialoges[i]._Dialoges;
            }
            yield return new WaitForSeconds(time);

            if (i == m_Dialoges.Count - 1)
                PlayClip?.Invoke(true);
        }
    }

    

}
