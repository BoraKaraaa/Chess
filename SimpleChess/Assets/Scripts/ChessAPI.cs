using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ChessAPI
{
    public static bool IsCheckMate()
    {
        return false;
    }

    public static bool IsCheck()
    {
        return false;
    }

    public static List<ChessPiece> GetWhitePieces()
    {
        return ChessPieceSpawner.Instance.WhitePieces;
    }
    
    public static List<ChessPiece> GetBlackPieces()
    {
        return ChessPieceSpawner.Instance.BlackPieces;
    }
    
    public static (Move[], Move[]) GetLegalAndCaptureMoves()
    {
        var chessPieces = GetWhitePieces().Concat(GetBlackPieces());

        List<ChessPiece> totalLegalMoves = new List<ChessPiece>();
        List<ChessPiece> totalLegalCaptureMoves = new List<ChessPiece>();
        
        foreach (var chessPiece in chessPieces)
        {
            //(Move[], Move[]) legalMoves = chessPiece.LegalMoveSquares();
            //totalLegalMoves.AddRange(legalMoves.Item1);
        }
        
        return (null, null);
    }
    
    public static Move[] GetLegalMoves()
    {
        return null;
    }

    public static Move[] GetCaptureMoves()
    {
        return null;
    }
}
