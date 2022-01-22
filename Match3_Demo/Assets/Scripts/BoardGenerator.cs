using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    private Board board;
    
    private ObjectPooler objectPooler;

    private int randomNum, boardSize;

    private float cellSize;

    private Vector3 spawnPos;
    
    void Start()
    {
        board = Board.instance;
        objectPooler = ObjectPooler.instance;
        boardSize = board.BoardSize;
        cellSize = board.CellSize;
        GenerateGameBoard();
    }

    private void GenerateGameBoard()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                randomNum = Random.Range(0, 4);
                CheckSequants(j, i, randomNum);
                spawnPos = new Vector3(i * cellSize, 0f, j * -cellSize);
                
                GameObject obj = objectPooler.SpawnFromPool((DropColor.DropColorState)randomNum, spawnPos, Quaternion.identity);
                obj.GetComponent<Drop>().PositionInfo = new Vector3Int(j, 0, i);
                obj.GetComponent<IPooledObject>().OnObjectSpawn();
                board.boardArray[j, i] = obj;
            }
        }
    }

    private void CheckSequants(int row, int column, int num)
    {
        bool isSequentInRow;
        bool isSequentInColumn;

        DropColor.DropColorState twoUpperDrop = 0;
        DropColor.DropColorState upperDrop = 0;
        DropColor.DropColorState twoPreviousDrop = 0;
        DropColor.DropColorState previousDrop = 0;
        DropColor.DropColorState currentDrop = (DropColor.DropColorState)num;

        if (row - 2 < 0 )
        {
            isSequentInRow = false;
        }
        else
        {
            twoUpperDrop = board.boardArray[row - 2, column].GetComponent<Drop>().DropColorInfo;
            upperDrop = board.boardArray[row - 1, column].GetComponent<Drop>().DropColorInfo;
            isSequentInRow = (currentDrop == twoUpperDrop) && (currentDrop == upperDrop);
        }

        if (column - 2 < 0)
        {
            isSequentInColumn = false;
        }
        else
        {
            twoPreviousDrop = board.boardArray[row, column - 2].GetComponent<Drop>().DropColorInfo;
            previousDrop = board.boardArray[row, column - 1].GetComponent<Drop>().DropColorInfo;
            isSequentInColumn = (currentDrop == twoPreviousDrop) && (currentDrop == previousDrop);
        }

        while (isSequentInRow || isSequentInColumn)
        {
            num = Random.Range(0, 4);
            currentDrop = (DropColor.DropColorState)num;
            isSequentInColumn = (currentDrop == twoPreviousDrop) && (currentDrop == previousDrop);
            isSequentInRow = (currentDrop == twoUpperDrop) && (currentDrop == upperDrop);
        }

        randomNum = num;
    }
}
