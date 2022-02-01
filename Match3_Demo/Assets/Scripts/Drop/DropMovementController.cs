using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropMovementController : MonoBehaviour, IMoveable
{
    private Board board;

    private Drop drop;

    private int boardSize;

    private float cellSize, moveBackDuration = 0.1f, swipeMovementDuration = 0.2f;

    private Vector3Int positionInfo, previousPositionInfo;

    private bool isMoved;

    private void Awake()
    {
        drop = GetComponent<Drop>();
    }

    private void OnEnable()
    {
        EventManager.OnDropMatch.AddListener( () => previousPositionInfo = positionInfo);
        EventManager.OnNoMatch.AddListener(GoBackPreviousPosition);
    }

    private void OnDisable()
    {
        EventManager.OnDropMatch.RemoveListener( () => previousPositionInfo = positionInfo);
        EventManager.OnNoMatch.RemoveListener(GoBackPreviousPosition);
    }

    private void Start()
    {
        board = Board.instance;
        cellSize = board.CellSize;
        isMoved = false;
        positionInfo = GetComponent<Drop>().PositionInfo;
        previousPositionInfo = positionInfo;
    }

    public void OnSwiped(Vector3 movementDir)
    {
        positionInfo = drop.PositionInfo;
        int rowPosition = positionInfo.x;
        int columnPosition = positionInfo.z;

        if (movementDir == Vector3.forward)
        {
            MoveUpward(rowPosition, columnPosition);
        }
        else if (movementDir == Vector3.back)
        {
            MoveDownward(rowPosition, columnPosition);
        }
        else if (movementDir == Vector3.right)
        {
            MoveRight(rowPosition, columnPosition);
        }
        else if (movementDir == Vector3.left)
        {
            MoveLeft(rowPosition, columnPosition);
        }
        
        drop.PositionInfo = positionInfo;
    }

    private void MoveUpward(int rowPosition, int columnPosition)
    {
        if (rowPosition - 1 < 0 || !board.boardArray[rowPosition - 1, columnPosition].gameObject.activeInHierarchy) return;

        isMoved = true;
        board.boardArray[rowPosition - 1, columnPosition].GetComponent<DropMovementController>()?.ChangePlace(Vector3.back, 1, 0);
        Tween myTween = transform.DOMove(transform.position + Vector3.forward * cellSize, swipeMovementDuration);         
        (board.boardArray[rowPosition, columnPosition], board.boardArray[rowPosition - 1, columnPosition]) = (board.boardArray[rowPosition - 1, columnPosition], board.boardArray[rowPosition, columnPosition]);
        positionInfo.x--;
        myTween.OnComplete( () => EventManager.OnPlayerSwiped?.Invoke());
    }

    private void MoveDownward(int rowPosition, int columnPosition)
    {
        if (rowPosition + 1 >= board.BoardSize || !board.boardArray[rowPosition + 1, columnPosition].gameObject.activeInHierarchy) return;

        isMoved = true;
        board.boardArray[rowPosition + 1, columnPosition].GetComponent<DropMovementController>()?.ChangePlace(Vector3.forward, -1, 0);
        Tween myTween = transform.DOMove(transform.position + Vector3.back * cellSize, swipeMovementDuration);
        (board.boardArray[rowPosition, columnPosition], board.boardArray[rowPosition + 1, columnPosition]) = (board.boardArray[rowPosition + 1, columnPosition], board.boardArray[rowPosition, columnPosition]);
        positionInfo.x++;
        myTween.OnComplete( () => EventManager.OnPlayerSwiped?.Invoke());  
    }
    
    private void MoveRight(int rowPosition, int columnPosition)
    {
        if (columnPosition + 1 >= board.BoardSize || !board.boardArray[rowPosition, columnPosition + 1].gameObject.activeInHierarchy) return;

        isMoved = true;
        board.boardArray[rowPosition, columnPosition + 1].GetComponent<DropMovementController>()?.ChangePlace(Vector3.left, 0, -1);
        Tween myTween = transform.DOMove(transform.position + Vector3.right * cellSize, swipeMovementDuration);
        (board.boardArray[rowPosition, columnPosition], board.boardArray[rowPosition, columnPosition + 1]) = (board.boardArray[rowPosition, columnPosition + 1], board.boardArray[rowPosition, columnPosition]);
        positionInfo.z++;
        myTween.OnComplete( () => EventManager.OnPlayerSwiped?.Invoke());
    }
    
    private void MoveLeft(int rowPosition, int columnPosition)
    {
        if (columnPosition - 1 < 0 || !board.boardArray[rowPosition, columnPosition - 1].gameObject.activeInHierarchy) return;

        isMoved = true;
        board.boardArray[rowPosition, columnPosition - 1].GetComponent<DropMovementController>()?.ChangePlace(Vector3.right, 0, 1);
        Tween myTween = transform.DOMove(transform.position + Vector3.left * cellSize, swipeMovementDuration);
        (board.boardArray[rowPosition, columnPosition], board.boardArray[rowPosition, columnPosition - 1]) = (board.boardArray[rowPosition, columnPosition - 1], board.boardArray[rowPosition, columnPosition]);
        positionInfo.z--;
        myTween.OnComplete( () => EventManager.OnPlayerSwiped?.Invoke());
    }

    private void ChangePlace(Vector3 movementDir, int rowChange, int columnChange)
    {
        positionInfo = drop.PositionInfo;
        isMoved = true;
        positionInfo.x += rowChange;
        positionInfo.z += columnChange;
        transform.DOMove(transform.position + movementDir * cellSize, swipeMovementDuration);
        
        drop.PositionInfo = positionInfo;
    }

    public void MoveUp()
    {
        if (positionInfo.x <= 0 || this.gameObject.activeInHierarchy) return;

        transform.position += Vector3.forward * cellSize;
        positionInfo = new Vector3Int(positionInfo.x - 1, 0, positionInfo.z);
        drop.PositionInfo = positionInfo;
    }

    private void GoBackPreviousPosition()
    {
        if (!isMoved) return;

        Vector3 previousPosition = new Vector3(previousPositionInfo.z * cellSize, 0f, previousPositionInfo.x * -cellSize);
        transform.DOMove(previousPosition, moveBackDuration);
        board.boardArray[previousPositionInfo.x, previousPositionInfo.z] = this.gameObject;
        positionInfo = previousPositionInfo;
        drop.PositionInfo = positionInfo;
        isMoved = false;
    }
}
