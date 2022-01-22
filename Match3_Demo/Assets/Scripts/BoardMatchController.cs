using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardMatchController : MonoBehaviour
{
    private Board board;

    private GameObject[,] boardArr;

    private float boardSize;

    private void OnEnable()
    {
        EventManager.OnPlayerSwiped.AddListener( () => boardArr = board.boardArray );
        EventManager.OnPlayerSwiped.AddListener(CheckMatchInRows);
        EventManager.OnPlayerSwiped.AddListener(CheckMatchInColumns);
    }

    private void OnDisable()
    {
        EventManager.OnPlayerSwiped.RemoveListener( () => boardArr = board.boardArray );
        EventManager.OnPlayerSwiped.RemoveListener(CheckMatchInRows);
        EventManager.OnPlayerSwiped.RemoveListener(CheckMatchInColumns);
    }

    void Start()
    {
        board = Board.instance;
        boardArr = board.boardArray;
        boardSize = board.BoardSize;
        CheckMatchInRows();
        CheckMatchInColumns();
    }

    private void CheckMatchInRows()
    {
        for (int row = 0; row < boardSize; row++)
        {
            for (int column = 2; column < boardSize; column++)
            {
                DropColor.DropColorState twoPreviousDrop = boardArr[row, column - 2].GetComponent<Drop>().DropColorInfo;;
                DropColor.DropColorState previousDrop = boardArr[row, column - 1].GetComponent<Drop>().DropColorInfo;
                DropColor.DropColorState currentDrop = boardArr[row, column].GetComponent<Drop>().DropColorInfo;

                if ((twoPreviousDrop == currentDrop) && (previousDrop == currentDrop))
                {
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
                                    Debug.Log("It's a 5 row match!");
                                }
                                else
                                {
                                    Debug.Log("It's a 4 row match!");
                                }
                            }
                            else
                            {
                                Debug.Log("It's a 4 row match!");
                            }
                        }
                        else
                        {
                            Debug.Log("It's a 3 row match!");
                        }
                    }
                }
            }
        }
    }

    private void CheckMatchInColumns()
    {
        for (int column = 0; column < boardSize; column++)
        {
            for (int row = 2; row < boardSize; row++)
            {
                DropColor.DropColorState twoUpperDrop = boardArr[row - 2, column].GetComponent<Drop>().DropColorInfo;;
                DropColor.DropColorState upperDrop = boardArr[row - 1, column].GetComponent<Drop>().DropColorInfo;
                DropColor.DropColorState currentDrop = boardArr[row, column].GetComponent<Drop>().DropColorInfo;

                if ((twoUpperDrop == currentDrop) && (upperDrop == currentDrop))
                {
                    if (row + 1 < boardSize)
                    {
                        DropColor.DropColorState lowerDrop = boardArr[row + 1, column].GetComponent<Drop>().DropColorInfo;

                        if (lowerDrop == currentDrop)
                        {
                            if (row + 2 < boardSize)
                            {
                                DropColor.DropColorState twoFollowingDrop = boardArr[row + 2, column].GetComponent<Drop>().DropColorInfo;

                                if (twoFollowingDrop == currentDrop)
                                {
                                    Debug.Log("It's a 5 column match!");
                                }
                                else
                                {
                                    Debug.Log("It's a 4 column match!");
                                }
                            }
                            else
                            {
                                Debug.Log("It's a 4 column match!");
                            }
                        }
                        else
                        {
                            Debug.Log("It's a 3 column match!");
                        }
                    }
                    else
                    {
                        Debug.Log("It's a 3 column match!");
                    }
                }
            }
        }
    }
}
