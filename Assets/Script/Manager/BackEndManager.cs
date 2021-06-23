using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackEndManager : Singleton<BackEndManager>
{
    protected BackEndManager () { }
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
        Debug.Log("초기화");
        // 세 번째 방법 (비동기)
        Backend.InitializeAsync(true, callback => {
            if (callback.IsSuccess())
            {
                // 초기화 성공 시 로직
                LoginManager.Instance.Init();
            }
            else
            {
                // 초기화 실패 시 로직
            }
        });
    }
}
