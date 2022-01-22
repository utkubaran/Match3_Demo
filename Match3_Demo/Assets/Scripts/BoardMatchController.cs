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
        EventManager.OnPlayerSwiped.AddListener(CheckForMatch);
    }

    private void OnDisable()
    {
        EventManager.OnPlayerSwiped.RemoveListener(CheckForMatch);
    }

    void Start()
    {
        board = Board.instance;
        boardArr = board.boardArray;
        boardSize = board.BoardSize;
    }

    private void CheckForMatch()
    {
        for (int row = 0; row < boardSize; row++)
        {
            for (int column = 0; column < boardSize; column++)
            {
                DropColor.DropColorState twoPreviousDrop = boardArr[row, column - 2].GetComponent<Drop>().DropColorInfo;;
                DropColor.DropColorState previousDrop = boardArr[row, column - 1].GetComponent<Drop>().DropColorInfo;
                DropColor.DropColorState currentDrop = boardArr[row, column].GetComponent<Drop>().DropColorInfo;
                DropColor.DropColorState followingDrop = boardArr[row, column + 1].GetComponent<Drop>().DropColorInfo;
                DropColor.DropColorState twoFollowingDrop = boardArr[row, column + 2].GetComponent<Drop>().DropColorInfo;

                if ((twoPreviousDrop == currentDrop) && (previousDrop == currentDrop) )
                {
                    if ( row == 0 )
                    Debug.Log("It's a match!");
                }
                else if ( (twoFollowingDrop == currentDrop) && (followingDrop == currentDrop) )
                {
                    Debug.Log("It's a match!");
                }
            }
        }
    }
}
