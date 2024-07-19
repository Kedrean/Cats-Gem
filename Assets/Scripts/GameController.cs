using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject[] catPrefabs;
    public GameObject[] gemPrefabs;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;
    public TMP_Text catsFoundText;
    public float gameTime = 60f;
    public int scoreToWin = 50;
    public AudioSource musicSource; // Reference to the AudioSource for background music
    public AudioSource sfxSource; // Reference to the AudioSource for SFX
    public AudioClip bgMusic; // Background music
    public AudioClip catGemSFX; // SFX for clicking cats and gems
    public AudioClip menuSFX; // SFX for menu interactions

    private int score;
    private float timer;
    private List<Vector3> catPositions = new List<Vector3>();
    private Vector3 catScale = new Vector3(0.05f, 0.05f, 0.05f);
    private float spawnBuffer = 2f;

    void Start()
    {
        score = 0;
        timer = gameTime;
        gameOverPanel.SetActive(false);
        catsFoundText.gameObject.SetActive(false);

        SpawnCats();
        UpdateUI();

        if (musicSource != null)
        {
            musicSource.clip = bgMusic; // Set background music
            musicSource.loop = true; // Loop the background music
            musicSource.Play(); // Start playing background music
        }
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateUI();
            if (score >= scoreToWin)
            {
                GameOver(true);
            }
        }
        else
        {
            GameOver(false);
        }
    }

    void SpawnCats()
    {
        for (int i = 0; i < 10; i++)
        {
            SpawnCat();
        }
    }

    void SpawnCat()
    {
        Vector3 position = GetValidSpawnPosition();
        GameObject cat = Instantiate(GetRandomCatPrefab(), position, Quaternion.identity);
        cat.transform.localScale = catScale;
        catPositions.Add(position);
    }

    void SpawnGem()
    {
        Vector3 position = GetValidSpawnPosition();
        GameObject gem = Instantiate(GetRandomGemPrefab(), position, Quaternion.identity);
        gem.transform.localScale = catScale;
        StartCoroutine(DestroyGemAfterDelay(gem, 1f));
    }

    Vector3 GetValidSpawnPosition()
    {
        Vector3 randomPosition;
        bool positionValid;

        float halfWidth = 18f / 2f;
        float halfHeight = 9f / 2f;
        float colliderWidth = 128f / 100f;
        float colliderHeight = 128f / 100f;

        do
        {
            randomPosition = new Vector3(
                Random.Range(-halfWidth + colliderWidth / 2, halfWidth - colliderWidth / 2),
                Random.Range(-halfHeight + colliderHeight / 2, halfHeight - colliderHeight / 2),
                0
            );
            positionValid = true;

            foreach (Vector3 pos in catPositions)
            {
                if (Vector3.Distance(randomPosition, pos) < spawnBuffer)
                {
                    positionValid = false;
                    break;
                }
            }
        }
        while (!positionValid);

        return randomPosition;
    }

    public void IncreaseScoreAndReplaceCat(GameObject cat)
    {
        if (cat != null)
        {
            StartCoroutine(HandleCatClick(cat));
            PlaySFX(catGemSFX); // Play SFX for cat/gem interactions
        }
    }

    private System.Collections.IEnumerator HandleCatClick(GameObject cat)
    {
        SpriteRenderer renderer = cat.GetComponent<SpriteRenderer>();
        renderer.color = Color.red;
        yield return new WaitForSeconds(1f);

        Destroy(cat);
        catPositions.Remove(cat.transform.position);
        score++;
        UpdateUI();

        if (score % 10 == 0)
        {
            SpawnGem();
        }
        
        if (score < scoreToWin)
        {
            SpawnCat();
        }
    }

    public void HandleGemInteraction(bool isGameOver)
    {
        if (isGameOver)
        {
            GameOver(false);
        }
        else
        {
            timer += 5f;
            UpdateUI();
        }
        PlaySFX(catGemSFX); // Play SFX for cat/gem interactions
    }

    void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    GameObject GetRandomCatPrefab()
    {
        return catPrefabs[Random.Range(0, catPrefabs.Length)];
    }

    GameObject GetRandomGemPrefab()
    {
        return gemPrefabs[Random.Range(0, gemPrefabs.Length)];
    }

    private System.Collections.IEnumerator DestroyGemAfterDelay(GameObject gem, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gem);
    }

    void UpdateUI()
    {
        scoreText.text = score.ToString();
        timerText.text = Mathf.Round(timer).ToString();
    }

    void GameOver(bool win)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = win ? "You Win!" : "Game Over";

        if (!win)
        {
            catsFoundText.text = $"You Found: {score} cats";
            catsFoundText.gameObject.SetActive(true);
        }
        else
        {
            catsFoundText.gameObject.SetActive(false);
        }

        Time.timeScale = 0;

        if (musicSource != null)
        {
            musicSource.Stop(); // Stop the music when the game is over
        }
    }

    public void OnMenuButtonClick()
    {
        PlaySFX(menuSFX); // Play menu button click SFX
    }
}
