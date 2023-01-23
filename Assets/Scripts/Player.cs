using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Joystick joystick;

    [SerializeField]
    private GameObject targetIndicator;

    [SerializeField]
    private Transform arrow;
    
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private GameObject sprite;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        arrow.GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        //make sure zoom is out for phones in Portrait and Landscape
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
            camera.orthographicSize = 7.5f;
        else
            camera.orthographicSize = 15f;
    }

    void FixedUpdate()
    {
        if (movement != Vector2.zero)
            SpriteRotation();

        //indication arrow
        Vector2 dir = MazeRenderer.exitDoor.position - targetIndicator.transform.position;
        if (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) > 20)
        {
            arrow.GetComponent<Renderer>().enabled = true;
            float angle = 0;
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            targetIndicator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
            arrow.GetComponent<Renderer>().enabled = false;
        //move
        movement = joystick.Direction;
        rb.velocity = movement * speed;
    }

    void SpriteRotation()
    {        
        var angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
        sprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnMove(InputValue axis)
    {
        movement = axis.Get<Vector2>();
    }    
}
