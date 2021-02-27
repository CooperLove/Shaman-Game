using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Tooltip : MonoBehaviour
{
    public enum Mode {ScreenToViewport, ScreenToWorld, WorldToViewPort, ViewportToWorld, WorldToScreen, ViewportToScreen}
    public enum Position {Position, LocalPosition, AnchoredPosition}
    [SerializeField] protected RectTransform rectTransform = null;
    [SerializeField] protected TMP_Text descText = null;
    [SerializeField] protected float yPadding = 0;
    [SerializeField] private RectTransform uiCanvas = null;
    public float xOffset = 30;
    public float yOffset = 30;
    protected Camera mainCamera = null;
    public Position position;
    public Mode mode;
    private void Awake() {
        mainCamera = Camera.main;
        //Hide();
    }

    protected void FollowMouse (){
        if (!Inventory.Instance.gameObject.activeInHierarchy && !SkillTree.Instance.gameObject.activeInHierarchy &&
            !(this is SkillTooltip))
            Hide();
            
        var v = mainCamera.ScreenToViewportPoint(Input.mousePosition);

        var uiCanvasRect = uiCanvas.rect;
        
        v = new Vector3(uiCanvasRect.width * v.x, uiCanvasRect.height * v.y, 0);
        v.y += yPadding;
        v.z = Mathf.Clamp(v.z, 0, 0);

        var rect = rectTransform.rect;
        
        var xLimit = uiCanvasRect.width - rect.width;
        var yLimit = uiCanvasRect.height - rect.height;
        
        v.x = Mathf.Clamp(v.x, 0, xLimit);
        v.y = Mathf.Clamp(v.y, 0, yLimit);
        
        rectTransform.anchoredPosition = v;
    }


    public abstract void Show (string s);

    public abstract void Hide ();
    public abstract void Resize (string s);

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        var cam = Camera.main;
        if (!cam) return;
        {
            Gizmos.DrawLine(cam.ViewportToWorldPoint(new Vector2(-1, 1)),
                cam.ViewportToWorldPoint(new Vector2(1, 1)));
            Gizmos.DrawLine(cam.ViewportToWorldPoint(new Vector2(-1, -1)),
                cam.ViewportToWorldPoint(new Vector2(1, -1)));
            Gizmos.DrawLine(cam.ViewportToWorldPoint(new Vector2(-1, -1)),
                cam.ViewportToWorldPoint(new Vector2(-1, 1)));
            Gizmos.DrawLine(cam.ViewportToWorldPoint(new Vector2(1, -1)),
                cam.ViewportToWorldPoint(new Vector2(1, 1)));
        }
    }

    public void SetSize (int x, int y){
        Debug.Log($"Size {x} {y}");
        xOffset = x;
        yOffset = y;
    }
}
