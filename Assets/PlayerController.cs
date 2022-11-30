using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public const float SPEED = 5f; // change this to change the movement speed
    private const int STOPPED = -1; // a moveDirection value
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer renderer;
    private Vector3 lastPlayerPosition;

    void Start() {
        animator = GetComponentInChildren<Animator>();
        rigidBody = GetComponentInChildren<Rigidbody2D>();
        renderer = GetComponentInChildren<SpriteRenderer>();
        lastPlayerPosition = transform.position;
    }

    void Update() {
        // Camera follows player y
        Camera.main.transform.position += new Vector3(0, rigidBody.transform.position.y - lastPlayerPosition.y, 0);
        lastPlayerPosition = rigidBody.transform.position;
        int moveDirection = 0; // degrees ccw from going right

        bool w = Input.GetKey("w");
        bool s = Input.GetKey("s");
        bool a = Input.GetKey("a");
        bool d = Input.GetKey("d");

        if (w && s) {
            w = false;
            s = false;
        }
        if (d && a) {
            d = false;
            a = false;
        }

        if (w) {
            if (d) {
                moveDirection = 45;
            } else if (a) {
                moveDirection = 135;
            } else {
                moveDirection = 90;
            }
        } else if (s) {
            if (d) {
                moveDirection = 315;
            } else if (a) {
                moveDirection = 225;
            } else {
                moveDirection = 270;
            }
        } else if (d) {
            moveDirection = 0;
        } else if (a) {
            moveDirection = 180;
        } else {
            moveDirection = STOPPED;
        }

        animator.SetInteger("walking", moveDirection);
        if (moveDirection > 90 && moveDirection <= 270) {
            renderer.flipX = false;
        } else if (moveDirection != STOPPED) {
            renderer.flipX = true;
        }
        
        if (moveDirection == STOPPED) {
            rigidBody.velocity = new Vector2(0, 0);
        } else {
            rigidBody.velocity = moveDirectionToVector(moveDirection);
        }

        // Stay on the ground
        if (rigidBody.transform.position.y > 5.26f) {
            rigidBody.transform.position -= new Vector3(0, rigidBody.transform.position.y - 5.26f, 0);
        }
    }

    static Vector2 moveDirectionToVector(int moveDirection) {
        float deltaX = Mathf.Cos(Mathf.Deg2Rad * (moveDirection)) * SPEED;
        float deltaY = Mathf.Sin(Mathf.Deg2Rad * (moveDirection)) * SPEED;
        return new Vector2(
            deltaX,
            deltaY
        );
    }
}
