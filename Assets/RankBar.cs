using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankBar : MonoBehaviour
{
    public Text text_name;
    public Text text_score;
    public Text text_rank;

    void Awake()
    {
        text_name.text = "none";
        text_score.text = "zero";
    }
    public string name
    {
        get{
            return text_name.text;
        }
        set{
            text_name.text = value;
        }
    }

    public string score
    {
        get{
            return text_score.text;
        }
        set{
            text_score.text = value;
        }
    }

    public string rank
    {
        get{
            return text_rank.text;
        }
        set{
            text_rank.text = value;
        }
    }
}
