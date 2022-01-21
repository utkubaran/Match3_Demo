using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField]
    DropData dropData;
    
    public DropColor.DropColorState DropColorInfo { get { return dropData.dropColorInfo; } }
}
