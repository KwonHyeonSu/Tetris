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
        
    }

    void OnEnable()
    {

        if(rankManager == null)
            rankManager = GameObject.Find("RankManager").GetComponent<RankManager>();
        StartCoroutine(GetRankingData());
    }


    IEnumerator GetRankingData()
    {
        Dictionary<string, int> sorted = rankManager.GetRank();

        int rank = 0;

        yield return null;

        foreach(KeyValuePair<string, int> rankData in sorted)
        {
            rank++;
            MakeRanking(rank, rankData.Key, rankData.Value.ToString());
        }
    }



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
