using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// protected classname() {} 선언해서 비 싱글톤 생성자 사용을 방지해야 함.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool _isAppClose = false;
    private static object _lock = new object();
    private static T _instance;

    public static T Instance
    {
        get
        {
            if(_isAppClose)
            {
#if UNITY_EDITOR || ACTIVE_LOG
                Debug.Log("[Singleton] Instance : " + typeof(T) + " already destroyed. Returning null.");
#endif
                return null;
            }

            lock (_lock)
            {
                if(_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if(_instance == null)
                    {
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        DontDestroyOnLoad(singletonObject);
                    }
                }
                else
                {
#if UNITY_EDITOR || ACTIVE_LOG
                    Debug.Log("[Singleton] Instance : " + _instance.name + " already exist.");
#endif
                }
                return _instance;
            }
        }
    }

    private void OnApplicationQuit()
    {
        _isAppClose = true;
    }

    private void OnDestroy()
    {
        _isAppClose = true;
    }
}
