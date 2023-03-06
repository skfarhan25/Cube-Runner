using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
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
    private string PlayerName;

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
        if (mixer.GetFloat(MIXER_EFFECT, out float value))
        {
            if (value != 22000)
            {
                IncreaseLowBass();
            }
        }
    }

    public void WinGame()
    {
        if (!GameEnd)
        { 
            IncreaseScore(GetName());
            distance.DistanceText.text = goal.position.z.ToString("0");
            Debug.Log("You Win!");
            GameEnd = true;
            Player.enabled = false;
            distance.enabled = false;
            LevelCompleteUI.SetActive(true);
            DecreaseLowBass();
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
        Debug.Log("ENTERED " + PlayerName);
        DuplicateCheck();
    }

    public void DuplicateCheck()
    {
        if (PlayerName == "") // if empty
        {
            PlayerName = "PLY";
            PlayerPrefs.SetString(PlayerName, PlayerName);
            Debug.Log("No name entered" + PlayerName);
            
        }
        if (PlayerPrefs.HasKey(PlayerName)) // if duplicate
        {
            Debug.Log("Welcome back " + PlayerName);
        }
        else // if new
        {
            PlayerPrefs.SetString(PlayerName, PlayerName);
            Debug.Log("Saved " + PlayerName);
        }
    }

    [ContextMenu("LoadName")]
    public void LoadName()
    {
        Debug.Log("Loaded " + PlayerName);
    }

    [ContextMenu("GetName")]
    public string GetName()
    {
        return PlayerName;
    }

    public void IncreaseScore(string keyname)
    {
        score = SceneManager.GetActiveScene().buildIndex;
        if (PlayerPrefs.HasKey(keyname)) // if key exists
        {
            if (PlayerPrefs.GetInt(keyname) < score) // if new highscore
            {
                PlayerPrefs.SetInt(keyname, score); // save new highscore
                Debug.Log("New highscore, Old Player: " + score); 
            }
            else // if not new highscore
            {
                Debug.Log("Highscore: " + PlayerPrefs.GetInt(keyname)); // load highscore
            }
        }
        else // if key doesn't exist
        {
            PlayerPrefs.SetInt(keyname, score); // save new highscore
            Debug.Log("New highscore, New Player: " + score);
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
}    
