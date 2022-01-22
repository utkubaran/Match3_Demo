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
        EventManager.OnPlayerSwiped.AddListener( () => boardArr = board.boardArray );
        EventManager.OnPlayerSwiped.AddListener(CheckMatchesInRows);
        EventManager.OnPlayerSwiped.AddListener(CheckMatchesInColumns);
        EventManager.OnPlayerSwiped.AddListener(HandleMatches);
    }

    private void OnDisable()
    {
        EventManager.OnPlayerSwiped.RemoveListener( () => boardArr = board.boardArray );
        EventManager.OnPlayerSwiped.RemoveListener(CheckMatchesInRows);
        EventManager.OnPlayerSwiped.RemoveListener(CheckMatchesInColumns);
        EventManager.OnPlayerSwiped.RemoveListener(HandleMatches);
    }

    void Start()
    {
        board = Board.instance;
        boardArr = board.boardArray;
        boardSize = board.BoardSize;
        matchedDrops = new List<Transform>();
        CheckMatchesInRows();
        CheckMatchesInColumns();
    }

    private void CheckMatchesInRows()
    {
        for (int row = 0; row < boardSize; row++)
        {
            for (int column = 2; column < boardSize; column++)
            {
                DropColor.DropColorState twoPreviousDrop = boardArr[row, column - 2].GetComponent<Drop>().DropColorInfo;;
                DropColor.DropColorState previousDrop = boardArr[row, column - 1].GetComponent<Drop>().DropColorInfo;
                DropColor.DropColorState currentDrop = boardArr[row, column].GetComponent<Drop>().DropColorInfo;

                if ((currentDrop == previousDrop) && (twoPreviousDrop == currentDrop))
                {
                    hasMatch = true;
                    matchedDrops.Add(boardArr[row, column - 2].transform);
                    matchedDrops.Add(boardArr[row, column - 1].transform);
                    matchedDrops.Add(boardArr[row, column].transform);

                    // boardArr[row, column - 2].gameObject.SetActive(false);
                    // boardArr[row, column - 1].gameObject.SetActive(false);
                    // boardArr[row, column].gameObject.SetActive(false);

                    if (column + 1 < boardSize)
                    {
                        DropColor.DropColorState followingDrop = boardArr[row, column + 1].GetComponent<Drop>().DropColorInfo;

                        if (followingDrop == currentDrop)
                        {
                            if (column + 2 < boardSize)
                            {
                                DropColor.DropColorState twoFollowingDrop = boardArr[row, column + 2].GetComponent<Drop>().DropColorInfo;

                                if (twoFollowingDrop == currentDrop)
                                {
                                    matchedDrops.Add(boardArr[row, column + 2].transform);
                                    // boardArr[row, column + 2].gameObject.SetActive(false);
                                    // Debug.Log("It's a 5 match in the same row!");
                                }
                                else
                                {
                                    matchedDrops.Add(boardArr[row, column + 1].transform);
                                    // boardArr[row, column + 1].gameObject.SetActive(false);
                                    // Debug.Log("It's a 4 match in the same row!");
                                }
                            }
                            else
                            {
                                matchedDrops.Add(boardArr[row, column + 1].transform);
                                // boardArr[row, column + 1 ].gameObject.SetActive(false);
                                // Debug.Log("It's a 4 match in the same row!");
                            }
                        }
                        else
                        {
                            Debug.Log("It's a 3 match in the same row!");
                        }
                    }
                    else
                    {
                        Debug.Log("It's a 3 match in the same row!");
                    }
                }

                /*
                if (hasMatch)
                {
                    column = 2;
                }
                */
            }
        }
    }

    private void CheckMatchesInColumns()
    {
        for (int column = 0; column < boardSize; column++)
        {
            for (int row = 2; row < boardSize; row++)
            {
                DropColor.DropColorState twoUpperDrop = boardArr[row - 2, column].GetComponent<Drop>().DropColorInfo;
                DropColor.DropColorState upperDrop = boardArr[row - 1, column].GetComponent<Drop>().DropColorInfo;
                DropColor.DropColorState currentDrop = boardArr[row, column].GetComponent<Drop>().DropColorInfo;

                if ((currentDrop == upperDrop) && (currentDrop == twoUpperDrop))
                {
                    hasMatch = true;
                    matchedDrops.Add(boardArr[row - 2, column].transform);
                    matchedDrops.Add(boardArr[row - 1, column].transform);
                    matchedDrops.Add(boardArr[row, column].transform);

                    // boardArr[row - 2, column].gameObject.SetActive(false);
                    // boardArr[row - 1, column].gameObject.SetActive(false);
                    // boardArr[row, column].gameObject.SetActive(false);

                    if (row + 1 < boardSize)
                    {
                        DropColor.DropColorState lowerDrop = boardArr[row + 1, column].GetComponent<Drop>().DropColorInfo;

                        if (lowerDrop == currentDrop)
                        {
                            if (row + 2 < boardSize)
                            {
                                DropColor.DropColorState twoLowerDrop = boardArr[row + 2, column].GetComponent<Drop>().DropColorInfo;

                                if (twoLowerDrop == currentDrop)
                                {
                                    matchedDrops.Add(boardArr[row + 2, column].transform);
                                    // boardArr[row + 2, column].gameObject.SetActive(false);
                                    // Debug.Log("It's a 5 match in the same column!");

                                }
                                else
                                {
                                    matchedDrops.Add(boardArr[row + 1, column].transform);
                                    // boardArr[row + 1, column].gameObject.SetActive(false);
                                    // Debug.Log("It's a 4 match in the same column!");
                                }
                            }
                            else
                            {
                                matchedDrops.Add(boardArr[row + 1, column].transform);
                                // boardArr[row + 1, column].gameObject.SetActive(false);
                                // Debug.Log("It's a 4 match in the same column!");
                            }
                        }
                        else
                        {
                            Debug.Log("It's a 3 match in the same column!");
                        }
                    }
                    else
                    {
                        Debug.Log("It's a 3 match in the same column!");
                    }
                }
            }
        }
    }

    private void HandleMatches()
    {
        
        matchedDrops = matchedDrops.Distinct().ToList();

        if (hasMatch)
        {
            foreach (var drop in matchedDrops)
            {
                drop.gameObject.SetActive(false);
            }

            EventManager.OnMatch?.Invoke(matchedDrops);
            hasMatch = false;
        }
        else
        {
            EventManager.OnNoMatch?.Invoke();
        }
    }
}
