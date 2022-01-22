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
        // GetInputFromPlayer();
    }

    private void GetInputFromPlayer()
    {
        if (!isPlaying) return;

        if (Input.GetMouseButton(0))
        {
            var ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                Debug.Log(movementDirection);
                hit.transform.position += movementDirection.normalized * dropMovementSpeed * Time.deltaTime;
            }
        }
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
                Debug.Log(hit.transform.name);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            finalPos = Input.mousePosition;
            // movementDirection = new Vector3(finalPos.x - startPos.x, 0f, finalPos.y - startPos.y);
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
        else if (Input.GetMouseButtonUp(0))
        {
            if (selectedDrop == null) return;
            selectedDrop.GetComponent<Drop>().OnSwiped(movementDirection);
            movementDirection = Vector3.zero;
            selectedDrop = null;
        }
    }
}
