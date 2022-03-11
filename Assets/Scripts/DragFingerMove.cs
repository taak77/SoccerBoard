using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragFingerMove : MonoBehaviour
{
    private GameController gameController;

    private bool isDragging = false;

    private void OnEnable()
    {
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.IsDrawMode && Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //We transform the touch position into word space from screen space and store it.
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                Vector2 touchPosWorld2D = new Vector2(touchPosition.x, touchPosition.y);
                // Debug.Log("touchPosWorld2D " + touchPosWorld2D);
                //We now raycast with this information. If we have hit something we can process it.
                RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

                if (hitInformation.collider != null)
                {
                    //We should have hit something with a 2D Physics collider!
                    GameObject touchedObject = hitInformation.transform.gameObject;
                    if (touchedObject.CompareTag("Draggable") && touchedObject.name == transform.name)
                    {
                        isDragging = true;
                        // Debug.Log("Touched " + touchedObject.transform.name);
                    }                    
                }
            } else if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isDragging = false;
                // Debug.Log("isDragging set to false ");
            }
        }
    }

    private void LateUpdate()
    {
        // Debug.Log("LateUpdate/isDragging: " + isDragging + "/ name:" + transform.name);
        if (isDragging)
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            transform.position = new Vector3(touchPos.x, touchPos.y, 0f);
        }
    }
}
