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
                // CheckDropsInColumn(i, j, randomNum);

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

        DropColor.DropColorState twoPreviousDrop = board[row - 2, column];
        DropColor.DropColorState previosDrop = board[row - 1, column];
        DropColor.DropColorState currentDrop = (DropColor.DropColorState)num;

        while((previosDrop == twoPreviousDrop) && (currentDrop == previosDrop))
        {
            Debug.Log("I worked at " + row + " " + column);
            num = Random.Range(0, 4);
            currentDrop = (DropColor.DropColorState)num;
        }

        randomNum = num;
    }

    private void CheckDropsInColumn(int row, int column, int num)
    {
        if (column - 2 < 0 ) return;

        DropColor.DropColorState twoUpperDrop = board[row, column - 2];
        DropColor.DropColorState upperDrop = board[row, column - 1];
        DropColor.DropColorState currentDrop = (DropColor.DropColorState)num;

        while ((upperDrop == twoUpperDrop) && (currentDrop == upperDrop))
        {
            Debug.Log(twoUpperDrop + " " + upperDrop + " " +  currentDrop);
            num = Random.Range(0, 4);
            currentDrop = (DropColor.DropColorState)num;
            Debug.Log(currentDrop);
        }

        randomNum = num;
    }
}
