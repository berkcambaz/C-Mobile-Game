//#define DEBUG       // To play game on pc
#define RELEASE   // To play game on mobile, specifically android

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
#if RELEASE
        // If received at least 1 touch
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            touchPos = Camera.main.ScreenToWorldPoint(touch.position);
        }
#endif

#if DEBUG
        if (Input.GetMouseButton(0))
        {
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
#endif
        if (UserData.isPlaying)
            DragAndDropMovement();
        ClampPlayerToMoveableArea();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // TODO: Screen shake effect

        // When colliding with obstacle
        Destroy(gameObject);

        // When colliding with power-up
    }

    void OnDestroy()
    {
        KillPlayer();
    }

    void KillPlayer()
    {
        if (!UserData.isQuitting)
        {
            GameObject particleInstance = Instantiate(particle, transform.position, transform.rotation);

            particleInstance.GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(0, UserData.skinSprites[UserData.playerSkinIndex]);

            ParticleSystem.MainModule psmain = particleInstance.GetComponent<ParticleSystem>().main;
            psmain.maxParticles = UserData.particles;

            UI.pauseButton.SetActive(false);    // Disable pause button when player is deleted
        }

        UserData.isAlive = false;
        ObstacleManager.mapTimeLimit = 0f;  // Stops spawning new obstacles
    }

    void DragAndDropMovement()
    {
#if RELEASE
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
#endif

#if DEBUG
        if (Input.GetMouseButtonDown(0))
        {
            isHeld = true;

            dragStartPos.x = touchPos.x - transform.localPosition.x;
            dragStartPos.y = touchPos.y - transform.localPosition.y;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isHeld = false;
        }
#endif

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
