using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class TitleSceneController : MonoBehaviour
{
    public Button _googleLoginButton;
    public Button _facebookLoginButton;
    public Button _guestLoginButton;

    bool bLoading = false;

    // Start is called before the first frame update
    void Start()
    {
        BackEndManager.Instance.Init();

        _googleLoginButton.onClick.AddListener(delegate { LoginManager.Instance.GPGSLogin(); });
        _facebookLoginButton.onClick.AddListener(delegate { LoginManager.Instance.FacebookSignup(); });
        _guestLoginButton.onClick.AddListener(delegate { LoginManager.Instance.GuestLogin(); });
    }

    // Update is called once per frame
    void Update()
    {
        Backend.AsyncPoll();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadingSceneController.LoadScene("LobbyScene");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            bLoading = !bLoading;
            //_popupManager.OnOffLoading(bLoading);
        }
    }
}
