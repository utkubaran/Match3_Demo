using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    private Board board;

    [SerializeField]
    private ObjectPooler objectPooler;

    private int boardSize;

    private float cellSize;

    void Start()
    {
        board = Board.instance;
        objectPooler = ObjectPooler.instance;
        boardSize = board.BoardSize;
        cellSize = board.CellSize;
        GenerateTileBase();
    }

    private void GenerateTileBase()
    {
        for (int row = 0; row < boardSize; row++)
        {
            for (int column = 0; column < boardSize; column++)
            {
                Vector3 spawnPos = new Vector3(row * cellSize, -0.6f, column * -cellSize);
                GameObject tileObj = objectPooler.SpawnFromPool(DropColor.DropColorState.Tile, spawnPos, Quaternion.Euler(90f, 0f, 0f));
                tileObj.transform.parent = board.transform;
            }
        }
    }
}
