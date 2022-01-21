using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] dropPrefabs;

    [SerializeField]
    private int boardSize;

    [SerializeField]
    private float cellSize;

    private int randomNum;

    private Vector3 spawnPos;

    private DropColor.DropColorState[,] board;
    
    void Start()
    {
        board = new DropColor.DropColorState[boardSize, boardSize];
        GenerateGameBoard();
    }

    private void GenerateGameBoard()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                randomNum = Random.Range(0, 4);

                CheckDropsInRow(i, j, randomNum);

                spawnPos = new Vector3(i * cellSize, 0f, j * cellSize );
                GameObject obj = Instantiate(dropPrefabs[randomNum], spawnPos, Quaternion.identity);
                obj.transform.parent = this.transform;
                board[i,j] = obj.GetComponent<Drop>().DropColorInfo;
            }
        }
    }

    private void CheckDropsInRow(int row, int column, int num)
    {
        if (row - 2 < 0 ) return;

        DropColor.DropColorState doublePreviosDrop = board[row - 2, column];
        DropColor.DropColorState previosDrop = board[row - 1, column];
        DropColor.DropColorState currentDrop = (DropColor.DropColorState)num;

        while((previosDrop == doublePreviosDrop) && (currentDrop == previosDrop))
        {
            randomNum = Random.Range(0, 4);
            currentDrop = (DropColor.DropColorState)num;
        }
    }
}
