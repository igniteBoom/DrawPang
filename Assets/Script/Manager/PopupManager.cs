﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    protected PopupManager() { }

    public enum Popup_Type
    {
        NONE,
        NICKNAME_POPUP,
        ABATAR_POPUP,
        ACC_POPUP,
        HAT_POPUP,
        WEAPON_POPUP,
    }

    PopupBase _getRefPopupByType(Popup_Type __createType)
    {
        switch (__createType)
        {
            case Popup_Type.NONE:               return null;
            case Popup_Type.NICKNAME_POPUP:     return _nickNamePopup;
            case Popup_Type.ABATAR_POPUP:       return _avatarPopup;
            case Popup_Type.ACC_POPUP:          return _accPopup;
            case Popup_Type.HAT_POPUP:          return _hatPopup;
            case Popup_Type.WEAPON_POPUP:       return _weaponPopup;
        }
        return null;
    }

    [SerializeField] private Transform _parentCanvas;

    public GameObject _loadingRotate;
    public PopupBase _nickNamePopup;
    public PopupBase _avatarPopup;
    public PopupBase _accPopup;
    public PopupBase _hatPopup;
    public PopupBase _weaponPopup;

    List<PopupBase> _listPopupBase = new List<PopupBase>();

    void Start()
    {
        Debug.Log("1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        Debug.Log("11");
    }

    public T CreatePopup<T>(Popup_Type __createType) where T : PopupBase
    {
        PopupBase newObj = Instantiate(_getRefPopupByType(__createType)) as PopupBase;
        newObj.transform.SetParent(_parentCanvas);
        _listPopupBase.Add(newObj);

        RectTransform rt = newObj.transform as RectTransform;
        rt.anchoredPosition = Vector3.zero;
        rt.localScale = Vector3.one;
        rt.localPosition = Vector3.zero;

        return (T)newObj;
    }

    public T GetSafePopupByType<T>(Popup_Type __type) where T : PopupBase
    {
        for(int i = 0; i < _listPopupBase.Count; i++)
        {
            if(_listPopupBase[i].popupType == __type)
            {
                T t = _listPopupBase[i].GetComponent<T>();
                return t;
            }
        }
        return null;        
    }

    public void AddLoadingRotate()
    {
        _loadingRotate.SetActive(true);
        _loadingRotate.transform.SetAsLastSibling();
    }

    public void DeleteLoadingRotate()
    {
        _loadingRotate.SetActive(false);
    }
}
