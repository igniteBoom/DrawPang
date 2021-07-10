using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyCtrl : MonoBehaviour
{
    public Material[] _skin;
    public Material[] _face;
    public GameObject[] _acc;
    public GameObject[] _head;
    public GameObject[] _weapon;

    public Renderer _skinRend;
    public Renderer _faceRend;

    // Start is called before the first frame update
    void Start()
    {
        _skinRend.material = _skin[4];
        _faceRend.material = _face[4];
        SetItem(_acc, "BackpackA");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetItem(GameObject[] arrObject, string objName)
    {
        for (int i = 0; i < arrObject.Length; i++)
        {
            if (arrObject[i].name == objName) arrObject[i].SetActive(true);
            else arrObject[i].SetActive(false);
        }
    }
}
