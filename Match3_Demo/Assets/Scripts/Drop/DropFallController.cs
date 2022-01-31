using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropFallController : MonoBehaviour
{
    private Board board;

    private Drop drop;

    private int boardSize;

    private float cellSize, fallDownDuration = 0.1f;

    private Vector3Int positionInfo;

    private bool isBelowEmpty;

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

    public void CheckBelow()
    {
        positionInfo = drop.PositionInfo;

        if (positionInfo.x + 1 >= boardSize) return;

        isBelowEmpty = !board.boardArray[positionInfo.x + 1, positionInfo.z].gameObject.activeInHierarchy;

        if (!isBelowEmpty) return;

        StartCoroutine(MoveDownWithDelay());
    }

    private IEnumerator MoveDownWithDelay()
    {        
        int rowPosition = positionInfo.x;
        int columnPosition = positionInfo.z;

        transform.DOMove(transform.position + Vector3.back * cellSize, fallDownDuration);
        (board.boardArray[rowPosition, columnPosition], board.boardArray[rowPosition + 1, columnPosition]) = (board.boardArray[rowPosition + 1, columnPosition], board.boardArray[rowPosition, columnPosition]);
        board.boardArray[rowPosition, columnPosition].GetComponent<DropMovementController>()?.MoveUp();
        positionInfo = new Vector3Int(rowPosition + 1, 0, columnPosition);
        drop.PositionInfo = positionInfo;
        yield return new WaitForSeconds(fallDownDuration);
        CheckBelow();
    }
}
