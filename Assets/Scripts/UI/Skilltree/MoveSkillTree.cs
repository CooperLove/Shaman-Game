using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveSkillTree : MonoBehaviour, IDragHandler
{
    [SerializeField] private float speed = 0;
    [SerializeField] private float scrollSpeed = 0;
    [SerializeField] private float minZoom = 0;
    [SerializeField] private float maxZoom = 0;
    private Vector2 delta = new Vector2();
    private Vector3 pos = new Vector3();

    BoxCollider2D _boxCollider = null;
    RectTransform _rect = null;
    RectTransform _parent = null;
    [SerializeField] Vector2 _rectSize = new Vector2();
    [SerializeField] Vector2 _parentRectSize = new Vector2();

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform parent = transform.parent.GetComponent<RectTransform>();
        //Debug.Log($"{parent.rect} {_rectSize} {eventData.position} {transform.position} {transform.localPosition}");
        delta = eventData.delta;
        pos.x = Mathf.Clamp(transform.localPosition.x + (delta.x * speed), -_rectSize.x, _rectSize.x);
        pos.y = Mathf.Clamp(transform.localPosition.y + (delta.y * speed), -_rectSize.y, _rectSize.y);
        pos.z = Mathf.Clamp(pos.z, 0,0);
        transform.localPosition = pos;
        _boxCollider.offset = new Vector2(-pos.x/ _rect.localScale.x, -pos.y/ _rect.localScale.x);
    }

    private void OnMouseOver() {
        //Debug.Log($"Mouse hovering {gameObject.name} with {Input.mouseScrollDelta} scroll value");
        if (Input.mouseScrollDelta.y != 0){
            float zoom = Mathf.Clamp((Input.mouseScrollDelta.y * scrollSpeed) + transform.localScale.x, minZoom, maxZoom);
            transform.localScale = new Vector3 (zoom, zoom, 0);
            AdjustSizes ();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _rect = GetComponent<RectTransform>();
        _parent = transform.parent.GetComponent<RectTransform>();
        AdjustSizes ();
    }

    // Update is called once per frame
    void Update()
    {
        if (_parentRectSize.x != _parent.rect.width || _parentRectSize.y != _parent.rect.height){
            AdjustSizes ();
        }

    }

    private void AdjustSizes () {
        _boxCollider.size = new Vector2(_parent.rect.width/_rect.localScale.x, _parent.rect.height/_rect.localScale.x);
        _parentRectSize = new Vector2(_parent.rect.width, _parent.rect.height);
        _rectSize = new Vector2((_rect.rect.width * _rect.localScale.x - _parent.rect.width)/2, 
                                (_rect.rect.height * _rect.localScale.y - _parent.rect.height)/2);
    }

    private void OnEnable() {
        Debug.Log("Centering");
        transform.localPosition = Vector3.zero;
    }
}
