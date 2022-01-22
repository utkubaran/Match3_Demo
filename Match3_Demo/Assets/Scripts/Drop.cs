using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour, IPooledObject, IDrop
{
    [SerializeField]
    DropData dropData;
    
    public DropColor.DropColorState DropColorInfo { get { return dropData.dropColorInfo; } }

    private Board board;

    private Vector3Int positionInfo;
    public Vector3Int PositionInfo { get { return positionInfo; } set { positionInfo = value; } }

    private void OnEnable()
    {
        // EventManager.OnPlayerSwiped.AddListener(CheckPosition);
    }

    private void OnDisable()
    {
        // EventManager.OnPlayerSwiped.RemoveListener(CheckPosition);
    }

    private void Awake()
    {
        board = Board.instance;
    }

    public void OnObjectSpawn()
    {
        return;
    }

    public void OnObjectRespawn()
    {
        return;
    }

    public void OnSwiped(Vector3 movementDir)
    {
        this.transform.position += movementDir * board.CellSize;

        int xPos = positionInfo.x;
        int zPos = positionInfo.z;

        if (movementDir == Vector3.right)
        {
            board.boardArray[xPos+1, zPos].GetComponent<Drop>().ChangePlace(Vector3.left);
            // board.boardArray[xPos, zPos] = board.boardArray[xPos+1, zPos];
            // board.boardArray[xPos+1, zPos] = this.gameObject;
            (board.boardArray[xPos, zPos], board.boardArray[xPos+1, zPos]) = (board.boardArray[xPos+1, zPos], board.boardArray[xPos, zPos]);
            positionInfo.x++;
        }
        else if (movementDir == Vector3.left)
        {
            board.boardArray[xPos-1, zPos].GetComponent<Drop>().ChangePlace(Vector3.right);
            // board.boardArray[xPos, zPos] = board.boardArray[xPos-1, zPos];
            // board.boardArray[xPos-1, zPos] = this.gameObject;
            (board.boardArray[xPos, zPos], board.boardArray[xPos-1, zPos]) = (board.boardArray[xPos-1, zPos], board.boardArray[xPos, zPos]);
            positionInfo.x--;
        }
        else if (movementDir == Vector3.forward)
        {
            board.boardArray[xPos, zPos-1].GetComponent<Drop>().ChangePlace(Vector3.back);
            board.boardArray[xPos, zPos] = board.boardArray[xPos, zPos-1];
            board.boardArray[xPos, zPos-1] = this.gameObject;
            // (board.boardArray[xPos, zPos], board.boardArray[xPos, zPos-1]) = (board.boardArray[xPos, zPos-1], board.boardArray[xPos, zPos]);
            positionInfo.z--;
        }
        else if (movementDir == Vector3.back)
        {
            board.boardArray[xPos, zPos+1].GetComponent<Drop>().ChangePlace(Vector3.forward);
            board.boardArray[xPos, zPos] = board.boardArray[xPos, zPos+1];
            board.boardArray[xPos, zPos+1] = this.gameObject;
            // (board.boardArray[xPos, zPos], board.boardArray[xPos, zPos+1]) = (board.boardArray[xPos, zPos+1], board.boardArray[xPos, zPos]);
            positionInfo.z++;
        }
    }

    private void ChangePlace(Vector3 movementDir)
    {
        this.transform.position += movementDir * board.CellSize;
    }

    private void CheckPosition()
    {
        bool isMoved = board.boardArray[positionInfo.x, positionInfo.z] == this.gameObject;

        if (!isMoved) return;
        Debug.Log("I moved : " + transform.name);

        bool isUp = board.boardArray[positionInfo.x, positionInfo.z + 1] == this.gameObject;
        bool isDown = board.boardArray[positionInfo.x, positionInfo.z - 1] == this.gameObject;
        bool isRight = board.boardArray[positionInfo.x + 1, positionInfo.z] == this.gameObject;
        bool isLeft = board.boardArray[positionInfo.x - 1, positionInfo.z] == this.gameObject;

        if (isUp)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z + board.CellSize);
        }
        else if (isDown)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z - board.CellSize);
        }
        else if (isRight)
        {
            transform.position = new Vector3(transform.position.x + board.CellSize, 0f, transform.position.z);
        }
        else if (isLeft)
        {
            transform.position = new Vector3(transform.position.x - board.CellSize, 0f, transform.position.z);
        }
    }
}
