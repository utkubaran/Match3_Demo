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
}
