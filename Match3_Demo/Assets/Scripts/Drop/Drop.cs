using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Drop : MonoBehaviour, IPooledObject, IDrop
{
    [SerializeField]
    DropData dropData;
    
    public DropColor.DropColorState DropColorInfo { get { return dropData.dropColorInfo; } }

    private Board board;

    private Vector3Int previousPositionInfo;

    private Vector3Int positionInfo;
    public Vector3Int PositionInfo { get { return positionInfo; } set { positionInfo = value; } }

    private float destroyTimer = 0.2f;

    private void Start()
    {
        board = Board.instance;
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
        StartCoroutine(ScaleDownWithDelay());
    }

    private IEnumerator ScaleDownWithDelay()
    {
        transform.DOScale(Vector3.one * 0.01f, destroyTimer);
        yield return new WaitForSeconds(destroyTimer);
        this.gameObject.SetActive(false);
        transform.position = (Vector3.forward + Vector3.left) * 5f;
    }
}
