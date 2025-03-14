using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2Int position;
    public Vector2Int direction = Vector2Int.zero;
    //public GridNum gridNum;
    public Cell[,] cells;
    //public float moveSpeed = 5f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        Vector2Int newPosition = this.position;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            newPosition = new Vector2Int(newPosition.x, newPosition.y - 1);
            direction = Vector2Int.up;
            Debug.Log("current position : " + newPosition);
            MoveTo(newPosition);

            Debug.Log("target position : " + newPosition);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            newPosition = new Vector2Int(newPosition.x - 1, newPosition.y);
            direction = Vector2Int.left;
            Debug.Log("current position : " + newPosition);
            MoveTo(newPosition);

            Debug.Log("target position : " + newPosition);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            newPosition = new Vector2Int(newPosition.x, newPosition.y + 1);
            direction = Vector2Int.down;
            Debug.Log("current position : " + newPosition);
            MoveTo(newPosition);

            Debug.Log("target position : " + newPosition);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            newPosition = new Vector2Int(newPosition.x + 1, newPosition.y);
            direction = Vector2Int.right;
            Debug.Log("current position : " + newPosition);
            MoveTo(newPosition);

            Debug.Log("target position : " + newPosition);
        }
    }

    public void MoveTo(Vector2Int newPosition)
    {
        if (IsValidMove(newPosition))
        {
            position = newPosition;
            Cell target = cells[position.y, position.x];
            if (target != null)
            {
                Vector3 targetPosition = target.transform.position;
                rb.MovePosition(targetPosition);
            }
        }
    }

    private bool IsValidMove(Vector2Int newPosition)
    {
        if (newPosition.x < 0 || newPosition.y < 0 || newPosition.x >= 9 || newPosition.y >= 9)
        {
            return false;
        }
        Cell targetCell = cells[newPosition.y, newPosition.x];
        if (targetCell.isBox)
        {
            if (!BoxIsValid(newPosition))
            {
                return false;
            }
        }

        return targetCell != null;
    }

    private bool BoxIsValid(Vector2Int newPosition)
    {

        if (direction == Vector2Int.up)
        {
            Vector2Int nextPos = new Vector2Int(newPosition.x, newPosition.y - 1);
            if (nextPos.x < 0 || nextPos.y < 0 || nextPos.x >= 9 || nextPos.y >= 9)
            {
                return false;
            }
            Cell nextCell = cells[nextPos.y, nextPos.x];
            if (nextCell.isBox)
            {
                return false;
            }
            PushBox(newPosition, nextPos);
        } else if (direction == Vector2Int.down)
        {
            Vector2Int nextPos = new Vector2Int(newPosition.x, newPosition.y + 1);
            if (nextPos.x < 0 || nextPos.y < 0 || nextPos.x >= 9 || nextPos.y >= 9)
            {
                return false;
            }
            Cell nextCell = cells[nextPos.y, nextPos.x];
            if (nextCell.isBox)
            {
                return false;
            }
            PushBox(newPosition, nextPos);
        } else if (direction == Vector2Int.left)
        {
            Vector2Int nextPos = new Vector2Int(newPosition.x - 1, newPosition.y);
            if (nextPos.x < 0 || nextPos.y < 0 || nextPos.x >= 9 || nextPos.y >= 9)
            {
                return false;
            }
            Cell nextCell = cells[nextPos.y, nextPos.x];
            if (nextCell.isBox)
            {
                return false;
            }
            PushBox(newPosition, nextPos);
        } else if (direction == Vector2Int.right)
        {
            Vector2Int nextPos = new Vector2Int(newPosition.x + 1, newPosition.y);
            if (nextPos.x < 0 || nextPos.y < 0 || nextPos.x >= 9 || nextPos.y >= 9)
            {
                return false;
            }
            Cell nextCell = cells[nextPos.y, nextPos.x];
            if (nextCell.isBox)
            {
                return false;
            }
            PushBox(newPosition, nextPos);
        }

        return true;
    }

    private void PushBox(Vector2Int currentBoxPos, Vector2Int targetBoxPos)
    {
        Cell currentCell = cells[currentBoxPos.y, currentBoxPos.x];
        Cell nextCell = cells[targetBoxPos.y, targetBoxPos.x];

        if (nextCell == null && currentCell == null)
        {
            Debug.LogError("currentCell is null , boxPos : " + currentBoxPos);
            Debug.LogError("nextCell Ϊ null��boxPos: " + targetBoxPos);
            return;
        }

        Vector3 boxOldPos = currentCell.transform.position;
        Vector3 boxNewPos = nextCell.transform.position;
        BoxCell currentBoxCell = currentCell as BoxCell;
        //BoxCell nextBoxCell = nextCell as BoxCell;

        if (currentBoxCell == null)
        {
            Debug.LogError("current��Ԫ���� BoxCell!");
            return;
        }

        currentBoxCell.MoveBox(currentBoxCell, boxNewPos);

        cells[targetBoxPos.y, targetBoxPos.x] = currentBoxCell;
        cells[targetBoxPos.y, targetBoxPos.x].isBox = true;

        if (currentCell is BoxCell)
        {
            Cell oldPosCell = currentCell.AddComponent<Cell>();
            oldPosCell.Init(0);
            cells[currentBoxPos.y, currentBoxPos.x] = oldPosCell;
        }
        // cells[currentBoxPos.y, currentBoxPos.x].isBox = false;
        // cells[currentBoxPos.y, currentBoxPos.x].Init(0);
        

        //cells[currentBoxPos.y, currentBoxPos.x].isBox = false;
        //cells[currentBoxPos.y, currentBoxPos.x] = currentCell.Init(0);
        //nextCell = currentBoxCell;
        //nextCell.isBox = true;
        //cells[targetBoxPos.y, targetBoxPos.x] = currentBoxCell;
        //currentBoxCell.DestroyBox();
        Debug.Log("position : "+ currentBoxPos+ "cells : " + cells[currentBoxPos.y, currentBoxPos.x]);
        // currentCell = currentBoxCell.GetComponent<Cell>();
        // currentCell.Init(0);
        // currentCell.isBox = false;
        // cells[currentBoxPos.y, currentBoxPos.x] = currentCell;

        position = currentBoxPos;
        transform.position = boxOldPos;
    }

    public void InitializePlayer(Vector2Int initialLogicalPos, Vector3 initialWorldPos, Cell[,] cells)
    {
        transform.position = initialWorldPos;
        this.position = initialLogicalPos;
        this.cells = cells;
    }
}
