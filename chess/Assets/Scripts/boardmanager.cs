using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class boardmanager : MonoBehaviour
{

    public GameObject cam1;
    public static boardmanager Instance { set; get; }
    public bool[,] AllowedMoves { set; get; }
    public ChessMan[,] ChessMans { set; get; }
    private ChessMan SelectedChessMan;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    
    private int selectionX = -1;
    private int selectionY = -1;

    private static int PosEatI = -1;
    private static int PosEatJ = -1;

    private static int PosVideI = -1;
    private static int PosVideJ = -1;

    public List<GameObject> chessmanPrefabs;
    public List<GameObject> activeChessman=new List<GameObject>();

    private Material PreviousMat;
    public Material SelectedMat;

    public int[] EnPassantMove;

    public static bool isWhiteturn = true;
    public static bool isWhiteturnR = true;

    public static int GameMode;

    private bool MoveOrNot=false;

    private  int PawnPromotion = -1;

    public GameObject PromotionMenuUI;
    
    private PromotionPiece Piece;

    struct PromotionPiece
    {
        public int x;
        public int y;
        public bool isWhiteturn;
    }

    private void Start()
    {
        SwitchCamera();
        EnPassantMove = new int[2] { -1, -1 };
        Instance = this;
        SpawnAllChessMan();
    }
    private void Update()
    {
        if (Checkmate())
        {
            if (isWhiteturn)
            {
                Debug.Log("White team win");
                Score.WhiteScore += 1;
                EndScore.Info = "White team";
            }
            else
            {
                Score.BlackScore += 1;
                Debug.Log("Black team win");
                EndScore.Info = "Black team";
            }
            SceneManager.LoadScene("End");
        }
        switch(GameMode)
        {
            case 1:
                DrawChessBoard();
                Promotion();
                UpdateSelection();

                if (Input.GetMouseButtonDown(0))
                {
                    if (selectionX >= 0 && selectionY >= 0)
                    {
                        if (SelectedChessMan == null)
                        {
                            //select the chess
                            SelectChessMan(selectionX, selectionY);
                        }
                        else
                        {
                            //move the chess
                            MoveChessman(selectionX, selectionY);
                        }
                    }
                }
                break;
            case 2:
                DrawChessBoard();
                Promotion();
                if(isWhiteturn)
                {
                    UpdateSelection();
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (selectionX >= 0 && selectionY >= 0)
                        {
                            if (SelectedChessMan == null)
                            {
                                //select the chess
                                SelectChessMan(selectionX, selectionY);
                            }
                            else
                            {
                                //move the chess
                                MoveChessman(selectionX, selectionY);
                            }
                        }
                    }
                }
                break;
        }
    }

    public void Dont()
    {
        MoveOrNot = true;
    }

    public void QueenPromotion()
    {
        PawnPromotion = 2;
    }
    public void KnightPromotion()
    {
        PawnPromotion = 3;
    }
    public void BichopPromotion()
    {
        PawnPromotion = 4;
    }
    public void RockPromotion()
    {
        PawnPromotion = 5;
    }

    private void SelectChessMan(int x,int y)
    {
        if(!PauseMenu.GameIsPause)
        {
            if (ChessMans[x, y] == null)
                       return;
            if (ChessMans[x, y].isWhite != isWhiteturn)
                return;

            bool HasatLeastOneMove = false;
            AllowedMoves = ChessMans[x, y].PossibleMove();
            for (int i=0;i<8;i++)
            {
                for(int j=0;j<8;j++)
                {
                    if (AllowedMoves[i,j])
                    {
                        HasatLeastOneMove = true;
                    }
                }
            }

            if (!HasatLeastOneMove)
                 return;

            if(ChessMans[x,y].GetType()!=typeof(King))
            {
                if (Clouage(ChessMans[x,y]))
                {
                    if(GameMode==1)
                    {
                        return;
                    }
                    else
                    {
                         Debug.Log("Im in");
                         for (int k = 0; k < 8; k++)
                         {
                             for (int l = 0; l < 8; l++)
                             {
                                 if (ChessMans[k, l] != null && ChessMans[k, l].GetType() == typeof(King) && ChessMans[k, l].isWhite == false)
                                 {
                                     bool[,] KingMoves = ChessMans[k, l].PossibleMove();
                                     SelectChessMan(k, l);
                                     for (int m = 0; m < 8; m++)
                                     {
                                         for (int n = 0; n < 8; n++)
                                         {
                                             if (KingMoves[m, n] == true)
                                                 MoveChessman(m, n);
                                         }
                                     }
                                 }
                             }
                         }
                    }
                }
            }

            SelectedChessMan = ChessMans[x, y];
            PreviousMat = SelectedChessMan.GetComponent<MeshRenderer>().material;
            SelectedMat.mainTexture = PreviousMat.mainTexture;
            SelectedChessMan.GetComponent<MeshRenderer>().material = SelectedMat;
            BoardHilghits.Instance.HighlitAllowedMoves(AllowedMoves);
        }
    }

   

    private void MoveChessman(int x,int y)
    {
        if(AllowedMoves[x,y])
        {
            Castling(x, y);
            ChessMan c = ChessMans[x, y];
            if (c!=null && c.isWhite != isWhiteturn)
            {
                //capture a piece
               
                //if its a king end the game
                if (c.GetType()==typeof(King))
                {
                    EndGame();
                    return;
                }
                
                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            if(x==EnPassantMove[0] && y==EnPassantMove[1])
            {
                if(isWhiteturn)
                    c = ChessMans[x, y-1];
                else
                    c = ChessMans[x, y + 1];

                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            EnPassantMove[0] = -1;
            EnPassantMove[1] = -1;
            if (SelectedChessMan.GetType()==typeof(Pawn))
            {
                //Promotion
                if (y == 7)
                {
                    if(GameMode==1)
                        PromotionMenuUI.SetActive(true);
                    else
                    {
                        PawnPromotion = Random.Range(2, 5);
                    }
                    Piece.x = x;
                    Piece.y = y;
                    Piece.isWhiteturn = isWhiteturn;
                }
                else if (y == 0)
                {
                    if (GameMode == 1)
                        PromotionMenuUI.SetActive(true);
                    else
                    {
                        PawnPromotion = Random.Range(2, 5);
                    }
                    Piece.x = x;
                    Piece.y = y;
                    Piece.isWhiteturn = isWhiteturn;
                }
                if (SelectedChessMan.CurrentY==1 && y==3)
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y - 1;
                }
                else if(SelectedChessMan.CurrentY==6 && y==4)
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y + 1;
                }
            }

            if(MoveOrNot)
            {
                if (SelectedChessMan.GetType() != typeof(Dragon))
                    ChessMans[SelectedChessMan.CurrentX, SelectedChessMan.CurrentY] = null;
                else
                {
                    if (c == null)
                        ChessMans[SelectedChessMan.CurrentX, SelectedChessMan.CurrentY] = null;
                }
                if (SelectedChessMan.tag == "Pawn")
                    StartCoroutine(Move_Routine(SelectedChessMan.transform, SelectedChessMan.transform.position, GetTileCenter(x, y) + new Vector3(0, 0.23f, 0)));
                else
                {
                    if (c == null)
                        StartCoroutine(Move_Routine(SelectedChessMan.transform, SelectedChessMan.transform.position, GetTileCenter(x, y)));
                    else
                    {
                        if (SelectedChessMan.GetType() != typeof(Dragon))
                            StartCoroutine(Move_Routine(SelectedChessMan.transform, SelectedChessMan.transform.position, GetTileCenter(x, y)));
                        else
                        {
                            Debug.Log("Im here");
                            Debug.Log(SelectedChessMan.CurrentX);
                            Debug.Log(SelectedChessMan.CurrentY);
                            Debug.Log(x);
                            Debug.Log(y);
                            if (y != SelectedChessMan.CurrentY + 1 && x != SelectedChessMan.CurrentY + 1 && y != SelectedChessMan.CurrentY - 1 && x != SelectedChessMan.CurrentY - 1)
                            {
                                StartCoroutine(Move_Routine(SelectedChessMan.transform, SelectedChessMan.transform.position, GetTileCenter(x, y)));
                            }
                        }

                    }
                }



                if (SelectedChessMan.GetType() != typeof(Dragon))
                {
                    SelectedChessMan.SetPosition(x, y);
                    ChessMans[x, y] = SelectedChessMan;
                }
                else
                {
                    if (c == null)
                    {
                        SelectedChessMan.SetPosition(x, y);
                        ChessMans[x, y] = SelectedChessMan;
                    }
                }
            }
            else
            {
                if (SelectedChessMan.tag == "Pawn")
                    StartCoroutine(Move_Routine(SelectedChessMan.transform, SelectedChessMan.transform.position, GetTileCenter(x, y) + new Vector3(0, 0.23f, 0)));
                else
                    StartCoroutine(Move_Routine(SelectedChessMan.transform, SelectedChessMan.transform.position, GetTileCenter(x, y)));
                SelectedChessMan.SetPosition(x, y);
                ChessMans[x, y] = SelectedChessMan;
            }

           
            
            isWhiteturn = !isWhiteturn;
        }

        SelectedChessMan.GetComponent<MeshRenderer>().material = PreviousMat;

        BoardHilghits.Instance.HideHighlits();
        SelectedChessMan = null;
        MoveOrNot = false;
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane"))) 
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void SpawnChessman(int index,int x,int y)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], GetTileCenter(x,y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        ChessMans[x, y] = go.GetComponent<ChessMan>();
        ChessMans[x, y].SetPosition(x, y);
        activeChessman.Add(go);
    }


    
    private void SpawnAllChessMan()
    {
        activeChessman = new List<GameObject>();
        ChessMans = new ChessMan[8, 8];
        EnPassantMove = new int[2]{ -1, -1 };

        int R1= Random.Range(0, 7);
        int R2= Random.Range(1, 6);

        int K= Random.Range(R1, R2);


        // new chessmans

        SpawnChessman(12, 6, 1);
        SpawnChessman(14, 7, 1);
        SpawnChessman(13, 6, 6);
        SpawnChessman(15, 7, 6);

        //Spawn White team

        // king
        SpawnChessman(0, 4, 0);
        //Queen
        SpawnChessman(1, 3, 0);

        //Rooks
        SpawnChessman(2, 0, 0);
        SpawnChessman(2, 7, 0);

        //Bishops
        SpawnChessman(3, 2, 0);
        SpawnChessman(3, 5, 0);

        //Knights
        SpawnChessman(4, 1, 0);
        SpawnChessman(4, 6, 0);

        for (int i = 0; i < 6; i++)
        {
            SpawnChessman(5, i, 1);
        }

        //Spawn Black team

        // king
        SpawnChessman(6, 4, 7);

        //Queen
        SpawnChessman(7, 3, 7);

        //Rooks
        SpawnChessman(8, 0, 7);
        SpawnChessman(8, 7, 7);

        //Bishops
        SpawnChessman(9, 2, 7);
        SpawnChessman(9, 5, 7);

        //Knights
        SpawnChessman(10, 1, 7);
        SpawnChessman(10, 6, 7);

        for (int i = 0; i < 6; i++)
        {
            SpawnChessman(11, i, 6);
        }
    }

    private Vector3 GetTileCenter(int x,int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x = (TILE_SIZE * x)+TILE_OFFSET;
        origin.z = (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    private void DrawChessBoard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heigthLine = Vector3.forward * 8;
        for(int i=0;i<=8;i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start,start+widthLine);
            for(int j=0;j<=8;j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heigthLine);
            }
        }
        if(selectionX>=0 && selectionY>=0)
        {
            Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX, Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));
            Debug.DrawLine(Vector3.forward * (selectionY+1) + Vector3.right * selectionX, Vector3.forward * selectionY  + Vector3.right * (selectionX + 1));
        }
    }

    private void EndGame()
    {
        if (isWhiteturn)
        {
            Debug.Log("White team win");
            Score.WhiteScore += 1;
            EndScore.Info = "White team";
        }
        else
        {
            Score.BlackScore += 1;
            Debug.Log("Black team win");
            EndScore.Info = "Black team";
        }
        SceneManager.LoadScene("End");
        foreach (GameObject go in activeChessman)
            Destroy(go);

        isWhiteturn = true;
        BoardHilghits.Instance.HideHighlits();
        SpawnAllChessMan();
    }

    private void Promotion()
    {
        if (PawnPromotion != -1)
        {
            PromotionMenuUI.SetActive(false);
            //White team
            if (Piece.isWhiteturn)
            {
                
                switch (PawnPromotion)
                {
                    case 2://Queen
                        activeChessman.Remove(ChessMans[Piece.x, Piece.y].gameObject);
                        Destroy(ChessMans[Piece.x, Piece.y].gameObject);
                        SpawnChessman(1, Piece.x, Piece.y);
                        break;
                    case 3://Knight
                        activeChessman.Remove(ChessMans[Piece.x, Piece.y].gameObject);
                        Destroy(ChessMans[Piece.x, Piece.y].gameObject);
                        SpawnChessman(4, Piece.x, Piece.y);
                        break;
                    case 4://Bishop
                        activeChessman.Remove(ChessMans[Piece.x, Piece.y].gameObject);
                        Destroy(ChessMans[Piece.x, Piece.y].gameObject);
                        SpawnChessman(3, Piece.x, Piece.y);
                        break;
                    case 5://Rook
                        activeChessman.Remove(ChessMans[Piece.x, Piece.y].gameObject);
                        Destroy(ChessMans[Piece.x, Piece.y].gameObject);
                        SpawnChessman(2, Piece.x, Piece.y);
                        break;
                }
            }
            //Black team
            else
            {
                switch (PawnPromotion)
                {

                    case 2://Queen
                        activeChessman.Remove(ChessMans[Piece.x, Piece.y].gameObject);
                        Destroy(ChessMans[Piece.x, Piece.y].gameObject);
                        SpawnChessman(7, Piece.x, Piece.y);
                        break;
                    case 3://Knight
                        activeChessman.Remove(ChessMans[Piece.x, Piece.y].gameObject);
                        Destroy(ChessMans[Piece.x, Piece.y].gameObject);
                        SpawnChessman(10, Piece.x, Piece.y);
                        break;
                    case 4://Bishop
                        activeChessman.Remove(ChessMans[Piece.x, Piece.y].gameObject);
                        Destroy(ChessMans[Piece.x, Piece.y].gameObject);
                        SpawnChessman(9, Piece.x, Piece.y);
                        break;
                    case 5://Rook
                        activeChessman.Remove(ChessMans[Piece.x, Piece.y].gameObject);
                        Destroy(ChessMans[Piece.x, Piece.y].gameObject);
                        SpawnChessman(8, Piece.x, Piece.y);
                        break;
                }
            }
            SwitchCamera();
        }
        PawnPromotion = -1;
    }

    private IEnumerator Move_Routine(Transform transform, Vector3 from, Vector3 to)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t));
            if(t>=1)
            {
                FindObjectOfType<AudioManager>().Play("Move");
                if (GameMode == 1)
                    SwitchCamera();
                else
                {
                    if (!isWhiteturn)
                    {
                        IAChessMans();
                    }
                }   
            }
            yield return null;
        }
    }

    private void SwitchCamera()
    {
        if (isWhiteturn)
        {
            Camera.main.transform.position = new Vector3(3.83f, 6.65f, -1.1f);
            Camera.main.transform.rotation = Quaternion.Euler(57.996f, -0.28f, -0.382f);
        }
        else
        {
            Camera.main.transform.position = new Vector3(4.19f, 6.65f, 9.1f);
            Camera.main.transform.rotation = Quaternion.Euler(57.996f, -180.06f, 0f);
        }
    }
    private bool Checkmate()
    {
        ChessMan King = null;
        //searching for the king
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (ChessMans[i, j] != null && ChessMans[i, j].GetType() == typeof(King) && ChessMans[i, j].isWhite == isWhiteturn)
                {
                    King = ChessMans[i, j];
                }
            }
        }

        bool[,] KingMoves = King.PossibleMove();
        KingMoves[King.CurrentX, King.CurrentY] = true;
        bool[,] EnemieMoves = new bool[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (ChessMans[i, j] != null && ChessMans[i, j].isWhite != King.isWhite)
                {
                    ChessMan enemie = ChessMans[i, j];
                    EnemieMoves = enemie.PossibleMove();
                    //Eliminate the king moves where there is a denger
                    for (int k = 0; k < 8; k++)
                    {
                        for (int l = 0; l < 8; l++)
                        {
                            if (EnemieMoves[k, l] == true && KingMoves[k, l] == true)
                            {
                                KingMoves[k, l] = false;
                            }
                                
                        }
                    }
                }
            }
        }
        //Look if the king can still move safely if not there is a checkmate
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (KingMoves[i, j] == true)
                    return false;
            }
        }
        return true;
    }
    private bool Clouage(ChessMan ClouagePiece)
    {
        ChessMan King = null;

        //searching for the king
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (ChessMans[i, j] != null && ChessMans[i, j].GetType() == typeof(King) && ChessMans[i, j].isWhite == isWhiteturn)
                {
                    King = ChessMans[i, j];
                }
            }
        }

        //Look If an enemie piece can capture the king if we move the piece
        bool[,] EnemieMoves;
        ChessMans[ClouagePiece.CurrentX, ClouagePiece.CurrentY] = null;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (ChessMans[i, j] != null && ChessMans[i, j].isWhite != isWhiteturn)
                {
                    EnemieMoves = ChessMans[i, j].PossibleMove();
                    if (EnemieMoves[King.CurrentX, King.CurrentY])
                    {
                        ChessMans[ClouagePiece.CurrentX, ClouagePiece.CurrentY] = ClouagePiece;
                        return true;
                    }
                }
            }
        }

        ChessMans[ClouagePiece.CurrentX, ClouagePiece.CurrentY] = ClouagePiece;
        return false;
    }

    private void Castling(int x,int y)
    {
        //Castling
        if (SelectedChessMan.GetType() == typeof(King))
        {
            //Right castling
            if ((x - SelectedChessMan.CurrentX) > 1)
            {
                ChessMan cl = ChessMans[7, SelectedChessMan.CurrentY];
                if (isWhiteturn)
                    SpawnChessman(2, x - 1, y);
                else
                    SpawnChessman(8, x - 1, y);
                Destroy(cl.gameObject);
            }
            //Left castling
            else if ((SelectedChessMan.CurrentX - x) > 1)
            {
                ChessMan cl = ChessMans[0, SelectedChessMan.CurrentY];
                if (isWhiteturn)
                    SpawnChessman(2, x + 1, y);
                else
                    SpawnChessman(8, x + 1, y);
                Destroy(cl.gameObject);
            }
        }
    }

    private bool CanEat(int x, int y)
    {
        bool[,] Bot = ChessMans[x, y].PossibleMove();
        for (int h = 0; h < 8; h++)
        {
            for (int k = 0; k < 8; k++)
            {
                if (ChessMans[h, k] != null && ChessMans[h, k].isWhite != isWhiteturn)
                {
                    if (Bot[ChessMans[h, k].CurrentX, ChessMans[h, k].CurrentY])
                    {
                        PosEatI = h;
                        PosEatJ = k;
                        
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void IAChessMans()
    { 
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (ChessMans[i, j] != null && ChessMans[i, j].isWhite == false)
                {
                    if (CanEat(i, j))
                    {
                        SelectChessMan(i, j);
                        MoveChessman(PosEatI, PosEatJ);
                    }
                }
            }
        }

        if(PosEatI==-1 && PosEatJ==-1)
        {
            for(int i=0;i<8;i++)
            {
                for (int j=0;j<8;j++)
                {
                    if(ChessMans[i,j]!=null && ChessMans[i,j].isWhite == false)
                    {
                        if (MoveVide(i, j))
                        {
                            SelectChessMan(i, j);
                            MoveChessman(PosVideI, PosVideJ);
                        }
                    }
                }
            }
        }
        PosEatI = -1;
        PosEatJ = -1;
    }

    private bool MoveVide(int x,int y)
    {
        bool[,] Moves = ChessMans[x, y].PossibleMove();
        for(int i=Random.Range(0, 8);i<8;i++)
        {
            for(int j=0;j<8;j++)
            {
                if(ChessMans[i,j]==null)
                {
                    if (Moves[i,j])
                    {
                        PosVideI = i;
                        PosVideJ = j;
                        return true;
                    }
                }
            }
        }
        return false;
    }



}

