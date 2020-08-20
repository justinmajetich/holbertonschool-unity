using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 20f;
    public int health = 5;
    public Text scoreText;
    public Text healthText;
    public Image winLoseLabel;
    public Text winLoseText;

    private Rigidbody rb;
    private float xForce = 0;
    private float zForce = 0;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Fixed-update is called once physics refresh
    void FixedUpdate()
    {
        // Take force applied against horizontal axis (i.e left/right, a/d)
        xForce = Input.GetAxis("Horizontal") * speed;
        // Take force applied against vertical axis (i.e forward/backward, w/s)
        zForce = Input.GetAxis("Vertical") * speed;

        // Apply force to player rigidbody
        rb.AddForce(xForce, 0, zForce); 
    }

    void Update()
    {
        if (health == 0)
        {
            SetLoseLabel();
            StartCoroutine(LoadScene(3));
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("menu");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Pickup":  // On collision with coin
                score++;
                Destroy(other.gameObject);
                SetScoreText();
                break;

            case "Trap":  // On collision with trap
                health--;
                SetHealthText();
                break;

            case "Goal":  // On collision with goal
                SetWinLabel();
                StartCoroutine(LoadScene(3));
                break;

            default:
                Debug.Log("Unknown trigger.");
                break;
        }
    }

    IEnumerator LoadScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void SetScoreText()
    {
        scoreText.text = $"Score: {score}";
    }

    void SetHealthText()
    {
        healthText.text = $"Health: {health}";
    }

    void SetWinLabel()
    {
        winLoseText.color = Color.black;
        winLoseLabel.color = Color.green;
        winLoseText.text = "You Win!";
        winLoseLabel.gameObject.SetActive(true);
    }

    void SetLoseLabel()
    {
        winLoseText.color = Color.white;
        winLoseLabel.color = Color.red;
        winLoseText.text = "Game Over!";
        winLoseLabel.gameObject.SetActive(true);
    }
}