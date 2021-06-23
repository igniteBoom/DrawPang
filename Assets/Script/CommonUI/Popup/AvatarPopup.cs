using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarPopup : PopupBase
{
    public Toggle[] _arrAvatar;

    // Start is called before the first frame update
    void Start()
    {
        GetAvatarData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void GetAvatarData()
    {
        List<int> avatarData = new List<int>();
        avatarData = TableManager.Instance.ListAvatar;

        _arrAvatar[avatarData[0] - 1].isOn = true;
    }

    public void ClosePopup()
    {
        Destroy(this.gameObject);
    }

    public void ClickCat()
    {
        if(_arrAvatar[0].isOn)
        {

        }
    }
    public void ClickBunny()
    {
        if (_arrAvatar[0].isOn)
        {

        }
    }
    public void ClickBear()
    {
        if (_arrAvatar[0].isOn)
        {

        }
    }

    //private void 
}
