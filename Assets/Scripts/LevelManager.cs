using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Button level0Button;
    public Button level1Button;

    private string currentMode;

    private void Start()
    {
        currentMode = PlayerPrefs.GetString("GameMode", "Sudoku");
        level0Button.onClick.AddListener(OnClickedButton0);
        level1Button.onClick.AddListener(OnClickedButton1);
    }

    public void SelectMode(string mode)
    {
        currentMode = mode;
        PlayerPrefs.SetString("GameMode", currentMode);
        PlayerPrefs.Save();
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene("Sudoku");
    }

    private void OnClickedButton0()
    {
        SelectMode("Sudoku");
    }

    private void OnClickedButton1()
    {
        SelectMode("BoxCell");
    }
}
