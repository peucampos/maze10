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
    private RawImage joystick;

    [SerializeField]
    private GameObject targetIndicator;

    [SerializeField]
    private Transform arrow;

    [SerializeField]
    private Camera camera;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 touchOrigin;
    bool facingLeft = true;


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
        //flip
        if (movement.x > 0)
        {
            transform.localScale = Vector3.one;
            facingLeft = true;
        }
        else if (movement.x < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            facingLeft = false;
        }
        
        //indication arrow
        Vector2 dir = MazeRenderer.exitDoor.position - targetIndicator.transform.position;
        if (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) > 20)
        {
            arrow.GetComponent<Renderer>().enabled = true;
            float angle = 0;
            if (facingLeft)
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            else
                angle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
            targetIndicator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
            arrow.GetComponent<Renderer>().enabled = false;

        //move
        rb.velocity = movement * speed;
    }

    void OnMove(InputValue axis)
    {
        movement = axis.Get<Vector2>();
    }

    void TouchOnFingerDown(Finger obj)
    {
        touchOrigin = obj.screenPosition;        
    }

    void TouchOnFingerUp(Finger obj)
    {        
        movement = Vector2.zero;        
    }

    void TouchOnFingerMove(Finger obj)
    {        
        var touchDif = camera.ScreenToWorldPoint(obj.screenPosition) - camera.ScreenToWorldPoint(touchOrigin);
        if (touchDif.x > 1)
            touchDif.x = 1;
        else if (touchDif.x < -1)
            touchDif.x = -1;

        if (touchDif.y > 1)
            touchDif.y = 1;
        else if (touchDif.y < -1)
            touchDif.y = -1;

        movement = touchDif;
    }

    void OnEnable() 
    {
        EnhancedTouchSupport.Enable(); 
        ETouch.Touch.onFingerDown += TouchOnFingerDown;
        ETouch.Touch.onFingerUp += TouchOnFingerUp;
        ETouch.Touch.onFingerMove += TouchOnFingerMove;
    }


    void OnDisable() 
    {
        ETouch.Touch.onFingerDown -= TouchOnFingerDown;
        ETouch.Touch.onFingerUp -= TouchOnFingerUp;
        ETouch.Touch.onFingerMove -= TouchOnFingerMove;
        EnhancedTouchSupport.Disable();            
    }
}
