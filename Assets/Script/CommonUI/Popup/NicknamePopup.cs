using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BackEnd;

public class NicknamePopup : PopupBase
{
    public TextMeshProUGUI _textError;
    public TextMeshProUGUI _textNickName;

    string inDate = Backend.UserInDate;
    string nickName = Backend.UserNickName;

    // Start is called before the first frame update
    void Start()
    {
        if(nickName == "")
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
        Debug.Log("nickName : " + nickName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void haha()
    {
        Debug.Log("haha");
    }
    public void CheckNicknameDuplication()
    {
        Debug.Log("CheckNicknameDuplication");
        BackendReturnObject bro = Backend.BMember.CheckNicknameDuplication(_textNickName.text);
        if (bro.IsSuccess())
        {
            Debug.Log("CheckNicknameDuplication : 성공(" + _textNickName.text + ")");
            _textError.text = "해당 닉네임으로 수정 가능합니다.";
        }
        else if(bro.GetStatusCode() == "400")
        {
            Debug.Log("CheckNicknameDuplication : 공백");
            _textError.text = "닉네임에 앞/뒤 공백이 있습니다.";
        }
        else if(bro.GetStatusCode() == "409")
        {
            Debug.Log("CheckNicknameDuplication : 중복 닉네임");
            _textError.text = "이미 중복된 닉네임이 있습니다.";            
        }
        else
        {
            Debug.Log("CheckNicknameDuplication : else");
            _textError.text = "다른 닉네임을 사용해 주세요.";
        }
    }

    public void CreateNickname()
    {
        Debug.Log("CreateNickname");
        Backend.BMember.CreateNickname(_textNickName.text);
        GameObject.FindWithTag("Lobby").GetComponent<LobbySceneManager>().UpdateNickName(_textNickName.text);
        Destroy(this.gameObject);
    }
}
