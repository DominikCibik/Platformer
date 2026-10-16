using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D rb;
    public int moveSpeed = 4;
    public float jumpHeight = 0.75f;
    public Animator myAnimator;
    public LayerMask groundLayer;
    private int smer = 1;
    private int healthCount = 3;
    public GameObject h1;
    public GameObject h2;
    public GameObject h3;
    public GameObject PausePanel;
    public GameObject GameOverPanel;
    public GameObject SuccessPanel;

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        healthCount = PlayerPrefs.GetInt("player_health", 3);
        vykresliStav();
    }
    
    void Update()  {
        if (Input.GetKey(KeyCode.D)) {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            myAnimator.Play("right_player");
            smer = 1;

        }
        if (Input.GetKey(KeyCode.A)) {
            transform.position += Vector3.right * -moveSpeed * Time.deltaTime;
            myAnimator.Play("left_player");
            smer = -1;

        }
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            PausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        vykresliStav();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "enemy") {
            myAnimator.Play("colision_player");
            healthCount--;
            if (healthCount == 0) { 
                Time.timeScale = 0;
                koniec(); 
                return; 
            }
            if (smer == 1) {
                transform.position += Vector3.right * -100 * Time.deltaTime;
                transform.position += Vector3.up * 100 * Time.deltaTime;
            } else {
                transform.position += Vector3.right * +100 * Time.deltaTime;
                transform.position += Vector3.up * 100 * Time.deltaTime;
            }
        }
        if (other.gameObject.tag == "killZone") {
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "deathZone") {
            healthCount = 0;
            Time.timeScale = 0;
            koniec();
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "heart") {
            if (healthCount < 3) {
                healthCount++;
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.tag == "key"){
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            int highestLevel = PlayerPrefs.GetInt("highest_level", 1); 

            if (currentLevel >= highestLevel) {
                PlayerPrefs.SetInt("highest_level", currentLevel + 1);
                PlayerPrefs.Save();
            }

            PlayerPrefs.SetInt("player_health", healthCount);
            PlayerPrefs.Save();

            if (currentLevel == 3){
                SuccessPanel.SetActive(true);
                Time.timeScale = 0;
            }

            if (currentLevel + 1 < SceneManager.sceneCountInBuildSettings) {
                SceneManager.LoadScene(currentLevel + 1);
            }
        }
    }
    private void vykresliStav() {
        if (healthCount == 1) {h1.SetActive(true);h2.SetActive(false);h3.SetActive(false);}
        if (healthCount == 2) {h1.SetActive(true);h2.SetActive(true);h3.SetActive(false);}
        if (healthCount == 3) {h1.SetActive(true);h2.SetActive(true);h3.SetActive(true);}
    } 

    private void koniec() {
        int level = PlayerPrefs.GetInt("highest_level");
        PlayerPrefs.SetInt("player_health", 3);
        PlayerPrefs.Save();
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private bool IsGrounded() {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;
        
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null) return true;
        return false;
    }
    public void mainMenu() {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("player_health", 3);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume() {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void startAgain() {
        PlayerPrefs.SetInt("player_health", 3);
        PlayerPrefs.Save();
        SceneManager.LoadScene("level1");
        Time.timeScale = 1;
    }
}