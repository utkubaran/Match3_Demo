using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFallController : MonoBehaviour
{
    private Board board;

    private Drop drop;

    private int boardSize;

    private float cellSize;

    private Vector3Int positionInfo, previousPositionInfo;

    private bool isBelowEmpty;

    private float controlTimer = 0.05f;

    private void OnEnable()
    {
        // EventManager.OnDropMatch.AddListener(CheckBelow);
    }
    private void OnDisable()
    {
        // EventManager.OnDropMatch.RemoveListener(CheckBelow);
    }

    private void Awake()
    {
        drop = GetComponent<Drop>();
    }

    void Start()
    {
        board = Board.instance;
        boardSize = board.BoardSize;
        cellSize = board.CellSize;
        positionInfo = drop.PositionInfo;
        isBelowEmpty = false;
    }

    private void FixedUpdate()
    {
        CheckBelow();
    }


    public bool CheckBelowEmpty()
    {
        return !board.boardArray[positionInfo.x + 1, positionInfo.z].gameObject.activeInHierarchy;
    }

/*
    private IEnumerator CheckBelowWithDelay()
    {
        float waitTime = positionInfo.x * controlTimer;

        yield return new WaitForSeconds(waitTime);

        positionInfo = drop.PositionInfo;

        if (positionInfo.x + 1 < boardSize)
        {
            isBelowEmpty = !board.boardArray[positionInfo.x + 1, positionInfo.z].gameObject.activeInHierarchy;

            while (isBelowEmpty && positionInfo.x + 1 < boardSize)
            {
                MoveDown();
            }
        }
    }
    */

    private void CheckBelow()
    {
        positionInfo = drop.PositionInfo;

        if (positionInfo.x + 1 >= boardSize) return;

        isBelowEmpty = !board.boardArray[positionInfo.x + 1, positionInfo.z].gameObject.activeInHierarchy;

        while (isBelowEmpty && positionInfo.x < boardSize - 1)
        {
            MoveDown();
        }
    }

    public void MoveDown()
    {
        positionInfo = drop.PositionInfo;
        int rowPosition = positionInfo.x;
        int columnPosition = positionInfo.z;

        transform.position += Vector3.back * cellSize;
        (board.boardArray[rowPosition, columnPosition], board.boardArray[rowPosition + 1, columnPosition]) = (board.boardArray[rowPosition + 1, columnPosition], board.boardArray[rowPosition, columnPosition]);
        positionInfo = new Vector3Int(rowPosition + 1, 0, columnPosition);
        board.boardArray[rowPosition, columnPosition].GetComponent<DropMovementController>().MoveUp();
        // rowPosition++;      // todo refactor

        drop.PositionInfo = positionInfo;
        isBelowEmpty = !board.boardArray[rowPosition + 2, columnPosition].gameObject.activeInHierarchy;
    }
    
    private IEnumerator CheckBelowWithDelay()
    {
        yield return new WaitForSeconds((boardSize - positionInfo.x) * 0.1f);
        positionInfo = drop.PositionInfo;

        if (positionInfo.x + 1 < boardSize)
        {
            isBelowEmpty = !board.boardArray[positionInfo.x + 1, positionInfo.z].gameObject.activeInHierarchy;

            while (isBelowEmpty && positionInfo.x < boardSize - 1)
            {
                MoveDown();
            }
        }
    }
}
