using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public Transform target;  // Character to follow
    private Bounds cameraBounds;
    private Vector3 targetPosition;
    private Camera mainCamera;

    private void Awake() => mainCamera = Camera.main;

    private void Start()
    {
        var height = mainCamera.orthographicSize;
        var width = height * mainCamera.aspect;

        var minX = Globals.WorldBounds.min.x + width;
        var maxX = Globals.WorldBounds.max.x - width;

        var minY = Globals.WorldBounds.min.y + height;
        var maxY = Globals.WorldBounds.max.y - height;

        cameraBounds = new Bounds();
        cameraBounds.SetMinMax(
            new Vector3(minX, minY, 0.0f),
            new Vector3(maxX, maxY, 0.0f)
        );
    }

    void Update()
    {
        // Define the desired position based on the target's position
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
        
        // Smoothly move the camera towards the target position
        targetPosition = Vector3.Lerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
        
        // Clamp the camera position to stay within the bounds
        targetPosition = GetCameraBounds();
        
        // Apply the position to the camera
        transform.position = targetPosition;
    }

    private Vector3 GetCameraBounds()
    {
        // Clamp the camera's target position within the defined world bounds
        return new Vector3(
            Mathf.Clamp(targetPosition.x, cameraBounds.min.x, cameraBounds.max.x),
            Mathf.Clamp(targetPosition.y, cameraBounds.min.y, cameraBounds.max.y),
            transform.position.z // Maintain the camera's Z position
        );
    }
}
