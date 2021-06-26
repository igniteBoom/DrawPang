using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class AvatarPopup : PopupBase
{
    public Toggle[] _arrAvatar;
    public GameObject[] _arrScrollview;
    public TextMeshProUGUI _textExplain;

    public Toggle[] _arrAvatarSkin;
    public Scrollbar[] _scrollbar;
    private bool _isCoroutine = false;
    // Start is called before the first frame update
    void Start()
    {
        GetAvatarData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init()
    {
        GetAvatarData();
    }
    private void GetAvatarData()
    {
        List<int> avatarData = new List<int>();
        avatarData = TableManager.Instance.ListAvatar;

        //_arrAvatar[avatarData[0] - 1].isOn = true;
        UpDateSkinWindow();
    }

    public void ClosePopup()
    {
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
        }
    }
    public void ClickSelectButtonScrollView()
    {
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
    public void UpDateSkinWindow()
    {
        for(int i = 0; i < _arrAvatar.Length; i++)
        {
            if(_arrAvatar[i].isOn == true)
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
    }
    //private void 
}
