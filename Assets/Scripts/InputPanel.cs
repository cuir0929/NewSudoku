using UnityEngine;
using UnityEngine.UI;

public class InputPanel : MonoBehaviour
{
    [SerializeField] private Button[] numberButtons;
    [SerializeField] private Button clearButton;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        for (int i = 0; i < numberButtons.Length; i++)
        {
            int number = i + 1;
            numberButtons[i].onClick.AddListener(() => OnNumberClick(number));
        }

        if (clearButton != null)
        {
            clearButton.onClick.AddListener(() => OnNumberClick(0));
        }
    }

    private void OnNumberClick(int number)
    {
        gameManager.UpdateCellValue(number);

        InputObjectPool.Instance.ReturnToPool(this);

        gameManager.ClearCurrentInputPanel();
    }

}
