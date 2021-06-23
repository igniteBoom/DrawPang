using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;

public class TableManager : Singleton<TableManager>
{
    protected TableManager() { }

    // JSON 크기 https://www.debugbear.com/json-size-analyzer
    // 요금 https://developer.thebackend.io/outline/manual/dbguide/dbUse/
    // JSON 확인 Debug.Log(BRO.GetReturnValue());

    private bool _isSuccessInit = false;
    public bool isSuccessInit { get { return _isSuccessInit; } }

    private List<int> _listAvatar = new List<int>();
    public List<int> ListAvatar { get { return _listAvatar; } set { _listAvatar = value; } }

    private List<int> _listSkin = new List<int>();
    public List<int> ListSkin { get { return _listSkin; } set { _listSkin = value; } }

    private List<int> _listFace = new List<int>();
    public List<int> ListFace { get { return _listFace; } set { _listFace = value; } }

    private List<int> _listHat = new List<int>();
    public List<int> ListHat { get { return _listHat; } set { _listHat = value; } }

    private List<int> _listAcc = new List<int>();
    public List<int> ListAcc { get { return _listAcc; } set { _listAcc = value; } }

    private List<int> _listWeapon = new List<int>();
    public List<int> ListWeapon { get { return _listWeapon; } set { _listWeapon = value; } }

    //1.초기화
    //2.저장하기
    //3.불러오기

    //update 방법
    //1.List get set으로 list를 갱신
    //2.해당 update 함수 호출
    public void Init()
    {
        //1. 해당 테이블이 있는가?
        //2. 테이블이 없으면 새로 생성
        //          있으면 Log 출력.
        IsItemTable();

        _isSuccessInit = true;
    }

    private void InsertItemDataTable()
    {
        Param param = new Param();

        string avatar = "01000000";   // cat bunny bear
        string skin =   "01000000000000000000000000000000";
        string face =   "0100000000000000000000000000000000000000000000000000000000";
        string hat =    "0000000000000000000000000000000000";
        string acc =    "0000000000000000000000";
        string weapon = "00000000000000000000";

        param.Add("avatar", avatar);
        param.Add("skin", skin);
        param.Add("face", face);
        param.Add("hat", hat);
        param.Add("acc", acc);
        param.Add("weapon", weapon);

        BackendReturnObject BRO = Backend.GameData.Insert("item", param);

        if (BRO.IsSuccess())
        {
            Debug.Log("indate : " + BRO.GetInDate());
            InsertItemDataList(avatar, skin, face, hat, acc, weapon);
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "404":
                    Debug.Log("존재하지 않는 tableName인 경우");
                    break;

                case "412":
                    Debug.Log("비활성화 된 tableName인 경우");
                    break;

                case "413":
                    Debug.Log("하나의 row( column들의 집합 )이 400KB를 넘는 경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
                    break;
            }
        }
    }

    private void InsertItemDataList(string avatar, string skin, string face, string hat, string acc, string weapon)
    {
        // 앞에서부터 두자리씩 잘라 List에 추가해준다.
        for (int i = 0; i <= avatar.Length - 2; i += 2)
        {
            string SubsTmp = avatar.Substring(i, 2);
            _listAvatar.Add(Convert.ToInt32(SubsTmp, 16));
        }

        for (int i = 0; i <= skin.Length - 2; i += 2)
        {
            string SubsTmp = skin.Substring(i, 2);
            _listSkin.Add(Convert.ToInt32(SubsTmp, 16));
        }

        for (int i = 0; i <= face.Length - 2; i += 2)
        {
            string SubsTmp = face.Substring(i, 2);
            _listFace.Add(Convert.ToInt32(SubsTmp, 16));
        }

        for (int i = 0; i <= hat.Length - 2; i += 2)
        {
            string SubsTmp = hat.Substring(i, 2);
            _listHat.Add(Convert.ToInt32(SubsTmp, 16));
        }

        for (int i = 0; i <= acc.Length - 2; i += 2)
        {
            string SubsTmp = acc.Substring(i, 2);
            _listAcc.Add(Convert.ToInt32(SubsTmp, 16));
        }

        for (int i = 0; i <= weapon.Length - 2; i += 2)
        {
            string SubsTmp = weapon.Substring(i, 2);
            _listWeapon.Add(Convert.ToInt32(SubsTmp, 16));
        }

        for (int i = 0; i < _listAvatar.Count; i++)
        {
            Debug.Log("_listAvatar[" + i + "] : " + _listAvatar[i]);
        }
        for (int i = 0; i < _listSkin.Count; i++)
        {
            Debug.Log("_listSkin[" + i + "] : " + _listSkin[i]);
        }
        for (int i = 0; i < _listFace.Count; i++)
        {
            Debug.Log("_listFace[" + i + "] : " + _listFace[i]);
        }
        for (int i = 0; i < _listHat.Count; i++)
        {
            Debug.Log("_listHat[" + i + "] : " + _listHat[i]);
        }
        for (int i = 0; i < _listAcc.Count; i++)
        {
            Debug.Log("_listAcc[" + i + "] : " + _listAcc[i]);
        }
        for (int i = 0; i < _listWeapon.Count; i++)
        {
            Debug.Log("_listWeapon[" + i + "] : " + _listWeapon[i]);
        }
    }

