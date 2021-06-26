using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AvatarPopup : PopupBase
{
    public Toggle[] _arrAvatar;
    public GameObject[] _arrScrollview;
    public TextMeshProUGUI _textExplain;

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
