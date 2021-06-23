using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Facebook.Unity;

public class LoginManager : Singleton<LoginManager>
{
    protected LoginManager() { }

    public enum LoginType
    {
        None,
        Google,
        FaceBook,
        Guest
    }

    private LoginType _userLoginType = LoginType.None;
    public LoginType UserLoginType { get { return _userLoginType; }}

    string IdToken;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        //Google
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            .RequestServerAuthCode(false)
            .RequestIdToken()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        if (!Backend.Utils.GetGoogleHash().Equals(""))
            Debug.Log(Backend.Utils.GetGoogleHash());

        //Facebook
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    public void printTest()
    {
        Debug.Log("LoginManager Test");
    }

    //Google
    public void GPGSLogin()
    {
        Debug.Log("-------------GPGS-------------");
        GoogleLogin(true, false);
    }

    private void GoogleLogin(bool async, bool changeFed)
    {
        // 이미 로그인 된 경우
        if (Social.localUser.authenticated == true)
        {
            BackendAuthorize(async, changeFed);
        }
        else
        {
            Social.localUser.Authenticate((success, errorMessage) =>
            {
                if (success)
                {
                    // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입요청
                    BackendAuthorize(async, changeFed);
                    _userLoginType = LoginType.Google;
                }
                else
                {
                    // 로그인 실패
                    Debug.Log("Login failed for some reason\n" + errorMessage);
                }
            });
        }
    }

    private void BackendAuthorize(bool async, bool changeFed)
    {
        // 커스텀 -> 페더레이션 변경
        if (changeFed)
        {
            // 비동기
            if (async)
            {
                Backend.BMember.ChangeCustomToFederation(GetTokens(), FederationType.Google, isComplete =>
                {
                    Debug.Log(isComplete.ToString());
                });
            }
            // 동기
            else
            {
                BackendReturnObject BRO = Backend.BMember.ChangeCustomToFederation(GetTokens(), FederationType.Google);
                Debug.Log(BRO);
            }
        }
        // 페더레이션 인증
        else
        {
            // 비동기
            if (async)
            {
                // AuthorizeFederation 대신 AuthorizeFederationAsync 사용
                SendQueue.Enqueue(Backend.BMember.AuthorizeFederation, GetTokens(), FederationType.Google, "gpgs", callback =>
                {
                    Debug.Log(callback);
                });
            }
            // 동기
            else
            {
                BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
                Debug.Log(BRO);
            }
        }
    }

    // 구글 토큰 받아옴
    private string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // 유저 토큰 받기 첫번째 방법
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            // 두번째 방법
            // string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            Debug.Log(_IDtoken);
            return _IDtoken;
        }
        else
        {
            Debug.Log("접속되어있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            return null;
        }
    }

    //FaceBook
    // Facebook SDK 초기화
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...

            //AFacebookSignup();
            //Backend.Initialize(HandleBackendCallback);
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    // 페이스북 로그아웃
    public void FacebookLogOut()
    {
        FB.LogOut();
    }

    // 페이스북 인증
    public void FacebookSignup()
    {
        Debug.Log("-------------FacebookSignUp-------------");
        // 읽어올 권한을 설정
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, FBAuthCallbackSync);
    }

    private void FBAuthCallbackSync(ILoginResult result)
    {
        //Debug.Log(result.RawResult);
        // 로그인 성공
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;

            // Print current access token's User IDDebug.Log(aToken);
            IdToken = aToken.TokenString;
            longLog(IdToken);
            // 뒤끝 서버에 획득한 페이스북 토큰으로 가입요청
            Debug.Log(Backend.BMember.AuthorizeFederation(IdToken, FederationType.Facebook));
            Debug.Log(Backend.BMember.CheckUserInBackend(IdToken, FederationType.Facebook));
            LoadingSceneController.LoadScene("LobbyScene", true);
            _userLoginType = LoginType.FaceBook;
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }
    public static void longLog(string str)
    {
        int offset = 1000;
        Debug.Log("length: " + str.Length);

        if (str.Length > offset)
        {
            Debug.Log(str.Substring(0, offset));
            longLog(str.Substring(offset));
        }
        else
            Debug.Log(str);
    }

    public void GuestLogin()
    {
        Debug.Log("게스트 로그인 시도");
        //GameObject.FindWithTag("PopupManager").GetComponent<PopupManager>().OnOffLoading(true);
        // 게스트 로그인
        Backend.BMember.GuestLogin("게스트 로그인으로 로그인함", callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("게스트 로그인에 성공했습니다");
                //GameObject.FindWithTag("PopupManager").GetComponent<PopupManager>().OnOffLoading(false);
                LoadingSceneController.LoadScene("LobbyScene", true);
                _userLoginType = LoginType.Guest;
            }
        });
    }
}
