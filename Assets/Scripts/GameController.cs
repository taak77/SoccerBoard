using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public bool IsDrawMode { get; protected set; } = false;

    [SerializeField]
    private GameObject line;

   

    private GameObject currentLine;
    private List<GameObject> lines;


    private LineRenderer lineRenderer;
    private List<Vector2> fingerPositions;

    private bool isFieldTouched = false;

    private LayoutManager layoutManager;
    

    public void ResetGame()
    {
        SceneManager.LoadScene("2D");
    }

    public void ClearLines()
    {
        if (lines.Count == 0)
            return;

        while(lines.Count > 0)
        {
            GameObject line = lines[lines.Count - 1];
            Destroy(line);
            lines.RemoveAt(lines.Count - 1);
        }
    }

    public void Undo()
    {
        if (lines.Count == 0)
            return;

        GameObject line = lines[lines.Count - 1];
        Destroy(line);
        lines.RemoveAt(lines.Count - 1);
    }

    public void OnChangeDrawMode(bool value)
    {
        IsDrawMode = value;
    }

    private void Update()
    {
        if (IsDrawMode)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    //We transform the touch position into word space from screen space and store it.
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                    Vector2 touchPosWorld2D = new Vector2(touchPosition.x, touchPosition.y);
                    RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
                    // Debug.Log($"hitInformation.collider: {hitInformation.collider}");
                    if (hitInformation.collider != null)
                    {
                        //We should have hit something with a 2D Physics collider!
                        GameObject touchedObject = hitInformation.transform.gameObject;
                        if (touchedObject.CompareTag("Field"))
                        {
                            isFieldTouched = true;
                        }
                    }
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    isFieldTouched = false;
                }
            }


            if (isFieldTouched)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    CreateLine();
                }
                if (Input.GetMouseButton(0))
                {
                    Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (Vector2.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]) > .1f)
                    {
                        UpdateLine(tempFingerPos);
                    }
                }
            }
        }
    }

    void CreateLine()
    {
        currentLine = Instantiate(line, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        if (fingerPositions == null)
        {
            fingerPositions = new List<Vector2>();
        }
        fingerPositions.Clear();
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        lineRenderer.SetPosition(0, fingerPositions[0]);
        lineRenderer.SetPosition(1, fingerPositions[1]);
        if (lines == null)
        {
            lines = new List<GameObject>();
        }
        lines.Add(currentLine);
    }

    void UpdateLine(Vector2 newFingerPos)
    {
        fingerPositions.Add(newFingerPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
    }
}
