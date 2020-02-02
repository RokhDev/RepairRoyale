using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Player[] players;
    public int repairsToWin;
    public string winSceneName;
    public float sceneChangeTime;

    public static Sprite winGraphic;

    private Dictionary<Player, int> playerResources = new Dictionary<Player, int>();
    private bool gameOver = false;
    private float sceneChangeCounter;
    private AudioSource takeoff;

    private void Awake()
    {
        foreach (Player p in players)
        {
            playerResources.Add(p, 0);
        }
        sceneChangeCounter = sceneChangeTime;
        winGraphic = null;
        takeoff = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(winSceneName);
        }

        if (!gameOver) { return; }
        
        sceneChangeCounter -= Time.deltaTime;

        if (sceneChangeCounter <= 0)
        {
            SceneManager.LoadScene(winSceneName);
        }
    }

    public void RepairShip(Player player, int resources)
    {
        playerResources[player] += resources;
        
        if (playerResources[player] >= repairsToWin)
        {
            GameOver(player);
        }
    }

    private void GameOver(Player winner)
    {
        foreach (Player p in players)
        {
            p.GameOver(winner);
        }
        gameOver = true;
        takeoff.Play();
    }
}
