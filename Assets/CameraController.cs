using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float sensitivity = 1;

    Vector2 inputs;
    float scroll;
    Vector3 limitPosition;

    private void Update()
    {
        inputs = Vector2.right * Input.GetAxisRaw("Horizontal") + Vector2.up * Input.GetAxisRaw("Vertical");
        scroll = Input.GetAxisRaw("Mouse ScrollWheel");
    }

    private void LateUpdate()
    {
        if (!GameManager.Instance.CanPlay) return;

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + scroll * sensitivity * sensitivity * Time.deltaTime, 3, 6);
        transform.Translate(Vector3.right * inputs.x * sensitivity * Time.deltaTime + Vector3.up * inputs.y * sensitivity * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, limitPosition.x), Mathf.Clamp(transform.position.y, 0, limitPosition.y), transform.position.z);
    }

    public void SetReferencePosition(int sizeX, int sizeY)
    {
        var centerPos = new Vector3((sizeX / 2) * BoardManager.DOTS_DISTANCE, (sizeY / 2) * BoardManager.DOTS_DISTANCE, transform.position.z);
        transform.position = centerPos;
        limitPosition = new Vector3(sizeX * BoardManager.DOTS_DISTANCE, sizeY * BoardManager.DOTS_DISTANCE, transform.position.z);
        print(limitPosition);
    }
}
