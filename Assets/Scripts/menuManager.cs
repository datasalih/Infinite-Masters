using UnityEngine;
using UnityEngine.UI;
public class menuManager : MonoBehaviour
{
  [SerializeField] private GameObject startMenuObj;
    public Text scoretext;
    int score;
    int points;
    bool started;
    

    private void Start()
    {

      points =  PlayerPrefs.GetInt("points" , points);

    }

    public void StartTheGame()
  {
       

        started = true;

        Time.timeScale = 1;
        
    startMenuObj.SetActive(false);

    PlayerManager.PlayerManagerInstance.gameState = true;
    
    PlayerManager.PlayerManagerInstance.player.GetChild(1).GetComponent<Animator>().SetBool("run",true);
  }


    private void Update()



    {
        scoretext.text = score.ToString() + "m " ;

        if(started)
        {
            score++;
        }


        if (score > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", score);
        }

        PlayerPrefs.SetInt("points", points);
        PlayerPrefs.SetInt("score", score);

       
    }

}
