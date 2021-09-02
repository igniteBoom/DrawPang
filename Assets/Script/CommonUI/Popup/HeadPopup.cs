using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HeadPopup : PopupBase
{
    public GameObject _Scrollview;
    public TextMeshProUGUI _textExplain;

    public Toggle[] _arrHeadSkin;
    public ArrayList _arrListHeadSkin = new ArrayList();
    public GameObject[] _arrHeadSkinOff;

    public Scrollbar _scrollbar;
    private bool _isCoroutine = false;

    private List<int> headData = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        GetHeadData();

        //Debug.Log("_arrHeadSkin[i].onValueChanged1 : " + _arrHeadSkin.Length);
        for (int i = 0; i < _arrHeadSkin.Length; i++)
        {
            if (_arrHeadSkin[i].gameObject.activeSelf)
            {
                //Debug.Log("_arrHeadSkin save[" + i + "]");
                _arrHeadSkin[i].onValueChanged.AddListener(changeText);
            }
        }

        //선택된 아바타 현재 위치로 이동
        ClickSelectButtonScrollView();
    }

    public void changeText(bool bOn)
    {
        for (int i = 0; i < _arrHeadSkin.Length; i++)
        {
            if (_arrHeadSkin[i].gameObject.activeSelf && _arrHeadSkin[i].isOn)
            {
                _textExplain.text = StringManager.Instance.GetString(_arrHeadSkin[i].name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void GetHeadData()
    {
        headData = TableManager.Instance.ListHead;

        /*
        for (int i = 0; i < avatarData.Count; i++)
        {
            Debug.Log("[" + i + "] avatarData : " + avatarData[i]);
        }*/

        // ListHead의 첫번째는 유저가 착용한 아이템을 가리킨다.
        //_arrHeadSkin[headData[0] - 1].isOn = true;
        InitUI();
    }

    public void ClosePopup()
    {
        UpDateData();
        for (int i = 0; i < _arrHeadSkin.Length; i++)
        {
            if (_arrHeadSkin[i].gameObject.activeSelf)
            {
                _arrHeadSkin[i].onValueChanged.RemoveListener(changeText);
            }
        }
        UpdateInvenPlayer();
        Destroy(this.gameObject);
    }
    
    public void ClickSelectButtonScrollView()
    {
        for (int i = 0; i < _arrHeadSkin.Length; i++)
        {
            
                    if(_arrHeadSkin[i].isOn == true)
                    {
                        float tmpValue = 1.0f / (_arrHeadSkin.Length - 1) * i;
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
        OnOffHead();
                
        //받은 데이터로 toggle 버튼, scroll view 초기화
        for (int i = 0; i < _arrHeadSkin.Length; i++)
        {
            if(i == headData[0] - 1) _arrHeadSkin[i].isOn = true;
            else _arrHeadSkin[i].isOn = false;
        }

        //text 초기화
        changeText(false);
    }

    private void OnOffHead()
    {
        for (int i = 1; i < headData.Count; i++)
        {
            if (headData[i] == 0)
            {
                _arrHeadSkin[i - 1].gameObject.SetActive(false);
                _arrHeadSkinOff[i - 1].SetActive(true);
            }
            else
            {
                _arrHeadSkin[i - 1].gameObject.SetActive(true);
                _arrHeadSkinOff[i - 1].SetActive(false);
            }
        }
    }

    private void UpDateData()
    {
        //변경된 데이터 서버 저장
        for (int i = 0; i < _arrHeadSkin.Length; i++)
        {
            if(_arrHeadSkin[i].isOn)
            {
                TableManager.Instance.ListHead[0] = i + 1;
                TableManager.Instance.UpdateHeadDataTable();
            }
        }         
    }
    public void UpdateInvenPlayer()
    {
        GameObject tmpNestedScrollManager;
        tmpNestedScrollManager = GameObject.FindGameObjectWithTag("NestedScrollManager");
        tmpNestedScrollManager.GetComponent<NestedScrollManager>().UpdateInvenPlayer();
    }
}
