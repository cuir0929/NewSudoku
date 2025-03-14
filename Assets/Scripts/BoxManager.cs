using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public class Box
    {
        public Vector2Int position;
        public int number;
        public bool isPlaced;
    }

    private Dictionary<Vector2Int, Box> boxes = new Dictionary<Vector2Int, Box>();
    private Vector2Int playerPosition;
    private const int GRID_SIZE = 9;

    public void InitializeBoxes(int[,] boxLayout)
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if (boxLayout[i, j] != 0)
                {
                    Box box = new Box
                    {
                        position = new Vector2Int(i, j),
                        number = boxLayout[i, j],
                        isPlaced = false
                    };
                    boxes.Add(box.position, box);
                }
            }
        }
    }

    public bool TryMoveBox(Vector2Int from, Vector2Int to)
    {
        if (!boxes.ContainsKey(from))
        {
            return false;
        }

        if (boxes.ContainsKey(to))
        {
            return false;
        }

        if (to.x < 0 || to.x >= GRID_SIZE || to.y < 0 || to.y >= GRID_SIZE)
        {
            return false;
        }

        Box box = boxes[from];
        boxes.Remove(from);
        boxes.Add(to, box);
        box.position = to;
        return true;
    }

    public void SetPlayerPosition(Vector2Int pos)
    {
        playerPosition = pos;
    }

    public bool CanPlayerMove(Vector2Int newPos)
    {
        if (newPos.x < 0 || newPos.x >= GRID_SIZE || newPos.y < 0 || newPos.y >= GRID_SIZE)
        {
            return false;
        }

        if (boxes.ContainsKey(newPos))
        {
            Vector2Int pushDirection = newPos - playerPosition;
            Vector2Int boxNewPos = newPos + pushDirection;
            return TryMoveBox(newPos, boxNewPos);
        }

        return true;
    }
}
