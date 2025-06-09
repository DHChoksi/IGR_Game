using static Constants.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StorySceneChanger : MonoBehaviour
{

    public void LoadScene()
    {
        LoadTargetScene(SceneName.Phase_1);
    }

    public void LoadTargetScene(SceneName SceneName)
    {
        GeneralEvents.OnSceneChangeRequest?.Invoke(SceneName.Phase_1);
    }
}