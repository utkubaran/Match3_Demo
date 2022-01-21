using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] dropPrefabs;

    [SerializeField]
    private int boardSize;

    private DropColor.DropColorState[,] board;
    
    void Start()
    {
        board = new DropColor.DropColorState[boardSize, boardSize];
        GenerateGameBoard();
    }

    private void GenerateGameBoard()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; i++)
            {
                int randomNum = Random.Range((int) DropColor.DropColorState.Red, (int) DropColor.DropColorState.Green + 1);
                Vector2 spawnPos = new Vector3(j * 1.5f, 0f, i * 1.5f );
                GameObject obj = Instantiate(dropPrefabs[randomNum], spawnPos, Quaternion.identity);
                obj.transform.parent = transform.parent;

                // board[i,j] = obj.GetComponent<Drop>().DropColorInfo;
            }
        }
    }
}
