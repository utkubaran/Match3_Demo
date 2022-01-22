using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    #region Level Events
    public static UnityEvent OnLevelStart = new UnityEvent();
    public static UnityEvent OnLevelFail = new UnityEvent();
    public static UnityEvent OnLevelFinish = new UnityEvent();
    #endregion

    #region Player Events
    public static UnityEvent OnPlayerSwiped = new UnityEvent();
    #endregion

    #region Match Events
    public static UnityEvent OnNoMatch = new UnityEvent();
    public static MatchEvent OnMatch = new MatchEvent();
    #endregion
}

public class MatchEvent : UnityEvent<List<Transform>> { }
