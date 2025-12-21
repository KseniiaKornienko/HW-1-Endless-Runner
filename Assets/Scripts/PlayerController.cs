using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    public GameManager gm;
    private Vector3 dir;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Text playerHealthText;
    [SerializeField] private Score scoreScript;
    private bool isImmortal;

    private int lineToMove = 1;
    public float lineDistance = 4;
    private float maxSpeed = 80;
    private float currentHealth;
    private int wallObstacleDamage = 20;
    private int sphereObstacleDamage = 10;
    private int healthBonus = 10;
    private HashSet<GameObject> hitObjects = new HashSet<GameObject>();

    void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<CharacterController>();
        Time.timeScale = 1;
        StartCoroutine(SpeedIncrease());
        isImmortal = false;
    }

    private void Update() {
        if (SwipeController.swipeRight) {
            if (lineToMove < 2) {
                lineToMove++;
            }
        }
        if (SwipeController.swipeLeft) {
            if (lineToMove > 0) {
                lineToMove--;
            }
        }

        if (SwipeController.swipeUp) {
            if (controller.isGrounded) {
                Jump();
            }
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (lineToMove == 0 ) {
            targetPosition += Vector3.left * lineDistance;
        } else if (lineToMove == 2) {
            targetPosition += Vector3.right * lineDistance;
        }
        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);

    }

    private void Jump() {
        dir.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "WallObstacle" && !hitObjects.Contains(hit.gameObject))
        {
            hitObjects.Add(hit.gameObject);
            if (!isImmortal) {
                currentHealth -= wallObstacleDamage;
            }
            Debug.Log(currentHealth);
            if (currentHealth <= 0) {
                playerHealthText.text = "Health: 0";
                int lastRunScore = int.Parse(scoreScript.scoreText.text.ToString());
                PlayerPrefs.SetInt("lastRunScore", lastRunScore);
                gm.EndGame();
            } else {
                playerHealthText.text = "Health: " + currentHealth.ToString();
                Destroy(hit.gameObject);
            }
        }
        if (hit.gameObject.tag == "SphereObstacle" && !hitObjects.Contains(hit.gameObject))
        {
            hitObjects.Add(hit.gameObject);
            if (!isImmortal) {
                currentHealth -= sphereObstacleDamage;
            }
            Debug.Log(currentHealth);
            if (currentHealth <= 0) {
                playerHealthText.text = "Health: 0";
                int lastRunScore = int.Parse(scoreScript.scoreText.text.ToString());
                PlayerPrefs.SetInt("lastRunScore", lastRunScore);
                gm.EndGame();
            } else {
                playerHealthText.text = "Health: " + currentHealth.ToString();
                Destroy(hit.gameObject);
            }
        }
        if (hit.gameObject.tag == "HealthBonus" && !hitObjects.Contains(hit.gameObject))
        {
            hitObjects.Add(hit.gameObject);
            if (currentHealth < 100f) {
                currentHealth += healthBonus;
            }
            playerHealthText.text = "Health: " + currentHealth.ToString();
            Debug.Log(currentHealth);
            Destroy(hit.gameObject);
        }

        if (hit.gameObject.tag == "ShieldBonus" && !hitObjects.Contains(hit.gameObject))
        {
            StartCoroutine(ShieldBonus());
            Destroy(hit.gameObject);

        }
    }

    private void OnControllerColliderExit(Collider other)
{
    if (other.CompareTag("WallObstacleacle") || other.CompareTag("SphereObstacle") || other.CompareTag("HealthBonus"))
    {
        hitObjects.Remove(other.gameObject);
    }
}


    void FixedUpdate()
    {
        dir.z = speed;
        dir.y += gravity * Time.fixedDeltaTime;
        controller.Move(dir * Time.fixedDeltaTime);
    }

    private IEnumerator SpeedIncrease()
    {
        yield return new WaitForSeconds(1);
        if (speed < maxSpeed)
        {
            speed += 0.5f;
            StartCoroutine(SpeedIncrease());
        }
    }

    private IEnumerator ShieldBonus()
    {
        isImmortal = true;
        yield return new WaitForSeconds(5);
        isImmortal = false;
    }
}
