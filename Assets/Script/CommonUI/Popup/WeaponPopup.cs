using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponPopup : PopupBase
{
    public GameObject _Scrollview;
    public TextMeshProUGUI _textExplain;

    public Toggle[] _arrWeaponSkin;
    public ArrayList _arrListWeaponSkin = new ArrayList();
    public GameObject[] _arrWeaponSkinOff;

    public Scrollbar _scrollbar;
    private bool _isCoroutine = false;

    private List<int> weaponData = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        GetweaponData();

        //Debug.Log("_arrWeaponSkin[i].onValueChanged1 : " + _arrWeaponSkin.Length);
        for (int i = 0; i < _arrWeaponSkin.Length; i++)
        {
            if (_arrWeaponSkin[i].gameObject.activeSelf)
            {
                //Debug.Log("_arrWeaponSkin save[" + i + "]");
                _arrWeaponSkin[i].onValueChanged.AddListener(changeText);
            }
        }

        //선택된 아바타 현재 위치로 이동
        ClickSelectButtonScrollView();
    }

    public void changeText(bool bOn)
    {
        for (int i = 0; i < _arrWeaponSkin.Length; i++)
        {
            if (_arrWeaponSkin[i].gameObject.activeSelf && _arrWeaponSkin[i].isOn)
            {
                _textExplain.text = StringManager.Instance.GetString(_arrWeaponSkin[i].name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void GetweaponData()
    {
        weaponData = TableManager.Instance.ListWeapon;

        /*
        for (int i = 0; i < avatarData.Count; i++)
        {
            Debug.Log("[" + i + "] avatarData : " + avatarData[i]);
        }*/

        // ListHead의 첫번째는 유저가 착용한 아이템을 가리킨다.
        //_arrWeaponSkin[weaponData[0] - 1].isOn = true;
        InitUI();
    }

    public void ClosePopup()
    {
        UpDateData();
        for (int i = 0; i < _arrWeaponSkin.Length; i++)
        {
            if (_arrWeaponSkin[i].gameObject.activeSelf)
            {
                _arrWeaponSkin[i].onValueChanged.RemoveListener(changeText);
            }
        }
        Destroy(this.gameObject);
    }
    
    public void ClickSelectButtonScrollView()
    {
        for (int i = 0; i < _arrWeaponSkin.Length; i++)
        {
            
                    if(_arrWeaponSkin[i].isOn == true)
                    {
                        float tmpValue = 1.0f / (_arrWeaponSkin.Length - 1) * i;
                        //DOTween.To(() => _scrollbar[i].value, x => _scrollbar[i].value = x, tmpValue, 0.5f);
                        if (_isCoroutine)
                        {
                            _isCoroutine = false;
                            StopCoroutine(ScrollBarValue(i, tmpValue));
                        }
                        StartCoroutine(ScrollBarValue(i, tmpValue));
                    }
        }
    }

    IEnumerator ScrollBarValue(int index, float value)
    {
        _isCoroutine = true;
        
        while (Mathf.Abs(_scrollbar.value - value) >= 0.001f)
        {
            yield return null;
            _scrollbar.value = Mathf.Lerp(_scrollbar.value, value, 0.1f);
        }
    }
    public void InitUI()
    {
        //받은 데이터로 head item 활성 비활성화
        OnOffChest();
                
        //받은 데이터로 toggle 버튼, scroll view 초기화
        for (int i = 0; i < _arrWeaponSkin.Length; i++)
        {
            if(i == weaponData[0] - 1) _arrWeaponSkin[i].isOn = true;
            else _arrWeaponSkin[i].isOn = false;
        }

        //text 초기화
        changeText(false);
    }

    private void OnOffChest()
    {
        for (int i = 1; i < weaponData.Count; i++)
        {
            if (weaponData[i] == 0)
            {
                _arrWeaponSkin[i - 1].gameObject.SetActive(false);
                _arrWeaponSkinOff[i - 1].SetActive(true);
            }
            else
            {
                _arrWeaponSkin[i - 1].gameObject.SetActive(true);
                _arrWeaponSkinOff[i - 1].SetActive(false);
            }
        }
    }

    private void UpDateData()
    {
        
        for (int i = 0; i < _arrWeaponSkin.Length; i++)
        {
            if(_arrWeaponSkin[i].isOn)
            {
                TableManager.Instance.ListWeapon[0] = i + 1;
                TableManager.Instance.UpdateWeaponDataTable();
            }
        }
    }
    //private void 
}