    public void ReadItemDataTable(BackendReturnObject BRO)
    {
        if (BRO.IsSuccess())
        {
            GetItemContents(BRO.GetReturnValuetoJSON());
        }
        else
        {
            CheckErrorRead(BRO);
        }
    }

    private void GetItemContents(JsonData returnData)
    {
        if (returnData != null)
        {
            Debug.Log("데이터가 존재합니다.");

            // rows 로 전달받은 경우 
            if (returnData.Keys.Contains("rows"))
            {
                JsonData rows = returnData["rows"];
                for (int i = 0; i < rows.Count; i++)
                {
                    GetData(rows[i]);
                }
            }

            // row 로 전달받은 경우
            else if (returnData.Keys.Contains("row"))
            {
                JsonData row = returnData["row"];
                GetData(row[0]);
            }
        }
        else
        {
            Debug.Log("데이터가 없습니다.");
        }
    }

    void GetData(JsonData data)
    {
        string avatar = string.Empty;   // cat bunny bear
        string skin = string.Empty;
        string face = string.Empty;
        string hat = string.Empty;
        string acc = string.Empty;
        string weapon = string.Empty;

        // 해당 값이 배열로 저장되어 있을 경우는 아래와 같이 키가 존재하는지 확인합니다.
        if (data.Keys.Contains("avatar"))
        {
            JsonData avatarData = data["avatar"];
            if (avatarData.Keys.Contains("S"))
            {
                avatar = avatarData[0].ToString();
                //Debug.Log("avatarData[0] : " + avatarData[0] + ", avatar : " + avatar);
            }
            else
            {
                Debug.Log("존재하지 않는 키 입니다.");
            }
        }

        if (data.Keys.Contains("skin"))
        {
            JsonData skinData = data["skin"];
            if (skinData.Keys.Contains("S"))
            {
                skin = skinData[0].ToString();
                //Debug.Log("skinData[0] : " + skinData[0] + ", skin : " + skin);
            }
            else
            {
                Debug.Log("존재하지 않는 키 입니다.");
            }
        }

        if (data.Keys.Contains("face"))
        {
            JsonData faceData = data["face"];
            if (faceData.Keys.Contains("S"))
            {
                face = faceData[0].ToString();
                //Debug.Log("faceData[0] : " + faceData[0] + ", face : " + face);
            }
            else
            {
                Debug.Log("존재하지 않는 키 입니다.");
            }
        }

        if (data.Keys.Contains("hat"))
        {
            JsonData hatData = data["hat"];
            if (hatData.Keys.Contains("S"))
            {
                hat = hatData[0].ToString();
                //Debug.Log("hatData[0] : " + hatData[0] + ", hat : " + hat);
            }
            else
            {
                Debug.Log("존재하지 않는 키 입니다.");
            }
        }

        if (data.Keys.Contains("acc"))
        {
            JsonData accData = data["acc"];
            if (accData.Keys.Contains("S"))
            {
                acc = accData[0].ToString();
                //Debug.Log("accData[0] : " + accData[0] + ", acc : " + acc);
            }
            else
            {
                Debug.Log("존재하지 않는 키 입니다.");
            }
        }

        if (data.Keys.Contains("weapon"))
        {
            JsonData weaponData = data["weapon"];
            if (weaponData.Keys.Contains("S"))
            {
                weapon = weaponData[0].ToString();
                //Debug.Log("weaponData[0] : " + weaponData[0] + ", weapon : " + weapon);
            }
            else
            {
                Debug.Log("존재하지 않는 키 입니다.");
            }
        }

        InsertItemDataList(avatar, skin, face, hat, acc, weapon);
    }

    void CheckErrorRead(BackendReturnObject BRO)
    {
        switch (BRO.GetStatusCode())
        {
            case "200":
                Debug.Log("해당 유저의 데이터가 테이블에 없습니다.");
                break;

            case "404":
                if (BRO.GetMessage().Contains("gamer not found"))
                {
                    Debug.Log("gamerIndate가 존재하지 gamer의 indate인 경우");
                }
                else if (BRO.GetMessage().Contains("table not found"))
                {
                    Debug.Log("존재하지 않는 테이블");
                }
                break;

            case "400":
                if (BRO.GetMessage().Contains("bad limit"))
                {
                    Debug.Log("limit 값이 100이상인 경우");
                }

                else if (BRO.GetMessage().Contains("bad table"))
                {
                    // public Table 정보를 얻는 코드로 private Table 에 접근했을 때 또는
                    // private Table 정보를 얻는 코드로 public Table 에 접근했을 때 
                    Debug.Log("요청한 코드와 테이블의 공개여부가 맞지 않습니다.");
                }
                break;

            case "412":
                Debug.Log("비활성화된 테이블입니다.");
                break;

            default:
                Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
                break;

        }
    }

