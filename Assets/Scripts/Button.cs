using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Button : MonoBehaviour
{

    //메뉴 Scene
    public void Btn_Start()
    {
        SceneManager.LoadScene("Game");
    }


    //게임 Scene
    public void Btn_Regame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }


    public InputField Input_Name;
    public static bool isDone_SetRank = false;
    public void Btn_SetRank()
    {
        bool check = RankManager.Instance.SetRank(Input_Name.text, T.SCORE);
        
        if(check)
            ToMenu();
    }


}
