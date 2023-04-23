using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class stickManManager : MonoBehaviour
{

    private Animator StickManAnimator;
    private void Start()
    {
        StickManAnimator = GetComponent<Animator>();
        StickManAnimator.SetBool("run", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("red") && other.transform.parent.childCount > 0)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);


        }

        if (other.CompareTag("obstacle"))
        {
            {
                Destroy(gameObject);




            }

            switch (other.tag)
            {
                case "red":
                    if (other.transform.parent.childCount > 0)
                    {
                        Destroy(other.gameObject);
                        Destroy(gameObject);
                    }

                    break;
                case "obstacle":
                    {

                        Destroy(gameObject);


                    }
                    break;



            }




        }
    }
}

