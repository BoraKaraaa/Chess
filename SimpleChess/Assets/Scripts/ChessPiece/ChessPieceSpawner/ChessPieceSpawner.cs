using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class ChessPieceSpawner : Singleton<ChessPieceSpawner>
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

    [SerializeField] private ChessPiece whiteKingInstance; 
    [SerializeField] private ChessPiece blackKingInstance;
    
    [SerializeField] private ChessPiece whiteLeftRook;
    [SerializeField] private ChessPiece whiteRightRook;
    
    [SerializeField] private ChessPiece blackLeftRook;
    [SerializeField] private ChessPiece blackRightRook;

    public ChessPiece WhiteKingInstance => whiteKingInstance;
    public ChessPiece BlackKingInstance => blackKingInstance;
    
    public ChessPiece WhiteLeftRook => whiteLeftRook;
    public ChessPiece WhiteRightRook => whiteRightRook;
    
    public ChessPiece BlackLeftRook => blackLeftRook;
    public ChessPiece BlackRightRook => blackRightRook;
    
    public List<ChessPiece> WhitePieces;
    public List<ChessPiece> BlackPieces;

    [Button]
    private void InitChessPieces()
    {
        ClearChessPieces();

        WhitePieces = new List<ChessPiece>();
        BlackPieces = new List<ChessPiece>();
        
        // Spawn Pawns
        for (int i = 0; i < chessBoard.Width; i++)
        {
            Pawn createdWhitePawn = Instantiate(whitePawn, chessBoard.Board[1][i].transform.position,
                Quaternion.identity, chessPieceParent);
            
            createdWhitePawn.Square = chessBoard.Board[1][i];
            createdWhitePawn.Square.ChessPiece = createdWhitePawn;
            WhitePieces.Add(createdWhitePawn);

            Pawn createdBlackPawn = Instantiate(blackPawn, chessBoard.Board[6][i].transform.position,
                Quaternion.identity, chessPieceParent);
            
            createdBlackPawn.Square = chessBoard.Board[6][i];
            createdBlackPawn.Square.ChessPiece = createdBlackPawn;
            BlackPieces.Add(createdBlackPawn);
        }
        
        // Spawn Knights

        Knight createdWhiteKnight = Instantiate(whiteKnight, chessBoard.Board[0][1].transform.position,
            Quaternion.identity, chessPieceParent);

        createdWhiteKnight.Square = chessBoard.Board[0][1];
        createdWhiteKnight.Square.ChessPiece = createdWhiteKnight;
        WhitePieces.Add(createdWhiteKnight);

        createdWhiteKnight = Instantiate(whiteKnight, chessBoard.Board[0][6].transform.position,
            Quaternion.identity, chessPieceParent);
        
        createdWhiteKnight.Square = chessBoard.Board[0][6];
        createdWhiteKnight.Square.ChessPiece = createdWhiteKnight;
        WhitePieces.Add(createdWhiteKnight);

        Knight createdBlackKnight = Instantiate(blackKnight, chessBoard.Board[7][1].transform.position,
            Quaternion.identity, chessPieceParent);

        createdBlackKnight.Square = chessBoard.Board[7][1];
        createdBlackKnight.Square.ChessPiece = createdBlackKnight;
        BlackPieces.Add(createdBlackKnight);

        createdBlackKnight = Instantiate(blackKnight, chessBoard.Board[7][6].transform.position,
            Quaternion.identity, chessPieceParent);

        createdBlackKnight.Square = chessBoard.Board[7][6];
        createdBlackKnight.Square.ChessPiece = createdBlackKnight;
        BlackPieces.Add(createdBlackKnight);
        
        // Spawn Bishops

        Bishop createdWhiteBishop = Instantiate(whiteBishop, chessBoard.Board[0][2].transform.position,
            Quaternion.identity, chessPieceParent);

        createdWhiteBishop.Square = chessBoard.Board[0][2];
        createdWhiteBishop.Square.ChessPiece = createdWhiteBishop;
        WhitePieces.Add(createdWhiteBishop);

        createdWhiteBishop = Instantiate(whiteBishop, chessBoard.Board[0][5].transform.position,
            Quaternion.identity, chessPieceParent);

        createdWhiteBishop.Square = chessBoard.Board[0][5];
        createdWhiteBishop.Square.ChessPiece = createdWhiteBishop;
        WhitePieces.Add(createdWhiteBishop);

        Bishop createdBlackBishop = Instantiate(blackBishop, chessBoard.Board[7][2].transform.position,
            Quaternion.identity, chessPieceParent);

        createdBlackBishop.Square = chessBoard.Board[7][2];
        createdBlackBishop.Square.ChessPiece = createdBlackBishop;
        BlackPieces.Add(createdBlackBishop);

        createdBlackBishop = Instantiate(blackBishop, chessBoard.Board[7][5].transform.position,
            Quaternion.identity, chessPieceParent);
        
        createdBlackBishop.Square = chessBoard.Board[7][5];
        createdBlackBishop.Square.ChessPiece = createdBlackBishop;
        BlackPieces.Add(createdBlackBishop);
        
        // Spawn Rooks

        Rook createdWhiteRook = Instantiate(whiteRook, chessBoard.Board[0][0].transform.position,
            Quaternion.identity, chessPieceParent);

        createdWhiteRook.Square = chessBoard.Board[0][0];
        createdWhiteRook.Square.ChessPiece = createdWhiteRook;
        WhitePieces.Add(createdWhiteRook);
        whiteLeftRook = createdWhiteRook;
        
        createdWhiteRook = Instantiate(whiteRook, chessBoard.Board[0][7].transform.position,
            Quaternion.identity, chessPieceParent);

        createdWhiteRook.Square = chessBoard.Board[0][7];
        createdWhiteRook.Square.ChessPiece = createdWhiteRook;
        WhitePieces.Add(createdWhiteRook);
        whiteRightRook = createdWhiteRook;
        
        createdWhiteRook = Instantiate(blackRook, chessBoard.Board[7][0].transform.position,
            Quaternion.identity, chessPieceParent);

        createdWhiteRook.Square = chessBoard.Board[7][0];
        createdWhiteRook.Square.ChessPiece = createdWhiteRook;
        BlackPieces.Add(createdWhiteRook);
        blackLeftRook = createdWhiteRook;
        
        createdWhiteRook = Instantiate(blackRook, chessBoard.Board[7][7].transform.position,
            Quaternion.identity, chessPieceParent);

        createdWhiteRook.Square = chessBoard.Board[7][7];
        createdWhiteRook.Square.ChessPiece = createdWhiteRook;
        BlackPieces.Add(createdWhiteRook);
        blackRightRook = createdWhiteRook;
        
        // SpawnQueens

        Queen createdWhiteQueen = Instantiate(whiteQueen, chessBoard.Board[0][3].transform.position,
            Quaternion.identity, chessPieceParent);

        createdWhiteQueen.Square = chessBoard.Board[0][3];
        createdWhiteQueen.Square.ChessPiece = createdWhiteQueen;
        WhitePieces.Add(createdWhiteQueen);

        Queen createdBlackQueen = Instantiate(blackQueen, chessBoard.Board[7][3].transform.position,
            Quaternion.identity, chessPieceParent);

        createdBlackQueen.Square = chessBoard.Board[7][3];
        createdBlackQueen.Square.ChessPiece = createdBlackQueen;
        BlackPieces.Add(createdBlackQueen);
        
        // Spawn Kings

        King createdWhiteKing = Instantiate(whiteKing, chessBoard.Board[0][4].transform.position,
            Quaternion.identity, chessPieceParent);

        createdWhiteKing.Square = chessBoard.Board[0][4];
        createdWhiteKing.Square.ChessPiece = createdWhiteKing;
        WhitePieces.Add(createdWhiteKing);

        whiteKingInstance = createdWhiteKing;

        King createdBlackKing = Instantiate(blackKing, chessBoard.Board[7][4].transform.position,
            Quaternion.identity, chessPieceParent);

        createdBlackKing.Square = chessBoard.Board[7][4];
        createdBlackKing.Square.ChessPiece = createdBlackKing;
        BlackPieces.Add(createdBlackKing);

        blackKingInstance = createdBlackKing;
        
        WhitePieces.Reverse();
        BlackPieces.Reverse();
    }

    private void ClearChessPieces()
    {
        int chessPieceCount = chessPieceParent.childCount;
        
        for (int i = 0; i < chessPieceCount; i++)
        {
            DestroyImmediate(chessPieceParent.GetChild(0).gameObject);
        }
        
        WhitePieces.Clear();
        BlackPieces.Clear();
    }
}
