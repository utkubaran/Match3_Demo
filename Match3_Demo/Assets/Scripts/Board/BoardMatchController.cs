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

    private bool hasMatch = false;

    private void OnEnable()
    {
        // EventManager.OnPlayerSwiped.AddListener( () => boardArr = board.boardArray );
        // EventManager.OnPlayerSwiped.AddListener(CheckMatchesInRows);
        // EventManager.OnPlayerSwiped.AddListener(CheckMatchesInColumns);
        // EventManager.OnPlayerSwiped.AddListener(HandleMatches);
        EventManager.OnPlayerSwiped.AddListener(CheckForMatchedDrops);
        // EventManager.OnDropsFall.AddListener(CheckForMatchedDrops);
        // EventManager.OnDropMatch.AddListener(CheckMatchesInRows);
        // EventManager.OnDropMatch.AddListener(CheckMatchesInColumns);
    }

    private void OnDisable()
    {
        // EventManager.OnPlayerSwiped.RemoveListener( () => boardArr = board.boardArray );
        // EventManager.OnPlayerSwiped.RemoveListener(CheckMatchesInRows);
        // EventManager.OnPlayerSwiped.RemoveListener(CheckMatchesInColumns);
        // EventManager.OnPlayerSwiped.RemoveListener(HandleMatches);
        EventManager.OnPlayerSwiped.RemoveListener(CheckForMatchedDrops);
        // EventManager.OnDropsFall.RemoveListener(CheckForMatchedDrops);
        // EventManager.OnDropMatch.RemoveListener(CheckMatchesInRows);
        // EventManager.OnDropMatch.RemoveListener(CheckMatchesInColumns);
    }

    void Start()
    {
        board = Board.instance;
        boardArr = board.boardArray;
        boardSize = board.BoardSize;
        matchedDrops = new List<Transform>();
        // InvokeRepeating("CheckMatchesInRows", 1f, 1f);
        // InvokeRepeating("CheckMatchesInColumns", 1f, 1f);
        CheckMatchesInRows();
        CheckMatchesInColumns();
    }

    private void CheckForMatchedDrops()
    {
        StartCoroutine(CheckForMatchedDropsWithDelay());
    }

    private IEnumerator CheckForMatchedDropsWithDelay()
    {
        yield return new WaitForSeconds(0.25f);
        CheckMatchesInRows();
        CheckMatchesInColumns();
        yield return new WaitForSeconds(0.25f);
        HandleMatches();
    }

    private void CheckMatchesInRows()
    {
        for (int row = 0; row < boardSize; row++)
        {
            for (int column = 2; column < boardSize; column++)
            {
                boardArr = board.boardArray;

                // bool isTwoPreviousInScene = boardArr[row, column - 2].gameObject.activeInHierarchy;
                // bool isPreviousInScene = boardArr[row, column - 1].gameObject.activeInHierarchy;
                // bool isInScene = boardArr[row, column].gameObject.activeInHierarchy;

                // if (!isInScene || !isPreviousInScene || !isTwoPreviousInScene) return;

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

                        if (column + 1 < boardSize && boardArr[row, column + 1].gameObject.activeInHierarchy)
                        {
                            DropColor.DropColorState followingDrop = boardArr[row, column + 1].GetComponent<Drop>().DropColorInfo;

                            if (followingDrop == currentDrop)
                            {
                                if (column + 2 < boardSize && boardArr[row, column + 2].gameObject.activeInHierarchy)
                                {
                                    DropColor.DropColorState twoFollowingDrop = boardArr[row, column + 2].GetComponent<Drop>().DropColorInfo;

                                    if (twoFollowingDrop == currentDrop)
                                    {
                                        matchedDrops.Add(boardArr[row, column + 2].transform);
                                    }
                                    else
                                    {
                                        matchedDrops.Add(boardArr[row, column + 1].transform);
                                    }
                                }
                                else
                                {
                                    matchedDrops.Add(boardArr[row, column + 1].transform);

                                }
                            }
                        }
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

                // bool isTwoPreviousInScene = boardArr[row - 2, column].gameObject.activeInHierarchy;
                // bool isPreviousInScene = boardArr[row - 1, column].gameObject.activeInHierarchy;
                // bool isInScene = boardArr[row, column].gameObject.activeInHierarchy;

                // if (!isInScene || !isPreviousInScene || !isTwoPreviousInScene) return;

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

                        if (row + 1 < boardSize && boardArr[row + 1, column].gameObject.activeInHierarchy)
                        {
                            DropColor.DropColorState lowerDrop = boardArr[row + 1, column].GetComponent<Drop>().DropColorInfo;

                            if (lowerDrop == currentDrop)
                            {
                                if (row + 2 < boardSize && boardArr[row + 2, column].gameObject.activeInHierarchy)
                                {
                                    DropColor.DropColorState twoLowerDrop = boardArr[row + 2, column].GetComponent<Drop>().DropColorInfo;

                                    if (twoLowerDrop == currentDrop)
                                    {
                                        matchedDrops.Add(boardArr[row + 2, column].transform);

                                    }
                                    else
                                    {
                                        matchedDrops.Add(boardArr[row + 1, column].transform);

                                    }
                                }
                                else
                                {
                                    matchedDrops.Add(boardArr[row + 1, column].transform);
                                }
                            }
                        }
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
            Debug.Log("matchhhh!");
            StartCoroutine(DestroyMatchedDrops());
            EventManager.OnDropMatch?.Invoke();
        }
    }

    private IEnumerator DestroyMatchedDrops()
    {
        yield return new WaitForSeconds(0.5f);
        
        foreach (var drop in matchedDrops)
        {
            board.boardArray[drop.GetComponent<Drop>().PositionInfo.x, drop.GetComponent<Drop>().PositionInfo.z].gameObject.SetActive(false);
        }

        matchedDrops.Clear();
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
