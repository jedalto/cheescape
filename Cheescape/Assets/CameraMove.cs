using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float _mouseSensitivity = 3.0f;

    private float _rotationY;
    private float _rotationX;

    public Transform _target;

    public float _distanceFromTarget = 3.0f;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    private float _smoothTime = 0.2f;

    private Vector2 _rotationXMinMax = new Vector2(-40, 40);

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _rotationX += mouseX;
        _rotationY += mouseY;

        // Apply clamping for x rotation
        _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

        // Apply damp btwn rotation changes
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        transform.position =  _target.position - transform.forward * _distanceFromTarget;
    }
}
