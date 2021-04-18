using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class BadGuyController : MonoBehaviour
{
    [SerializableAttribute]
    public struct MoveZone {
        public Vector2 right;
        public Vector2 left;
    }

    [SerializableAttribute]
    public enum Direction : sbyte {
        LEFT = -1,
        RIGHT = 1
    }

    [SerializableAttribute]
    public enum Movements {
        HorizontalRotate,
        HorizontalStatic,
        VerticalRotate,
        VerticalStatic,
        DOntCare
    }

    public RigidbodyConstraints2D rigidbodyConstraints2D;
    // public Movements movements;
    public int speed = 1;
    public Direction direction = Direction.LEFT;
    public Transform moveLeft;
    public Transform moveRight;
    private MoveZone _moveZone;
    private new Rigidbody2D rigidbody;
    private Vector2 destination;

    // Start is called before the first frame update
    void Start()
    {
        _moveZone.left = new Vector2(moveLeft.position.x, moveLeft.position.y);
        _moveZone.right = new Vector2(moveRight.position.x, moveRight.position.y);
        rigidbody = GetComponent<Rigidbody2D>();
        // if (movements == Movements.HorizontalStatic) {
        //     rigidbodyConstraints2D = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        // }
        rigidbody.constraints = rigidbodyConstraints2D;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveHorizontaly();
    }

    public void MoveHorizontaly() {
        // Debug.LogFormat("transform.position.x: {0}; transform.position.y: {1}", transform.position.x, transform.position.y);
        // Debug.LogFormat("_moveZone.left.x: {0}; _moveZone.right.x: {1}", _moveZone.left.x, _moveZone.right.x);

        //Handles cases where the x position is out of the MoveZone bounds
        //Sets the x position to the closest bound and sets the Direction towards the opposite bound
        if (transform.position.x <= _moveZone.left.x && direction == Direction.LEFT) {
            // Debug.Log("transform.position.x <= _moveZone.left.x");
            transform.position = new Vector2(_moveZone.left.x, transform.position.y);
            direction = Direction.RIGHT;
        }
        else if (transform.position.x >= _moveZone.right.x && direction == Direction.RIGHT) {
            // Debug.Log("transform.position.x <= _moveZone.left.x");
            transform.position = new Vector2(_moveZone.right.x, transform.position.y);
            direction = Direction.LEFT;
        }

        //Moves the GameObject from it's current position to destination over time
        if (direction == Direction.RIGHT) {
            destination = new Vector2(_moveZone.right.x, transform.position.y);
            // Debug.LogFormat("destination.x: {0}; destination.y: {1}", destination.x, destination.y);
            // Debug.Log("direction == Direction.RIGHT");
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }
        else if (direction == Direction.LEFT) {
            destination = new Vector2(_moveZone.left.x, transform.position.y);
            // Debug.LogFormat("destination.x: {0}; destination.y: {1}", destination.x, destination.y);
            // Debug.Log("direction == Direction.LEFT");
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }
    }
}
