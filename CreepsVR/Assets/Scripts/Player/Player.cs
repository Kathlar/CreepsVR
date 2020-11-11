using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public Camera mainCamera { get; private set; }
    public GameObject pauseMenuObject;
    public TurnTimer timer { get; private set; }

    public LayerMask raycastLayer;
    public bool raycastOn { get; private set; } = true;
    protected abstract Transform raycastPoint { get; }
    protected LineRenderer raycastLine;
    public Transform raycastEnd;
    protected IHoverOver lastHoverOver;
    protected IClickable lastClickable;
    protected GameObject lastClickableGO;
    private float startRaycastSize;

    public enum PlayerType { flat, vr }
    public abstract PlayerType playerType { get; }

    public float rotationSpeed = 80;
    protected float rotationValue;

    protected virtual void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
        raycastLine = GetComponentInChildren<LineRenderer>();
        timer = GetComponentInChildren<TurnTimer>();
        timer.Set(delegate { LevelFlow.OnTimerEnd(); });
        startRaycastSize = raycastLine.startWidth;
    }

    protected virtual void Start()
    {
        pauseMenuObject.SetActive(false);
        raycastLine.positionCount = 2;
        SetRaycast(true);
    }

    protected virtual void Update()
    {
        transform.Rotate(Vector3.up, rotationValue * Time.deltaTime * rotationSpeed);
        if (raycastOn)
        {
            Ray ray = new Ray(raycastPoint.position, raycastPoint.forward);
            raycastLine.SetPosition(0, raycastPoint.position);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, raycastLayer))
            {
                GameObject hitGameObject = hit.transform.gameObject;
                raycastEnd.gameObject.SetActive(true);
                raycastLine.SetPosition(1, hit.point);
                raycastEnd.position = hit.point;
                if (hitGameObject.TryGetComponent(out IHoverOver hoverOver))
                {
                    if(hoverOver != lastHoverOver)
                    {
                        if (lastHoverOver != null) lastHoverOver.OnHoverEnd();
                        lastHoverOver = hoverOver;
                        hoverOver.OnHoverStart();
                    }
                }
                else if (lastHoverOver != null)
                {
                    lastHoverOver.OnHoverEnd();
                    lastHoverOver = null;
                }
                if (hitGameObject.TryGetComponent(out IClickable clickable))
                {
                    lastClickable = clickable;
                    lastClickableGO = hitGameObject;
                }
                else
                {
                    lastClickable = null;
                    lastClickableGO = null;
                }
            }
            else
            {
                if (Physics.Raycast(ray, out hit))
                {
                    raycastEnd.gameObject.SetActive(true);
                    raycastLine.SetPosition(1, hit.point);
                    raycastEnd.position = hit.point;
                }
                else
                {
                    raycastLine.SetPosition(1, raycastPoint.position + raycastPoint.forward * 1000);
                    raycastEnd.gameObject.SetActive(false);
                }
                if (lastHoverOver != null) lastHoverOver.OnHoverEnd();
                lastHoverOver = null;
                lastClickable = null;
                lastClickableGO = null;
            }
        }
        if (lastClickable != null && !lastClickableGO.activeInHierarchy)
        {
            lastClickable = null;
            lastClickableGO = null;
        }
        raycastEnd.transform.LookAt(mainCamera.transform.position);
    }

    public virtual void SetRaycast(bool on)
    {
        raycastLine.enabled = on;
        raycastOn = on;
        raycastEnd.gameObject.SetActive(on);
        if(!on)
        {
            if (lastHoverOver != null) lastHoverOver.OnHoverEnd();
            lastHoverOver = null;
            lastClickable = null;
        }
    }

    public virtual void SetRaycastSize(float size)
    {
        raycastLine.SetWidth(startRaycastSize * size, startRaycastSize * size);
    }

    public void ShowPauseMenu(bool on)
    {
        if (on) pauseMenuObject.transform.rotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);
        pauseMenuObject.SetActive(on);
    }

    public abstract void EquipItem(Item weapon);
    public abstract void UnequipItem();
}
