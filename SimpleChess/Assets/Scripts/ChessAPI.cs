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
        List<ChessPiece> chessPieces;
        
        if (TurnController.Instance.CurrentTurn == EColor.WHITE)
        {
            chessPieces = GetWhitePieces();
        }
        else
        {
            chessPieces = GetBlackPieces();
        }

        List<Move> totalLegalMoves = new List<Move>();
        List<Move> totalLegalCaptureMoves = new List<Move>();
        
        foreach (ChessPiece chessPiece in chessPieces)
        {
            (Move[], Move[]) legalMoves = chessPiece.GetLegalMoves();
            
            totalLegalMoves.AddRange(legalMoves.Item1);
            totalLegalCaptureMoves.AddRange(legalMoves.Item2);
        }
        
        return (totalLegalMoves.ToArray(), totalLegalCaptureMoves.ToArray());
    }

    public static Move[] GetLegalMoves()
    {
        List<ChessPiece> chessPieces;
        
        if (TurnController.Instance.CurrentTurn == EColor.WHITE)
        {
            chessPieces = GetWhitePieces();
        }
        else
        {
            chessPieces = GetBlackPieces();
        }
        
        List<Move> totalLegalMoves = new List<Move>();
        
        foreach (ChessPiece chessPiece in chessPieces)
        {
            (Move[], Move[]) legalMoves = chessPiece.GetLegalMoves();
            
            totalLegalMoves.AddRange(legalMoves.Item1);
        }

        return totalLegalMoves.ToArray();
    }

    public static Move[] GetCaptureMoves()
    {
        List<ChessPiece> chessPieces;
        
        if (TurnController.Instance.CurrentTurn == EColor.WHITE)
        {
            chessPieces = GetWhitePieces();
        }
        else
        {
            chessPieces = GetBlackPieces();
        }
        
        List<Move> totalLegalCaptureMoves = new List<Move>();
        
        foreach (ChessPiece chessPiece in chessPieces)
        {
            (Move[], Move[]) legalMoves = chessPiece.GetLegalMoves();

            totalLegalCaptureMoves.AddRange(legalMoves.Item2);
        }
        
        return totalLegalCaptureMoves.ToArray();
    }
    
}
