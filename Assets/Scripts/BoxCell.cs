using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxCell : Cell
{
    public Cell cell { get; private set; }
    public Rigidbody2D boxRb;
    public TextMeshProUGUI numberText;
    public int number;
    //public Button buttonNumber;
    //public Image buttonImage;
    public TextMeshProUGUI boxTxtNumber;
    //public CellState currentState = CellState.Box;

    private void Awake()
    {
        boxRb = GetComponent<Rigidbody2D>();
    }

    public void InitBoxCell(int newNum, Vector3 position)
    {
        //Init(0);
        number = newNum;
        numberText.text = number.ToString();
        transform.position = position;
    }

    public void MoveBox(BoxCell boxCell, Vector3 newPosition)
    {
        boxRb.MovePosition(newPosition);
    }

    // public Cell ConvertToCell()
    // {
        
    // }
}
