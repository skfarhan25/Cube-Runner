using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    bool GameEnd = false;
    public PlayerMovement Player;
    public Distance distance;
    public Transform goal;
    public GameObject LevelCompleteUI;
    public AudioManager audioManager;
    public AudioMixer mixer;
    public TMP_InputField inputField;
    public int score;
    const string MIXER_EFFECT = "LowPassFreq";
    public static GameManager instance;
    public string PlayerName;
    public Text highscore;
    public int highscoreValue;
    public GameObject panel;
    public GameObject OptionsMenu;
    public PlayfabManager playfabManager;
    public GameObject NameAndEnter;
    public GameObject PlayAndHighscore;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1f;
        if (mixer.GetFloat(MIXER_EFFECT, out float value))
        {
            if (value != 22000)
            {
                IncreaseLowBass();
            }
        }

        if (SceneManager.GetActiveScene().buildIndex == 16)
        {
            DecreaseLowBass();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (Input.GetKeyDown(KeyCode.Space) && GameEnd && Player.isActiveAndEnabled)
        {
            LoadNextLevel();
        }
    }

    public void WinGame()
    {
        if (!GameEnd)
        { 
            IncreaseScore();
            distance.DistanceText.text = goal.position.z.ToString("0");
            Debug.Log("You Win!");
            GameEnd = true;
            Player.enabled = true;
            distance.enabled = false;
            LevelCompleteUI.SetActive(true);
            DecreaseLowBass();
            playfabManager.SendLeaderboard(score);
            // playfabManager.GetLeaderboard();
        }
    }

    public void EndGame()
    {
        if (GameEnd == false)
        {
            Time.timeScale = 0.5f;
            Player.forwardforce = 0;
            Player.sidewaysforce = 0;
            Player.enabled = false;
            distance.enabled = false;
            GameEnd = true;
            Invoke("Restart", 1f);
        }
    }

    public void LoadPreviousLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            Debug.Log("No previous level to load");
        }        
    }

    public void LoadNextLevel()
    {
        // Checks if the current scene is the last scene in the build order
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            Debug.Log("You have reached the end of the game");
        }
    }

    public void LoadlLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public IEnumerator ChangeFrequency(float startvalue, float endvalue, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            mixer.SetFloat(MIXER_EFFECT, Mathf.Lerp(startvalue, endvalue, time / duration));
            yield return null;
        }
    }

    public void DecreaseLowBass()
    {
        StartCoroutine(ChangeFrequency(22000, 5000, 0.5f));
    }

    public void IncreaseLowBass()
    {
        StartCoroutine(ChangeFrequency(5000, 22000, 0.5f));
    }

    public void SaveName()
    {
        PlayerName = inputField.text;
        if (string.IsNullOrEmpty(PlayerName))
        {
            PlayerName = "PLY";
        }
        Debug.Log("ENTERED " + PlayerName);
        DuplicateCheck();
        playfabManager.Login();
    }

    public void DuplicateCheck()
    {
        if (PlayerPrefs.HasKey(PlayerName)) // if duplicate
        {
            Debug.Log("Welcome back " + PlayerName);
        }
        else // if new
        {
            PlayerPrefs.SetString(PlayerName, PlayerName);
            PlayerPrefs.SetInt(PlayerName, 0);
            Debug.Log("New player Saved " + PlayerName);
        }
    }

    [ContextMenu("ShowName")]
    public void LoadName()
    {
        Debug.Log("Loaded " + PlayerName);
    }

    public void IncreaseScore()
    {
        score = SceneManager.GetActiveScene().buildIndex;
        if (PlayerPrefs.HasKey(PlayerName)) // If key exists
        {
            if (PlayerPrefs.GetInt(PlayerName) < score) // if new highscore
            {
                PlayerPrefs.SetInt(PlayerName, score); // save new highscore
                Debug.Log("New highscore, Old Player: " + score);
                highscoreValue = score;
                PlayerPrefs.SetInt("Highscore", highscoreValue);
            }
            else // if not new highscore
            {
                Debug.Log("Highscore: " + PlayerPrefs.GetInt(PlayerName)); // load highscore
            }
        }
        else // if key doesn't exist
        {
            PlayerPrefs.SetInt(PlayerName, score); // save new highscore
            Debug.Log("New highscore, New Player: " + score);
            highscoreValue = score;
            PlayerPrefs.SetInt("Highscore", highscoreValue);
        }
    }

    public void SetHighscoreText()
    {
        if (PlayerPrefs.HasKey(PlayerName))
        {
            highscore.text = "Highscore: " + PlayerPrefs.GetInt("Highscore"); 
        }
    }

    [ContextMenu("LoadScore")]
    public void LoadScore()
    {
        Debug.Log("Score: " + PlayerPrefs.GetInt(PlayerName));
    }

    [ContextMenu("DeleteAll")]
    private void PlayerprefsDeleteAll()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All PlayerPrefs deleted");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Pause()
    {
        if (Time.timeScale == 1)
        {
            panel.SetActive(true);
            OptionsMenu.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            panel.SetActive(false);
            OptionsMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}