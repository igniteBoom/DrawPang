using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using TMPro;

public class LobbySceneManager : MonoBehaviour
{
    public TextMeshProUGUI _topNickName;
    private string _nickName = Backend.UserNickName;

    // Start is called before the first frame update
    void Start()
    {
        //PopupManager.Instance.CreatePopup<NicknamePopup>(PopupManager.Popup_Type.NICKNAME_POPUP);
        CheckNickName();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            PopupManager.Instance.CreatePopup<NicknamePopup>(PopupManager.Popup_Type.NICKNAME_POPUP);
            //_popupManager.OnOffLoading(bLoading);
        }
    }

    public void OnValueChanged()
    {
        Debug.Log("OnValueChanged");
    }

    public void OnEndEdit()
    {
        Debug.Log("OnEndEdit");
    }

    public void OnSelect()
    {
        Debug.Log("OnSelect");
    }

    public void OnDeslect()
    {
        Debug.Log("OnDeslect");
    }

    public void CheckNickName()
    {
        if (_nickName == "")
        {
            PopupManager.Instance.CreatePopup<NicknamePopup>(PopupManager.Popup_Type.NICKNAME_POPUP);
        }
        else
        {
            _topNickName.text = _nickName;
        }
    }

    public void UpdateNickName(string nickName)
    {
        _topNickName.text = nickName;
    }
}
