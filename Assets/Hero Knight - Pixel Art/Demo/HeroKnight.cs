using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    public float Speed
    {
        get
        {
            return m_speed;
        }
    }

    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] GameObject m_slideDust;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;

    private bool                m_rolling = false;
    public bool Rolling
    {
        get
        {
            return m_rolling;
        }
    }

    private int                 m_facingDirection = 1;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;

    public bool isRunning = false;
    private int m_spacePresses = 0;       
    private float m_timeSinceFirstSpacePress = 0.0f;
    private float m_comboResetTime; 
    private bool m_comboStarted = false;
    private float m_timeSinceSpacePress = 0.0f;

    private bool m_isBlocking = false;
    public bool IsBlocking
    {
        get
        {
            return m_isBlocking;
        }
    }

    [SerializeField] private GameObject playerHud;
    private PlayerHUD playerHudScript;

    [SerializeField] private Transform attackPoint;
    private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayers;

    public int maxHealth = 100;
    private int currentHealth;

    public float maxStamina = 100f;
    private float currentStamina;
    private float staminaRegenRate = 0.2f;
    private float m_timeSinceLastSpacePress = 0.0f;

    private Animator animator;

    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject staminaBar;

    [SerializeField] private FearMeter fearMeterScript;
    private float fearReset = 3f;
    private float fearResetTimer = 0f;
    private bool isReducingFear = false;

    [SerializeField] private GameObject runningControls;
    [SerializeField] private GameObject combatControls;

    public KeyCode activeKey = KeyCode.Space;
    private KeyCode[] alternativeKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.Q,
                                          KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.O, KeyCode.P, KeyCode.Space };

    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();

        playerHudScript = playerHud.GetComponent<PlayerHUD>();

        currentHealth = maxHealth;
        currentStamina = maxStamina;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls space combo
        m_timeSinceFirstSpacePress += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
        {
            m_rolling = false;
            m_rollCurrentTime = 0.0f;
        }

        // Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        // Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        switch (isRunning)
        {
            case true:
                m_comboResetTime = 0.17f;
                break;
            case false:
                m_comboResetTime = 0.35f;
                break;
        }

        // Block
        // Check if space bar is being held down
        if (Input.GetKey(activeKey) && !m_rolling)
        {
            m_timeSinceSpacePress += Time.deltaTime;

            if (m_timeSinceSpacePress > 0.2f && !m_isBlocking)
            {
                m_animator.SetTrigger("Block");
                m_animator.SetBool("IdleBlock", true);
                m_isBlocking = true;
                Debug.Log("Blocking");
            }
        }
        else
        {
            m_timeSinceSpacePress = 0.0f;
        }

        // Handle combo logic
        if (Input.GetKeyUp(activeKey) && !m_rolling && !m_isBlocking)
        {
            m_animator.SetBool("IdleBlock", false);

            // Reset the timer if this is the first space press
            if (!m_comboStarted)
            {
                m_timeSinceFirstSpacePress = 0.0f;
                m_comboStarted = true;  // Mark combo as started
            }

            // Register the number of presses (max 3)
            m_spacePresses = Mathf.Clamp(m_spacePresses + 1, 1, 3);

            m_timeSinceLastSpacePress = 0.0f;
        }

        // Trigger an action if the combo timer exceeds the combo reset time
        if (m_timeSinceFirstSpacePress > m_comboResetTime && m_comboStarted)
        {
            if (!isRunning) // Combat mode
            {
                int attackDamage = 0;
                int attackStaminaDrain = 0;

                switch (m_spacePresses)
                {
                    case 1:
                        attackDamage = 10;
                        attackStaminaDrain = 10;
                        break;
                    case 2:
                        attackDamage = 20;
                        attackStaminaDrain = 20;
                        break;
                    case 3:
                        attackDamage = 40;
                        attackStaminaDrain = 40;
                        break;
                }

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                foreach (Collider2D enemy in hitEnemies)
                {
                    if (currentStamina - attackStaminaDrain >= 0)
                    {
                        enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
                        currentStamina -= attackStaminaDrain;

                        // Trigger the appropriate attack based on the number of presses
                        m_animator.SetTrigger("Attack" + m_spacePresses);
                        playerHudScript.UpdatePlayerAttackInfo(m_spacePresses);
                    }
                }
            }
            else // Running mode
            {
                switch (m_spacePresses)
                {
                    case 1:
                        // Jump
                        if (m_grounded && !m_rolling && !m_isBlocking)
                        {
                            m_animator.SetTrigger("Jump");
                            m_grounded = false;
                            m_animator.SetBool("Grounded", m_grounded);
                            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                            m_groundSensor.Disable(0.2f);

                            Debug.Log("Jump");
                        }
                        break;
                    case 2:
                    case 3:
                        // Roll
                        if (!m_rolling && !m_isWallSliding && !m_isBlocking)
                        {
                            m_rolling = true;
                            m_animator.SetTrigger("Roll");
                            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);

                            Debug.Log("Roll");
                        }
                        break;
                }
            }

            // Reset variables for the next combo input
            m_spacePresses = 0;
            m_comboStarted = false;
            m_timeSinceFirstSpacePress = 0.0f;
        }

        // Stop blocking when space is released
        if (Input.GetKeyUp(activeKey) && m_isBlocking)
        {
            m_animator.SetBool("IdleBlock", false);
            m_isBlocking = false;
            m_timeSinceSpacePress = 0.0f;
        }

        m_timeSinceLastSpacePress += Time.deltaTime;

        if (m_timeSinceLastSpacePress > 1f && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }

        if (fearResetTimer < fearReset)
        {
            fearResetTimer += Time.deltaTime;
        }
        else if (!isReducingFear)
        {
            StartCoroutine(ReduceFearLevel());
        }
    }

    private void FixedUpdate()
    {
        // Automatic movement
        if (isRunning)
        {
            m_body2d.velocity = new Vector2(m_speed, m_body2d.velocity.y);
            m_animator.SetInteger("AnimState", 1);
        }
        else
        {
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void TakeDamage(int damage)
    {
        if (m_isBlocking)
        {
            return;
        }

        currentHealth -= damage;
        Debug.Log(currentHealth);
        animator.SetTrigger("Hurt");

        fearMeterScript.UpdateFearLevel(5);
        ResetFearTimer();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        animator.SetTrigger("Death");
        m_body2d.gravityScale = 0;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        StartCoroutine(RestartGame());
    }

    private IEnumerator ReduceFearLevel()
    {
        isReducingFear = true;

        while (fearMeterScript.CurrentFear > 0 && fearResetTimer >= fearReset)
        {
            fearMeterScript.UpdateFearLevel(-1);
            yield return new WaitForSeconds(1f);
        }

        isReducingFear = false;
    }

    public void ResetFearTimer()
    {
        fearResetTimer = 0f;

        // Switch active key
        KeyCode newActiveKey = alternativeKeys[UnityEngine.Random.Range(0, alternativeKeys.Length)];

        while (newActiveKey == activeKey)
        {
            newActiveKey = alternativeKeys[UnityEngine.Random.Range(0, alternativeKeys.Length)];
        }

        activeKey = newActiveKey;
        playerHudScript.UpdateActionSprite();
        Debug.Log("New active key: " + activeKey);

        StartCoroutine(FlashSprite());
    }

    private IEnumerator FlashSprite()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        for (int i = 0; i < 2; i++)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public float GetHealthPercentage()
    {
        return Mathf.Clamp((float)currentHealth / maxHealth, 0, 1);
    }

    public float GetStaminaPercentage()
    {
        return Mathf.Clamp((float)currentStamina / maxStamina, 0, 1);
    }

    public void UpdateGameMode(bool isCombatMode)
    {
        if (isCombatMode)
        {
            isRunning = false;
            healthBar.SetActive(true);
            staminaBar.SetActive(true);

            combatControls.SetActive(true);
            runningControls.SetActive(false);
        }
        else
        {
            isRunning = true;
            healthBar.SetActive(false);
            staminaBar.SetActive(false);

            combatControls.SetActive(false);
            runningControls.SetActive(true);
        }
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}