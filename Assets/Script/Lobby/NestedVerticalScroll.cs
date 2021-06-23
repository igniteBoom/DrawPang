using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NestedVerticalScroll : ScrollRect
{
    bool _isParentDrag = default;
    NestedScrollManager _nestedScrollManager = default;
    ScrollRect _parentScrollRect = default;

    protected override void Start()
    {
        _nestedScrollManager = GameObject.FindWithTag("NestedScrollManager").GetComponent<NestedScrollManager>();
        _parentScrollRect = GameObject.FindWithTag("NestedScrollManager").GetComponent<ScrollRect>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        // 마우스의 움직임이 x축이 큰지 y축이 큰지 비교
        _isParentDrag = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);

        if(_isParentDrag)
        {
            _nestedScrollManager.OnBeginDrag(eventData);
            _parentScrollRect.OnBeginDrag(eventData);
        }
        else base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (_isParentDrag)
        {
            _nestedScrollManager.OnDrag(eventData);
            _parentScrollRect.OnDrag(eventData);
        }
        else base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (_isParentDrag)
        {
            _nestedScrollManager.OnEndDrag(eventData);
            _parentScrollRect.OnEndDrag(eventData);
        }
        else base.OnEndDrag(eventData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
