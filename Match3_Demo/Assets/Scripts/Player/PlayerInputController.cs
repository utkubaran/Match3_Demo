using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    private Camera mainCam;

    private Vector3 startPos, finalPos, swipeDirection;

    private Transform selectedDrop;

    private float timeRemaining, moveTimer = 1.5f;

    private bool isPlaying;

    private void OnEnable()
    {
        EventManager.OnLevelStart.AddListener( () => isPlaying = true );
        EventManager.OnLevelFinish.AddListener( () => isPlaying = false );
        EventManager.OnLevelFail.AddListener( () => isPlaying = false );
    }

    private void OnDisable()
    {
        EventManager.OnLevelStart.RemoveListener( () => isPlaying = true );
        EventManager.OnLevelFinish.RemoveListener( () => isPlaying = false );
        EventManager.OnLevelFail.RemoveListener( () => isPlaying = false );
    }

    private void Awake()
    {
        mainCam = Camera.main;
    }

    void Start()
    {
        timeRemaining = moveTimer;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        GetSwipeDirectionFromPlayer();
    }

    private void GetSwipeDirectionFromPlayer()
    {
        if (!isPlaying) return;

        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;

            var ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                selectedDrop = hit.transform;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (selectedDrop == null) return;

            finalPos = Input.mousePosition;
            bool isExceededThreshold = (finalPos - startPos).magnitude * Time.deltaTime >= 0.25f ? true : false;

            if (!isExceededThreshold || !Checktimer()) return;

            swipeDirection = GetSwipeDirection(finalPos - startPos);
            selectedDrop.GetComponent<DropMovementController>()?.OnSwiped(swipeDirection);
            swipeDirection = Vector3.zero;
            selectedDrop = null;
        }
    }

    private Vector3 GetSwipeDirection(Vector3 inputDir)
    {
        float xDif = inputDir.x;
        float zDif = inputDir.y;
        bool isMovementOnX = Mathf.Abs(xDif) > Mathf.Abs(zDif);

        if (isMovementOnX && xDif > 0)
        {
            return Vector3.right;
        }
        else if (isMovementOnX && xDif <= 0)
        {
            return  Vector3.left;
        }
        else if (!isMovementOnX && zDif > 0)
        {
            return Vector3.forward;
        }
        else if (!isMovementOnX && zDif <= 0)
        {
            return Vector3.back;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private bool Checktimer()
    {
        if (timeRemaining > 0) return false;

        timeRemaining = moveTimer;
        return true;
    }
}
