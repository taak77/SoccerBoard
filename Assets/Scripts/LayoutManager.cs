using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutManager : MonoBehaviour
{
    public static LayoutManager Instance { get; private set; }

    public delegate void ScreenSizeChangeEventHandler(int width, int height);
    public event ScreenSizeChangeEventHandler ScreenSizeChangeEvent;
    protected virtual void RaiseScreenSizeChange(int width, int height)
    {
        ScreenSizeChangeEvent?.Invoke(width, height);
    }

    [SerializeField]
    private GameObject field;
    [SerializeField]
    private GameObject fieldCollider;
    [SerializeField]
    private GameObject team1;
    [SerializeField]
    private GameObject team2;

    private Vector2 currentResolution;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        currentResolution = new Vector2(Screen.width, Screen.height);
        Instance.ScreenSizeChangeEvent += OnScreenSizeChange;
        OnScreenSizeChange(Screen.width, Screen.height);
    }

    private void Update()
    {
        Vector2 tempResolution = new Vector2(Screen.width, Screen.height);
        if (tempResolution != currentResolution)
        {
            currentResolution = tempResolution;
            RaiseScreenSizeChange(Screen.width, Screen.height);
        }
    }

    private void OnScreenSizeChange(int width, int height)
    {
        LayoutField(width, height);
    }

    void LayoutField(int width, int height)
    {
        
    }
}
