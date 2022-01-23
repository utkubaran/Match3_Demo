using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFallController : MonoBehaviour
{
    private Board board;

    private int boardSize;

    private float cellSize;

    private Vector3Int positionInfo;

    private Vector3Int previousPositionInfo;

    private bool isBelowEmpty;

    private void OnEnable()
    {
        EventManager.OnDropMatch.AddListener(CheckBelow);
    }
    private void OnDisable()
    {
        EventManager.OnDropMatch.RemoveListener(CheckBelow);
    }

    void Start()
    {
        board = Board.instance;
        boardSize = board.BoardSize;
        cellSize = board.CellSize;
        positionInfo = GetComponent<Drop>().PositionInfo;
        isBelowEmpty = false;
    }

    private void Update()
    {
        CheckBelow();

        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckBelow();
        }
        */        
    }

    private void CheckBelow()
    {
        if (positionInfo.x + 1 >= boardSize) return;

        isBelowEmpty = !board.boardArray[positionInfo.x + 1, positionInfo.z].gameObject.activeInHierarchy;

        while (isBelowEmpty && positionInfo.x + 1 < boardSize)
        {
            MoveDown();
        }

    }

    public void MoveDown()
    {
        int rowPosition = positionInfo.x;
        int columnPosition = positionInfo.z;

        transform.position += Vector3.back * cellSize;
        (board.boardArray[rowPosition, columnPosition], board.boardArray[rowPosition + 1, columnPosition]) = (board.boardArray[rowPosition + 1, columnPosition], board.boardArray[rowPosition, columnPosition]);
        isBelowEmpty = !board.boardArray[positionInfo.x + 1, positionInfo.z].gameObject.activeInHierarchy;
        positionInfo = new Vector3Int(rowPosition + 1, 0, columnPosition);
    }
}
