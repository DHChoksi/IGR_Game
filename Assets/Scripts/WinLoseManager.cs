using UnityEngine;
using TMPro;
using System.Collections;
using static Constants.Constants;

public class WinLoseManager : MonoBehaviour
{
    public GameObject m_ResultPanel;
    public TextMeshProUGUI m_ResultText;
    private int m_CurrentLevel = 0;

    private void OnEnable()
    {
        GeneralEvents.OnGameResult += ShowResult;
    }

    private void OnDisable()
    {
        GeneralEvents.OnGameResult -= ShowResult;
    }

    public void ShowResult(bool isWin)
    {
        m_ResultPanel.SetActive(true);
        m_ResultText.text = isWin ? "You Win!" : "You Lose!";
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        m_CurrentLevel = currentLevel;
        
        StartCoroutine(DelayedSceneChange());

        int nextLevel = currentLevel == 1 ? 2 : 1;
        Debug.Log("Current Level" + currentLevel + " Next Level" + nextLevel);
        PlayerPrefs.SetInt("CurrentLevel", nextLevel);
        PlayerPrefs.Save();
    }

    private IEnumerator DelayedSceneChange()
    {
        yield return new WaitForSecondsRealtime(1f); // Wait 1 second (real time, not affected by timeScale)
        Time.timeScale = 1f;
        SceneName sceneName = m_CurrentLevel == 1 ? SceneName.Instruction : SceneName.StoryBoard;
        GeneralEvents.OnSceneChangeRequest?.Invoke(sceneName);
      
    } 

    public void HideResult()
    {
        m_ResultPanel.SetActive(false);
    }
}
