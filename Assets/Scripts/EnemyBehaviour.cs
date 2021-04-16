using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public class Obstacles
    {
        public int speed;
        public int hp;
        public int scoreValue;
        public float scale;
    }

    public class Meteorites : Obstacles
    {
        public Meteorites(int speed, int hp, int scoreValue, float scale)
        {
            this.speed = speed;
            this.hp = hp;
            this.scoreValue = scoreValue;
            this.scale = scale;
        }
    }

    //Meteorite types - (spd, hp, score, scale)
    static Meteorites Big = new Meteorites(3, 45, 30, 0.4f);
    static Meteorites Medium = new Meteorites(6, 30, 20, 0.3f);
    static Meteorites Small = new Meteorites(9, 15, 10, 0.2f);
    Obstacles[] typesOfObstacles = { Big, Medium, Small };

    //Object properties
    int objectSpeed;
    int objectHp;
    int objectScoreValue;

    float speedMultiplier = 1;

    //Links
    GameObject managerObj;
    GameManager gameManager;
    GameObject player;
    int playerDmg;

    //Choosing type of obstacle
    Obstacles chooseObstacle()
    {
        return typesOfObstacles[Random.Range(0, typesOfObstacles.Length)];
    }

    //Movement
    void enemyMovement()
    {
        if (this.gameObject.transform.position.y < -6)
        {
            addScore(objectScoreValue / 2);
            Destroy(this.gameObject);
        }
        else 
        {
            this.gameObject.transform.Translate(Vector2.down * objectSpeed * speedMultiplier * Time.deltaTime);
        }
    }

    void addScore(int add)
    {
        gameManager.score += add;
        gameManager.changeText();
    }

    //Called when in collision, taking damage from bullet
    void takeDamage(int dmg)
    {
        objectHp -= dmg;
        
        if (objectHp <= 0)
        {
            addScore(objectScoreValue);
            Destroy(this.gameObject);
        }
    }

    //Handles damage delivery for player and meteorite
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            takeDamage(playerDmg);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerBehaviour>().takeDamage();
            gameManager.changeText();
            Destroy(this.gameObject);
        }
    }

    //Setting variables when object spawns
    void Awake()
    {
        managerObj = GameObject.FindGameObjectWithTag("Manager");
        gameManager = managerObj.GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerDmg = player.GetComponent<PlayerBehaviour>().playerDamage;

        Obstacles chosenOne = chooseObstacle();
        this.gameObject.transform.localScale = new Vector3(chosenOne.scale, chosenOne.scale, 1);
        objectSpeed = chosenOne.speed;
        objectHp = chosenOne.hp;
        objectScoreValue = chosenOne.scoreValue;
    }

    void Update()
    {
        enemyMovement();
    }
}
