using System;
using UnityEngine;
using CodeMonkey;
public class Bird_Andy : MonoBehaviour
{
    private const float Jump_Amount = 100f;
    private static Bird_Andy instance;

    public static Bird_Andy GetInstance()
    {
        return instance;
    }

    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;
    private Rigidbody2D birdRigidbody2D;
    private State state;
    private enum State
    {
        WaitingToStart,
        Playing,
        Dead,
        
    }

    private void Awake()
    {
        instance = this;
        birdRigidbody2D = GetComponent<Rigidbody2D>();
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
    }
    private void Start()
    {
        Score_Andy.Start(); // ✅ 等 Bird 自己準備好時自己叫分數系統初始化
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (Input.GetKeyDown(KeyCode.Space)||Input.GetMouseButtonDown(0))
                {
                    state = State.Playing;
                    birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if(OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);

                }
                break;
            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space)||Input.GetMouseButtonDown(0))
                {
                    Jump();

                }
                break;
            case State.Dead:
                break;
            
                
            
        }

    }

    void Jump()
    {
        birdRigidbody2D.linearVelocity = Vector2.up * Jump_Amount;
        SoundManager_Andy.PlaySound(SoundManager_Andy.Sound.birdJump);
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        SoundManager_Andy.PlaySound(SoundManager_Andy.Sound.Lose);
        if (OnDied!=null)OnDied(this, EventArgs.Empty);
    }

}
