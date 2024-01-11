using System.Collections.Generic;
using UnityEngine;

public static class ChessBoardAPI
{
    private static bool IsInsideBounds(int row, int col) 
    {
        return (row >= 0 && row < ChessBoard.Instance.Height) && (col >= 0 && col < ChessBoard.Instance.Width);
    }
    
    public static float GetTwoSquareHypotenuseDistance(Square square1, Square square2)
    {
        return Mathf.Sqrt(Mathf.Pow((square1.Row - square2.Row), 2) + Mathf.Pow((square1.Col - square2.Col), 2));
    }
    
    /// <summary>
    /// Returns available squares to the right and left.
    /// </summary>
    public static (Square[], Square[]) CheckHorizontalMoves(Square refSquare, int distance, EColor currentPlayerColor)
    {
        List<Square> horizontalMoves = new List<Square>();
        List<Square> horizontalCaptureMoves = new List<Square>();
        
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
                    horizontalMoves.Add(currentSquare);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    horizontalMoves.Add(currentSquare);
                    horizontalCaptureMoves.Add(currentSquare);
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
    public static (Square[], Square[]) CheckVerticalMoves(ChessPiece chessPiece, Square refSquare, int distance, EColor currentPlayerColor)
    {
        List<Square> verticalMoves = new List<Square>();
        List<Square> verticalCaptureMoves = new List<Square>();
        
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
                    verticalMoves.Add(currentSquare);
                }
                else if (chessPiece.EChessPiece != EChessPiece.PAWN &&
                         currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    verticalMoves.Add(currentSquare);
                    verticalCaptureMoves.Add(currentSquare);
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
    public static (Square[], Square[]) CheckDiagonalMoves(Square refSquare, int distance, EColor currentPlayerColor) 
    {
        List<Square> diagonalMoves = new List<Square>();
        List<Square> diagonalCaptureMoves = new List<Square>();
        
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
                    diagonalMoves.Add(currentSquare);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    diagonalMoves.Add(currentSquare);
                    diagonalCaptureMoves.Add(currentSquare);
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
                    diagonalMoves.Add(currentSquare);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    diagonalMoves.Add(currentSquare);
                    diagonalCaptureMoves.Add(currentSquare);
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
    public static (Square[], Square[]) CheckAroundMoves(Square refSquare, int distance, EColor currentPlayerColor)
    {
        List<Square> checkAroundMoves = new List<Square>();
        List<Square> checkAroundCaptureMoves = new List<Square>();
        
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
                            checkAroundMoves.Add(currentSquare);
                        }
                    }
                    else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                    {
                        if (i == refTileRow - distance || i == refTileRow + distance
                                                       || j == refTileCol - distance 
                                                       || j == refTileCol + distance)
                        {
                            checkAroundMoves.Add(currentSquare);
                            checkAroundCaptureMoves.Add(currentSquare);
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
