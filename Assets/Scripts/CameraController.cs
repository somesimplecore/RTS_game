using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float moveSmoothing = 5f;
    private Vector3 topLeftBound;
    private Vector3 bottomRightBound;

    private Vector3 targetPosition;
    private Vector3 input;

    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float rotationSmoothing = 5f;

    private void Start()
    {
        var hexesList = HexGridLayout.Instance.hexes;
        topLeftBound = hexesList[0].transform.position + Vector3.forward * 30 + Vector3.left * 30;
        bottomRightBound = hexesList[hexesList.Count - 1].transform.position + Vector3.back * 34;
    }

    void Update()
    {
        HandleInput();
        Move();
    }

    private void HandleInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 right = transform.right * x;
        Vector3 forward = transform.forward * z;

        input = (forward + right).normalized;
    }


    private void Move()
    {
        Vector3 nextTargetPosition = targetPosition + input * moveSpeed;
        if (IsInBounds(nextTargetPosition)) targetPosition = nextTargetPosition;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSmoothing);
    }

    private bool IsInBounds(Vector3 position)
    {
        return position.x > topLeftBound.x &&
        position.x < bottomRightBound.x &&
        position.z > bottomRightBound.z &&
        position.z < topLeftBound.z;
    }

    public void SetTargetPosition(Vector3 newPos)
    {
        targetPosition = newPos;
    }
}
