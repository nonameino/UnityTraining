using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private BoardManager boardScript;
	private int level = 1;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    //Awake is always called before any Start functions
	void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
		
		DontDestroyOnLoad(gameObject);

		boardScript = GetComponent<BoardManager>();
		InitGame();
	}

    void InitGame() {
		boardScript.SetupScene(level);
	}

    public void GameOver() {
		enabled = false;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
