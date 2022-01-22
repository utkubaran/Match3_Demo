using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    private Camera mainCam;

    private Vector3 startPos, finalPos, movementDirection;

    private Transform selectedDrop;

    private float dropMovementSpeed = 1f;

    private bool isPlaying;

    private void Awake()
    {
        mainCam = Camera.main;
    }
    void Start()
    {
        isPlaying = true;       // todo refactor after events are enabled
    }

    void Update()
    {
        GetMovementDirectionFromplayer();
    }

    private void GetMovementDirectionFromplayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;

            var ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                selectedDrop = hit.transform;
                // Debug.Log(hit.transform.GetComponent<Drop>().PositionInfo);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            finalPos = Input.mousePosition;
            movementDirection = finalPos - startPos;

            bool isExceededThreshold = movementDirection.magnitude * Time.deltaTime >= 0.25f ? true : false;        // todo refactor

            if (isExceededThreshold)
            {
                float xDif = finalPos.x - startPos.x;
                float zDif = finalPos.y - startPos.y;
                bool isMovementOnX = Mathf.Abs(xDif) > Mathf.Abs(zDif);

                if (isMovementOnX && xDif > 0)
                {
                    movementDirection = Vector3.right;
                }
                else if (isMovementOnX && xDif <= 0)
                {
                    movementDirection = Vector3.left;
                }
                else if (!isMovementOnX && zDif > 0)
                {
                    movementDirection = Vector3.forward;
                }
                else if (!isMovementOnX && zDif <= 0)
                {
                    movementDirection = Vector3.back;
                }
            }
            else
            {
                movementDirection = Vector3.zero;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (selectedDrop == null || movementDirection.magnitude <= 0.25f) return;

            selectedDrop.GetComponent<Drop>().OnSwiped(movementDirection);
            EventManager.OnPlayerSwiped?.Invoke();
            movementDirection = Vector3.zero;
            selectedDrop = null;
        }
    }

}
