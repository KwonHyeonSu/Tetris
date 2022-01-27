using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    public Transform Content;
    public GameObject rankBar;

    public RankManager rankManager;

    void Awake()
    {
        if(rankManager == null)
            rankManager = GameObject.Find("RankManager").GetComponent<RankManager>();
    }

    void OnEnable()
    {
        if(rankManager == null)
            rankManager = GameObject.Find("RankManager").GetComponent<RankManager>();
        StartCoroutine(GetRankingData());
    }


    IEnumerator GetRankingData()
    {
        WWWForm form = new WWWForm();
        WWW www = new WWW(GetURL, form);

        yield return www;

        Dictionary<string, int> sorted = rankManager.Indexing(www.text);

        yield return sorted;

        int rank = 0;

        if(Content.childCount != 0)
        {
            for(int i=0;i<Content.childCount;i++)
            {
                Transform DesGo = Content.GetChild(i);
                Destroy(DesGo.gameObject);
            }
            Content.DetachChildren();
        }

        foreach(KeyValuePair<string, int> rankData in sorted)
        {
            rank++;
            MakeRanking(rank, rankData.Key, rankData.Value.ToString());
        }
    }

    private string GetURL = "http://gotgam.dothome.co.kr/GetData.php";




    public void MakeRanking(int rank, string name, string score)
    {
        
        

        GameObject ob = Instantiate(rankBar);
        ob.transform.SetParent(Content);
        ob.transform.localScale = new Vector3(1,1,1);

        RankBar bar = ob.GetComponent<RankBar>();
        bar.rank = rank.ToString();
        bar.name = name;
        bar.score = score;
        
    }

    
}
