using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public Trash[] allTrash;
    private int score = 0;
    private int trashThrown = 0;
    public TextMeshProUGUI ScoreText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ScoreText.text =  "0 / " + (allTrash.Length).ToString();
    }

    public void OnTrashThrown(Trash trash)
    { 
        if (trashThrown >= allTrash.Length)
        {
            WinLoseManager.Instance?.ShowResult(true);
            return;
        }

        score += 1;
        trashThrown++;
        ScoreText.text = score.ToString() + " / " + (allTrash.Length).ToString();
        
    }

    public int GetScore() => score;
}