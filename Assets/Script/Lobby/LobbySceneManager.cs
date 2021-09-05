using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using TMPro;

public class LobbySceneManager : MonoBehaviour
{
    public TextMeshProUGUI _topNickName;
    public TextMeshProUGUI _topCoin;
    private string _nickName = Backend.UserNickName;

    private int _dpCoin;
    // Start is called before the first frame update
    void Start()
    {
        //PopupManager.Instance.CreatePopup<NicknamePopup>(PopupManager.Popup_Type.NICKNAME_POPUP);
        CheckNickName();
        InitCoin();
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
    private void InitCoin()
    {
        _dpCoin = TableManager.Instance.DpCoin;
        _topCoin.text = string.Format("{0:#,##0}", _dpCoin); //_dpCoin.ToString();
    }
    public void ChangeNickName(string nickName)
    {
        _topNickName.text = nickName;
    }
    public void UpdateCoin(int coin)
    {
        TableManager.Instance.DpCoin = coin;
        TableManager.Instance.UpdateCoinDataTable();
        ChangeCoin(coin);
    }
    public void ChangeCoin(int coin)
    {
        _topCoin.text = string.Format("{0:#,##0}", coin);// coin.ToString();
    }
}
