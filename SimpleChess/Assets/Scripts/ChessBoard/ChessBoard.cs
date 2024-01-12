using UnityEngine;
using EasyButtons;

public class ChessBoard : Singleton<ChessBoard>
{
    [SerializeField] private Square blackSquare;
    [SerializeField] private Square whiteSquare;

    [SerializeField] private Transform boardStartPos;
    [SerializeField] private Transform squareParentObject;
    
    [SerializeField] private float squareDistance;
    
    [SerializeField] private Square[][] board;
    
    private const int height = 8; 
    private const int width = 8;

    public int Height => height;
    public int Width => width;

    public Square[][] Board => board;
    
    private readonly string[][] SQUARE_NOTATION = 
    {
        new string[] { "a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1" },
        new string[] { "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2" },
        new string[] { "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3" },
        new string[] { "a4", "b4", "c4", "d4", "e4", "f4", "g4", "h4" },
        new string[] { "a5", "b5", "c5", "d5", "e5", "f5", "g5", "h5" },
        new string[] { "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6" },
        new string[] { "a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7" },
        new string[] { "a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8" }
    };

    private void Awake()
    {
        InitSquareArray();
    }

    private void InitSquareArray()
    {
        board = new Square[8][];

        for (int i = 0; i < 8; i++)
        {
            board[i] = new Square[8];
            
            for (int j = 0; j < 8; j++)
            {
                board[i][j] = squareParentObject.GetChild(i * 8 + j).GetComponent<Square>();
            }
        }
    }

    [Button]
    private void InitChessBoard()
    {
        ClearSquares();

        board = new Square[height][];
        
        Vector3 currentSquarePos = boardStartPos.position;

        float defXval = currentSquarePos.x;

        for (int i = 0; i < height; i++)
        {
            currentSquarePos.x = defXval;

            board[i] = new Square[width];
            
            for (int j = 0; j < width; j++)
            {
                if ((i + j) % 2 == 0)
                {
                    board[i][j] = Instantiate(blackSquare, currentSquarePos, Quaternion.identity, squareParentObject);
                    board[i][j].SquareNotation = SQUARE_NOTATION[i][j];
                    board[i][j].Row = i;
                    board[i][j].Col = j;
                }
                else
                {
                    board[i][j] = Instantiate(whiteSquare, currentSquarePos, Quaternion.identity, squareParentObject);
                    board[i][j].SquareNotation = SQUARE_NOTATION[i][j];
                    board[i][j].Row = i;
                    board[i][j].Col = j;
                }

                currentSquarePos.x += squareDistance;
            }

            currentSquarePos.y += squareDistance;
        }   
    }

    private void ClearSquares()
    {
        int squareCount = squareParentObject.childCount;
        
        for (int i = 0; i < squareCount; i++)
        {
            DestroyImmediate(squareParentObject.GetChild(0).gameObject);
        }
    }
}