    private void CheckErrorUpdate(BackendReturnObject BRO)
    {
        if (BRO.IsSuccess())
        {
            Debug.Log("수정 완료");
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "405":
                    Debug.Log("param에 partition, gamer_id, inDate, updatedAt 네가지 필드가 있는 경우");
                    break;

                case "403":
                    Debug.Log("퍼블릭테이블의 타인정보를 수정하고자 하였을 경우");
                    break;

                case "404":
                    Debug.Log("존재하지 않는 tableName인 경우");
                    break;

                case "412":
                    Debug.Log("비활성화 된 tableName인 경우");
                    break;

                case "413":
                    Debug.Log("하나의 row( column들의 집합 )이 400KB를 넘는 경우");
                    break;
            }
        }
    }
    private void IsItemTable()
    {
        BackendReturnObject BRO = Backend.GameData.GetMyData("item", new Where(), 1);

        if (BRO.GetReturnValuetoJSON()["rows"].Count <= 0)
        {
            //Debug.Log("BRO.GetReturnValuetoJSON()[rows].Count : " + BRO.GetReturnValuetoJSON()["rows"].Count);
            InsertItemDataTable();
        }
        else
        {
            ReadItemDataTable(BRO);
        }
        Debug.Log(BRO.GetReturnValue());        
    }

    public void UpdateAvatarDataTable()
    {
        string tmp2 = string.Empty;
        string result = string.Empty;
        for (int i = 0; i < _listAvatar.Count; i++)
        {
            tmp2 = Convert.ToString(_listAvatar[i], 16);    // 10진수 data값ㅇ르 16진수로 변환
            tmp2 = tmp2.PadLeft(2, '0');                    // 두자리에 맞춰 0을 채워넣음

            result += tmp2;
        }

        Param param = new Param();
        param.Add("avatar", result);

        BackendReturnObject BRO = Backend.GameData.Update("item", new Where(), param);

        CheckErrorUpdate(BRO);   
    }

    public void UpdateSkinDataTable()
    {
        string tmp2 = string.Empty;
        string result = string.Empty;
        for (int i = 0; i < _listSkin.Count; i++)
        {
            tmp2 = Convert.ToString(_listSkin[i], 16);    // 10진수 data값ㅇ르 16진수로 변환
            tmp2 = tmp2.PadLeft(2, '0');                    // 두자리에 맞춰 0을 채워넣음

            result += tmp2;
        }

        Param param = new Param();
        param.Add("skin", result);

        BackendReturnObject BRO = Backend.GameData.Update("item", new Where(), param);

        CheckErrorUpdate(BRO);
    }

    public void UpdateFaceDataTable()
    {
        string tmp2 = string.Empty;
        string result = string.Empty;
        for (int i = 0; i < _listFace.Count; i++)
        {
            tmp2 = Convert.ToString(_listFace[i], 16);    // 10진수 data값ㅇ르 16진수로 변환
            tmp2 = tmp2.PadLeft(2, '0');                    // 두자리에 맞춰 0을 채워넣음

            result += tmp2;
        }

        Param param = new Param();
        param.Add("face", result);

        BackendReturnObject BRO = Backend.GameData.Update("item", new Where(), param);

        CheckErrorUpdate(BRO);
    }

    public void UpdateHatDataTable()
    {
        string tmp2 = string.Empty;
        string result = string.Empty;
        for (int i = 0; i < _listHat.Count; i++)
        {
            tmp2 = Convert.ToString(_listHat[i], 16);    // 10진수 data값ㅇ르 16진수로 변환
            tmp2 = tmp2.PadLeft(2, '0');                    // 두자리에 맞춰 0을 채워넣음

            result += tmp2;
        }

        Param param = new Param();
        param.Add("hat", result);

        BackendReturnObject BRO = Backend.GameData.Update("item", new Where(), param);

        CheckErrorUpdate(BRO);
    }

    public void UpdateAccDataTable()
    {
        string tmp2 = string.Empty;
        string result = string.Empty;
        for (int i = 0; i < _listAcc.Count; i++)
        {
            tmp2 = Convert.ToString(_listAcc[i], 16);    // 10진수 data값ㅇ르 16진수로 변환
            tmp2 = tmp2.PadLeft(2, '0');                    // 두자리에 맞춰 0을 채워넣음

            result += tmp2;
        }

        Param param = new Param();
        param.Add("acc", result);

        BackendReturnObject BRO = Backend.GameData.Update("item", new Where(), param);

        CheckErrorUpdate(BRO);
    }

    public void UpdateWeaponDataTable()
    {
        string tmp2 = string.Empty;
        string result = string.Empty;
        for (int i = 0; i < _listWeapon.Count; i++)
        {
            tmp2 = Convert.ToString(_listWeapon[i], 16);    // 10진수 data값ㅇ르 16진수로 변환
            tmp2 = tmp2.PadLeft(2, '0');                    // 두자리에 맞춰 0을 채워넣음

            result += tmp2;
        }

        Param param = new Param();
        param.Add("weapon", result);

        BackendReturnObject BRO = Backend.GameData.Update("item", new Where(), param);

        CheckErrorUpdate(BRO);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}