using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropFallController : MonoBehaviour
{
    private Board board;

    private Drop drop;

    private int boardSize;

    private float cellSize;

    private Vector3Int positionInfo, previousPositionInfo;

    private bool isBelowEmpty;

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

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            CheckBelow();
        }
        */
    }

    public void CheckBelow()
    {
        positionInfo = drop.PositionInfo;

        if (positionInfo.x + 1 >= boardSize) return;

        isBelowEmpty = !board.boardArray[positionInfo.x + 1, positionInfo.z].gameObject.activeInHierarchy;

        while (isBelowEmpty && positionInfo.x < boardSize - 1)
        {
            MoveDown();
        }

        // EventManager.OnDropsFall?.Invoke();
    }

    private void MoveDown()
    {
        positionInfo = drop.PositionInfo;
        int rowPosition = positionInfo.x;
        int columnPosition = positionInfo.z;

        transform.position += Vector3.back * cellSize;
        // transform.DOMove(transform.position + Vector3.back, 0.05f);
        (board.boardArray[rowPosition, columnPosition], board.boardArray[rowPosition + 1, columnPosition]) = (board.boardArray[rowPosition + 1, columnPosition], board.boardArray[rowPosition, columnPosition]);
        positionInfo = new Vector3Int(rowPosition + 1, 0, columnPosition);
        board.boardArray[rowPosition, columnPosition].GetComponent<DropMovementController>().MoveUp();
        drop.PositionInfo = positionInfo;

        // todo refactor
        if (rowPosition + 2 >= boardSize)
        {
            isBelowEmpty = false;
        }
        else
        {
            isBelowEmpty = !board.boardArray[rowPosition + 2, columnPosition].gameObject.activeInHierarchy;
        }
    }
}
