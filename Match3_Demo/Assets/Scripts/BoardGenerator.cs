using System.Collections;
using System.Linq;
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
        dropPrefabs = dropPrefabs.OrderBy(prefab => prefab.GetComponent<Drop>().DropColorInfo).ToArray();
        GenerateGameBoard();
    }

    private void GenerateGameBoard()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                randomNum = Random.Range(0, 4);

                CheckRandomStreaks(i, j, randomNum);
                // CheckDropsInColumn(i, j, randomNum);

                spawnPos = new Vector3(i * cellSize, 0f, j * cellSize );
                GameObject obj = Instantiate(dropPrefabs[randomNum], spawnPos, Quaternion.identity);
                obj.transform.parent = this.transform;
                board[i,j] = obj.GetComponent<Drop>().DropColorInfo;
            }
        }
    }

    private void CheckRandomStreaks(int row, int column, int num)
    {
        bool isSequentInRow;
        bool isSequentInColumn;

        DropColor.DropColorState twoPreviousDrop = 0;
        DropColor.DropColorState previousDrop = 0;

        DropColor.DropColorState twoUpperDrop = 0;
        DropColor.DropColorState upperDrop = 0;

        DropColor.DropColorState currentDrop = (DropColor.DropColorState)num;

        if (row - 2 < 0 )
        {
            isSequentInRow = false;
        }
        else
        {
            twoPreviousDrop = board[row - 2, column];
            previousDrop = board[row - 1, column];
            isSequentInRow = (currentDrop == twoPreviousDrop) && (currentDrop == previousDrop);
        }

        if (column - 2 < 0)
        {
            isSequentInColumn = false;
        }
        else
        {
            twoUpperDrop = board[row, column - 2];
            upperDrop = board[row, column - 1];
            isSequentInColumn = (currentDrop == twoUpperDrop) && (currentDrop == upperDrop);
        }

        while (isSequentInRow || isSequentInColumn)
        {
            num = Random.Range(0, 4);
            currentDrop = (DropColor.DropColorState)num;

            if (isSequentInColumn)
            {
                isSequentInColumn = (currentDrop == twoUpperDrop) && (currentDrop == upperDrop);
            } 
            else if(isSequentInRow)
            {
                isSequentInRow = (currentDrop == twoPreviousDrop) && (currentDrop == previousDrop);
            }
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
            num = Random.Range(0, 4);
            currentDrop = (DropColor.DropColorState)num;
        }

        randomNum = num;
    }
}
