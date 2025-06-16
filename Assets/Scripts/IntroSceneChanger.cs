using static Constants.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneChanger : MonoBehaviour
{
    private void Start()
    {
        GeneralEvents.OnStartMission?.Invoke(SceneName.Instruction, 3f);
    }

    public void LoadScene()
    {
        LoadTargetScene(SceneName.Phase_2);
    }

    public static void LoadTargetScene(SceneName SceneName)
    {
        GeneralEvents.OnSceneChangeRequest?.Invoke(SceneName.Phase_2);
    }
}