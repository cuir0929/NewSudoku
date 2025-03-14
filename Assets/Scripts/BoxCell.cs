using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxCell : Cell
{
    public Cell cell { get; private set; }
    public Rigidbody2D boxRb;
    public TextMeshProUGUI numberText;
    private int number;
    //public Button buttonNumber;
    //public Image buttonImage;
    public TextMeshProUGUI boxTxtNumber;
    //public CellState currentState = CellState.Box;

    private void Awake()
    {
        boxRb = GetComponent<Rigidbody2D>();
        //buttonNumber = GetComponent<Button>();
    }

    //public void Init(int newValue)
    //{
    //    // isIncorrect = false -> number is wrong
    //    // isIncorrect = true -> number is right
    //    isIncorrect = false;
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
        //Init(0);
        number = newNum;
        numberText.text = number.ToString();
        transform.position = position;
    }

    public void MoveBox(BoxCell boxCell, Vector3 newPosition)
    {
        // int boxCellValue = boxCell.number;
        // Destroy(boxCell.gameObject);
        // InitBoxCell(boxCellValue, newPosition);
        boxRb.MovePosition(newPosition);
    }

    // public void DestroyBox()
    // {
    //     Destroy(gameObject);
    // }

    // public void SpawnBox(Cell cell, int number)
    // {
    //     this.number = number;
    //     numberText.text = number.ToString();
    //     this.cell = cell;
    //     this.cell.isBox = true;
    //     transform.position = cell.transform.position;
    // }

    // public void MoveTo(Cell cell)
    // {
    //     this.cell = cell;
    //     this.cell.isBox = true;
    //     transform.position = cell.transform.position;
    // }
}
