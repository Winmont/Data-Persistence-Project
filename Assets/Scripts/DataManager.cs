using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    [SerializeField] private InputField input;

    public string playerName;
    public int highscore = 0;
    public string highscoreName;
    public string dataPath;

    public Text ScoreText;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighscoreAndName();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateInputField();
        UpdateHighscoreText();
    }

    private void Update()
    {
        
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int highscore;
        public string highscoreName;
    }

    public void SaveHighscoreAndName()
    {
        SaveData data = new SaveData();
        data.playerName = playerName;
        data.highscore = highscore;
        data.highscoreName = highscoreName;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        
    }

    public void LoadHighscoreAndName()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        dataPath = path;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerName = data.playerName;
            highscore = data.highscore;
            highscoreName = data.highscoreName;
        }
    }

    public void UpdateName()
    {
        
        if (ScoreText != null)
        {
            playerName = input.text;
        }
    }

    private void UpdateInputField()
    {
        input.text = playerName;
    }

    public void Exit()
    {
        SaveHighscoreAndName();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else

        Application.Quit();
#endif
    }

    public void StartGame()
    {
        SaveHighscoreAndName();
        SceneManager.LoadScene(1);
    }

    public void Test()
    {
        Debug.Log("Test!");
    }

    public void EvaluateScore(int score)
    {
        if (score > highscore)
        {
            highscoreName = playerName;
            highscore = score;
            UpdateHighscoreText();
            SaveHighscoreAndName();
        }
    }

    public void UpdateHighscoreText()
    {
        ScoreText = GameObject.Find("ScoreText (1)").GetComponent<Text>();
        if (ScoreText != null)
        {
            ScoreText.text = "Best Score: " + highscoreName + " : " + highscore;
        }
    }

   }
