using System.Collections.Generic;
using UnityEngine;

public class InputObjectPool : MonoBehaviour
{
    private static InputObjectPool instance;
    public static InputObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("InputObjectPool");
                instance = go.AddComponent<InputObjectPool>();
            }
            return instance;
        }
    }

    private Queue<InputPanel> inputPanelPool = new Queue<InputPanel>();
    private InputPanel inputPanelPrefab;
    private Canvas boardCanvas;
    private Transform poolParent;
    private const int INITIAL_POOL_SIZE = 2;
    private InputPanel activePanel;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void InitializePool(InputPanel prefab, Canvas board)
    {
        //if (poolParent != null)
        //{
        //    foreach (InputPanel panel in inputPanelPool)
        //    {
        //        if (panel != null)
        //        {
        //            Destroy(panel.gameObject);
        //        }
        //    }

        //    Destroy(poolParent.gameObject);
        //    inputPanelPool.Clear();
        //}

        inputPanelPrefab = prefab;
        boardCanvas = board;

        GameObject poolObj = new GameObject("InputPanelPool");
        poolParent = poolObj.transform;
        poolParent.SetParent(boardCanvas.transform, false);

        //RectTransform poolRectTransform = poolParent.gameObject.AddComponent<RectTransform>();
        if (!poolObj.TryGetComponent<RectTransform>(out _))
        {
            poolObj.AddComponent<RectTransform>();
        }


        for (int i = 0; i < INITIAL_POOL_SIZE; i++)
        {
            CreateNewInputPanel();
        }
    }

    private void CreateNewInputPanel()
    {
        InputPanel panel = Instantiate(inputPanelPrefab, poolParent);
        panel.gameObject.SetActive(false);
        inputPanelPool.Enqueue(panel);
    }

    public InputPanel GetInputPanel()
    {
        if (activePanel != null)
        {
            ReturnToPool(activePanel);
        }

        if (inputPanelPool.Count == 0)
        {
            CreateNewInputPanel();
        }

        InputPanel panel = inputPanelPool.Dequeue();
        panel.gameObject.SetActive(true);
        panel.transform.SetParent(boardCanvas.transform, false);

        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.localScale = new Vector3(3f, 3f, 1f);
            rectTransform.anchoredPosition = new Vector2(2300, 700);
        }

        activePanel = panel;
        return panel;
    }

    public void ReturnToPool(InputPanel panel)
    {
        if (panel == null)
        {
            return;
        }

        panel.gameObject.SetActive(false);
        panel.transform.SetParent(poolParent, false);
        inputPanelPool.Enqueue(panel);

        if (panel == activePanel)
        {
            activePanel = null;
        }
    }
}
