using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public float gravityModifier;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSfx;
    public AudioClip crashSfx;

    private Rigidbody rb;
    private InputAction jumpAction;
    private bool isOnGround = true;

    private Animator playerAnim;
    private AudioSource playerAudio;

    public bool gameOver = false;

    private int maxAirJump = 1;
    private int currentAirJump = 0;

    private InputAction DashAction;
    public bool isDash = false;

    public GameObject efxObstacle;
    public int playerHp = 10;



    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity *= gravityModifier;

        jumpAction = InputSystem.actions.FindAction("Jump");
        DashAction = InputSystem.actions.FindAction("Dash");

        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpAction.triggered && isOnGround && !gameOver)
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSfx);
        }
        else if (jumpAction.triggered && currentAirJump < maxAirJump) 
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSfx);

            currentAirJump++;
        }

        if (DashAction.IsPressed())
        {
            isDash = true;
        }
        else 
        {
            isDash = false;
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            currentAirJump = 0;

            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerHp -= 1;

            
            GameObject efxHit = Instantiate(efxObstacle, transform.position, Quaternion.identity);  //deleted FX_Explosion_Smoke from plyer and then will add from here #play on awake
            Destroy(efxHit, 0.5f);

            playerAudio.PlayOneShot(crashSfx);
            Destroy(collision.gameObject);

            if (playerHp <= 0)
            {
                Debug.Log("Game Over!");
                gameOver = true;
                playerAnim.SetBool("Death_b", true);
                playerAnim.SetInteger("DeathType_int", 1);
                dirtParticle.Stop();
            }
            


        }
    }

}