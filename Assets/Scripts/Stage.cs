using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    private SoundManager soundManager;

    [Header("게임 오브젝트 및 Transform")]
    public GameObject tilePref;
    public GameObject panel_GameOver;
    private GameObject GhostTetromino = null;

    public Transform Background;
    public Transform Board;
    public Transform Tetromino;
    public Transform Tetromino_Hold;
    public Transform Temp_Hold;

    private List<int> indexList;

    [Header("UI")]
    public Text txt_Score;
    public Text txt_PressAnyKey;

    [Header("ELSE")]
    [Range(0, 30)]
    public int width;
    [Range(0, 30)]
    public int height;
    
    private int width_half;
    private int height_half;
    private int howManyList = 5;

    [SerializeField]
    private int CurIndex = -1;

    [Header("시간 관련")]
    private float curTime = 0.0f;
    private float downTime = 1.0f;
    private float _downTime = 1.0f;
    private float timeC_down = 0.0f;
    private float maxTime = 0.05f;

    private float timeC_Max = 0.2f;
    private float timeC_Left = 0.0f;
    private float timeC_Left_2 = 0.0f;
    private float timeC_Right = 0.0f;
    private float timeC_Right_2 = 0.0f;

    private string playerName = null;

    void Start()
    {
        Init();

        BackgroundInit();

        BoardInit();

        CreateTetromino(Tetromino);

        txt_Score.text = "SCORE " + T.SCORE.ToString();
    }

    //씬 로드 시 변수 및 오브젝트 초기화
    void Init()
    {
        Panel_Pause.SetActive(false);
        panel_GameOver.SetActive(false);
        txt_PressAnyKey.gameObject.SetActive(true);

        indexList = new List<int>();

        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        //howmanyList 개수 만큼 리스트를 만듬
        for(int i=0;i<howManyList;i++)
        {
            indexList.Add(Random.Range(0,7));
            
            
        }

        T.SCORE = 0;
        T.CurrentGameState = GameState.Ready;

    }


