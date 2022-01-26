using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [HideInInspector]
    public GameObject[,] boardArray;

    [SerializeField]
    private int boardSize;

    [SerializeField]
    private float cellSize;

    public int BoardSize { get { return boardSize; } }

    public float CellSize { get { return cellSize; } }

    #region Singleton
    public static Board instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    private void Start()
    {
        boardArray = new GameObject[boardSize, boardSize];
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            foreach (var obj in boardArray)
            {
                if (obj.activeSelf)
                {
                    Debug.Log("active");
                }
                else
                {
                    Debug.Log("deactive");
                }

                if (obj.activeInHierarchy)
                {
                    Debug.Log("Second method active");
                }
                else
                {
                    Debug.Log("Second method deactive");
                }
            }
        }
    }
}
