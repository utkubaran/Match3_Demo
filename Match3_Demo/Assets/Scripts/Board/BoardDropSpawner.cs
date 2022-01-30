using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDropSpawner : MonoBehaviour
{
    [SerializeField, Tooltip("Please select spawner columns.")]
    private bool[] spawnerColumns;          // To choose which columns have respawn feature during gamepaly

    private Board board;

    private ObjectPooler objectPooler;

    private float cellSize;

    private GameObject[,] boardArr;

    private int boardSize, randomNum, previousNum;

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

    private void SpawnDrops()
    {
        StartCoroutine(SpawnDropsWithDelay());
    }

    private IEnumerator SpawnDropsWithDelay()
    {
        yield return new WaitForSeconds(0.15f);
        bool isSpawned = false;
        previousNum = 10;

        for (int i = 0; i < boardSize; i++)
        {
            boardArr = board.boardArray;
            bool isActiveInScene = boardArr[0, i].gameObject.activeInHierarchy;

            if (!isActiveInScene && spawnerColumns[i])
            {
                isSpawned = true;
                int randomNum = Random.Range((int)0, (int)4);

                while (randomNum == previousNum)
                {
                    randomNum = Random.Range((int)0, (int)4);
                }

                Vector3 spawnPos = new Vector3(i * cellSize, 0f, 0f);
                GameObject obj = objectPooler.SpawnFromPool((DropColor.DropColorState)randomNum, spawnPos, Quaternion.Euler(90f, 0f, 0f));
                obj.GetComponent<Drop>().PositionInfo = new Vector3Int(0, 0, i);
                board.boardArray.SetValue(obj, 0, i);
                yield return new WaitForSeconds(0.15f);
                // EventManager.OnDropSpawned?.Invoke();
                obj.GetComponent<DropFallController>()?.CheckBelow();
                // spawnedDrops.Add(obj);
                previousNum = randomNum;
            }
        }

        if (isSpawned)
        {
            SpawnDrops();
            // yield return new WaitForSeconds(0.125f);
            // EventManager.OnDropsFall?.Invoke();
        }
        else
        {
            yield return new WaitForSeconds(0.25f);
            EventManager.OnBoardCheck?.Invoke();
        }
    }

    private void CheckSequents(int row, int column, int num, int prevNum)
    {
        bool isSequentInRow;
        bool isSequentInColumn;

        DropColor.DropColorState lowerDrop = 0;
        DropColor.DropColorState followingDrop = 0;
        DropColor.DropColorState previousDrop = 0;
        DropColor.DropColorState currentDrop = (DropColor.DropColorState)num;

        if (row - 1 < 0 || row + 1  >= boardSize)        // todo refactor, bugs
        {
            isSequentInRow = false;
        }
        else
        {
            lowerDrop = board.boardArray[row + 1, column].GetComponent<Drop>().DropColorInfo;
            isSequentInRow = (currentDrop == lowerDrop);
        }

        if (column - 1 < 0 || column + 1 >= boardSize)       // todo refactor, bugs
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
            isSequentInRow = (currentDrop == lowerDrop) || num == prevNum;
        }

        randomNum = num;
    }
}
