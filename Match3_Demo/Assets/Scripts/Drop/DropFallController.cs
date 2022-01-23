using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFallController : MonoBehaviour
{
    private Board board;

    private int boardSize;

    private float cellSize;

    private Vector3Int positionInfo;

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
    }

    private void CheckBelow()
    {
        if (positionInfo.x + 1 >= boardSize) return;

        bool isBelowEmpty = !board.boardArray[positionInfo.x + 1, positionInfo.z].gameObject.activeInHierarchy;

        Debug.Log(isBelowEmpty);

        if (!isBelowEmpty) return;

        MoveDown();
    }

    public void MoveDown()
    {
        transform.position += Vector3.back * cellSize;
    }
}
