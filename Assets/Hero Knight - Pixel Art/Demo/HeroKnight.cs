using UnityEngine;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;
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
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
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

    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
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
            m_rolling = false;

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

        // -- Handle input and movement --
        // float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        /*if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }*/

        // Manual movemet; DISABLED
        /*if (!m_rolling )
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);*/

        // Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        // Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        /*// Death
        if (Input.GetKeyDown("e") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }
            
        // Hurt
        else if (Input.GetKeyDown("q") && !m_rolling)
            m_animator.SetTrigger("Hurt");*/

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
        if (Input.GetKey(KeyCode.Space) && !m_rolling)
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
        if (Input.GetKeyUp(KeyCode.Space) && !m_rolling && !m_isBlocking)
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
        }

        // Trigger an action if the combo timer exceeds the combo reset time
        if (m_timeSinceFirstSpacePress > m_comboResetTime && m_comboStarted)
        {
            if (!isRunning) // Combat mode
            {
                // Trigger the appropriate attack based on the number of presses
                m_animator.SetTrigger("Attack" + m_spacePresses);
                Debug.Log("Attack" + m_spacePresses);
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
        if (Input.GetKeyUp(KeyCode.Space) && m_isBlocking)
        {
            m_animator.SetBool("IdleBlock", false);
            m_isBlocking = false;
            m_timeSinceSpacePress = 0.0f;
        }

        /*//Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }
        */
    }

    private void FixedUpdate()
    {
        // Automatic movement
        m_body2d.velocity = new Vector2(m_speed, m_body2d.velocity.y);
        m_animator.SetInteger("AnimState", 1);
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
