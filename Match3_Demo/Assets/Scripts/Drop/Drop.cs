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

    private float cellSize;

    private bool isMoved;

    private void OnEnable()
    {
        // EventManager.OnNoMatch.AddListener(GoPreviousPosition);
        // EventManager.OnPlayerSwiped.AddListener(CheckPosition);
    }

    private void OnDisable()
    {
        // EventManager.OnNoMatch.RemoveListener(GoPreviousPosition);
        // EventManager.OnPlayerSwiped.RemoveListener(CheckPosition);
    }

    private void Start()
    {
        board = Board.instance;
        cellSize = board.CellSize;
        isMoved = false;
        previousPositionInfo = positionInfo;
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

            isMoved = true;
            board.boardArray[rowPosition - 1, columnPosition].GetComponent<Drop>().ChangePlace(Vector3.back, 1, 0);
            this.transform.position += movementDir * cellSize;            
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

            isMoved = true;
            board.boardArray[rowPosition + 1, columnPosition].GetComponent<Drop>().ChangePlace(Vector3.forward, -1, 0);
            this.transform.position += movementDir * cellSize;
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

            isMoved = true;
            board.boardArray[rowPosition, columnPosition + 1].GetComponent<Drop>().ChangePlace(Vector3.left, 0, -1);
            this.transform.position += movementDir * cellSize;
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

            isMoved = true;
            board.boardArray[rowPosition, columnPosition - 1].GetComponent<Drop>().ChangePlace(Vector3.right, 0, 1);
            this.transform.position += movementDir * cellSize;
            (board.boardArray[rowPosition, columnPosition], board.boardArray[rowPosition, columnPosition - 1]) = (board.boardArray[rowPosition, columnPosition - 1], board.boardArray[rowPosition, columnPosition]);
            positionInfo.z--;
        }
    }

    private void ChangePlace(Vector3 movementDir, int rowChange, int columnChange)
    {
        isMoved = true;
        positionInfo.x += rowChange;
        positionInfo.z += columnChange;
        this.transform.position += movementDir * cellSize;
    }

    private void GoPreviousPositionWithDelay()
    {
        if (isMoved) StartCoroutine(GoPreviousPosition());
        // if (!isMoved) return;

        /*
        transform.position = new Vector3(previousPositionInfo.z * cellSize, 0f, previousPositionInfo.x * -cellSize);
        // board.boardArray[previousPositionInfo.x, previousPositionInfo.z] = this.gameObject;
        positionInfo = previousPositionInfo;
        isMoved = false;
        */
    }

    private IEnumerator GoPreviousPosition()
    {
        Debug.Log("works");
        yield return new WaitForSeconds(2f);

        // Debug.Log("works");
        // Debug.Log(previousPositionInfo + " " + positionInfo);

        transform.position = new Vector3(previousPositionInfo.z * cellSize, 0f, previousPositionInfo.x * -cellSize);
        board.boardArray[previousPositionInfo.x, previousPositionInfo.z] = this.gameObject;
        positionInfo = previousPositionInfo;
        isMoved = false;
    }
}
