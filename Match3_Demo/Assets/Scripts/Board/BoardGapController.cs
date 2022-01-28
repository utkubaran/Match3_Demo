using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGapController : MonoBehaviour
{
    private GameObject[,] boardArr;

    private Board board;

    private void OnEnable()
    {
        EventManager.OnDropMatch.AddListener(CheckGaps);
        EventManager.OnDropSpawned.AddListener(CheckGaps);
    }

    private void OnDisable()
    {
        EventManager.OnDropMatch.RemoveListener(CheckGaps);
        EventManager.OnDropSpawned.RemoveListener(CheckGaps);
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
        // one cycle
        yield return new WaitForSeconds(0.05f);

        boardArr = board.boardArray;

        for (int row = board.BoardSize - 2; row >= 0 ; row--)
        {
            for (int column = 0; column < board.BoardSize; column++)
            {
                if (board.boardArray[row, column].activeInHierarchy)
                {
                    board.boardArray[row, column].GetComponent<DropFallController>().CheckBelow();
                }
            }

            yield return new WaitForSeconds(0.01f);
        }

        EventManager.OnDropsFall?.Invoke();
    }
}
