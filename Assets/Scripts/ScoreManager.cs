using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ScoreManager : MonoBehaviour
{

    public Text highscoretext;
    public Text scoretext;


    void Start()
    {
        PlayerPrefs.GetInt("score");


        scoretext.text = PlayerPrefs.GetInt("score").ToString();
        highscoretext.text = PlayerPrefs.GetInt("highscore").ToString();

    }

   public void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
