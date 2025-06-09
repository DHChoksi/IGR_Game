using static Constants.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneChanger : MonoBehaviour
{   
   
    public void LoadScene()
    {
        LoadTargetScene(SceneName.Phase_2);
    }

    public static void LoadTargetScene(SceneName SceneName)
    {
        GeneralEvents.OnSceneChangeRequest?.Invoke(SceneName.Phase_2);
    }
}