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

    private float destroyTimer = 2f;

    private float cellSize;

    private void Start()
    {
        board = Board.instance;
        cellSize = board.CellSize;
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

    public void OnMatch()
    {
        while (destroyTimer > 0)
        {
            destroyTimer -= Time.deltaTime;
            transform.localScale  = Vector3.Lerp(transform.localScale, Vector3.one * 0.1f, 0.1f);
        }

        this.gameObject.SetActive(false);
        transform.position = (Vector3.forward + Vector3.left) * 5f;
    }
}
