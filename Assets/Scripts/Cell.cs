using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [HideInInspector] public Vector2Int coordinates;
    [HideInInspector] public bool isBox = false;
    public enum CellState
    {
        Initial,
        Correct,
        Incorrect,
        Highlighted,
        Selected,
        Win,
        Box,
    }

    [HideInInspector] public int value;
    [HideInInspector] public int row;
    [HideInInspector] public int col;
    [HideInInspector] public bool isLocked;
    [HideInInspector] public bool isIncorrect;

    [Header("Text Colors")]
    [SerializeField] private Color initialNumberColor = new Color(0.4f, 0.25f, 0.15f, 1f);    // ����ɫ (#663F26)
    [SerializeField] private Color correctNumberColor = new Color(0.2f, 0.4f, 0.2f, 1f);      // ����ɫ (#336633)
    [SerializeField] private Color incorrectNumberColor = new Color(0.7f, 0.2f, 0.2f, 1f);    // ���ɫ (#B33333)
    [SerializeField] private Color winNumberColor = new Color(0.25f, 0.35f, 0.6f, 1f);        // ����ɫ (#405C99)

    [Header("Button Colors")]
    [SerializeField] private Color32 buttonImageColor = new Color32(208, 172, 139, 255);
    
    private TextMeshProUGUI txtNumber;
    private Button buttonNumber;
    private Image buttonImage;
    private CellState currentState = CellState.Initial;

    private void Awake()
    {
        buttonNumber = GetComponent<Button>();
        buttonNumber.onClick.AddListener(OnCellClick);
        txtNumber = GetComponentInChildren<TextMeshProUGUI>();
        buttonImage = GetComponent<Image>();
    }

    public Vector3 GetCellGlobalPosition()
    {
        return transform.position;
    }

    public void UpdateState(CellState state)
    {
        currentState = state;

        switch (currentState)
        {
            case CellState.Initial:
                txtNumber.color = initialNumberColor;
                buttonImage.color = buttonImageColor;
                break;

            case CellState.Correct:
                txtNumber.color = correctNumberColor;
                buttonImage.color = buttonImageColor;
                break;

            case CellState.Incorrect:
                txtNumber.color = incorrectNumberColor;
                buttonImage.color = buttonImageColor;
                break;

            case CellState.Highlighted:
                txtNumber.color = isLocked ? initialNumberColor :
                                  isIncorrect ? incorrectNumberColor : correctNumberColor;
                buttonImage.color = Color.white;
                break;

            case CellState.Selected:
                txtNumber.color = isIncorrect ? incorrectNumberColor : correctNumberColor;
                buttonImage.color = Color.white;
                break;

            case CellState.Win:
                txtNumber.color = winNumberColor;
                buttonImage.color = buttonImageColor;
                break;

            case CellState.Box:
                isBox = true;
                break;
        }
    }

    public void Init(int newValue)
    {
        // isIncorrect = false -> number is wrong
        // isIncorrect = true -> number is right
        isIncorrect = false;
        value = newValue;
        buttonNumber.interactable = value == 0;

        if (value == 0)
        {
            isLocked = false;
            txtNumber.text = "";
        }
        else
        {
            isLocked = true;
            txtNumber.text = value.ToString();
        }

        UpdateState(CellState.Initial);
    }

    public void UpdateValue(int newValue)
    {
        value = newValue;
        txtNumber.text = value == 0 ? "" : value.ToString();
    }

    public void UpdateWin()
    {
        UpdateState(CellState.Win);
    }

    public void Reset()
    {

        UpdateState(isLocked ? CellState.Initial :
                    isIncorrect ? CellState.Incorrect : CellState.Correct);
    }

    private void OnCellClick()
    {
        GameManager.instance.OnCellSelected(this);
    }

    public void HighLight()
    {
        UpdateState(CellState.Highlighted);
    }

    public void Select()
    {
        UpdateState(CellState.Selected);
    }

    private void OnDestroy()
    {
        buttonNumber.onClick.RemoveListener(OnCellClick);
    }
}
