using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOverlayManager : MonoBehaviour
{
    public static UIOverlayManager Instance {  get; private set; }
    [SerializeField] Canvas canvas;

    [SerializeField] Camera UIOverlayCamera;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        UIOverlayCamera = GameObject.FindGameObjectWithTag("UIOverlayCamera").GetComponent<Camera>();
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }

    public void SetUIElementPosition(Transform UITransform, Vector3 worldPosition)
    {
        UITransform.position = UIOverlayCamera.WorldToScreenPoint(worldPosition);
        //UITransform.rotation = UIOverlayCamera.transform.rotation;
    }
}
