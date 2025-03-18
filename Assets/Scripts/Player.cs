using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2Int position;
    public Vector2Int direction = Vector2Int.zero;
    public Box[,] boxes;
    public Box boxPrefab;
    public Canvas board;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        Vector2Int newPosition = this.position;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector2Int.up;
            newPosition -= direction;

            if (IsValidMove(newPosition))
            {
                Debug.Log("newposition arrary out :" + newPosition);
                Debug.Log("arrart out : " + direction);
                MoveTo(newPosition);
                PushBox(newPosition, NextPos(newPosition, direction));
                
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Vector2Int.left;
            newPosition += direction;

            if (IsValidMove(newPosition))
            {
                Debug.Log("newposition arrary out :" + newPosition);
                Debug.Log("arrart out : " + direction);
                MoveTo(newPosition);
                PushBox(newPosition, NextPos(newPosition, direction));
                
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector2Int.down;
            newPosition -= direction;

            if (IsValidMove(newPosition))
            {
                Debug.Log("newposition arrary out :" + newPosition);
                Debug.Log("arrart out : " + direction);
                MoveTo(newPosition);
                PushBox(newPosition, NextPos(newPosition, direction));
                
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector2Int.right;
            newPosition += direction;

            if (IsValidMove(newPosition))
            {
                Debug.Log("newposition arrary out :" + newPosition);
                Debug.Log("arrart out : " + direction);
                MoveTo(newPosition);
                PushBox(newPosition, NextPos(newPosition, direction));
                
            }
        }
    }

    public void MoveTo(Vector2Int newPosition)
    {
        position = newPosition;
        Box targetBox = boxes[position.y, position.x];
        if (targetBox != null)
        {
            Vector3 targetPosition = targetBox.transform.position;
            transform.position = targetPosition;
        }
    }

    private bool IsValidGrid(Vector2Int newPosition)
    {
        if (newPosition.x < 0 || newPosition.y < 0 || newPosition.x >= 9 || newPosition.y >= 9)
        {
            return false;
        }

        return true;
    }

    private bool IsValidMove(Vector2Int newPosition)
    {
        if (!IsValidGrid(newPosition))
        {
            return false;
        }
        Box targetBox = boxes[newPosition.y, newPosition.x];
        if (targetBox.isBox)
        {
            if (!BoxIsValid(newPosition))
            {
                return false;
            }
        }

        //return targetBox != null;
        return true;
    }

    private Vector2Int NextPos(Vector2Int newPosition, Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            return newPosition - direction;
        }
        else if (direction == Vector2Int.down)
        {
            return newPosition - direction;
        }
        else if (direction == Vector2Int.left)
        {
            return newPosition + direction;
        }
        else if (direction == Vector2Int.right)
        {
            return newPosition + direction;
        }

        return newPosition;
    }

    private bool BoxIsValid(Vector2Int newPosition)
    {

        Vector2Int nextPos = NextPos(newPosition, direction);
        if (!IsValidGrid(nextPos))
        {
            return false;
        }
        Box nextBox = boxes[nextPos.y, nextPos.x];
        if (nextBox.isBox)
        {
            return false;
        }

        return true;
    }

    private void PushBox(Vector2Int currentBoxPos, Vector2Int targetBoxPos)
    {
        Box currentBox = boxes[currentBoxPos.y, currentBoxPos.x];
        Box nextBox = boxes[targetBoxPos.y, targetBoxPos.x];

        if (currentBox == null || nextBox == null)
        {
            Debug.LogError("currentBox is null , boxPos : " + currentBoxPos);
            Debug.LogError("nextBox is null , boxPos: " + targetBoxPos);
            return;
        }

        if (nextBox.value != 0)
        {
            Debug.Log("nextBox is not empty!");
            return;
        }

        Vector3 boxOldPos = currentBox.transform.position;
        Vector3 boxNewPos = nextBox.transform.position;
        int currentBoxNum = currentBox.value;

        Destroy(currentBox.gameObject);
        Destroy(nextBox.gameObject);
        Box oldBox = Instantiate(boxPrefab, board.transform);
        oldBox.Init(0, boxOldPos);
        boxes[currentBoxPos.y, currentBoxPos.x] = oldBox;
        
        Box newBox = Instantiate(boxPrefab, board.transform);
        newBox.Init(currentBoxNum, boxNewPos);
        boxes[targetBoxPos.y, targetBoxPos.x] = newBox;

        position = currentBoxPos;
        transform.position = boxOldPos;
    }

    public void InitializePlayer(Vector2Int initialLogicalPos, Vector3 initialWorldPos, Box[,] boxes)
    {
        transform.position = initialWorldPos;
        this.position = initialLogicalPos;
        this.boxes = boxes;
    }
}
