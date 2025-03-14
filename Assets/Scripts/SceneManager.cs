using static Constants.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private static bool created = false;

    [SerializeField]
    private SceneName m_SceneName = SceneName.Gameplay;

    void Awake()
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

    public void LoadTargetScene()
    {
        SceneManager.LoadScene(m_SceneName.ToString());
    }
}