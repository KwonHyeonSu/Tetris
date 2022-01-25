using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private List<AudioSource> Tetromino_Down = new List<AudioSource>();

    void Start()
    {
        if(Tetromino_Down.Count == 0)
        {
            Tetromino_Down.Add(transform.Find("Tetromino_Down_1").gameObject.GetComponent<AudioSource>());
            Tetromino_Down.Add(transform.Find("Tetromino_Down_2").gameObject.GetComponent<AudioSource>());
            Tetromino_Down.Add(transform.Find("Tetromino_Down_3").gameObject.GetComponent<AudioSource>());
            Tetromino_Down.Add(transform.Find("Tetromino_Down_4").gameObject.GetComponent<AudioSource>());
        }
    }


    //테트로미노 바닥에 닿을 때
    public void Sound_Tetromino_Down()
    {
        Tetromino_Down[Random.Range(0,Tetromino_Down.Count)].Play();
    }
}
