using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{

    //private Touch initialTouch = new Touch();
    //private float distance = 0;
    //private bool hasSwiped = false;


    public bool jump = false;
    public bool slide = false;

    public GameObject trigger;
    public Animator anim;

    public float score = 0;
    public int currentTime;
    public float initialTime = 0;


    public bool boost = false;
    public Rigidbody rbody;
    public CapsuleCollider myCollider;

    public bool death = false;
    public Image gameOverImg;
    public Text scoreText;
    public Text bestScoreText;
    public float lastScore;

    public Text timeText;
    public Text timeUIText;

    public GameObject PauseOB;

    public float moveSpeed = 5f;
    public float jumpForce = 1f;
    public float slideDuration = 1f;
    // Use this for initialization
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<CapsuleCollider>();

        initialTime = Time.time;

        lastScore = PlayerPrefs.GetFloat("MyScore");
        moveSpeed = 5f;

        //timeUIText = GameObject.Find("TimeUIText").GetComponent<Text>();
    }

    void Update()
    {
        if (!death)
        {
            currentTime = Mathf.FloorToInt(Time.time - initialTime);
            UpdateTimeText();
        }

    }

    void UpdateTimeText()
    {
        timeText.text = "Time: " + currentTime.ToString() + "s";
    }

    void FixedUpdate()
    {
        //foreach (Touch t in Input.touches)
        //{
        //    if (t.phase == TouchPhase.Began)
        //    {
        //        initialTouch = t;
        //    }
        //    else if (t.phase == TouchPhase.Moved && !hasSwiped)
        //    {
        //        float deltaX = initialTouch.position.x - t.position.x;
        //        float deltaY = initialTouch.position.y - t.position.y;
        //        distance = Mathf.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
        //        bool swipedSideWay = Mathf.Abs(deltaX) > Mathf.Abs(deltaY);

        //        if (distance > 100f)
        //        {
        //            if (swipedSideWay && deltaX > 0)
        //            {
        //                // swiped left
        //            }
        //            if (swipedSideWay && deltaX <= 0)
        //            {
        //                // swiped right

        //            }
        //            if (!swipedSideWay && deltaY > 0)
        //            {
        //                // swiped down
        //                slide = true;
        //                StartCoroutine(SlideController());
        //            }

        //            if (!swipedSideWay && deltaY <= 0)
        //            {
        //                // swiped up
        //                jump = true;
        //                StartCoroutine(JumpController());
        //            }

        //            hasSwiped = true;
        //        }
        //    }
        //    else if (t.phase == TouchPhase.Ended)
        //    {
        //        initialTouch = new Touch();
        //        hasSwiped = false;
        //    }
        //}

        scoreText.text = "Score: " + score.ToString();


        if (score > lastScore)
        {
            bestScoreText.text = "Best Score : " + score.ToString();
        }
        else
        {
            bestScoreText.text = "Your Score  " + score.ToString();
        }

       

        // player controll start
        if (score >= 100 && death != true)
        {
            transform.Translate(0, 0, 0.2f);
        }
        else if (score >= 200 && death != true)
        {
            transform.Translate(0, 0, 0.3f);
        }
        else if (score >= 400 && death != true)
        {
            transform.Translate(0, 0, 0.4f);
            StartCoroutine(Fisnish());
        }
        else if (death == true)
        {
            transform.Translate(0, 0, 0);
        }
        else
        {
            transform.Translate(0, 0, 0.1f);
        }


        if (boost == true)
        {
            transform.Translate(0, 0, 1f);
            myCollider.enabled = false;
            rbody.isKinematic = true;
        }
        else
        {
            myCollider.enabled = true;
            rbody.isKinematic = false;
        }


        if (jump == true)
        {
            anim.SetBool("isJump", jump);
            transform.Translate(0, 0.5f, 0.1f);
        }
        else if (jump == false)
        {
            anim.SetBool("isJump", jump);
        }

        if (slide == true)
        {
            anim.SetBool("isSlide", slide);
            transform.Translate(0, 0, 0.1f);
            myCollider.height = 1.8f;
        }
        else if (slide == false)
        {
            anim.SetBool("isSlide", slide);
            myCollider.height = 2.05f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * UnityEngine.Time.deltaTime);

        // Player Control End

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // jump = true;
            StartCoroutine(JumpController());
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            slide = true;
            StartCoroutine(SlideController());
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerTrigger")
        {
            Destroy(trigger.gameObject);
        }

        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject, 0.5f);
            score += 5f;
            //Debug.Log(score);
        }
        if (other.gameObject.tag == "CoinBlue")
        {
            Destroy(other.gameObject, 0.5f);
            score += 3f;
           // Debug.Log(score);
        }

        if (other.gameObject.tag == "Boost")
        {
            Destroy(other.gameObject);
            StartCoroutine(BoostController());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            death = true;
            anim.SetTrigger("Die");
            StartCoroutine(LoadNextSceneAfterDelay(1.8f, "RePlay"));


            if (score > lastScore)
            {
                PlayerPrefs.SetFloat("MyScore", score);
                PlayerPrefs.Save();
            }
            initialTime = Time.time;
        }
    }

    IEnumerator LoadNextSceneAfterDelay(float delay, string sceneName)
    {
        yield return new WaitForSeconds(delay);
        if (death == true)
        {
            gameOverImg.gameObject.SetActive(true);
            PauseOB.gameObject.SetActive(false);
            timeText.gameObject.SetActive(false);

            timeUIText.text = "Duration  " + currentTime.ToString() + " s";
        }
    }

    IEnumerator Fisnish()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Win");
    }

    IEnumerator BoostController()
    {
        boost = true;
        yield return new WaitForSeconds(3);
        boost = false;
    }

    IEnumerator JumpController()
    {
        jump = true;
        rbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.2f);
        jump = false;
    }

    IEnumerator SlideController()
    {
        slide = true;
        myCollider.height = 1.8f;
        yield return new WaitForSeconds(0.7f);
        slide = false;
        myCollider.height = 2.05f;
    }

    
}