public GameState gameState;
    void Update()
    {
        //inspector 확인용
        gameState = T.CurrentGameState;

        //게임 대기상태
        if(T.CurrentGameState == GameState.Ready)
        {
            if(Input.anyKeyDown)
            {
                T.CurrentGameState = GameState.Play;
                txt_PressAnyKey.gameObject.SetActive(false);
            }
        }


        //게임 플레이 중
        if(T.CurrentGameState == GameState.Play)
        {
            KeyInput();
        }


        //일시 정지 중
        if(T.CurrentGameState == GameState.Pause)
        {

            timePause += Time.deltaTime;
            if(timePause > timePause_Max && Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("this");
                Panel_Pause.SetActive(false);
                T.CurrentGameState = GameState.Play;
            }
            
        }


    }

    private float timePause = 0.0f;
    private float timePause_Max = 0.1f;


    void KeyInput()
    {
        Vector2 moveDir = Vector2.zero;
        bool isRotate = false;

        //일시정지
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Panel_Pause.SetActive(true);
            T.CurrentGameState = GameState.Pause;
            timePause = 0.0f;
            return;
        }

        //홀드
        if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.F))
        {
            HoldTetromino();
            return;
        }

        //좌
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if(timeC_Left == 0.0f)
            {
                MoveTetromino(Tetromino, Vector2.left, false);
            }

            if(timeC_Left < timeC_Max)
            {
                timeC_Left += Time.deltaTime;
            }

            else
            {
                timeC_Left_2 += Time.deltaTime;
                if(timeC_Left_2 > maxTime)
                {
                    MoveTetromino(Tetromino, Vector2.left, false);
                    timeC_Left_2 = 0.0f;
                }
                
            }
        }

        else
        {
            timeC_Left = 0.0f;
        }

        //우
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if(timeC_Right == 0.0f)
            {
                MoveTetromino(Tetromino, Vector2.right, false);
            }

            if(timeC_Right < timeC_Max)
            {
                timeC_Right += Time.deltaTime;
            }

            else
            {
                timeC_Right_2 += Time.deltaTime;
                if(timeC_Right_2 > maxTime)
                {
                    MoveTetromino(Tetromino, Vector2.right, false);
                    timeC_Right_2 = 0.0f;
                }
                
            }
        }

        else
        {
            timeC_Right = 0.0f;
        }

        //아래 한칸
        if(Input.GetKey(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            timeC_down += Time.deltaTime;
            if(timeC_down > maxTime)
            {
                MoveTetromino(Tetromino, Vector2.down, false);
                timeC_down = 0.0f;
                curTime = 0.0f;
            }
        }

        //회전
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            isRotate = true;
        }

        //아래로 쭉
        if(Input.GetKeyDown(KeyCode.Space))
        {
            while(MoveTetromino(Tetromino, Vector2.down, false));
        }

        //downTime마다 아래로 한칸씩
        curTime += Time.deltaTime;
        if(curTime > _downTime)
        {
            curTime = 0.0f;
            MoveTetromino(Tetromino, Vector2.down, false);
        }

        if(moveDir != Vector2.zero || isRotate)
        {
            MoveTetromino(Tetromino, moveDir, isRotate);
        }
    
    }

    public GameObject Panel_Pause;
    private bool canHold = true;
    void HoldTetromino()
    {
        if(canHold)
        {
            if(Tetromino_Hold.childCount == 0)
            {
                MoveParent(Tetromino, Tetromino_Hold);

                CreateTetromino(Tetromino);

                CreateGhostTetromino();
                canHold = false;
            }

            else
            {
                MoveParent(Tetromino, Temp_Hold);
                MoveParent(Tetromino_Hold, Tetromino);
                MoveParent(Temp_Hold, Tetromino_Hold);

                Tetromino.position = new Vector3(0,height_half-1, 0);

                CreateGhostTetromino();

                canHold = false;
            }
        }
        
        
    }

    void MoveParent(Transform from, Transform to)
    {
        for(int i=0;i<4; i++)
        {
            Transform node = from.GetChild(0);
            Vector3 pos = node.localPosition;
            Vector3 scale = node.localScale;

            node.parent = to;
            node.localPosition = pos;
            node.localScale = scale;
        }
        from.DetachChildren();
    }

    bool MoveTetromino(Transform root, Vector2 moveDir, bool isRotate)
    {
        Vector2 oldPos = root.position;
        Quaternion oldRot = root.rotation;

        root.position += (Vector3)moveDir;

        if(isRotate)
        {
            root.rotation *= Quaternion.Euler(0,0,90);
        }

        if(!CanMoveTo(root))
        {
            root.position = oldPos;
            root.rotation = oldRot;

            if(root.name == "Tetromino" && moveDir == Vector2.down && !isRotate)
            {
                AddToBoard();

                CheckBoard();

                StartCoroutine(MoveEffect());

                soundManager.Sound_Tetromino_Down();

                if(T.CurrentGameState == GameState.Play)
                {
                    CreateTetromino(Tetromino);
                    CreateGhostTetromino();
                }
            }

            return false;
        }

        if(root.name == "Tetromino")
            CreateGhostTetromino();

        return true;
    }

    //카메라를 움직인다.
    public Transform cam;
    IEnumerator MoveEffect()
    {
        Vector3 oldPos = cam.position;
        cam.position += new Vector3(Random.Range(0.0f, 0.2f), Random.Range(0.0f, 0.2f), 0);
        Vector3 changedPos = cam.position;
        
        float curT = 0.0f;
        while(cam.position != oldPos)
        {
            curT += Time.deltaTime;
            cam.position = Vector3.Lerp(cam.position, oldPos, curT);
            yield return null;
        }
        cam.position = oldPos;

        Debug.Log("end");
        yield return null;
    }
    
    public Transform MovingPart;
    void CreateGhostTetromino()
    {
        if(GhostTetromino != null)
        {
            Destroy(GhostTetromino.gameObject);
        }
        
        GhostTetromino = Instantiate(Tetromino.gameObject);

        GhostTetromino.transform.parent = MovingPart.transform;
        GhostTetromino.name = "GhostTetromino";
        GhostTetromino.transform.position = Tetromino.position;

        while(MoveTetromino(GhostTetromino.transform, Vector2.down, false));

        foreach(Transform node in GhostTetromino.transform)
        {
            Tile tile = node.GetComponent<Tile>();
            Color color = Color.grey;
            color.a = 0.7f;

            tile.color = color;
        }

    }

    void GameOver()
    {
        if(T.MAX_SCORE < T.SCORE)
        {
            T.MAX_SCORE = T.SCORE;
        }
        
        panel_GameOver.SetActive(true);
        T.CurrentGameState = GameState.End;
    }

    

    void CheckBoard()
    {
        bool isCleared = false;
        int howManyCol = 0;

        //꽉찬 줄 파괴
        foreach(Transform col in Board)
        {
            //게임오버
            if(col.name == (height-1).ToString() && col.childCount > 0)
            {
                GameOver();
            }


            //한줄 꽉 찼으면
            if(col.childCount == width)
            {
                for(int i=0;i<width;i++)
                {
                    Destroy(col.GetChild(i).gameObject);
                }
                col.DetachChildren();

                isCleared = true;
                howManyCol++;
            }
        }

        if(isCleared)
        {
            #region <점수 올리기>
            //점수 올리기
            int raiseScore = 0;
            switch(howManyCol)
            {
                //한 줄
                case 1:
                    raiseScore = 10;
                    break;

                //두 줄
                case 2:
                    raiseScore = 30;
                    break;
                
                //세 줄
                case 3:
                    raiseScore = 60;
                    break;
                
                //네 줄
                case 4:
                    raiseScore = 100;
                    break;
            }
            
            T.SCORE += raiseScore;

            txt_Score.text = "SCORE " + T.SCORE.ToString();
            #endregion

            //점수 만큼 테트로미노 떨어지는 속도 올리기
            _downTime = downTime * Mathf.Pow(0.8f, T.SCORE * 0.01f);
            Debug.Log("downtime : " + _downTime);

            for(int i=1;i<height;i++)
            {
                int emptyCol = 0;

                Transform curCol = Board.Find(i.ToString());

                if(curCol.childCount == 0) continue;

                int j = i-1;

                while(j>=0)
                {
                    Transform checkCol = Board.Find(j.ToString());
                    if(checkCol.childCount == 0)
                    {
                        emptyCol++;
                    }
                    j--;
                }

                if(emptyCol > 0)
                {
                    Transform TargetCol = Board.Find((i-emptyCol).ToString());
                    
                    //curCol 자식들을 -> TargetCol로 옮기기
                    while(curCol.childCount > 0)
                    {
                        Transform node = curCol.GetChild(0);
                        node.parent = TargetCol;
                        node.position += new Vector3(0, -emptyCol, 0);
                    }
                }

                
            }
        }
    }

    void AddToBoard()
    {
        while(Tetromino.childCount>0)
        {
            Transform node = Tetromino.GetChild(0);

            int x = Mathf.RoundToInt(node.position.x + width_half);
            int y = Mathf.RoundToInt(node.position.y + height_half);

            node.parent = Board.Find(y.ToString());
            node.name = x.ToString();

        }

        Tetromino.DetachChildren();
    }

    bool CanMoveTo(Transform root)
    {
        foreach(Transform node in root)
        {
            int x = Mathf.RoundToInt(node.position.x + width_half);
            int y = Mathf.RoundToInt(node.position.y + height_half);

            if(x < 0 || x >= width) return false;

            if(y < 0) return false;

            Transform col = Board.Find(y.ToString());
            if(col != null && col.Find(x.ToString()) != null)
            {
                return false;
            }
        }

        return true;
    }

    
    void CreateTetromino(Transform parent)
    {
        int CurIndex = indexList[0];

        indexList.RemoveAt(0);

        indexList.Add(Random.Range(0,7));

        if(parent.name == "Tetromino")
            parent.position = new Vector2(0, height_half-1);

        SetTetromino(CurIndex, parent);

        TetrominoIndexUpdate();

        canHold = true;
    }

    public Transform TetrominoIndex;
    void TetrominoIndexUpdate()
    {
        if(TetrominoIndex.childCount > 0)
        {
            Transform destroyNode = null;
            Transform destroyTile = null;

            for(int i=0;i<TetrominoIndex.childCount;i++)
            {
                destroyNode = TetrominoIndex.GetChild(i);

                //테트리스 타일 하나씩 파괴
                for(int j=0;j<destroyNode.childCount;j++)
                {
                    destroyTile = destroyNode.GetChild(j);
                    Destroy(destroyTile.gameObject);
                }
                destroyNode.DetachChildren();
                Destroy(destroyNode.gameObject);
            }
            TetrominoIndex.DetachChildren();
        }

        for(int i=0; i<howManyList; i++)
        {
            GameObject go = new GameObject(i.ToString());
            go.transform.parent = TetrominoIndex;
            go.transform.localPosition = new Vector2(0, -5*i);
            go.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            SetTetromino(indexList[i], go.transform);
        }
    }

    void SetTetromino(int index, Transform parent)
    {
        Color32 color = Color.white;

        switch(index)
        {
            //하늘색 l
            case 0:
                color = new Color32(153, 255, 255, 255);
                CreateTile(parent, new Vector2(0,0), color);
                CreateTile(parent, new Vector2(0,1), color);
                CreateTile(parent, new Vector2(0,-1), color);
                CreateTile(parent, new Vector2(0,-2), color);
                break;

            //주황색 ㄴ
            case 1:
                color = new Color32(255, 204, 153, 255);
                CreateTile(parent, new Vector2(0,0), color);
                CreateTile(parent, new Vector2(0,-1), color);
                CreateTile(parent, new Vector2(1,-1), color);
                CreateTile(parent, new Vector2(0,1), color);
                break;

            //핑크 ㅓ
            case 2:
                color = new Color32(255, 204, 255, 255);
                CreateTile(parent, new Vector2(0,1), color);
                CreateTile(parent, new Vector2(0,0), color);
                CreateTile(parent, new Vector2(0,-1), color);
                CreateTile(parent, new Vector2(-1,0), color);
                break;

            //노랑 ㅁ
            case 3:
                color = new Color32(255, 255, 153, 255);
                CreateTile(parent, new Vector2(0,1), color);
                CreateTile(parent, new Vector2(1,1), color);
                CreateTile(parent, new Vector2(0,0), color);
                CreateTile(parent, new Vector2(1,0), color);
                break;

            //파랑 ㄴ 좌우 반전
            case 4:
                color = new Color32(153, 204, 255, 255);
                CreateTile(parent, new Vector2(0,1), color);
                CreateTile(parent, new Vector2(0,0), color);
                CreateTile(parent, new Vector2(0,-1), color);
                CreateTile(parent, new Vector2(-1,-1), color);
                break;

            //초록 ㄱㄴ 좌우 반전
            case 5:
                color = new Color32(153, 255, 153, 255);
                CreateTile(parent, new Vector2(0,0), color);
                CreateTile(parent, new Vector2(0,1), color);
                CreateTile(parent, new Vector2(1, 1), color);
                CreateTile(parent, new Vector2(-1,0), color);
                break;

            //빨강 ㄱㄴ
            case 6:
                color = new Color32(255, 153, 153, 255);
                CreateTile(parent, new Vector2(0,0), color);
                CreateTile(parent, new Vector2(0,1), color);
                CreateTile(parent, new Vector2(-1, 1), color);
                CreateTile(parent, new Vector2(1,0), color);
                break;
        }
    }

    void BoardInit()
    {
        for(int y = 0 ; y < height; y++)
        {
            GameObject col = new GameObject(y.ToString());
            col.transform.parent = Board;
            col.transform.position = new Vector2(0, y-height_half);

        }
    }

    void BackgroundInit()
    {
        width_half = Mathf.RoundToInt(width*0.5f);
        height_half = Mathf.RoundToInt(height*0.5f);

        Color color = Color.grey;

        //중앙
        color.a = 0.5f;
        for(int x=-width_half; x < width_half ; x++)
        {
            for(int y = -height_half; y < height_half ; y++)
            {
                CreateTile(Background, new Vector2(x, y), color);
            }
        }

        //좌우
        color.a = 0.8f;
        for(int y = -height_half;y<height_half;y++)
        {
            CreateTile(Background, new Vector2(-width_half-1,y), color);
            CreateTile(Background, new Vector2(width_half,y), color);
        }

        //아래
        for(int x = -width_half-1;x<=width_half;x++)
        {
            CreateTile(Background, new Vector2(x, -height_half-1), color);
        }
    }

    void CreateTile(Transform parent, Vector2 position, Color color)
    {
        GameObject go = Instantiate(tilePref);
        go.transform.parent = parent;
        go.transform.localPosition = position;

        Tile tile = go.GetComponent<Tile>();
        tile.color = color;
    }

    
}
