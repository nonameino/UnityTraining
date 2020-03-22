using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float turnDelay = 0.1f;
    public static GameManager instance = null;
    private BoardManager boardScript;
	private int level = 1;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;
    private List<Enemy> enemies;
    private bool enemiesMoving;	

    //Awake is always called before any Start functions
	void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
		
		DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
		boardScript = GetComponent<BoardManager>();
		InitGame();
	}

    void InitGame() {
        enemies.Clear();
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
		if(playersTurn || enemiesMoving)
			return;
		
		StartCoroutine (MoveEnemies ());
    }

    public void AddEnemyToList(Enemy script) {
		enemies.Add(script);
	}

    IEnumerator MoveEnemies() {
		enemiesMoving = true;
		yield return new WaitForSeconds(turnDelay);
		
		if (enemies.Count == 0) {
			yield return new WaitForSeconds(turnDelay);
		}
		
		for (int i = 0; i < enemies.Count; i++) {
			enemies[i].MoveEnemy ();
			yield return new WaitForSeconds(enemies[i].moveTime);
		}

		playersTurn = true;
		enemiesMoving = false;
	}
}
