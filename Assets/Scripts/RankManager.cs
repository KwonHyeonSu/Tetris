using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RankManager : MonoBehaviour
{
    public static RankManager instance = null;
    void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }

    public static RankManager Instance
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private string SetURL = "http://gotgam.dothome.co.kr/SetData.php";
    public bool isDone_Setrank = false;
    public bool SetRank(string name, int score)
    {
        //rank 데이타 먼저 불러옴
        WWWForm form_get = new WWWForm();
        WWW www_get = new WWW(GetURL, form_get);

        while(!www_get.isDone)
        {
            Debug.Log("Connecting...");
        }

        Dictionary<string, int> RankData = new Dictionary<string, int>();

        RankData = Indexing(www_get.text);

        if(!RankData.ContainsKey(name))
        {
            WWWForm form = new WWWForm();
            form.AddField("usernamePost", name);
            form.AddField("scorePost", score);
            
            WWW www = new WWW(SetURL, form);
            while(!www.isDone)
            {
                Debug.Log("Connecting...");
            }

            Debug.Log("setrank : " + www.text);
        }
        else
        {
            Debug.Log("중복!");
            return false;
        }

        return true;

    }


    private string GetURL = "http://gotgam.dothome.co.kr/GetData.php";


    public Dictionary<string, int> GetRank()
    {
        WWWForm form = new WWWForm();
        WWW www = new WWW(GetURL, form);

        
        int tryCount = 0;
        while(!www.isDone && tryCount < 1000)
        {
            Debug.Log("Connecting...");
            tryCount++;
        }

        if(www.isDone)
            return Indexing(www.text);

        else{

            Debug.Log("랭크 불러오기 실패");
            return null;
        }
    }


    public Dictionary<string, int> Indexing(string text)
    {
        Dictionary<string, int> RankData = new Dictionary<string, int>();

        string[] indexed = text.Split(',');


        for(int i=0;i<(indexed.Length-1) * 0.5f; i++)
        {
            RankData.Add(indexed[i*2], int.Parse(indexed[i*2+1]));
        }
        
        Dictionary<string, int> sorted = RankData.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        

        return sorted;
    }
}
