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
    private Camera camera;

    [SerializeField]
    private RawImage joystick;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 touchOrigin;

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
