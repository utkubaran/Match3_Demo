using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDropSpawner : MonoBehaviour
{
    private Board board;

    private ObjectPooler objectPooler;

    private List<Vector3> matchedDrops;

    private float cellSize;

    private void OnEnable()
    {
        EventManager.OnMatch.AddListener(SpawnDrops);
    }

    private void OnDisable()
    {
        EventManager.OnMatch.RemoveListener(SpawnDrops);
    }

    void Start()
    {
        board = Board.instance;
        objectPooler = ObjectPooler.instance;
        cellSize = board.CellSize;
    }

    private void SpawnDrops(List<Transform> drops)
    {
        return;
        
        foreach (var drop in drops)
        {
            // todo check neighbour drops before spawning

            /*   
            int randomNum = Random.Range(0 ,4);
            GameObject obj = objectPooler.SpawnFromPool((DropColor.DropColorState)randomNum, drop.position, Quaternion.identity);
            int rowPosition = (int)drop.position.x / (int)cellSize;
            int columnPosition = (int)drop.position.z / -(int)cellSize;
            obj.GetComponent<Drop>().PositionInfo = new Vector3Int(rowPosition, 0, columnPosition);
            Debug.Log(rowPosition + " " + columnPosition);
            board.boardArray[rowPosition, columnPosition] = obj;
            */
        }
    }
}
