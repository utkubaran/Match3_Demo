using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGapController : MonoBehaviour
{
    private GameObject[,] boardArr;

    private Board board;

    private bool isBoardMoved;

    private void OnEnable()
    {
        EventManager.OnDropMatch.AddListener(CheckGaps);
    }

    private void OnDisable()
    {
        EventManager.OnDropMatch.RemoveListener(CheckGaps);
    }

    private void Awake()
    {
        board = Board.instance;
    }
    
    private void Start()
    {
        boardArr = board.boardArray;
    }

    private void CheckGaps()
    {
        StartCoroutine(CheckGapsWithDelay());
    }

    private IEnumerator CheckGapsWithDelay()
    {
        /// <summary>
        /// Rather than assigning each drop to control their belows, this function activates their CheckBelow method
        /// row by row starting from row index = 6.
        /// </summary>
        /// <returns></returns>
        yield return new WaitForSeconds(0.1f);
        isBoardMoved = false;

        boardArr = board.boardArray;

        for (int row = board.BoardSize - 2; row >= 0 ; row--)
        {
            for (int column = 0; column < board.BoardSize; column++)
            {
                if (board.boardArray[row, column].activeInHierarchy)
                {
                    board.boardArray[row, column].GetComponent<DropFallController>()?.CheckBelow();
                }
                else
                {
                    isBoardMoved = true;
                }
            }

            yield return new WaitForSeconds(0.15f);
        }

        if (isBoardMoved)   EventManager.OnDropsFall?.Invoke();
    }
}
