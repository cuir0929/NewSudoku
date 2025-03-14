using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player playerPrefab;
    //public GridNum gridNum;
    public enum BigLevelType
    {
        Sudoku,
        BoxCell,
    }

    public BoxCell boxCellPrefab;
    public InputPanel inputPanelPrefab;
    public Canvas board;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timeText;
    public Zoom[] zooms;

    private InputPanel currentInputPanel;
    private bool hasGameFinished;
    private Cell[,] cells;
    private Cell selectedCell;
    private int[,] initialPuzzle;
    private float currentTime;
    private bool isTimerRunning = false;

    private const int GRID_SIZE = 9;
    private const int ZOOM_SIZE = 3;

    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        PlayerPrefs.DeleteKey("LEVEL");

        hasGameFinished = false;
        cells = new Cell[GRID_SIZE, GRID_SIZE];
        selectedCell = null;

        //SpawnCells();
        string gameMode = PlayerPrefs.GetString("GameMode", "Sudoku");
        if (gameMode == "Sudoku")
        {
            SpawnCells(BigLevelType.Sudoku);
        } else if (gameMode == "BoxCell")
        {
            SpawnCells(BigLevelType.BoxCell);
        }

        ResetTimer();

        // InputPanel use ObjectPool
        InputObjectPool.Instance.InitializePool(inputPanelPrefab, board);
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    public void SpawnCells(BigLevelType bigLevelType)
    {
        int[,] puzzleGrid = new int[GRID_SIZE, GRID_SIZE];
        int level = PlayerPrefs.GetInt("LEVEL", 0);

        // maybe have trouble in following
        // generate sudoku grid remove cell is 28-34
        if (bigLevelType == BigLevelType.BoxCell)
        {
            CreateAndStoreLevel(puzzleGrid, 350);
        }
        else
        {
            if (level == 0)
            {
                CreateAndStoreLevel(puzzleGrid, 1);
                level = 1;
            }
            else
            {
                GetCurrentLevel(puzzleGrid);
            }
        }
        // up

        levelText.text = "LEVEL" + level.ToString();
        isTimerRunning = true;

        initialPuzzle = new int[GRID_SIZE, GRID_SIZE];
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                initialPuzzle[i, j] = puzzleGrid[i, j];
                Debug.Log("puzzlegrid" + puzzleGrid[i, j]);
            }
        }

        //Debug.Log("puzzlegrid" + puzzleGrid);

        for (int i = 0; i < GRID_SIZE; i++)
        {
            Zoom zoom = zooms[i];
            List<Cell> zoomCells = zoom.cells;
            int startRow = (i / 3) * 3;
            int startCol = (i % 3) * 3;

            for (int j = 0; j < GRID_SIZE; j++)
            {
                Cell cell = zoomCells[j];
                cell.row = startRow + j / 3;
                cell.col = startCol + j % 3;
                int cellValue = puzzleGrid[cell.row, cell.col];

                Vector3 cellPosition = cell.GetCellGlobalPosition();

                switch (bigLevelType)
                {
                    case BigLevelType.BoxCell:
                        if (cellValue != 0)
                        {
                            BoxCell boxCell = Instantiate(boxCellPrefab, board.transform);
                            boxCell.InitBoxCell(cellValue, cellPosition);
                            cells[cell.row, cell.col] = boxCell;
                            cells[cell.row, cell.col].isBox = true;
                        }
                        else
                        {
                            cell.Init(cellValue);
                            cells[cell.row, cell.col] = cell;
                            cells[cell.row, cell.col].isBox = false;
                        }
                        break;

                    case BigLevelType.Sudoku:
                        cell.Init(cellValue);
                        cells[cell.row, cell.col] = cell;
                        break;
                }
            }
        }
        InitializePlayerPosition();
    }

    private void InitializePlayerPosition()
    {
        Cell[] cellArr = new Cell[9];
        for (int zoomIndex = 6;  zoomIndex <= 8; zoomIndex++)
        {
            Zoom zoom = zooms[zoomIndex];
            List<Cell> zoomCells = zoom.cells;

            for (int cellIndex = 0; cellIndex < 3; cellIndex++)
            {
                cellArr[(zoomIndex - 6) * 3 + cellIndex] = zoomCells[6 + cellIndex];
            }
        }

        int maxRetries = 8;
        int attempts = 0;

        while (attempts < maxRetries)
        {
            int index = Random.Range(0, 9);
            Cell playerCell = cells[cellArr[index].row, cellArr[index].col];

            if (!playerCell.isBox)
            {
                //Vector2Int playerPos = playerCell.coordinates;
                Vector2Int playerLogicalPos = new Vector2Int(cellArr[index].col, cellArr[index].row);
                Vector3 playerWorldPos = playerCell.transform.position;
                Player player = Instantiate(playerPrefab, board.transform);
                player.InitializePlayer(playerLogicalPos, playerWorldPos, cells);
                return;
            }
            
            attempts++;
        }
    }

    private void CreateAndStoreLevel(int[,] grid, int level)
    {
        int[,] tempGrid = Generator.GeneratePuzzle((Generator.DifficultyLevel)(level / 100));
        string arrayString = "";

        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                arrayString += tempGrid[i, j].ToString() + ",";
                grid[i, j] = tempGrid[i, j];
            }
        }

        arrayString = arrayString.TrimEnd(',');
        PlayerPrefs.SetInt("LEVEL", level);
        PlayerPrefs.SetString("Grid", arrayString);
    }

    private void GetCurrentLevel(int[,] grid)
    {
        string arrayString = PlayerPrefs.GetString("Grid");
        string[] arrayValue = arrayString.Split(',');
        int index = 0;
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                grid[i, j] = int.Parse(arrayValue[index]);
                index++;
            }
        }
    }

    public void RetryPuzzle()
    {
        hasGameFinished = false;
        selectedCell = null;

        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                cells[i, j].Init(initialPuzzle[i, j]);
            }
        }

        ResetGrid();
    }

    private void GoToNextLevel()
    {
        int level = PlayerPrefs.GetInt("LEVEL", 0);
        CreateAndStoreLevel(new int[GRID_SIZE, GRID_SIZE], level + 1);
        RestartGame();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void BackMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void ShowInputPanel()
    {
        if (currentInputPanel != null)
        {
            return;
        }

        //currentInputPanel = Instantiate(inputPanelPrefab, board.transform);

        // InputPanel use ObjectPool
        currentInputPanel = InputObjectPool.Instance.GetInputPanel();
    }

    public void ClearCurrentInputPanel()
    {
        currentInputPanel = null;
    }

    

    public void UpdateCellValue(int value)
    {
        if (hasGameFinished || selectedCell == null)
        {
            return;
        }

        selectedCell.UpdateValue(value);
        HighLight();
        CheckWin();
    }

    private void HighLight()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                cells[i, j].isIncorrect = !IsValid(cells[i, j], cells);
            }
        }

        int currentRow = selectedCell.row;
        int currentCol = selectedCell.col;
        int zoomRow = currentRow - currentRow % ZOOM_SIZE;
        int zoomCol = currentCol - currentCol % ZOOM_SIZE;

        for (int i = 0; i < GRID_SIZE; i++)
        {
            cells[i, currentCol].HighLight();
            cells[currentRow, i].HighLight();
            cells[zoomRow + i % 3, zoomCol + i / 3].HighLight();
        }

        cells[currentRow, currentCol].Select();
    }

    private bool IsValid(Cell cell, Cell[,] cells)
    {
        int row = cell.row;
        int col = cell.col;
        int value = cell.value;

        cell.value = 0;

        if (value == 0)
        {
            return true;
        }

        for (int i = 0; i < GRID_SIZE; i++)
        {
            if (cells[row, i].value == value || cells[i, col].value == value)
            {
                cell.value = value;
                return false;
            }
        }

        int zoomRow = row - row % ZOOM_SIZE;
        int zoomCol = col - col % ZOOM_SIZE;

        for (int r = zoomRow; r < zoomRow + ZOOM_SIZE; r++)
        {
            for (int c = zoomCol; c < zoomCol + ZOOM_SIZE; c++)
            {
                if (cells[r, c].value == value)
                {
                    cell.value = value;
                    return false;
                }
            }
        }

        cell.value = value;
        return true;
    }

    private void CheckWin()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if (cells[i, j].isIncorrect || cells[i, j].value == 0)
                {
                    return;
                }
            }
        }

        hasGameFinished = true;

        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                cells[i, j].UpdateWin();
            }
        }

        isTimerRunning = false;

        Invoke("GoToNextLevel", 2f);
    }

    public void OnCellSelected(Cell cell)
    {
        if (hasGameFinished || cell.isLocked)
        {
            return;
        }

        ResetGrid();
        selectedCell = cell;
        HighLight();
        ShowInputPanel();
    }

    private void ResetGrid()
    {
        for (int i = 0; i < GRID_SIZE;i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                cells[i, j].Reset();
            }
        }
    }

    public void StopGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ResetTimer()
    {
        currentTime = 0f;
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        float milliseconds = (currentTime % 1) * 100;

        timeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}
