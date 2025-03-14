using UnityEngine;

public class GridNum : MonoBehaviour
{
    public Zoom[] zooms {  get; private set; }
    public Cell[] cells;

    public int size => cells.Length;
    public int height => 9;
    public int width => 9;

    private void Awake()
    {
        zooms = GetComponentsInChildren<Zoom>();

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].coordinates = new Vector2Int(i % width, i / width);
        }
    }


    //public Cell GetCell(int x, int y)
    //{
    //    if (x >= 0 && y >= 0 && x < 9 && y < 9)
    //    {
    //        int zoomIndex = (y / 3) * 3 + (x / 3);
    //        int cellIndex = (y % 3) * 3 + (x % 3);
    //        return zooms[zoomIndex].GetCell(cellIndex);
    //    }

    //    return null;
    //}

    //public Cell GetCell(Vector2Int coordinates)
    //{
    //    return GetCell(coordinates.x, coordinates.y);
    //}
}
