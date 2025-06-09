using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    [SerializeField]
    private Trash[] m_TotalTrash;
    
    [SerializeField]
    private int m_TrashThrown = 0;
    
    [SerializeField] 
    private TextMeshProUGUI m_ScoreText;

    private void Awake()
    {
        Instance = this;
    } 

    private void Start()
    {
        m_ScoreText.text =  "0 / " + (m_TotalTrash.Length).ToString();
    }

    public void OnTrashThrown(Trash trash)
    {
        m_TrashThrown++;

        if (m_TrashThrown >= m_TotalTrash.Length)
        {
            /*Debug.Log("Trash thrown " + m_TrashThrown + " | " + m_TotalTrash.Length);*/
            GeneralEvents.OnGameResult?.Invoke(true);
            return;
        }

        m_ScoreText.text = m_TrashThrown.ToString() + " / " + (m_TotalTrash.Length).ToString();
    }

}