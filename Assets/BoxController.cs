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

        float dy = Mathf.Cos(Mathf.Deg2Rad * (rigidBody.rotation));
        float dx = -Mathf.Sin(Mathf.Deg2Rad * (rigidBody.rotation));
        if (w) {
            rigidBody.velocity += .01f * new Vector2(dx, dy);
        } else if (s) {
            rigidBody.velocity -= .01f * new Vector2(dx, dy);
        }
        if (d) {
            rigidBody.angularVelocity -= .2f;
        } else if (a) {
            rigidBody.angularVelocity += .2f;
        }

        bool moving = w || s;
        animator.SetBool("moving", moving);
    }

    public void SetHasCat(bool hasCat) {
        if (this.hasCat == hasCat) return;
        this.hasCat = hasCat;
        animator.SetBool("hascat", hasCat);
        if (this.hasCat) {
            rigidBody.rotation = 0;
            rigidBody.velocity = new Vector2(0, .2f);
        } else {
            rigidBody.velocity = Vector2.zero;
        }
        rigidBody.angularVelocity = 0;
    }
}
