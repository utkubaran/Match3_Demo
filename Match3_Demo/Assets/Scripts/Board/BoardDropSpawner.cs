using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDropSpawner : MonoBehaviour
{
    [SerializeField]
    private bool[] spawnerColumns;          // to choose which columns have respawn feature during gamepaly

    private Board board;

    private ObjectPooler objectPooler;

    private List<Vector3> matchedDrops;

    private float cellSize;

    private GameObject[,] boardArr;

    private int boardSize, randomNum;

    private void OnEnable()
    {
        EventManager.OnDropsFall.AddListener(SpawnDrops);
    }

    private void OnDisable()
    {
        EventManager.OnDropsFall.RemoveListener(SpawnDrops);
    }

    void Start()
    {
        board = Board.instance;
        objectPooler = ObjectPooler.instance;
        cellSize = board.CellSize;
        boardSize = board.BoardSize;
        boardArr = board.boardArray;
    }

    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("worksssss");
            SpawnDrops();
        }
    }

    private void SpawnDrops()
    {
        boardArr = board.boardArray;

        for (int i = 0; i < boardSize; i++)
        {
            boardArr = board.boardArray;
            bool isActiveInScene = boardArr[0, i].gameObject.activeInHierarchy;

            if (!isActiveInScene)
            {
                Debug.Log("works");

                int randomNum = Random.Range((int)0, (int)4);
                CheckSequants(GetLastDropInColumn(i), i, randomNum);
                Vector3 spawnPos = new Vector3(i * cellSize, 0f, 0f);
                GameObject obj = objectPooler.SpawnFromPool((DropColor.DropColorState)randomNum, spawnPos, Quaternion.identity);
                obj.GetComponent<Drop>().PositionInfo = new Vector3Int(0, 0, i);
                board.boardArray.SetValue(obj, 0, i);
            }
        }
    }

    private void CheckSequants(int row, int column, int num)
    {
        bool isSequentInRow;
        bool isSequentInColumn;

        DropColor.DropColorState lowerDrop = 0;
        DropColor.DropColorState upperDrop = 0;
        DropColor.DropColorState followingDrop = 0;
        DropColor.DropColorState previousDrop = 0;
        DropColor.DropColorState currentDrop = (DropColor.DropColorState)num;

        if (row - 1 < 0 || row + 1  > boardSize)        // todo refactor, bugs
        {
            isSequentInRow = false;
        }
        else
        {
            lowerDrop = board.boardArray[row + 1, column].GetComponent<Drop>().DropColorInfo;
            upperDrop = board.boardArray[row - 1, column].GetComponent<Drop>().DropColorInfo;
            isSequentInRow = (currentDrop == lowerDrop) && (currentDrop == upperDrop);
        }

        if (column - 1 < 0 || column + 1 > boardSize)       // todo refactor, bugs
        {
            isSequentInColumn = false;
        }
        else
        {
            followingDrop = board.boardArray[row, column + 1].GetComponent<Drop>().DropColorInfo;
            previousDrop = board.boardArray[row, column - 1].GetComponent<Drop>().DropColorInfo;
            isSequentInColumn = (currentDrop == followingDrop) && (currentDrop == previousDrop);
        }

        while (isSequentInRow || isSequentInColumn)
        {
            num = Random.Range(0, 4);
            currentDrop = (DropColor.DropColorState)num;
            isSequentInColumn = (currentDrop == followingDrop) && (currentDrop == previousDrop);
            isSequentInRow = (currentDrop == lowerDrop) && (currentDrop == upperDrop);
        }

        randomNum = num;
    }

    private int GetLastDropInColumn(int columnNo)
    {
        int rowIndex = 0;

        for (int i = 0; i < boardSize; i++)
        {
            bool isInScene = boardArr[i, columnNo].gameObject.activeInHierarchy;

            rowIndex = !isInScene ? rowIndex++ : rowIndex;
        }

        return rowIndex;
    }
}
