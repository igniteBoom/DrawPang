using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class AvatarPopup : PopupBase
{
    public Toggle[] _arrAvatar;
    public GameObject[] _arrAvatarOff;

    public GameObject[] _arrScrollview;
    public TextMeshProUGUI _textExplain;

    public Toggle[] _arrAvatarSkin;
    public ArrayList _arrListAvatarSkin = new ArrayList();
    public GameObject[] _arrAvatarSkinOff;

    public Scrollbar[] _scrollbar;
    private bool _isCoroutine = false;

    private List<int> avatarData = new List<int>();
    private List<int> avatarSkinData = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        GetAvatarData();

        //Debug.Log("_arrAvatarSkin[i].onValueChanged1 : " + _arrAvatarSkin.Length);
        for (int i = 0; i < _arrAvatarSkin.Length; i++)
        {
            if (_arrAvatarSkin[i].gameObject.activeSelf)
            {
                //Debug.Log("_arrAvatarSkin save[" + i + "]");
                _arrAvatarSkin[i].onValueChanged.AddListener(changeText);
            }
        }

        //선택된 아바타 현재 위치로 이동
        ClickSelectButtonScrollView();
    }

    public void changeText(bool bOn)
    {
        for (int i = 0; i < _arrAvatar.Length; i++)
        {
            if (_arrAvatar[i].isOn)
            {
                //cat[0], bunny[1], cat[2] 은 따로 배열 세개로 관리하지만
                //각각 15개씩 있는 스킨은 45개로 한꺼번에 받아서 처리
                int tmpSkinEndIndex = i * 15 + 15;
                for (int j = i * 15; j < tmpSkinEndIndex; j++)
                {
                    if (_arrAvatarSkin[j].gameObject.activeSelf && _arrAvatarSkin[j].isOn)
                    {
                        _textExplain.text = StringManager.Instance.GetString(_arrAvatarSkin[j].name);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void GetAvatarData()
    {
        avatarData = TableManager.Instance.ListAvatar;
        avatarSkinData = TableManager.Instance.ListSkin;

        /*
        for (int i = 0; i < avatarData.Count; i++)
        {
            Debug.Log("[" + i + "] avatarData : " + avatarData[i]);
        }*/
        //_arrAvatar[avatarData[0] - 1].isOn = true;
        InitUI();
    }

    public void ClosePopup()
    {
        UpDateData();
        for (int i = 0; i < _arrAvatarSkin.Length; i++)
        {
            if (_arrAvatarSkin[i].gameObject.activeSelf)
            {
                _arrAvatarSkin[i].onValueChanged.RemoveListener(changeText);
            }
        }
        Destroy(this.gameObject);
    }

    public void ClickCat()
    {
        if(_arrAvatar[0].isOn)
        {
            for (int i = 0; i < _arrScrollview.Length; i++)
            {
                if (i == 0)
                {
                    _arrScrollview[i].SetActive(true);
                }
                else
                {
                    _arrScrollview[i].SetActive(false);
                }
            }

            //text update
            changeText(false);
            //선택된 아바타 현재 위치로 이동
            ClickSelectButtonScrollView();
        }
    }
    public void ClickBunny()
    {
        if (_arrAvatar[1].isOn)
        {
            for(int i = 0; i < _arrScrollview.Length; i++)
            {
                if(i == 1)
                {
                    _arrScrollview[i].SetActive(true);
                }
                else
                {
                    _arrScrollview[i].SetActive(false);
                }
            }

            //text update
            changeText(false);
            //선택된 아바타 현재 위치로 이동
            ClickSelectButtonScrollView();
        }
    }
    public void ClickBear()
    {
        if (_arrAvatar[2].isOn)
        {
            for (int i = 0; i < _arrScrollview.Length; i++)
            {
                if (i == 2)
                {
                    _arrScrollview[i].SetActive(true);
                }
                else
                {
                    _arrScrollview[i].SetActive(false);
                }
            }

            //text update
            changeText(false);
            //선택된 아바타 현재 위치로 이동
            ClickSelectButtonScrollView();
        }
    }
    public void ClickSelectButtonScrollView()
    {
        Debug.Log("실행되나?");
        for (int i = 0; i < _arrAvatar.Length; i++)
        {
            if (_arrAvatar[i].isOn == true)
            {
                //cat[0], bunny[1], cat[2] 은 따로 배열 세개로 관리하지만
                //각각 15개씩 있는 스킨은 45개로 한꺼번에 받아서 처리
                int tmpSkinEndIndex = i * 15 + 15;
                for (int j = i * 15; j < tmpSkinEndIndex; j++)
                {
                    if(_arrAvatarSkin[j].isOn == true)
                    {
                        float tmpValue = 1.0f / 14.0f * (float)(j - i * 15);
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
        }
    }

    IEnumerator ScrollBarValue(int index, float value)
    {
        _isCoroutine = true;
        while (Mathf.Abs(_scrollbar[index].value - value) >= 0.001f)
        {
            yield return null;
            _scrollbar[index].value = Mathf.Lerp(_scrollbar[index].value, value, 0.1f);
        }
    }
    public void InitUI()
    {
        //받은 데이터로 avatar 활성 비활성화
        OnOffAvatar();

        //받은 데이터로 avatarSkin 활성 비활성화
        OnOffAvatarSkin();

        //받은 데이터로 avatar toggle 버튼, scroll view 초기화
        for (int i = 0; i < _arrAvatar.Length; i++)
        {
            if(i == avatarData[0] - 1) _arrAvatar[i].isOn = true;
            else _arrAvatar[i].isOn = false;

            if (_arrAvatar[i].isOn == true)
            {
                for (int j = 0; j < _arrScrollview.Length; j++)
                {
                    if (i == j)
                    {
                        _arrScrollview[j].SetActive(true);
                    }
                    else
                    {
                        _arrScrollview[j].SetActive(false);
                    }
                }
            }
        }

        for (int i = 0; i < _arrAvatarSkin.Length; i++)
        {
            if (i == avatarSkinData[0] - 1) _arrAvatarSkin[i].isOn = true;
            else _arrAvatarSkin[i].isOn = false;
        }

        //text 초기화
        changeText(false);
    }
    private void OnOffAvatar()
    {
        for (int i = 1; i < avatarData.Count; i++)
        {
            if(avatarData[i] == 0)
            {
                _arrAvatar[i - 1].gameObject.SetActive(false);
                _arrAvatarOff[i - 1].SetActive(true);
            }
            else
            {
                _arrAvatar[i - 1].gameObject.SetActive(true);
                _arrAvatarOff[i - 1].SetActive(false);
            }
        }
    }
    private void OnOffAvatarSkin()
    {
        for (int i = 1; i < avatarSkinData.Count; i++)
        {
            if (avatarSkinData[i] == 0)
            {
                _arrAvatarSkin[i - 1].gameObject.SetActive(false);
                _arrAvatarSkinOff[i - 1].SetActive(true);
            }
            else
            {
                _arrAvatarSkin[i - 1].gameObject.SetActive(true);
                _arrAvatarSkinOff[i - 1].SetActive(false);
            }
        }
    }

    private void UpDateData()
    {
        int tmpScrollViewIndex = 0;
        int tmpSkinStartIndex = 0;

        for (int i = 0; i < _arrAvatar.Length; i++)
        {
            if(_arrAvatar[i].isOn)
            {
                TableManager.Instance.ListAvatar[0] = i + 1;
                TableManager.Instance.UpdateAvatarDataTable();
            }
        }
        for (int i = 0; i < _arrScrollview.Length; i++)
        {
            if (_arrScrollview[i].activeSelf)
            {
                tmpScrollViewIndex = i;
                tmpSkinStartIndex = i * 15; // 각각 15개의 스킨 데이터
            }
        }

        for(int i = tmpSkinStartIndex; i < tmpSkinStartIndex + 15; i++)
        {
            if (_arrAvatarSkin[i].isOn)
            {
                TableManager.Instance.ListSkin[0] = i + 1;
                TableManager.Instance.UpdateSkinDataTable();
            }
        }
    }
    //private void 
}
