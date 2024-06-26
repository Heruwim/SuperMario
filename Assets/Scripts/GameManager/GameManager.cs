using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int World { get; private set; }
    public int Stage { get; private set; }
    public int Lives { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        NewGame();
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    private void NewGame()
    {
        Lives = 3;
        LoadLevel(1, 1);
    }

    private void LoadLevel(int world, int stage)
    {
        World = world;
        Stage = stage;
        SceneManager.LoadScene($"{World}-{Stage}");
    }

    public void NextLevel()
    {
        LoadLevel(World, Stage + 1);
    }

    public void ResetLevel()
    {
        Lives--;
        if (Lives > 0)
        {
            LoadLevel(World, Stage);
        }
        else
        {
            GameOver();
        }
    }

    public void ResetLevel(float delay)
    {
        Invoke(nameof(ResetLevel), delay);
    }

    private void GameOver()
    {
        NewGame();
    }
}
