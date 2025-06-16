using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Constants.Constants;

public class LevelInstructions : MonoBehaviour
{
    public static LevelInstructions Instance { get; private set; }

    [Serializable]
    public class LevelMissionData
    {
        public SceneName levelScene;
        public List<DialogEntry> dialogEntries = new List<DialogEntry>();
    }

    [Serializable]
    public class DialogEntry
    {
        public string dialogText;
        public AudioClip dialogAudioClip;
    }

    [SerializeField]
    private List<LevelMissionData> m_AllMissions = new List<LevelMissionData>();

    [SerializeField]
    private TextMeshProUGUI m_DialogText;

    [SerializeField]
    private AudioSource m_AudioSource;

    private void OnEnable()
    {
        GeneralEvents.OnStartMission += HandleStartMission;
    }

    private void OnDisable()
    {
        GeneralEvents.OnStartMission -= HandleStartMission;
    }

    private void HandleStartMission(SceneName level, float time)
    {
        StartCoroutine(PlayMissionDialog(level, time));
    }

    private IEnumerator PlayMissionDialog(SceneName level, float time)
    {
        LevelMissionData missionData = m_AllMissions.Find(data => data.levelScene == level);

        yield return new WaitForSeconds(time);

        if (missionData == null || missionData.dialogEntries.Count == 0)
        {
            m_DialogText.text = "";
            yield break;
        }

        foreach (var dialog in missionData.dialogEntries)
        {
            if (dialog.dialogAudioClip != null)
            {
                m_DialogText.text = dialog.dialogText;
                m_AudioSource.clip = dialog.dialogAudioClip;
                m_AudioSource.Play();
                yield return new WaitForSeconds(dialog.dialogAudioClip.length + 0.5f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        m_DialogText.text = "";
    }
} 
