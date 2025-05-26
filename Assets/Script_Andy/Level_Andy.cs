using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using CodeMonkey;
using CodeMonkey.Utils;

public class Level_Andy : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;
    private const float PIPE_MOVE_SPEED = 30f;
    private const float  PIPE_DESTROY_X_POSITION = -100f;
    private const float  PIPE_Spawn_X_POSITION = 100f;
    private const float BIRD_X_POSITION = 0f;
    
    private static Level_Andy instance ;

    public static Level_Andy GetInstance()
    {
        return instance;
    }

    private List<Pipe> pipeList;
    private int pipesPassedCount;
    private int pipeSpawned;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    private float gapSize;
    private State state;


    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible,
    }

    private enum State
    {
        WaitingToStart,
        Playing,
        BirdDead,
        
    }

    private void Awake()
    {
        instance = this;
        pipeList = new List<Pipe>();
        pipeSpawnTimerMax = 1f;
        SetDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;
    }

    private void Start()
    {
        Bird_Andy.GetInstance().OnDied += Bird_OnDied;
        Bird_Andy.GetInstance().OnStartedPlaying += Bird_OnStartedPlaying;
    }

    private void Bird_OnStartedPlaying(object sender, System.EventArgs e)
    {
        state = State.Playing;
    }

    private void Bird_OnDied(object sender, System.EventArgs e)
    {
        // CMDebug.TextPopupMouse("Dead!!");
        Debug.Log("🟥 Level_Andy 收到鳥死亡事件，改狀態為 BirdDead");
        state = State.BirdDead;
    }

    private void Update()
    {
        if (state == State.Playing)
        {
            Debug.Log($"[Level Update] State = {state}");
            HandlePipeMovement();
            HandlePipeSpawning();
        }
    }

    private void HandlePipeSpawning()
    {
        pipeSpawnTimer -= Time.deltaTime;
        if (pipeSpawnTimer <= 0f)
        {
            //Time to spawn another Pipe
            pipeSpawnTimer += pipeSpawnTimerMax;
            float heightEdgeLimit = 10f;
            float minHeight = gapSize * .5f+heightEdgeLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;
            float maxHeight = totalHeight -gapSize*.5f-heightEdgeLimit;
            float height = Random.Range(minHeight, maxHeight);
            CreateGapPipes(height,gapSize,PIPE_Spawn_X_POSITION);
        }

    }

    private void HandlePipeMovement()
    {
        for(int i=0;i<pipeList.Count;i++)
        {
            Pipe pipe = pipeList[i];
            bool isToTheRightOfBird = pipe.GetXPosition()>=BIRD_X_POSITION;
            pipe.Move();
            if (isToTheRightOfBird && pipe.GetXPosition() <= BIRD_X_POSITION && pipe.IsBottom())
            {
                //Pipe passed Bird
                pipesPassedCount++;
                SoundManager_Andy.PlaySound(SoundManager_Andy.Sound.Score);
            }

            if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION)
            {//Destory Pipe
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }

        }
        
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gapSize = 50f;
                pipeSpawnTimerMax = 1.2f;
                break;
            case Difficulty.Medium:
                gapSize = 40f;
                pipeSpawnTimerMax = 1.1f;
                break;
            case Difficulty.Hard:
                gapSize = 33f;
                pipeSpawnTimerMax = 1.0f;
                break;
            case Difficulty.Impossible:
                gapSize = 24f;
                pipeSpawnTimerMax = .9f;
                break;
            
        }
    }

    private Difficulty GetDifficulty()
    {  
        if(pipeSpawned >=30)return Difficulty.Impossible;
        if(pipeSpawned>=20)return Difficulty.Hard;
        if(pipeSpawned>=10)return Difficulty.Medium;
        return Difficulty.Easy;
        
        
    }

    private void CreateGapPipes(float gapY, float gapSize,float xPosition)
    {
        CreatePipe(gapY-gapSize*.5f,xPosition,true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2f- gapY-gapSize*.5f,xPosition,false);
        pipeSpawned++;
        SetDifficulty(GetDifficulty());

    }

    private void CreatePipe(float height,float xPosition,bool createBottom)
    {
        //Set Up Head
        Transform pipeHead = Instantiate(GameAssets_Andy.GetInstance().pfPipeHead);
        float pipeHeadYposition;
        if (createBottom)
        {
            pipeHeadYposition = -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * .5f;
        }
        else
        {
            pipeHeadYposition = CAMERA_ORTHO_SIZE - height + PIPE_HEAD_HEIGHT * .5f;
        }

        pipeHead.position = new Vector3(xPosition,pipeHeadYposition);
        //Set Up Body
        Transform pipeBody = Instantiate(GameAssets_Andy.GetInstance().pfPipeBody);
        float pipeBodyYposition;
        if (createBottom)
        {
            pipeBodyYposition = -CAMERA_ORTHO_SIZE;
        }
        else
        {
            pipeBodyYposition = CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }

        pipeBody.position = new Vector3(xPosition, pipeBodyYposition);
        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size =new Vector2(PIPE_WIDTH,height);
        BoxCollider2D pipeBodyBoxCollider2D = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider2D.size = new Vector2(PIPE_WIDTH,height);
        pipeBodyBoxCollider2D.offset = new Vector2(0f,height * .5f);
        
        Pipe pipe = new Pipe(pipeHead, pipeBody,createBottom);
        pipeList.Add(pipe);
        

    }

    public int GetPipeSpawned()
    {
        return pipeSpawned;
    }

    public int GetPipesPassedCount()
    {
        return pipesPassedCount;
    }
    //Represents a Single Pipe
    
    private class  Pipe
    {
        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool isBottom;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform,bool isBottom)
        {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
            this.isBottom = isBottom;
        }

        public void Move()
        {
            pipeHeadTransform.position+= new Vector3(-1,0,0)*PIPE_MOVE_SPEED*Time.deltaTime;
            pipeBodyTransform.position+= new Vector3(-1,0,0)*PIPE_MOVE_SPEED*Time.deltaTime;
            
        }

        public float GetXPosition()
        {
            return pipeHeadTransform.position.x;
        }

        public bool IsBottom()
        {
            return isBottom;
        }

        public void DestroySelf()
        {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
            
        }
    }
}
