using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text Txt_MaxScore;

    void Start()
    {
        Txt_MaxScore.text = T.MAX_SCORE.ToString();
    }
}
