using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearCtrl : MonoBehaviour
{
    public Material[] _skin;
    public Material[] _face;
    public GameObject[] _chest;
    public GameObject[] _head;
    public GameObject[] _weapon;

    public Renderer _skinRend;
    public Renderer _faceRend;

    private List<int> avatarData = new List<int>();
    private List<int> avatarSkinData = new List<int>();
    private List<int> faceData = new List<int>();
    private List<int> headData = new List<int>();
    private List<int> chestData = new List<int>();
    private List<int> weaponData = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        //_skinRend.material = _skin[3];
        //_faceRend.material = _face[3];
        //SetItem(_chest, "BackpackA");

        LoadData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetItem(GameObject[] arrObject, List<int> listObject)
    {
        for (int i = 0; i < arrObject.Length; i++)
        {
            if (i == listObject[0] - 1) arrObject[i].SetActive(true);
            else arrObject[i].SetActive(false);
        }
    }

    public void LoadData()
    {
        avatarData = TableManager.Instance.ListAvatar;
        OnOffInvenAvatar(avatarData[0]);
        if (this.gameObject.activeSelf == true)
        {
            avatarSkinData = TableManager.Instance.ListSkin;
            faceData = TableManager.Instance.ListFace;
            headData = TableManager.Instance.ListHead;
            chestData = TableManager.Instance.ListChest;
            weaponData = TableManager.Instance.ListWeapon;
            SetData();
        }
    }

    public void OnOffInvenAvatar(int index)
    {
        for (int i = 0; i < avatarData.Count - 1; i++)
        {
            Debug.Log("index : " + index + ", i : " + i);
            //[0]번 배열은 유저가 선택한 아바타 data는 1번부터 cat, hierarchy는 0번부터 cat
            if (index == i + 1) this.gameObject.transform.parent.gameObject.transform.GetChild(i).gameObject.SetActive(true);
            else this.gameObject.transform.parent.gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void SetData()
    {
        int tmpSkinIndex = (avatarData[0] - 1) == 0 ? avatarSkinData[0] - 1 : (avatarSkinData[0] - ((avatarData[0] - 1) * 15)) - 1;
        int tmpFaceIndex = faceData[0] - 1;
        _skinRend.material = _skin[tmpSkinIndex];
        _faceRend.material = _face[tmpFaceIndex];

        SetItem(_head, headData);
        SetItem(_chest, chestData);
        SetItem(_weapon, weaponData);
    }
}
