using UnityEngine;
using TMPro;

public class WinLoseManager : MonoBehaviour
{
    public static WinLoseManager Instance;
    public GameObject resultPanel;         // The UI panel (enable/disable it)
    public TextMeshProUGUI resultText;     // Reference to the TextMeshPro component
    public void ShowResult(bool isWin)
    { 
        resultPanel.SetActive(true);  // Show the panel
        resultText.text = isWin ? "You Win!" : "You Lose!";
    }

    public void HideResult()
    {
        resultPanel.SetActive(false); // Optional: hide panel if needed
    }
}
