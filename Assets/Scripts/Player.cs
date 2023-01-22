using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Camera camera;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 touchOrigin = -Vector2.one;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
            camera.orthographicSize = 5;
        else
            camera.orthographicSize = 10;
    }

    void FixedUpdate()
    {

        //flip
        if (movement.x > 0)
            transform.localScale = Vector3.one;
        else if (movement.x < 0)
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
    
        //move
        rb.velocity = movement * speed;
    }

    void OnMove(InputValue axis)
    {
        movement = axis.Get<Vector2>();
        Debug.Log(movement);
    }
}
