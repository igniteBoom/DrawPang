using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class NestedScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar _scrollbar;
    public Scrollbar _scrollbarBG;
    public Transform _trContent;
    public ParticleSystem _psBG;
    public RectTransform[] _btnRect, _btnImageRect;
    public RectTransform _player;
    public GameObject[] _avatar;

    const int PANELSIZE = 3;
    float[] _panelScrollValue = new float[PANELSIZE];
    float _panelDistance = default;
    float _halfPanelDistance = default;
    float _currentPanelScrollValue = default;
    float _targetPanelScrollValue = 1f;
    int _targetIndex = 2;

    bool _isDrag = default;

    // Start is called before the first frame update
    void Start()
    {
        _panelDistance = 1f / (PANELSIZE - 1);
        _halfPanelDistance = _panelDistance / 2;
        for (int i = 0; i < PANELSIZE; i++)
        {
            _panelScrollValue[i] = _panelDistance * i;
#if UNITY_EDITOR || ACTIVE_LOG
            Debug.Log("_panelScrollValue[" + i + "] : " + _panelDistance * i);
#endif
        }
        SetParticle();
        SetString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _currentPanelScrollValue = SetScrollValue();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDrag = false;
        _targetPanelScrollValue = SetScrollValue();

        if (_currentPanelScrollValue == _targetPanelScrollValue)
        {
            if(eventData.delta.x > 18 && _currentPanelScrollValue - _panelDistance >= 0)
            {
                --_targetIndex;
                _targetPanelScrollValue = _currentPanelScrollValue - _panelDistance;
            }
            else if(eventData.delta.x < -18 && _currentPanelScrollValue + _panelDistance <= 1.01f)
            {
                ++_targetIndex;
                _targetPanelScrollValue = _currentPanelScrollValue + _panelDistance;
            }
        }

        SetParticle();

        for (int i = 0; i < PANELSIZE; i++)
        {
            if(_trContent.GetChild(i).GetComponent<NestedVerticalScroll>() && _currentPanelScrollValue != _panelScrollValue[i]
                && _targetPanelScrollValue == _panelScrollValue[i])
            {
                _trContent.GetChild(i).GetChild(1).GetComponent<Scrollbar>().value = 1;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        _player.localPosition = new Vector3(_trContent.localPosition.x + 1620.0f, _player.localPosition.y, _player.localPosition.z);
        if (!_isDrag)
        {
            _scrollbar.value = Mathf.Lerp(_scrollbar.value, _targetPanelScrollValue, 0.1f);
            _scrollbarBG.value = Mathf.Lerp(_scrollbar.value, _targetPanelScrollValue, 0.1f);

            for (int i = 0; i < PANELSIZE; i++)
            {
                _btnRect[i].sizeDelta = new Vector2(i == _targetIndex ? 432 : 216, _btnRect[i].sizeDelta.y);
            }
        }

        if (Time.time < 0.1f) return;

        for (int i = 0; i < PANELSIZE; i++)
        {
            Vector3 btnTargetPos = _btnRect[i].anchoredPosition3D;
            Vector3 btnTargetScale = Vector3.one;
            bool textActive = false;

            if(i == _targetIndex)
            {
                btnTargetPos.y = -8f;
                btnTargetScale = new Vector3(1.2f, 1.2f, 1);
                textActive = true;
                _btnImageRect[i].transform.GetChild(0).gameObject.SetActive(textActive);
            }

            _btnImageRect[i].anchoredPosition3D = Vector3.Lerp(_btnImageRect[i].anchoredPosition3D, btnTargetPos, 0.25f);
            _btnImageRect[i].localScale = Vector3.Lerp(_btnImageRect[i].localScale, btnTargetScale, 0.25f);            
        }
    }


    private float SetScrollValue()
    {
        for (int i = 0; i < PANELSIZE; i++)
        {
            if (_scrollbar.value < _panelScrollValue[i] + _halfPanelDistance && _scrollbar.value > _panelScrollValue[i] - _halfPanelDistance)
            {
                _targetIndex = i;
#if UNITY_EDITOR || ACTIVE_LOG
                Debug.Log("_targetIndex[" + _targetIndex + "]");
#endif
                return _panelScrollValue[i];
            }
        }
        return 0;
    }

    private void SetString()
    {
        for (int i = 0; i < PANELSIZE; i++)
        {
            _btnImageRect[i].transform.GetChild(0).gameObject.SetActive(true);
            switch (i)
            {
                case 0:
                    _btnImageRect[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = StringManager.Instance.GetString("store_title");
                    break;
                case 1:
                    _btnImageRect[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = StringManager.Instance.GetString("inven_title");
                    break;
                case 2:
                    Debug.Log(_btnImageRect[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = StringManager.Instance.GetString("game_title"));
                    break;
                default:
                    Debug.LogError("Default");
                    break;
            }
            _btnImageRect[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    private void SetParticle()
    {
        var col = _psBG.colorOverLifetime;
        col.enabled = true;
        Gradient grad = new Gradient();
        switch (_targetIndex)
        {
            case 0:
                grad.SetKeys(new GradientColorKey[] { new GradientColorKey(new Color32(157, 83, 0, 255), 0.0f), new GradientColorKey(new Color32(241, 144, 35, 255), 0.5f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 0.153f), new GradientAlphaKey(0.0f, 1.0f) });
                col.color = new ParticleSystem.MinMaxGradient(grad);
                break;
            case 1:
                grad.SetKeys(new GradientColorKey[] { new GradientColorKey(new Color32(157, 113, 0, 255), 0.0f), new GradientColorKey(new Color32(241, 183, 35, 255), 0.5f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 0.153f), new GradientAlphaKey(0.0f, 1.0f) });
                col.color = new ParticleSystem.MinMaxGradient(grad);
                break;
            case 2:
                grad.SetKeys(new GradientColorKey[] { new GradientColorKey(new Color32(4, 71, 99, 255), 0.0f), new GradientColorKey(new Color32(28, 115, 152, 255), 0.5f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 0.153f), new GradientAlphaKey(0.0f, 1.0f) });
                col.color = new ParticleSystem.MinMaxGradient(grad);
                break;
            default:
#if UNITY_EDITOR || ACTIVE_LOG
                Debug.Log("SetParticle[" + _targetIndex + "] default");
#endif
                break;
        }
    }
    public void TabClick(int n)
    {
        _targetIndex = n;
        _targetPanelScrollValue = _panelScrollValue[n];
        SetParticle();
    }

    public void OnClickPlayer()
    {
        PopupManager.Instance.CreatePopup<AvatarPopup>(PopupManager.Popup_Type.AVATAR_POPUP);
        Debug.Log("Click OnClickPlayer");
    }

    public void OnClickHead()
    {
        PopupManager.Instance.CreatePopup<HeadPopup>(PopupManager.Popup_Type.HEAD_POPUP);
        Debug.Log("Click OnClickHat");
    }

    public void OnClickChest()
    {
        PopupManager.Instance.CreatePopup<ChestPopup>(PopupManager.Popup_Type.CHEST_POPUP);
        Debug.Log("Click OnClickAcc");
    }

    public void OnClickWeapon()
    {
        PopupManager.Instance.CreatePopup<WeaponPopup>(PopupManager.Popup_Type.WEAPON_POPUP);
        Debug.Log("Click OnClickWeapon");
    }

    public void OnClickRotation()
    {
        Debug.Log("Press OnPressRotation" + _player.transform.GetChild(0).gameObject.transform.rotation.eulerAngles.y);
        if(_player.transform.GetChild(0).gameObject.activeSelf == true)
            _player.transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(0, _player.transform.GetChild(0).gameObject.transform.rotation.eulerAngles.y + 180f, 0);
        else if (_player.transform.GetChild(1).gameObject.activeSelf == true)
            _player.transform.GetChild(1).gameObject.transform.rotation = Quaternion.Euler(0, _player.transform.GetChild(1).gameObject.transform.rotation.eulerAngles.y + 180f, 0);
        else
            _player.transform.GetChild(2).gameObject.transform.rotation = Quaternion.Euler(0, _player.transform.GetChild(2).gameObject.transform.rotation.eulerAngles.y + 180f, 0);
        //_player.transform.GetChild(0).gameObject.transform.localRotation = Quaternion.Euler(0.0f, tmp_Y, 0.0f);
    }
    public void UpdateInvenPlayer()
    {
        if (_avatar[0].activeSelf) _avatar[0].GetComponent<CatCtrl>().LoadData();
        else if (_avatar[1].activeSelf) _avatar[1].GetComponent<BunnyCtrl>().LoadData();
        else _avatar[2].GetComponent<BearCtrl>().LoadData();
        //if(this.transform.GetChild(0).gameObject.activeSelf) 
    }
}
