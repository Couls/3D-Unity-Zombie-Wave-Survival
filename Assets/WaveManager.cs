using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
[System.Serializable]
public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject baseZombie;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject ScoreUI;
    [SerializeField] private GameObject WaveUI;
    [SerializeField] public AudioClip WaveComplete;
    [SerializeField] private int CurrentScore = 0;
    [SerializeField] private int CurrentWave = 0;
    private int ZombiesPerWave = 3;
    private int ZombiesLeftinWave = 3;
    private int ZombiesToSpawn = 3;
    public void UpdateData()
    {
        WaveUI.GetComponent<TMP_Text>().text = "Wave: " + CurrentWave;
        ScoreUI.GetComponent<TMP_Text>().text = "Score: " + CurrentScore.ToString("0000");
    }
    void Start()
    {
        newWave();
    }
    public void ZombieDeath()
    {
        ZombiesLeftinWave -= 1;
        CurrentScore += 50;
        ScoreUI.GetComponent<TMP_Text>().text = "Score: " + CurrentScore.ToString("0000");
        if (ZombiesLeftinWave <= 0)
        {
            newWave();
        }
    }
    // All zombies dead or game starting, increment wave
    void newWave()
    {
        CurrentWave += 1; // increment wave
        if (CurrentWave == 7)
        {
            GameWon();
            return;
        }
        WaveUI.GetComponent<TMP_Text>().text = "Wave: " + CurrentWave;
        AudioSource audiosource = GetComponent<AudioSource>();
        audiosource.PlayOneShot(WaveComplete);
        ZombiesPerWave = (int)Mathf.Pow(CurrentWave, 2); // 1 -> 4 -> 8 -> 16 -> 25 -> 36
        ZombiesLeftinWave = ZombiesPerWave;
        ZombiesToSpawn = ZombiesPerWave;
        SpawnZombies();
    }
    void SpawnZombies() // loop through potential spawn points, spawning zombies until we're done
    {
        for (int i = 0; i < ZombiesToSpawn; i++)
        {
            Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            GameObject freshie = GameObject.Instantiate(baseZombie);
            freshie.SetActive(true);
            freshie.transform.position = spawnPosition;
        }
    }
    void GameWon()
    {
        SceneManager.LoadScene("GameWon");
    }
}
