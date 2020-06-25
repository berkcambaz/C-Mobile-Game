using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject particle;

    private Vector2 moveableAreaBounds;
    private Vector2 playerSize;

    private Touch touch;
    private Vector3 touchPos;

    private Vector2 dragStartPos;
    private bool isHeld = false;

    void Start()
    {
        moveableAreaBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        playerSize = new Vector2(transform.localScale.x / 2.0f, transform.localScale.y / 2.0f);
    }

    void Update()
    {
        // If received at least 1 touch
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            touchPos = Camera.main.ScreenToWorldPoint(touch.position);
        }

        DragAndDropMovement();
        ClampPlayerToMoveableArea();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // When colliding with obstacle
        // TODO: Screen shake effect
        Instantiate(particle, transform.position, transform.rotation);

        UserData.isAlive = false;

        Destroy(gameObject);

        // When colliding with power-up
    }

    void DragAndDropMovement()
    {
        // If touch has started
        if (touch.phase == TouchPhase.Began)
        {
            isHeld = true;

            dragStartPos.x = touchPos.x - transform.localPosition.x;
            dragStartPos.y = touchPos.y - transform.localPosition.y;
        }

        // If touch has stopped
        if (touch.phase == TouchPhase.Ended)
        {
            isHeld = false;
        }

        // If touching
        if (isHeld)
        {
            transform.position = new Vector3(touchPos.x - dragStartPos.x, touchPos.y - dragStartPos.y, transform.position.z);
        }
    }

    void ClampPlayerToMoveableArea()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -moveableAreaBounds.x + playerSize.x, moveableAreaBounds.x - playerSize.x);
        pos.y = Mathf.Clamp(pos.y, -moveableAreaBounds.y + playerSize.y, moveableAreaBounds.y - playerSize.y);
        transform.position = pos;
    }
}
