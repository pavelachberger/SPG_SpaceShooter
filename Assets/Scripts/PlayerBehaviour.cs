using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
    public class Spaceship
    {
        public int speed;
        public int hp;
        public int damage;
        public Sprite shipSprite;

        public Spaceship(int speed, int hp, int damage)
        {
            this.speed = speed;
            this.hp = hp;
            this.damage = damage;
        }
    }

    //Object properties
    int playerSpeed;
    public int playerHp;
    public int playerDamage;

    float fireRate = 0.3f;
    bool isShooting = false;
    
    //Spaceship constructors - (spd, hp, dmg)
    static Spaceship speedFocused = new Spaceship(10, 3, 5);
    static Spaceship hpFocused = new Spaceship(5, 5, 5);
    static Spaceship damageFocused = new Spaceship(5, 3, 10);
    Spaceship[] spaceShips = { speedFocused, hpFocused, damageFocused };

    //Links
    public GameObject managerObj;
    GameManager gameManager;
    public SpriteRenderer rend;
    public GameObject bullet;
    public GameObject damageVisual;

    public Sprite[] shipSprites;
    private int shipIndex = 0; //num of ship, 0=spd, 1=hp, 2=dmg

    //Time
    float remainingTime;
    float leaveTime;
    bool leftRange = false;
    public GameObject timerParent;
    public GameObject timer;

    //Player movement
    void playerMovement()
    {
        if (this.gameObject.transform.position.x < -4.25f || this.gameObject.transform.position.x > 4.25f)
        {

        }
        if (gameManager.gameStarted)
        {
            if (Input.GetKey("a"))
            {
                this.gameObject.transform.Translate(Vector2.left * playerSpeed * Time.deltaTime);
            }
            if (Input.GetKey("d"))
            {
                this.gameObject.transform.Translate(Vector2.right * playerSpeed * Time.deltaTime);
            }
        }
    }

    //Cycling through "shipSprites", changing ship as result
    public void chooseSpaceship(int direction)
    {
        //Changes sprite of ship
        shipIndex += direction;
        if (shipIndex < 0)
        {
            shipIndex = 2;
        }
        if (shipIndex > 2)
        {
            shipIndex = 0;
        }
        rend.sprite = shipSprites[shipIndex];

        //Changes atributes
        playerSpeed = spaceShips[shipIndex].speed;
        playerHp = spaceShips[shipIndex].hp;
        playerDamage = spaceShips[shipIndex].damage;

        Debug.Log("stats: spd" + playerSpeed + "hp" + playerHp + "dmg" + playerDamage);
    }

    IEnumerator shooting()
    {
        Instantiate(bullet, new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y), Quaternion.identity);
        yield return new WaitForSeconds(fireRate);
        StartCoroutine(shooting());
    }

    public void takeDamage()
    {
        showDamageVisual();
        playerHp -= 1;
        Debug.Log("HP: " + playerHp);
        if (playerHp <= 0)
        {
            gameManager.endGame();
        }
    }

    void showDamageVisual()
    {
        damageVisual.SetActive(true);
        StartCoroutine(RemoveVisual());

        IEnumerator RemoveVisual()
        {
            yield return new WaitForSeconds(0.5f);
            damageVisual.SetActive(false);
        }
    }

    void outOfRadar()
    {
        timerParent.SetActive(true);
        remainingTime = 3f;
        leaveTime = Time.deltaTime;
        leftRange = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "outOfRange")
        {
            outOfRadar();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "outOfRange")
        {
            leftRange = false;
            timerParent.SetActive(false);
        }
    }

    void Start()
    {
        gameManager = managerObj.GetComponent<GameManager>();
        rend = this.gameObject.GetComponent<SpriteRenderer>();
        chooseSpaceship(0);
    }

    void Update()
    {
        playerMovement();
        if (gameManager.gameStarted && !isShooting)
        {
            StartCoroutine(shooting());
            isShooting = true;
        }

        if (leftRange)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                timer.GetComponent<TextMeshProUGUI>().text = remainingTime.ToString("F0");
            }
            else
            {
                gameManager.endGame();
            }

        }
    }
}
