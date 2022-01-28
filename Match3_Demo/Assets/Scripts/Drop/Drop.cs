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

    private Vector3Int positionInfo, previousPositionInfo;
    public Vector3Int PositionInfo { get { return positionInfo; } set { positionInfo = value; } }

    private float destroyTimer = 2f;

    private bool isMatch;

    private void Start()
    {
        board = Board.instance;
        previousPositionInfo = positionInfo;
    }

    private void Update()
    {
        /*
        while (destroyTimer > 0 && isMatch)
        {
            destroyTimer -= Time.deltaTime;
            transform.localScale  = Vector3.Lerp(transform.localScale, Vector3.one * 0.1f, 0.5f);
        }

        if (destroyTimer > 0) return;
        this.gameObject.SetActive(false);
        transform.position = (Vector3.forward + Vector3.left) * 5f;
        */
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
        transform.DOScale(Vector3.one * 0.01f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
        transform.position = (Vector3.forward + Vector3.left) * 5f;
    }
}
