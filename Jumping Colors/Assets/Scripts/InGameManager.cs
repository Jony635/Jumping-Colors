using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public GameObject whiteCirclePrefab;
    public Transform checkPoint;
    public Transform player;
    public static InGameManager gameManager;
    public Text scoreText;
    public Text highScoreText;

    public float distanceToSpawnFromCheckpoint = 20f;

    [Range(0, 8)]
    public int minPercentBetweenColors = 8;

    public int maxAmountOfCirclesInScene = 6;
    private float amountBetweenColors;
    private List<GameObject> circlesSpawned;

    [HideInInspector]
    public int currentScore = -1;
    private int highScore = 0;

    public float secondsToRestart = 2f;
    public GameObject explosionPrefab;

    [HideInInspector]
    public bool paused = false;
    public GameObject pauseMenu;

    void Awake()
    {
        amountBetweenColors = ((float)minPercentBetweenColors/(float)100);
    }

    void Start()
    {
        if (!gameManager)
            gameManager = this;
        else
            Debug.Log("Hey, why is already an instance of the gameManager set?");

        if(circlesSpawned == null)
        {
            circlesSpawned = new List<GameObject>();
        }

        highScore = PlayerPrefs.GetInt("HighScore");
                
    }

    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            paused = !paused;
        }

        if(paused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void GenerateRandomCircle()
    {
        GameObject clone = Instantiate(whiteCirclePrefab, new Vector3 (0f, checkPoint.position.y + distanceToSpawnFromCheckpoint, checkPoint.position.z), Quaternion.identity);
        Color[] colorsInChilds = new Color[clone.transform.childCount];
        circlesSpawned.Add(clone);

        if (circlesSpawned.Count > maxAmountOfCirclesInScene)
        {
            Destroy(circlesSpawned[0]);
            circlesSpawned.RemoveAt(0);
        }
            

        for(int i = 0; i < clone.transform.childCount; ++i)
        {
            Color random;
            do
            {
                random = Random.ColorHSV(0f, 1f, .65f, .7f, .7f, .8f, 1f, 1f);
            } while (CheckIfAlreadySelected(colorsInChilds, random));

            colorsInChilds[i] = random;
            clone.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = random;
        }

        player.GetComponent<SpriteRenderer>().color = colorsInChilds[Random.Range(0, 4)];

    }

    bool CheckIfAlreadySelected(Color[] colors, Color random)
    {
        for (int i = 0; i < colors.Length; ++i)
        {
            float H_a, S, V;
            Color.RGBToHSV(colors[i], out H_a, out S, out V);
            float H_b, s_b, v_b;
            Color.RGBToHSV(random, out H_b, out s_b, out v_b);

            if(Mathf.Abs(H_a - H_b) < amountBetweenColors)
            {
                return true;
            }

               
        }

        return false;
    }

    public void IncrementScore()
    {
        currentScore++;
        scoreText.text = "Score: " + currentScore;
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        highScoreText.text = "HighScore: " + highScore;
    }

    private void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", currentScore);
        highScore = currentScore;
        highScoreText.text = "HighScore: " + highScore;
    }

    public void EndGame()
    {
        GameObject psystem = Instantiate(explosionPrefab, player.position, explosionPrefab.transform.rotation);
        var main = psystem.GetComponent<ParticleSystem>().main; 
        main.startColor = player.GetComponent<SpriteRenderer>().color;
        Destroy(player.gameObject);
        Destroy(psystem, 10f);

        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(secondsToRestart);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
