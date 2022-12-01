using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public GameObject cat;
    public const float SPEED = 5f; // change this to change the movement speed
    private const int STOPPED = -1; // a moveDirection value
    private bool hasCat = false;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private Vector3 lastPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidBody = GetComponentInChildren<Rigidbody2D>();
        lastPlayerPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Stay in the bounds
        if (rigidBody.transform.position.y < 5f) {
            rigidBody.transform.position -= new Vector3(0, rigidBody.transform.position.y - 5.26f, 0);
            rigidBody.velocity = Vector3.zero;
            SetHasCat(false);
            cat.GetComponent<PlayerController>().SetInBox(false);
        }
        if (rigidBody.transform.position.x > 3.4f) {
            rigidBody.transform.position -= new Vector3(rigidBody.transform.position.x - 3.4f, 0, 0);
        }
        if (rigidBody.transform.position.x < -3.4f) {
            rigidBody.transform.position -= new Vector3(rigidBody.transform.position.x + 3.4f, 0, 0);
        }


        if (!hasCat) return;

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

        animator.SetBool("moving", moveDirection != STOPPED);
        
        if (moveDirection == STOPPED) {
            rigidBody.velocity = new Vector2(0, 0);
        } else {
            rigidBody.velocity = moveDirectionToVector(moveDirection);
        }
    }

    public void SetHasCat(bool hasCat) {
        if (this.hasCat == hasCat) return;
        this.hasCat = hasCat;
        animator.SetBool("hascat", hasCat);
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
