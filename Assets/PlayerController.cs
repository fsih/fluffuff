using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject box;
    public const float SPEED = 5f; // change this to change the movement speed
    private const int STOPPED = -1; // a moveDirection value
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer renderer;
    private Vector3 lastPlayerPosition;
    private bool inBox = false;

    void Start() {
        animator = GetComponentInChildren<Animator>();
        rigidBody = GetComponentInChildren<Rigidbody2D>();
        renderer = GetComponentInChildren<SpriteRenderer>();
        lastPlayerPosition = transform.position;
    }

    void Update() {
        // Stay in the bounds
        if (rigidBody.transform.position.y > 5.26f) {
            rigidBody.transform.position -= new Vector3(0, rigidBody.transform.position.y - 5.26f, 0);
        }
        if (rigidBody.transform.position.y < 0f) {
            rigidBody.transform.position -= new Vector3(0, rigidBody.transform.position.y, 0);
        }
        if (rigidBody.transform.position.x > 3.4f) {
            rigidBody.transform.position -= new Vector3(rigidBody.transform.position.x - 3.4f, 0, 0);
        }
        if (rigidBody.transform.position.x < -3.4f) {
            rigidBody.transform.position -= new Vector3(rigidBody.transform.position.x + 3.4f, 0, 0);
        }

        if (inBox) return;

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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameObject.activeSelf) return;
        if (other.gameObject == box) {
            SetInBox(true);
            box.GetComponent<BoxController>().SetHasCat(true);
        }
    }

    public void SetInBox(bool inBox) {
        if (this.inBox == inBox) return;
        this.inBox = inBox;
        if (!inBox) {
            // move cat away from box so it doesn't collide
            rigidBody.transform.position = box.transform.position - new Vector3(0, 1f, 0);
        }
        gameObject.SetActive(!inBox);
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
