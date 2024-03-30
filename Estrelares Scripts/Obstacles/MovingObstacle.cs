using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : Obstacle
{
    [BoxGroup("Movement")]
    [SerializeField]
    private float _moveSpeed = 2f;
    [BoxGroup("Movement")]
    [SerializeField]
    private Direction _moveDirection = Direction.clockwise;
    private enum Direction
    {
        clockwise,
        counterclockwise
    }
    private Vector3 _direction = Vector3.back;

    private Vector3 _startPosition = Vector3.zero;
    private Quaternion _startRotation;

    [BoxGroup("World")]
    [SerializeField]
    private Transform _currentWorld = null;

    //------------------------------------------------------------------

    protected override void Start()
    {
        base.Start();

        _startPosition = transform.position;
        _startRotation = transform.rotation;

        switch (_moveDirection)
        {
            default:
            case Direction.clockwise:
                _direction = Vector3.back;
                break;
            case Direction.counterclockwise:
                _direction = Vector3.forward;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (_finishedSpawn)
            transform.RotateAround(_currentWorld.transform.localPosition, _direction, Time.fixedDeltaTime * _moveSpeed);
    }

    //------------------------------------------------------------------

    public override void ResetObject()
    {
        base.ResetObject();

        transform.position = _startPosition;
        transform.rotation = _startRotation;
    }
}
