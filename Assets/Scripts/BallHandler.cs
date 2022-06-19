using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{

    private Camera mainCamera;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Rigidbody2D pivot;

    Rigidbody2D currentBallRigidbody;
    SpringJoint2D currentBallSprintJoint;

    [SerializeField] float detachDelay = .5f;
    [SerializeField] float respawnDelay = 2f;

    private bool isDragging;

    void Start()
    {
        mainCamera = Camera.main;

        SpawnNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentBallRigidbody == null) { return; }
        
        
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }

            isDragging = false;

            return;
        }
        

        isDragging = true;
        currentBallRigidbody.isKinematic = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidbody.position = worldPosition;
        
    }



    void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);
        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSprintJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSprintJoint.connectedBody = pivot;

    }



    private void LaunchBall()
    {

        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;

        Invoke(nameof(DetachBall), detachDelay);


        
    }

    void DetachBall()
    {
        currentBallSprintJoint.enabled = false;
        currentBallSprintJoint = null;

        Invoke(nameof(SpawnNewBall), respawnDelay);

    }



}
