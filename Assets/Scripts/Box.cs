using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Box : MonoBehaviour
{
    public Button boxButton;
    public TextMeshProUGUI boxTxtNumber;
    public Image boxBorder;
    public TextMeshProUGUI boxTopNumber;

    public bool isBox = false;
    public int value;
    public bool isIncorrect;

    // private void Awake()
    // {
    //     boxButton = GetComponent<Button>();
    //     boxTxtNumber = GetComponentsInChildren<TextMeshProUGUI>()[0];
    //     boxBorder = GetComponentInChildren<Image>();
    //     boxTopNumber = GetComponentsInChildren<TextMeshProUGUI>()[1];
    // }

    private void Awake()
    {
        boxButton.onClick.AddListener(OnBoxClicked);
    }

    public void Init(int newValue, Vector3 position)
    {
        isIncorrect = false;
        value = newValue;
        transform.position = position;
        if (value == 0)
        {
            isBox = false;
            boxBorder.enabled = false;
            boxTopNumber.enabled = false;
            boxTxtNumber.text = "";
        }
        else
        {
            isBox = true;
            boxBorder.enabled = true;
            boxTopNumber.enabled = true;
            boxTxtNumber.enabled = false;
            boxTopNumber.text = value.ToString();
        }
    }

    public void UpdateValue(int newValue)
    {
        int oldValue = value;
        value = newValue;
        int topValue = int.Parse(boxTopNumber.text);
        if (topValue == value || oldValue == 0)
        {
            Debug.Log("anything");
            boxTxtNumber.enabled = true;
            boxTxtNumber.text = value.ToString();
        }
        else
        {
            Debug.Log("txtnumber != topnumber");
            return;
        }
    }

    private void OnBoxClicked()
    {
        GameManager.instance.OnBoxSelected(this);
    }

    public void MoveBox(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}
