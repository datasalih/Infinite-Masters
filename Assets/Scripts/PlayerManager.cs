using System.Collections;
using Cinemachine;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    public Transform player;
    private int numberOfStickmans=0;
     int   numberOfEnemyStickmans;
    [SerializeField] private TextMeshPro CounterTxt;
    [SerializeField] private GameObject stickMan;
    public Text pointstext;

    //****************************************************

    [Range(0f, 1f)][SerializeField] private float DistanceFactor, Radius;

    //*********** move the player ********************

    public bool moveByTouch, gameState;
    private Vector3 mouseStartPos, playerStartPos;
    public float playerSpeed, roadSpeed;
    private Camera camera;

    public Text limittext;

    public int points, limit = 100;

    [SerializeField] private Transform road;
    [SerializeField] private Transform enemy;
    private bool attack;
    public static PlayerManager PlayerManagerInstance;
    public GameObject SecondCam;
    public bool moveTheCamera;


   


    void Start()
    {

      


        limit = 100;

        Time.timeScale = 0;

        player = transform;

        camera = Camera.main;

        PlayerManagerInstance = this;

        gameState = false;

        points = PlayerPrefs.GetInt("points2", points);
         limit = PlayerPrefs.GetInt("limit2", limit);

       numberOfStickmans = PlayerPrefs.GetInt("NumberOfStickmans2", transform.childCount - 1);
       


       

        limittext.text = limit.ToString() + " Gems";
        pointstext.text = points.ToString();
        CounterTxt.text = numberOfStickmans.ToString();


        for (int i = 1; i < numberOfStickmans; i++)
        {
      
            Instantiate(stickMan, transform.position, quaternion.identity, transform);
            FormatStickMan();


        }

    }

    void Update()
    {
        

        

        pointstext.text = points.ToString();

        PlayerPrefs.SetInt("limit2", limit);
        PlayerPrefs.SetInt("points2", points);



        if (attack)
        {
            var enemyDirection = new Vector3(enemy.position.x, transform.position.y, enemy.position.z) - transform.position;

            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation =
                    Quaternion.Slerp(transform.GetChild(i).rotation, Quaternion.LookRotation(enemyDirection, Vector3.up), Time.deltaTime * 3f);
            }

            if (enemy.GetChild(1).childCount > 1)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    var Distance = enemy.GetChild(1).GetChild(0).position - transform.GetChild(i).position;

                    if (Distance.magnitude < 1.5f)
                    {
                        transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position,
                            new Vector3(enemy.GetChild(1).GetChild(0).position.x, transform.GetChild(i).position.y,
                                enemy.GetChild(1).GetChild(0).position.z), Time.deltaTime * 1f);
                    }
                }
            }

            else
            {
                attack = false;
                roadSpeed = 2f;

                FormatStickMan();

                for (int i = 1; i < transform.childCount; i++)
                    transform.GetChild(i).rotation = Quaternion.identity;


                enemy.gameObject.SetActive(false);

            }

            if (numberOfStickmans < 1)
            {
                SceneManager.LoadScene("EndScene");
            
            }


        }
        else
        {
            MoveThePlayer();

        }





        if (gameState)
        {
            road.Translate(road.forward * Time.deltaTime * roadSpeed);
            points++;


        }



        if (moveTheCamera && transform.childCount > 1)
        {
            var cinemachineTransposer = SecondCam.GetComponent<CinemachineVirtualCamera>()
              .GetCinemachineComponent<CinemachineTransposer>();

            var cinemachineComposer = SecondCam.GetComponent<CinemachineVirtualCamera>()
                .GetCinemachineComponent<CinemachineComposer>();

            cinemachineTransposer.m_FollowOffset = new Vector3(4.5f, Mathf.Lerp(cinemachineTransposer.m_FollowOffset.y,
                transform.GetChild(1).position.y + 2f, Time.deltaTime * 1f), -5f);

            cinemachineComposer.m_TrackedObjectOffset = new Vector3(0f, Mathf.Lerp(cinemachineComposer.m_TrackedObjectOffset.y,
                4f, Time.deltaTime * 1f), 0f);

        }

    }

    void MoveThePlayer()
    {
        if (Input.GetMouseButtonDown(0) && gameState)
        {
            moveByTouch = true;

            var plane = new Plane(Vector3.up, 0f);

            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var distance))
            {
                mouseStartPos = ray.GetPoint(distance + 1f);
                playerStartPos = transform.position;
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            moveByTouch = false;

        }

        if (moveByTouch)
        {
            var plane = new Plane(Vector3.up, 0f);
            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var distance))
            {
                var mousePos = ray.GetPoint(distance + 1f);

                var move = mousePos - mouseStartPos;

                var control = playerStartPos + move;


                if (numberOfStickmans > 50)
                    control.x = Mathf.Clamp(control.x, -0.7f, 0.7f);
                else
                    control.x = Mathf.Clamp(control.x, -1.1f, 1.1f);

                transform.position = new Vector3(Mathf.Lerp(transform.position.x, control.x, Time.deltaTime * playerSpeed)
                    , transform.position.y, transform.position.z);

            }
        }
    }

    public void FormatStickMan()
    {
        for (int i = 1; i < player.childCount; i++)
        {
            var x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
            var z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);

            var NewPos = new Vector3(x, -0.55f, z);

            player.transform.GetChild(i).DOLocalMove(NewPos, 0.5f).SetEase(Ease.OutBack);
        }
    }

   public void MakeStickMan(int number)
    {


        for (int i = numberOfStickmans; i < number; i++)
        {

            Instantiate(stickMan, transform.position, quaternion.identity, transform);
          

        }
    


        numberOfStickmans = transform.childCount - 1;
        CounterTxt.text = numberOfStickmans.ToString();
    
        FormatStickMan();
    }

    public void UpgradeStickMan()
    {

        if (points >= limit)
        {
            points -= limit;
            limit = limit * 2;

            for (int i = 0; i <= 1; i++)
            {
               
                Instantiate(stickMan, transform.position, quaternion.identity, transform);
                numberOfStickmans++;

              

                numberOfStickmans = transform.childCount - 1;

                pointstext.text = points.ToString();
                limittext.text = limit.ToString() + " Gems";
                CounterTxt.text = numberOfStickmans.ToString();
               PlayerPrefs.SetInt("NumberOfStickmans2", numberOfStickmans);
                
                FormatStickMan();
            }
        }



    }



    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("gate"))
        {
            other.transform.parent.GetChild(0).GetComponent<BoxCollider>().enabled = false; // gate 1
            other.transform.parent.GetChild(1).GetComponent<BoxCollider>().enabled = false; // gate 2

            var gateManager = other.GetComponent<GateManager>();

            numberOfStickmans = transform.childCount - 1;

            if (gateManager.multiply)
            {
                MakeStickMan(numberOfStickmans * gateManager.randomNumber);
              
            }
            else
            {
                MakeStickMan(numberOfStickmans + gateManager.randomNumber);
             
            }
        }

        if (other.CompareTag("enemy"))
        { 
            enemy = other.transform;
            attack = true;

            roadSpeed = 0.5f;
            
            other.transform.GetChild(1).GetComponent<enemyManager>().AttackThem(transform);

            StartCoroutine(UpdateTheEnemyAndPlayerStickMansNumbers());

        }

        if (other.CompareTag("obstacle"))
        {


            numberOfStickmans -= 10;
            if (numberOfStickmans < 0)
            {

                SceneManager.LoadScene("EndScene");
                
            }
            CounterTxt.text = numberOfStickmans.ToString();
            FormatStickMan();
            
        }


    }

 

    IEnumerator UpdateTheEnemyAndPlayerStickMansNumbers()
    {

        numberOfEnemyStickmans = enemy.transform.GetChild(1).childCount - 1;
        numberOfStickmans = transform.childCount - 1;

        while (numberOfEnemyStickmans > 0 && numberOfStickmans > 0)
        {
            numberOfEnemyStickmans--;
            numberOfStickmans--;

            enemy.transform.GetChild(1).GetComponent<enemyManager>().CounterTxt.text = numberOfEnemyStickmans.ToString();
            CounterTxt.text = numberOfStickmans.ToString();
            PlayerPrefs.SetInt("stickman2", numberOfStickmans);
            PlayerPrefs.Save();
            yield return null;
        }

        if (numberOfEnemyStickmans == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation = Quaternion.identity;
            }
        }
    }
}
