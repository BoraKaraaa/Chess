using EasyButtons;
using UnityEngine;

public class ChessBoard : Singleton<ChessBoard>
{
    [SerializeField] private Square blackSquare;
    [SerializeField] private Square whiteSquare;

    [SerializeField] private Transform boardStartPos;
    [SerializeField] private Transform squareParentObject;
    
    [SerializeField] private float squareDistance;
    
    private const int row = 8; 
    private const int col = 8;

    public int Row => row;
    public int Col => col;

    private Square[][] board;

    public Square[][] Board => board;
    
    [Button]
    private void InitChessBoard()
    {
        ClearSquares();

        board = new Square[row][];
        
        Vector3 currentSquarePos = boardStartPos.position;

        float defXval = currentSquarePos.x;

        for (int i = 0; i < row; i++)
        {
            currentSquarePos.x = defXval;

            board[i] = new Square[col];
            
            for (int j = 0; j < col; j++)
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
