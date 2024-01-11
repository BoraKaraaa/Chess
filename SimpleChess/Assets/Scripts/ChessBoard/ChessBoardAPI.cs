using System.Collections.Generic;
using UnityEngine;

public static class ChessBoardAPI
{
    private const string x = "x";
    
    private static bool IsInsideBounds(int row, int col) 
    {
        return (row >= 0 && row < ChessBoard.Instance.Height) && (col >= 0 && col < ChessBoard.Instance.Width);
    }
    
    public static float GetTwoSquareHypotenuseDistance(Square square1, Square square2)
    {
        return Mathf.Sqrt(Mathf.Pow((square1.Row - square2.Row), 2) + Mathf.Pow((square1.Col - square2.Col), 2));
    }

    public static string GetSquareNotation(ChessPiece chessPiece, Square square)
    {
        return chessPiece.GetChessPieceNotationChar() + square.SquareNotation;
    }
    
    public static string GetSquareNotation(ChessPiece chessPiece, Square square, ChessPiece capturedPiece)
    {
        return chessPiece.GetChessPieceNotationChar() + x + square.SquareNotation;
    }
    
    /// <summary>
    /// Returns available squares to the right and left.
    /// </summary>
    public static (Move[], Move[]) CheckHorizontalMoves(ChessPiece chessPiece, Square refSquare, int distance, EColor currentPlayerColor)
    {
        List<Move> horizontalMoves = new List<Move>();
        List<Move> horizontalCaptureMoves = new List<Move>();
        
        int refSquareRow = refSquare.Row;
        int refSquareCol = refSquare.Col;
        
        Square currentSquare;

        for (int i = refSquareCol - distance; i <= refSquareCol + distance; i++) 
        {
            if (IsInsideBounds(refSquareRow, i))
            {
                currentSquare = ChessBoard.Instance.Board[refSquareRow][i];

                if (currentSquare.IsSquareEmpty())
                {
                    Move legalMove = new Move(GetSquareNotation(chessPiece, currentSquare),
                        refSquare, currentSquare, chessPiece,
                        null, false, false);
                    
                    horizontalMoves.Add(legalMove);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, false, true);
                    
                    horizontalMoves.Add(captureMove);
                    horizontalCaptureMoves.Add(captureMove);
                }
                else
                {
                    break;
                }
            }
        }

        return (horizontalMoves.ToArray(), horizontalCaptureMoves.ToArray());
    }
    
    /// <summary>
    /// Returns available squares to the up and down.
    /// </summary>
    public static (Move[], Move[]) CheckVerticalMoves(ChessPiece chessPiece, Square refSquare, int distance, EColor currentPlayerColor)
    {
        List<Move> verticalMoves = new List<Move>();
        List<Move> verticalCaptureMoves = new List<Move>();
        
        int refSquareRow = refSquare.Row;
        int refSquareCol = refSquare.Col;

        Square currentSquare;

        for (int i = refSquareRow - distance; i <= refSquareRow + distance; i++) 
        {
            if (IsInsideBounds(i, refSquareCol)) 
            {
                currentSquare = ChessBoard.Instance.Board[i][refSquareCol];

                if (currentSquare.IsSquareEmpty()) 
                {
                    Move legalMove = new Move(GetSquareNotation(chessPiece, currentSquare),
                        refSquare, currentSquare, chessPiece,
                        null, false, false);
                    
                    verticalMoves.Add(legalMove);
                }
                else if (chessPiece.EChessPiece != EChessPiece.PAWN &&
                         currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, false, true);
                    
                    verticalMoves.Add(captureMove);
                    verticalCaptureMoves.Add(captureMove);
                }
                else
                {
                    break;
                }
            }
        }

        return (verticalMoves.ToArray(), verticalCaptureMoves.ToArray());
    }
    
    /// <summary>
    /// Returns available squares diagonally.
    /// </summary>
    public static (Move[], Move[]) CheckDiagonalMoves(ChessPiece chessPiece, Square refSquare, int distance, EColor currentPlayerColor) 
    {
        List<Move> diagonalMoves = new List<Move>();
        List<Move> diagonalCaptureMoves = new List<Move>();
        
        int refSquareRow = refSquare.Row;
        int refSquareCOl = refSquare.Col;

        Square currentSquare;

        int i = refSquareRow - distance;
        int j = refSquareCOl - distance;
        
        for (; i <= refSquareRow + distance && j <= refSquareCOl + distance; i++, j++) 
        {
            if (IsInsideBounds(i, j))
            {
                currentSquare = ChessBoard.Instance.Board[i][j];
                
                if (currentSquare.IsSquareEmpty()) 
                {
                    Move legalMove = new Move(GetSquareNotation(chessPiece, currentSquare),
                        refSquare, currentSquare, chessPiece,
                        null, false, false);
                    
                    diagonalMoves.Add(legalMove);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, false, true);
                    
                    diagonalMoves.Add(captureMove);
                    diagonalCaptureMoves.Add(captureMove);
                }
                else
                {
                    break;
                }
            }
        }   

        i = refSquareRow + distance;
        j = refSquareCOl - distance;
        
        for (; i >= refSquareRow - distance && j <= refSquareCOl + distance; i--, j++) 
        {
            if (IsInsideBounds(i, j))
            {
                currentSquare = ChessBoard.Instance.Board[i][j];
                
                if (currentSquare.IsSquareEmpty()) 
                {
                    Move legalMove = new Move(GetSquareNotation(chessPiece, currentSquare),
                        refSquare, currentSquare, chessPiece,
                        null, false, false);
                    
                    diagonalMoves.Add(legalMove);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, false, true);
                    
                    diagonalMoves.Add(captureMove);
                    diagonalCaptureMoves.Add(captureMove);
                }
                else
                {
                    break;
                }
            }
        }

        return (diagonalMoves.ToArray(), diagonalCaptureMoves.ToArray());
    }
    
    /// <summary>
    /// Returns all the tiles around.
    /// </summary>
    public static (Move[], Move[]) CheckAroundMoves(ChessPiece chessPiece, Square refSquare, int distance, EColor currentPlayerColor)
    {
        List<Move> checkAroundMoves = new List<Move>();
        List<Move> checkAroundCaptureMoves = new List<Move>();
        
        int refTileRow = refSquare.Row;
        int refTileCol = refSquare.Col;

        Square currentSquare;

        for (int i = refTileRow - distance; i <= refTileRow + distance; i++) 
        {
            for (int j = refTileCol - distance; j <= refTileCol + distance; j++) 
            {
                if (IsInsideBounds(i, j)) 
                {
                    currentSquare = ChessBoard.Instance.Board[i][j];

                    if (currentSquare.IsSquareEmpty()) 
                    {
                        if (i == refTileRow - distance || i == refTileRow + distance
                                                       || j == refTileCol - distance 
                                                       || j == refTileCol + distance)
                        {
                            Move legalMove = new Move(GetSquareNotation(chessPiece, currentSquare),
                                refSquare, currentSquare, chessPiece,
                                null, false, false);
                            
                            checkAroundMoves.Add(legalMove);
                        }
                    }
                    else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                    {
                        if (i == refTileRow - distance || i == refTileRow + distance
                                                       || j == refTileCol - distance 
                                                       || j == refTileCol + distance)
                        {
                            Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                                refSquare, currentSquare, chessPiece,
                                currentSquare.ChessPiece, false, true);
                            
                            checkAroundMoves.Add(captureMove);
                            checkAroundCaptureMoves.Add(captureMove);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        return (checkAroundMoves.ToArray(), checkAroundCaptureMoves.ToArray());
    }
    
    
    
    
}
