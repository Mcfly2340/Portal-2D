using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Dictionary<int, Vector3> savedPositions = new Dictionary<int, Vector3>();

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
        // Save player position before loading scene:
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        savedPositions[sceneIndex] = PlayerController.player.transform.position;
        SceneManager.LoadScene("Main Scene");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // After loading scene, check if we need to move player to previously-saved position:
        if (savedPositions.ContainsKey(scene.buildIndex))
        {
            PlayerController.player.transform.position = savedPositions[scene.buildIndex];
        }
        else
        {
            PlayerController.player.transform.position = PlayerController.playerSpawnPos.transform.position;
        }
    }
}
