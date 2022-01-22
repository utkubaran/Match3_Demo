using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour, IPooledObject, IDrop
{
    [SerializeField]
    DropData dropData;
    
    public DropColor.DropColorState DropColorInfo { get { return dropData.dropColorInfo; } }

    private Board board;

    private Vector3Int positionInfo, previousPositionInfo;
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
        int rowPosition = positionInfo.x;
        int columnPosition = positionInfo.z;

        if (movementDir == Vector3.forward)
        {
            if (rowPosition - 1 < 0)
            {
                Debug.Log("Can't Move!");
                return;
            }

            board.boardArray[rowPosition - 1, columnPosition].GetComponent<Drop>().ChangePlace(Vector3.back, 1, 0);
            // board.boardArray[rowPosition, columnPosition] = board.boardArray[rowPosition - 1, columnPosition];
            // board.boardArray[rowPosition - 1, columnPosition] = this.gameObject;
            this.transform.position += movementDir * board.CellSize;            
            (board.boardArray[rowPosition, columnPosition], board.boardArray[rowPosition - 1, columnPosition]) = (board.boardArray[rowPosition - 1, columnPosition], board.boardArray[rowPosition, columnPosition]);
            positionInfo.x--;
        }
        else if (movementDir == Vector3.back)
        {
            if (rowPosition + 1 >= board.BoardSize)
            {
                Debug.Log("Can't Move!");
                return;
            }

            board.boardArray[rowPosition + 1, columnPosition].GetComponent<Drop>().ChangePlace(Vector3.forward, -1, 0);
            // board.boardArray[rowPosition, columnPosition] = board.boardArray[rowPosition + 1, columnPosition];
            // board.boardArray[rowPosition + 1, columnPosition] = this.gameObject;
            this.transform.position += movementDir * board.CellSize;
            (board.boardArray[rowPosition, columnPosition], board.boardArray[rowPosition + 1, columnPosition]) = (board.boardArray[rowPosition + 1, columnPosition], board.boardArray[rowPosition, columnPosition]);
            positionInfo.x++;
        }
        else if (movementDir == Vector3.right)
        {
            if (columnPosition + 1 >= board.BoardSize)
            {
                Debug.Log("Can't Move!");
                return;
            }

            board.boardArray[rowPosition, columnPosition + 1].GetComponent<Drop>().ChangePlace(Vector3.left, 0, -1);
            // board.boardArray[rowPosition, columnPosition] = board.boardArray[rowPosition, columnPosition + 1];
            // board.boardArray[rowPosition, columnPosition + 1] = this.gameObject;
            this.transform.position += movementDir * board.CellSize;
            (board.boardArray[rowPosition, columnPosition], board.boardArray[rowPosition, columnPosition + 1]) = (board.boardArray[rowPosition, columnPosition + 1], board.boardArray[rowPosition, columnPosition]);
            positionInfo.z++;
        }
        else if (movementDir == Vector3.left)
        {
            if (columnPosition - 1 < 0)
            {
                Debug.Log("Can't Move!");
                return;
            }

            board.boardArray[rowPosition, columnPosition - 1].GetComponent<Drop>().ChangePlace(Vector3.right, 0, 1);
            // board.boardArray[rowPosition, columnPosition] = board.boardArray[rowPosition, columnPosition - 1];
            // board.boardArray[rowPosition, columnPosition - 1] = this.gameObject;
            this.transform.position += movementDir * board.CellSize;
            (board.boardArray[rowPosition, columnPosition], board.boardArray[rowPosition, columnPosition - 1]) = (board.boardArray[rowPosition, columnPosition - 1], board.boardArray[rowPosition, columnPosition]);
            positionInfo.z--;
        }
    }

    private void ChangePlace(Vector3 movementDir, int rowChange, int columnChange)
    {
        positionInfo.x += rowChange;
        positionInfo.z += columnChange;
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
