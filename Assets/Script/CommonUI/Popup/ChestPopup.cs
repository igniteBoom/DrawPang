using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestPopup : PopupBase
{
    public GameObject _Scrollview;
    public TextMeshProUGUI _textExplain;

    public Toggle[] _arrChestSkin;
    public ArrayList _arrListChestSkin = new ArrayList();
    public GameObject[] _arrChestSkinOff;

    public Scrollbar _scrollbar;
    private bool _isCoroutine = false;

    private List<int> chestData = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        GetChestData();

        //Debug.Log("_arrChestSkin[i].onValueChanged1 : " + _arrChestSkin.Length);
        for (int i = 0; i < _arrChestSkin.Length; i++)
        {
            if (_arrChestSkin[i].gameObject.activeSelf)
            {
                //Debug.Log("_arrChestSkin save[" + i + "]");
                _arrChestSkin[i].onValueChanged.AddListener(changeText);
            }
        }

        //선택된 아바타 현재 위치로 이동
        ClickSelectButtonScrollView();
    }

    public void changeText(bool bOn)
    {
        for (int i = 0; i < _arrChestSkin.Length; i++)
        {
            if (_arrChestSkin[i].gameObject.activeSelf && _arrChestSkin[i].isOn)
            {
                _textExplain.text = StringManager.Instance.GetString(_arrChestSkin[i].name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void GetChestData()
    {
        chestData = TableManager.Instance.ListChest;

        /*
        for (int i = 0; i < avatarData.Count; i++)
        {
            Debug.Log("[" + i + "] avatarData : " + avatarData[i]);
        }*/

        // ListHead의 첫번째는 유저가 착용한 아이템을 가리킨다.
        //_arrChestSkin[chestData[0] - 1].isOn = true;
        InitUI();
    }

    public void ClosePopup()
    {
        UpDateData();
        for (int i = 0; i < _arrChestSkin.Length; i++)
        {
            if (_arrChestSkin[i].gameObject.activeSelf)
            {
                _arrChestSkin[i].onValueChanged.RemoveListener(changeText);
            }
        }
        Destroy(this.gameObject);
    }
    
    public void ClickSelectButtonScrollView()
    {
        for (int i = 0; i < _arrChestSkin.Length; i++)
        {
            
                    if(_arrChestSkin[i].isOn == true)
                    {
                        float tmpValue = 1.0f / 12.0f * i;
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
        for (int i = 0; i < _arrChestSkin.Length; i++)
        {
            if(i == chestData[0] - 1) _arrChestSkin[i].isOn = true;
            else _arrChestSkin[i].isOn = false;
        }

        //text 초기화
        changeText(false);
    }

    private void OnOffChest()
    {
        for (int i = 1; i < chestData.Count; i++)
        {
            if (chestData[i] == 0)
            {
                _arrChestSkin[i - 1].gameObject.SetActive(false);
                _arrChestSkinOff[i - 1].SetActive(true);
            }
            else
            {
                _arrChestSkin[i - 1].gameObject.SetActive(true);
                _arrChestSkinOff[i - 1].SetActive(false);
            }
        }
    }

    private void UpDateData()
    {
        
        for (int i = 0; i < _arrChestSkin.Length; i++)
        {
            if(_arrChestSkin[i].isOn)
            {
                TableManager.Instance.ListChest[0] = i + 1;
                TableManager.Instance.UpdateChestDataTable();
            }
        }
    }
    //private void 
}
