using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour, IPooledObject
{
    [SerializeField]
    DropData dropData;
    
    public DropColor.DropColorState DropColorInfo { get { return dropData.dropColorInfo; } }

    private Vector3 positionInfo;
    public Vector3 PositionInfo { set { positionInfo = value; } }

    public void OnObjectSpawn()
    {
        positionInfo = transform.position;
    }

    public void OnObjectRespawn()
    {
        return;
    }
}
