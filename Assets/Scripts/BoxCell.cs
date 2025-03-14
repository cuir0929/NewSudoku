using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxCell : Cell
{
    public Rigidbody2D boxRb;
    public TextMeshProUGUI numberText;
    private int number;
    private Button inputBtn;
    public TextMeshProUGUI inputNumText;

    private void Awake()
    {
        boxRb = GetComponent<Rigidbody2D>();
        inputBtn = GetComponent<Button>();
    }

    //public void Init(int newValue)
    //{
    //    // isIncorrect = false -> number is wrong
    //    // isIncorrect = true -> number is right
    //    isIncorrect = false;
    //    isBox = false;
    //    value = newValue;
    //    inputBtn.interactable = value == 0;

    //    if (value == 0)
    //    {
    //        isLocked = false;
    //        inputNumText.text = "";
    //    }
    //    else
    //    {
    //        isLocked = true;
    //        inputNumText.text = value.ToString();
    //    }

    //    UpdateState(CellState.Initial);
    //}

    public void InitBoxCell(int newNum, Vector3 position)
    {
        number = newNum;
        numberText.text = number.ToString();
        transform.position = position;
    }

    public void MoveBox(Vector3 newPosition)
    {
        boxRb.MovePosition(newPosition);
    }
}
