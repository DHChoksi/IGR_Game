using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants.Constants;

public class SceneChanger : MonoBehaviour
{
    private static bool created = false;

    private void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(gameObject);
            created = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    { 
        GeneralEvents.OnSceneChangeRequest += LoadSceneByEnum;
    }

    private void OnDisable()
    {
        GeneralEvents.OnSceneChangeRequest -= LoadSceneByEnum;
    }

    private void LoadSceneByEnum(SceneName scene)
    {
        Debug.Log(scene.ToString());
        string sceneToLoad = scene.ToString(); // Convert enum to string
        SceneManager.LoadScene(sceneToLoad);
    } 
}
