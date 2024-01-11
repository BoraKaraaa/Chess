using EasyButtons;
using UnityEngine;

public class ChessBoard : Singleton<ChessBoard>
{
    [SerializeField] private Square blackSquare;
    [SerializeField] private Square whiteSquare;

    [SerializeField] private Transform boardStartPos;
    [SerializeField] private Transform squareParentObject;
    
    [SerializeField] private float squareDistance;
    
    private const int height = 8; 
    private const int width = 8;

    public int Height => height;
    public int Width => width;

    private Square[][] board;

    public Square[][] Board => board;
    
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
                }
                else
                {
                    board[i][j] = Instantiate(whiteSquare, currentSquarePos, Quaternion.identity, squareParentObject);
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
