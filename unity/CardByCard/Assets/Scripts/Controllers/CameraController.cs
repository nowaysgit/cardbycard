using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    private float _defaultHeight;
    private float _defaultWidth;

    private void Start()
    {
        _defaultHeight = camera.orthographicSize;
        _defaultWidth = camera.orthographicSize * camera.aspect;
    }

    private void Update()
    {
        camera.orthographicSize = (_defaultWidth / camera.aspect)*0.5f;
    }
}
