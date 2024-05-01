using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public struct EChessPieceSpec
{
    public EColor Color;
    public EChessPiece ChessPiece;
    public int Index;

    public EChessPieceSpec(EColor color, EChessPiece chessPiece, int index)
    {
        Color = color;
        ChessPiece = chessPiece;
        Index = index;
    }
    
    public static bool operator ==(EChessPieceSpec c1, EChessPieceSpec c2)
    {
        return c1.Color == c2.Color && c1.ChessPiece == c2.ChessPiece && c1.Index == c2.Index;
    }

    public static bool operator !=(EChessPieceSpec c1, EChessPieceSpec c2)
    {
        return !(c1 == c2);
    }

    public override bool Equals(object obj)
    {
        return this == (EChessPieceSpec)obj;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class ChessPieceSpecComparer : IEqualityComparer<EChessPieceSpec>
{
    public bool Equals(EChessPieceSpec o1, EChessPieceSpec o2)
    {
        return o1 == o2;
    }

    public int GetHashCode(EChessPieceSpec obj)
    {
        return obj.GetHashCode();
    }
}


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

    public Dictionary<EChessPieceSpec, ChessPiece> AllPieces;

    private void Awake()
    {
        AllPieces = new Dictionary<EChessPieceSpec, ChessPiece>(new ChessPieceSpecComparer());
    }
    
    private void InitAllWhitePieces()
    {
        for (int i = 0; i < chessBoard.Width; i++)
        {
            AllPieces.Add(new EChessPieceSpec(EColor.WHITE, EChessPiece.PAWN, i), WhitePieces[WhitePieces.Count-1-i]);
        }
        
        AllPieces.Add(new EChessPieceSpec(EColor.WHITE, EChessPiece.KNIGHT, 1), WhitePieces[7]);
        AllPieces.Add(new EChessPieceSpec(EColor.WHITE, EChessPiece.KNIGHT, 6), WhitePieces[6]);
        AllPieces.Add(new EChessPieceSpec(EColor.WHITE, EChessPiece.BISHOP, 2), WhitePieces[5]);
        AllPieces.Add(new EChessPieceSpec(EColor.WHITE, EChessPiece.BISHOP, 5), WhitePieces[4]);
        AllPieces.Add(new EChessPieceSpec(EColor.WHITE, EChessPiece.ROOK, 0), WhitePieces[3]);
        AllPieces.Add(new EChessPieceSpec(EColor.WHITE, EChessPiece.ROOK, 7), WhitePieces[2]);
        AllPieces.Add(new EChessPieceSpec(EColor.WHITE, EChessPiece.QUEEN, 3), WhitePieces[1]);
        AllPieces.Add(new EChessPieceSpec(EColor.WHITE, EChessPiece.KING, 4), WhitePieces[0]);
    }
    
    private void InitAllBlackPieces()
    {
        for (int i = 0; i < chessBoard.Width; i++)
        {
            AllPieces.Add(new EChessPieceSpec(EColor.BLACK, EChessPiece.PAWN, i), BlackPieces[BlackPieces.Count-1-i]);
        }
        
        AllPieces.Add(new EChessPieceSpec(EColor.BLACK, EChessPiece.KNIGHT, 1), BlackPieces[7]);
        AllPieces.Add(new EChessPieceSpec(EColor.BLACK, EChessPiece.KNIGHT, 6), BlackPieces[6]);
        AllPieces.Add(new EChessPieceSpec(EColor.BLACK, EChessPiece.BISHOP, 2), BlackPieces[5]);
        AllPieces.Add(new EChessPieceSpec(EColor.BLACK, EChessPiece.BISHOP, 5), BlackPieces[4]);
        AllPieces.Add(new EChessPieceSpec(EColor.BLACK, EChessPiece.ROOK, 0), BlackPieces[3]);
        AllPieces.Add(new EChessPieceSpec(EColor.BLACK, EChessPiece.ROOK, 7), BlackPieces[2]);
        AllPieces.Add(new EChessPieceSpec(EColor.BLACK, EChessPiece.QUEEN, 3), BlackPieces[1]);
        AllPieces.Add(new EChessPieceSpec(EColor.BLACK, EChessPiece.KING, 4), BlackPieces[0]);
    }

    public void SpawnChessPiece(EColor color, EChessPiece chessPiece, int row, int col)
    {
        switch (chessPiece)
        {
            case EChessPiece.PAWN:
                if (color == EColor.WHITE)
                    SpawnWhitePawn(row, col);
                else
                    SpawnBlackPawn(row, col);
                break;
            case EChessPiece.KNIGHT:
                if (color == EColor.WHITE)
                    SpawnWhiteKnight(row, col);
                else
                    SpawnBlackKnight(row, col);
                break;
            case EChessPiece.BISHOP:
                if (color == EColor.WHITE)
                    SpawnWhiteBishop(row, col);
                else
                    SpawnBlackBishop(row, col);
                break;
            case EChessPiece.ROOK:
                if (color == EColor.WHITE)
                    SpawnWhiteRook(row, col);
                else
                    SpawnBlackRook(row, col);
                break;
            case EChessPiece.QUEEN:
                if (color == EColor.WHITE)
                    SpawnWhiteQueen(row, col);
                else
                    SpawnBlackQueen(row, col);
                break;
            case EChessPiece.KING:
                if (color == EColor.WHITE)
                    SpawnWhiteKing(row, col);
                else
                    SpawnBlackKing(row, col);
                break;
        }
    }
    
    private void SpawnWhitePawn(int x, int y)
    {
        Pawn createdWhitePawn = Instantiate(whitePawn, chessBoard.Board[y][x].transform.position,
            Quaternion.identity, chessPieceParent);
    
        createdWhitePawn.Square = chessBoard.Board[y][x];
        createdWhitePawn.Square.ChessPiece = createdWhitePawn;
        createdWhitePawn.EChessPiece = EChessPiece.PAWN;
        
        createdWhitePawn.PieceIndex = x;
        
        WhitePieces.Add(createdWhitePawn);
    }

    private void SpawnBlackPawn(int x, int y)
    {
        Pawn createdBlackPawn = Instantiate(blackPawn, chessBoard.Board[y][x].transform.position,
            Quaternion.identity, chessPieceParent);

        createdBlackPawn.Square = chessBoard.Board[y][x];
        createdBlackPawn.Square.ChessPiece = createdBlackPawn;
        createdBlackPawn.EChessPiece = EChessPiece.PAWN;
        
        createdBlackPawn.PieceIndex = x;
        
        BlackPieces.Add(createdBlackPawn);
    }

    private void SpawnWhiteKnight(int x, int y)
    {
        Knight createdWhiteKnight = Instantiate(whiteKnight, chessBoard.Board[y][x].transform.position,
            Quaternion.identity, chessPieceParent);

        createdWhiteKnight.Square = chessBoard.Board[y][x];
        createdWhiteKnight.Square.ChessPiece = createdWhiteKnight;
        createdWhiteKnight.EChessPiece = EChessPiece.KNIGHT;
        
        createdWhiteKnight.PieceIndex = x;
        
        WhitePieces.Add(createdWhiteKnight);
    }

    private void SpawnBlackKnight(int x, int y)
    {
        Knight createdBlackKnight = Instantiate(blackKnight, chessBoard.Board[y][x].transform.position,
            Quaternion.identity, chessPieceParent);

        createdBlackKnight.Square = chessBoard.Board[y][x];
        createdBlackKnight.Square.ChessPiece = createdBlackKnight;
        createdBlackKnight.EChessPiece = EChessPiece.KNIGHT;
        
        createdBlackKnight.PieceIndex = x;
        
        BlackPieces.Add(createdBlackKnight);
    }

    private void SpawnWhiteBishop(int x, int y)
    {
        Bishop createdWhiteBishop = Instantiate(whiteBishop, chessBoard.Board[y][x].transform.position,
            Quaternion.identity, chessPieceParent);

        createdWhiteBishop.Square = chessBoard.Board[y][x];
        createdWhiteBishop.Square.ChessPiece = createdWhiteBishop;
        createdWhiteBishop.EChessPiece = EChessPiece.BISHOP;
        
        createdWhiteBishop.PieceIndex = x;
        
        WhitePieces.Add(createdWhiteBishop);
    }

    private void SpawnBlackBishop(int x, int y)
    {
        Bishop createdBlackBishop = Instantiate(blackBishop, chessBoard.Board[y][x].transform.position,
            Quaternion.identity, chessPieceParent);

        createdBlackBishop.Square = chessBoard.Board[y][x];
        createdBlackBishop.Square.ChessPiece = createdBlackBishop;
        createdBlackBishop.EChessPiece = EChessPiece.BISHOP;
        
        createdBlackBishop.PieceIndex = x;
        
        BlackPieces.Add(createdBlackBishop);
    }

    private void SpawnWhiteRook(int x, int y)
    {
        Rook createdWhiteRook = Instantiate(whiteRook, chessBoard.Board[y][x].transform.position,
            Quaternion.identity, chessPieceParent);
        
        createdWhiteRook.Square = chessBoard.Board[y][x];
        createdWhiteRook.Square.ChessPiece = createdWhiteRook;
        createdWhiteRook.EChessPiece = EChessPiece.ROOK;
        
        createdWhiteRook.PieceIndex = x;
        
        WhitePieces.Add(createdWhiteRook);

        if (x == 0 && y == 0)
        {
            whiteLeftRook = createdWhiteRook;
        }
        else if (x == 7 && y == 0)
        {
            whiteRightRook = createdWhiteRook;
        }
    }
    
    private void SpawnBlackRook(int x, int y)
    {
        Rook createdBlackRook = Instantiate(blackRook, chessBoard.Board[y][x].transform.position,
            Quaternion.identity, chessPieceParent);

        createdBlackRook.Square = chessBoard.Board[y][x];
        createdBlackRook.Square.ChessPiece = createdBlackRook;
        createdBlackRook.EChessPiece = EChessPiece.ROOK;
        
        createdBlackRook.PieceIndex = x;
        
        BlackPieces.Add(createdBlackRook);
        
        if (x == 0 && y == 7)
        {
            blackLeftRook = createdBlackRook;
        }
        else if (x == 7 && y == 7)
        {
            blackRightRook = createdBlackRook;
        }
    }

    private void SpawnWhiteQueen(int x, int y)
    {
        Queen createdWhiteQueen = Instantiate(whiteQueen, chessBoard.Board[y][x].transform.position,
            Quaternion.identity, chessPieceParent);

        createdWhiteQueen.Square = chessBoard.Board[y][x];
        createdWhiteQueen.Square.ChessPiece = createdWhiteQueen;
        createdWhiteQueen.EChessPiece = EChessPiece.QUEEN;
        
        createdWhiteQueen.PieceIndex = x;
        
        WhitePieces.Add(createdWhiteQueen);
    }

    private void SpawnBlackQueen(int x, int y)
    {
        Queen createdBlackQueen = Instantiate(blackQueen, chessBoard.Board[y][x].transform.position,
            Quaternion.identity, chessPieceParent);

        createdBlackQueen.Square = chessBoard.Board[y][x];
        createdBlackQueen.Square.ChessPiece = createdBlackQueen;
        createdBlackQueen.EChessPiece = EChessPiece.QUEEN;
        
        createdBlackQueen.PieceIndex = x;
        
        BlackPieces.Add(createdBlackQueen);
    }
    
    private void SpawnWhiteKing(int x, int y)
    {
        King createdWhiteKing = Instantiate(whiteKing, chessBoard.Board[y][x].transform.position,
            Quaternion.identity, chessPieceParent);

        createdWhiteKing.Square = chessBoard.Board[y][x];
        createdWhiteKing.Square.ChessPiece = createdWhiteKing;
        createdWhiteKing.EChessPiece = EChessPiece.KING;
        
        createdWhiteKing.PieceIndex = x;
        
        WhitePieces.Add(createdWhiteKing);
        whiteKingInstance = createdWhiteKing;
    }

    private void SpawnBlackKing(int x, int y)
    {
        King createdBlackKing = Instantiate(blackKing, chessBoard.Board[y][x].transform.position,
            Quaternion.identity, chessPieceParent);

        createdBlackKing.Square = chessBoard.Board[y][x];
        createdBlackKing.Square.ChessPiece = createdBlackKing;
        createdBlackKing.EChessPiece = EChessPiece.KING;
        
        createdBlackKing.PieceIndex = x;
        
        BlackPieces.Add(createdBlackKing);
        blackKingInstance = createdBlackKing;
    }
    
    [Button]
    public void InitChessPieces()
    {
        ClearChessPieces();

        WhitePieces = new List<ChessPiece>();
        BlackPieces = new List<ChessPiece>();

        // Spawn Pawns
        for (int i = 0; i < chessBoard.Width; i++)
        {
            SpawnWhitePawn(i, 1);
            SpawnBlackPawn(i, 6);
        }

        // Spawn Knights
        SpawnWhiteKnight(1, 0);
        SpawnWhiteKnight(6, 0);
        SpawnBlackKnight(1, 7);
        SpawnBlackKnight(6, 7);

        // Spawn Bishops
        SpawnWhiteBishop(2, 0);
        SpawnWhiteBishop(5, 0);
        SpawnBlackBishop(2, 7);
        SpawnBlackBishop(5, 7);

        // Spawn Rooks
        SpawnWhiteRook(0, 0);
        SpawnWhiteRook(7, 0);
        SpawnBlackRook(0, 7);
        SpawnBlackRook(7, 7);

        // Spawn Queens
        SpawnWhiteQueen(3, 0);
        SpawnBlackQueen(3, 7);

        // Spawn Kings
        SpawnWhiteKing(4, 0);
        SpawnBlackKing(4, 7);
        
        WhitePieces.Reverse();
        BlackPieces.Reverse();

        if (Application.isPlaying)
        {
            AllPieces.Clear();
            InitAllWhitePieces();
            InitAllBlackPieces();
        }
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
    
    public void ClearChessPieceRuntime()
    {
        int chessPieceCount = chessPieceParent.childCount;
        
        for (int i = 0; i < chessPieceCount; i++)
        {
            Destroy(chessPieceParent.GetChild(i).gameObject);
        }
        
        WhitePieces.Clear();
        BlackPieces.Clear();
    }
}
