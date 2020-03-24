using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject gamePlayRoot;

    public static GameController instance;

    public void PlayGame() {
        gamePlayRoot.SetActive(true);
        this.GetComponent<SpawnController>().enabled = true;
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        GameController.instance = this;
        this.GetComponent<SpawnController>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        gamePlayRoot.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
