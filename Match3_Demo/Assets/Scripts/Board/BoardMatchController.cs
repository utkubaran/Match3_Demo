using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BoardMatchController : MonoBehaviour
{
    private Board board;

    private GameObject[,] boardArr;

    private float boardSize;

    private List<Transform> matchedDrops;

    private void OnEnable()
    {
        EventManager.OnPlayerSwiped.AddListener(CheckForMatchedDrops);
        EventManager.OnBoardCheck.AddListener(CheckForMatchedDrops);
    }

    private void OnDisable()
    {
        EventManager.OnPlayerSwiped.RemoveListener(CheckForMatchedDrops);
        EventManager.OnBoardCheck.RemoveListener(CheckForMatchedDrops);
    }

    void Start()
    {
        board = Board.instance;
        boardArr = board.boardArray;
        boardSize = board.BoardSize;
        matchedDrops = new List<Transform>();
    }

    private void CheckForMatchedDrops()
    {
        StartCoroutine(CheckForMatchedDropsWithDelay());
    }

    private IEnumerator CheckForMatchedDropsWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        CheckMatchesInRows();
        CheckMatchesInColumns();
        HandleMatches();
    }

    private void CheckMatchesInRows()
    {
        for (int row = 0; row < boardSize; row++)
        {
            for (int column = 2; column < boardSize; column++)
            {
                boardArr = board.boardArray;

                if (CheckActivesInRows(row, column))
                {
                    DropColor.DropColorState twoPreviousDrop = boardArr[row, column - 2].GetComponent<Drop>().DropColorInfo;;
                    DropColor.DropColorState previousDrop = boardArr[row, column - 1].GetComponent<Drop>().DropColorInfo;
                    DropColor.DropColorState currentDrop = boardArr[row, column].GetComponent<Drop>().DropColorInfo;

                    if ((currentDrop == previousDrop) && (twoPreviousDrop == currentDrop))
                    {
                        matchedDrops.Add(boardArr[row, column - 2].transform);
                        matchedDrops.Add(boardArr[row, column - 1].transform);
                        matchedDrops.Add(boardArr[row, column].transform);
                    }
                }
            }
        }
    }

    private void CheckMatchesInColumns()
    {
        for (int column = 0; column < boardSize; column++)
        {
            for (int row = 2; row < boardSize; row++)
            {
                boardArr = board.boardArray;

                if (CheckActivesInColumns(row, column))
                {
                    DropColor.DropColorState twoUpperDrop = boardArr[row - 2, column].GetComponent<Drop>().DropColorInfo;
                    DropColor.DropColorState upperDrop = boardArr[row - 1, column].GetComponent<Drop>().DropColorInfo;
                    DropColor.DropColorState currentDrop = boardArr[row, column].GetComponent<Drop>().DropColorInfo;

                    if ((currentDrop == upperDrop) && (currentDrop == twoUpperDrop))
                    {
                        matchedDrops.Add(boardArr[row - 2, column].transform);
                        matchedDrops.Add(boardArr[row - 1, column].transform);
                        matchedDrops.Add(boardArr[row, column].transform);
                    }
                }
            }
        }
    }

    private void HandleMatches()
    { 
        matchedDrops = matchedDrops.Distinct().ToList();

        if (matchedDrops.Count == 0)
        {
            EventManager.OnNoMatch?.Invoke();
        }
        else
        {
            StartCoroutine(DestroyMatchedDrops());
        }
    }

    private IEnumerator DestroyMatchedDrops()
    {
        /*
        foreach (var drop in matchedDrops)
        {
            // board.boardArray[drop.GetComponent<Drop>().PositionInfo.x, drop.GetComponent<Drop>().PositionInfo.z].gameObject.GetComponent<Drop>().OnMatch();
        }
        */
        Debug.Log("match");

        matchedDrops.Clear();
        yield return new WaitForSeconds(0.35f);
        EventManager.OnDropMatch?.Invoke();
    }

    private bool CheckActivesInRows(int row, int column)
    {
        bool isTwoPreviousInScene = boardArr[row, column - 2].activeInHierarchy;
        bool isPreviousInScene = boardArr[row, column - 1].activeInHierarchy;
        bool isInScene = boardArr[row, column].activeInHierarchy;

        if (!isInScene || !isPreviousInScene || !isTwoPreviousInScene)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CheckActivesInColumns(int row, int column)
    {
        bool isTwoPreviousInScene = boardArr[row - 2, column].activeInHierarchy;
        bool isPreviousInScene = boardArr[row - 1, column].activeInHierarchy;
        bool isInScene = boardArr[row, column].activeInHierarchy;

        if (!isInScene || !isPreviousInScene || !isTwoPreviousInScene)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
