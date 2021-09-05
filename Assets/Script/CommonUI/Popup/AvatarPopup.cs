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
    public TextMeshProUGUI _textAvatarExplain;
    public TextMeshProUGUI _textFaceExplain;

    public Toggle[] _arrAvatarSkin;
    public ArrayList _arrListAvatarSkin = new ArrayList();
    public GameObject[] _arrAvatarSkinOff;

    public Toggle[] _arrFaceSkin;
    public ArrayList _arrListFaceSkin = new ArrayList();
    public GameObject[] _arrFaceSkinOff;

    public Scrollbar[] _scrollbar;
    private bool _isCoroutine = false;

    private List<int> avatarData = new List<int>();
    private List<int> avatarSkinData = new List<int>();
    private List<int> faceSkinData = new List<int>();
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
                _arrAvatarSkin[i].onValueChanged.AddListener(changeAvatarText);
            }
        }

        for (int i = 0; i < _arrFaceSkin.Length; i++)
        {
            if (_arrFaceSkin[i].gameObject.activeSelf)
            {
                //Debug.Log("_arrAvatarSkin save[" + i + "]");
                _arrFaceSkin[i].onValueChanged.AddListener(changeFaceText);
            }
        }

        //선택된 아바타 현재 위치로 이동
        ClickSelectButtonAvatarScrollView();

        //선택된 표졍 현재 위치로 이동
        ClickSelectButtonFaceScrollView();
    }

    public void changeAvatarText(bool bOn)
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
                        _textAvatarExplain.text = StringManager.Instance.GetString(_arrAvatarSkin[j].name);
                    }
                }
            }
        }
    }

    public void changeFaceText(bool bOn)
    {
        for (int i = 0; i < _arrFaceSkin.Length; i++)
        {
            if (_arrFaceSkin[i].gameObject.activeSelf && _arrFaceSkin[i].isOn)
            {
                _textFaceExplain.text = StringManager.Instance.GetString(_arrFaceSkin[i].name);
            }
        }
    }
    void Update()
    // Update is called once per frame
    {
        
    }
    private void GetAvatarData()
    {
        avatarData = TableManager.Instance.ListAvatar;
        avatarSkinData = TableManager.Instance.ListSkin;
        faceSkinData = TableManager.Instance.ListFace;
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
                _arrAvatarSkin[i].onValueChanged.RemoveListener(changeAvatarText);
            }
        }
        for (int i = 0; i < _arrFaceSkin.Length; i++)
        {
            if (_arrFaceSkin[i].gameObject.activeSelf)
            {
                _arrFaceSkin[i].onValueChanged.RemoveListener(changeFaceText);
            }
        }
        UpdateInvenPlayer();
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
            InitSkinToggle(0);
            //text update
            changeAvatarText(false);
            //선택된 아바타 현재 위치로 이동
            ClickSelectButtonAvatarScrollView();
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
            InitSkinToggle(1);
            //text update
            changeAvatarText(false);
            //선택된 아바타 현재 위치로 이동
            ClickSelectButtonAvatarScrollView();
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
            InitSkinToggle(2);
            //text update
            changeAvatarText(false);
            //선택된 아바타 현재 위치로 이동
            ClickSelectButtonAvatarScrollView();
        }
    }
    public void ClickSelectButtonAvatarScrollView()
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
                    if (_arrAvatarSkin[j].isOn == true)
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

    public void ClickSelectButtonFaceScrollView()
    {
        for (int i = 0; i < _arrFaceSkin.Length; i++)
        {
            if (_arrFaceSkin[i].isOn == true)
            {
                float tmpValue = 1.0f / (float)(_arrFaceSkin.Length - 1) * i;
                //DOTween.To(() => _scrollbar[i].value, x => _scrollbar[i].value = x, tmpValue, 0.5f);
                if (_isCoroutine)
                {
                    _isCoroutine = false;
                    StopCoroutine(ScrollBarValue(3, tmpValue)); // _scrollbar[3]은 face scollbar value 이다.
                }
                StartCoroutine(ScrollBarValue(3, tmpValue));
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

        //받은 데이터로 faceSkin 활성 비활성화
        OnOffFaceSkin();

        //받은 데이터로 avatar toggle 버튼, scroll view 초기화
        UpdateAvatarToggle();

        //받은 데이터로 face toggle 버튼 활성/비활성화
        UpdateFaceSkinToggle();
        //text 초기화
        changeAvatarText(false);
    }
    private void UpdateAvatarToggle()
    {
        for (int i = 0; i < _arrAvatar.Length; i++)
        {
            if (i == avatarData[0] - 1) _arrAvatar[i].isOn = true;
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

        UpdateAvatarSkinToggle();
    }

    private void UpdateAvatarSkinToggle()
    {        
        for (int i = 0; i < _arrAvatarSkin.Length; i++)
        {
            if (i == avatarSkinData[0] - 1) _arrAvatarSkin[i].isOn = true;
            else _arrAvatarSkin[i].isOn = false;
        }
    }
    private void UpdateFaceSkinToggle()
    {
        for (int i = 0; i < _arrFaceSkin.Length; i++)
        {
            if (i == faceSkinData[0] - 1) _arrFaceSkin[i].isOn = true;
            else _arrFaceSkin[i].isOn = false;
        }
    }
    private void InitSkinToggle(int i)
    {
        int j = i * 15;
        _arrAvatarSkin[j].gameObject.SetActive(true);
        _arrAvatarSkin[j].isOn = true; 
        _arrAvatarSkinOff[j].SetActive(false);
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
    private void OnOffFaceSkin()
    {
        for (int i = 1; i < faceSkinData.Count; i++)
        {
            if (faceSkinData[i] == 0)
            {
                _arrFaceSkin[i - 1].gameObject.SetActive(false);
                _arrFaceSkinOff[i - 1].SetActive(true);
            }
            else
            {
                _arrFaceSkin[i - 1].gameObject.SetActive(true);
                _arrFaceSkinOff[i - 1].SetActive(false);
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

        for(int i = 0; i < _arrFaceSkin.Length; i++)
        {
            if(_arrFaceSkin[i].isOn)
            {
                TableManager.Instance.ListFace[0] = i + 1;
                TableManager.Instance.UpdateFaceDataTable();
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
