using EasyButtons;
using UnityEngine;

public class ChessPieceSpawner : MonoBehaviour
{
    [SerializeField] private ChessBoard chessBoard;
    
    [Header("White Pieces")]
    [SerializeField] private Pawn whitePawn;
    [SerializeField] private Knight whiteKnight;
    [SerializeField] private Bishop whiteBishop;
    [SerializeField] private Rook whiteRook;
    [SerializeField] private Queen whiteQueen;
    [SerializeField] private King whiteKing;
    
    [Space(15)]
    
    [Header("Black Pieces")]
    [SerializeField] private Pawn blackPawn;
    [SerializeField] private Knight blackKnight;
    [SerializeField] private Bishop blackBishop;
    [SerializeField] private Rook blackRook;
    [SerializeField] private Queen blackQueen;
    [SerializeField] private King blackKing;

    [Space(10)]
    [SerializeField] private Transform chessPieceParent;
    
    
    [Button]
    private void InitChessPieces()
    {
        ClearChessPieces();
        
        // Spawn Pawns
        for (int i = 0; i < chessBoard.Width; i++)
        {
            Instantiate(whitePawn, chessBoard.Board[1][i].transform.position, Quaternion.identity, chessPieceParent);
            Instantiate(blackPawn, chessBoard.Board[6][i].transform.position, Quaternion.identity, chessPieceParent);
        }
        
        // Spawn Knights
        Instantiate(whiteKnight, chessBoard.Board[0][1].transform.position, Quaternion.identity, chessPieceParent);
        Instantiate(whiteKnight, chessBoard.Board[0][6].transform.position, Quaternion.identity, chessPieceParent);
        Instantiate(blackKnight, chessBoard.Board[7][1].transform.position, Quaternion.identity, chessPieceParent);
        Instantiate(blackKnight, chessBoard.Board[7][6].transform.position, Quaternion.identity, chessPieceParent);
        
        // Spawn Bishops
        Instantiate(whiteBishop, chessBoard.Board[0][2].transform.position, Quaternion.identity, chessPieceParent);
        Instantiate(whiteBishop, chessBoard.Board[0][5].transform.position, Quaternion.identity, chessPieceParent);
        Instantiate(blackBishop, chessBoard.Board[7][2].transform.position, Quaternion.identity, chessPieceParent);
        Instantiate(blackBishop, chessBoard.Board[7][5].transform.position, Quaternion.identity, chessPieceParent);
        
        // Spawn Rooks
        Instantiate(whiteRook, chessBoard.Board[0][0].transform.position, Quaternion.identity, chessPieceParent);
        Instantiate(whiteRook, chessBoard.Board[0][7].transform.position, Quaternion.identity, chessPieceParent);
        Instantiate(blackRook, chessBoard.Board[7][0].transform.position, Quaternion.identity, chessPieceParent);
        Instantiate(blackRook, chessBoard.Board[7][7].transform.position, Quaternion.identity, chessPieceParent);
        
        // SpawnQueens
        Instantiate(whiteQueen, chessBoard.Board[0][3].transform.position, Quaternion.identity, chessPieceParent);
        Instantiate(blackQueen, chessBoard.Board[7][3].transform.position, Quaternion.identity, chessPieceParent);
        
        // Spawn Kings
        Instantiate(whiteKing, chessBoard.Board[0][4].transform.position, Quaternion.identity, chessPieceParent);
        Instantiate(blackKing, chessBoard.Board[7][4].transform.position, Quaternion.identity, chessPieceParent);
        
    }

    private void ClearChessPieces()
    {
        int chessPieceCount = chessPieceParent.childCount;
        
        for (int i = 0; i < chessPieceCount; i++)
        {
            DestroyImmediate(chessPieceParent.GetChild(0).gameObject);
        }
    }
}
