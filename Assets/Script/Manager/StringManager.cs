using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class StringManager : Singleton<StringManager>
{
    protected StringManager() { }

    string ChartName, ChartFileId, Country;
    private bool _isSuccessInit = false;
    public bool isSuccessInit { get { return _isSuccessInit; } }

    // Start is called before the first frame update
    public void Init()
    {
        BackendReturnObject BRO = Backend.Chart.GetChartList();
        if(BRO.IsSuccess())
        {
            CheckCountry();
            JsonData rows = BRO.GetReturnValuetoJSON()["rows"];
            Debug.Log("Backend Chart 받아오기 완료" + rows.Count);

            for (int i = 0; i < rows.Count; i++)
            {
                ChartName = rows[i]["chartName"]["S"].ToString();
                ChartFileId = rows[i]["selectedChartFileId"]["N"].ToString();
                Debug.Log("[" + i + "] 차트네임 : " + ChartName + ", 필드아이디 : " + ChartFileId);

                //스트링 차트만 가져오기.
                if (ChartName == "string")
                {
                    //저장된 stringFileID와 서버 FildID 비교
                    if (PlayerPrefs.HasKey("StringChartFileID"))
                    {
                        Debug.Log("로컬 FildID : " + PlayerPrefs.GetString("StringChartFileID"));

                        //로컬에 저장된 FileID와 서버에서 받은 FildID가 다를 때
                        if(PlayerPrefs.GetString("StringChartFileID") != ChartFileId)
                        {
                            //기존에 있던 String Data 삭제 후 서버에서 받은 String Local 저장
                            Debug.Log("FildID 교체 : " + PlayerPrefs.GetString("StringChartFileID") + " -> " + ChartFileId);
                            Backend.Chart.DeleteLocalChartData(PlayerPrefs.GetString("StringChartFileID"));
                            PlayerPrefs.SetString("StringChartFileID", ChartFileId);
                            Backend.Chart.GetOneChartAndSave(ChartFileId);
                            Backend.Chart.GetLocalChartData(ChartFileId);
                        }
                        else
                        {
                            Debug.Log("서버 FildID == 로컬 FildID : " + ChartFileId);
                            Backend.Chart.GetLocalChartData(ChartFileId);
                        }
                    }
                    else
                    {
                        Debug.Log("저장된 FildID가 없음. 새로운 FildID 등록 : " + ChartFileId);
                        PlayerPrefs.SetString("StringChartFileID", ChartFileId);
                        Backend.Chart.GetOneChartAndSave(ChartFileId);
                        Backend.Chart.GetLocalChartData(ChartFileId);
                    }

                    JsonData chartJson = JsonMapper.ToObject(Backend.Chart.GetLocalChartData(ChartFileId));
                    Debug.Log("차트제이슨? " + chartJson);
                    //차트가 비어 있으면
                    if (chartJson.ToString() == "Uninitialized JsonData")
                    {
                        Backend.Chart.GetOneChartAndSave(ChartFileId);
                        if (BRO.IsSuccess())
                        {
                            Debug.Log("새로운 차트 fileID[" + ChartFileId + "]로 저장.");
                        }
                        else
                        {
                            Debug.Log("새로운 차트 fileID[" + ChartFileId + "]로 저장 실패.");
                        }
                    }
                    else
                    {
                        Debug.Log("이미 저장되 차트 fileID[" + ChartFileId + "]");
                    }
                }
            }            
        }
        else
        {
            Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
        }

        _isSuccessInit = true;
    }

    public string GetString(string string_name)
    {
        JsonData chartJson = JsonMapper.ToObject(Backend.Chart.GetLocalChartData(ChartFileId));

        if (chartJson.ToString() == "Uninitialized JsonData")
        {
            Debug.LogError("String Chart null.");
        }
        else
        {
            var rows = chartJson["rows"];

            for (int i = 0; i < rows.Count; i++)
            {
                if(rows[i]["string_name"][0].ToString() == string_name)
                {
                    return rows[i][Country][0].ToString();
                }
            }
        }
        return "null";
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckCountry()
    {
        SystemLanguage sl = Application.systemLanguage;
        Debug.Log("어느 나라신교? : " + sl);

        if (sl == SystemLanguage.Korean)
        {
            // 뒤끝 값
            Country = "Korean";
        }
        else // 영어
        {
            // 뒤끝 값
            Country = "English";
        }
    }
}
