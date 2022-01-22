using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] dropPrefabs;

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
                CheckSequants(i, j, randomNum);
                spawnPos = new Vector3(i * cellSize, 0f, j * -cellSize);
                
                GameObject obj = objectPooler.SpawnFromPool((DropColor.DropColorState)randomNum, spawnPos, Quaternion.identity);
                obj.GetComponent<Drop>().PositionInfo = new Vector3Int(i, 0, j);
                obj.GetComponent<IPooledObject>().OnObjectSpawn();
                board.boardArray[i,j] = obj;
            }
        }
    }

    private void CheckSequants(int row, int column, int num)
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
            twoPreviousDrop = board.boardArray[row - 2, column].GetComponent<Drop>().DropColorInfo;
            previousDrop = board.boardArray[row - 1, column].GetComponent<Drop>().DropColorInfo;
            isSequentInRow = (currentDrop == twoPreviousDrop) && (currentDrop == previousDrop);
        }

        if (column - 2 < 0)
        {
            isSequentInColumn = false;
        }
        else
        {
            twoUpperDrop = board.boardArray[row, column - 2].GetComponent<Drop>().DropColorInfo;
            upperDrop = board.boardArray[row, column - 1].GetComponent<Drop>().DropColorInfo;
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
}
