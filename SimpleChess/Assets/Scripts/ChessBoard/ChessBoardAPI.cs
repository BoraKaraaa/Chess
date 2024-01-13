using System;
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

    private static void DeletePieceFromBoard(ChessPiece chessPiece)
    {
        chessPiece.Square.ChessPiece = null;
    }

    private static void AddPieceToBoard(ChessPiece chessPiece)
    {
        chessPiece.Square.ChessPiece = chessPiece;
    }

    private static bool IsPromotion(int targetRow)
    {
        return (TurnController.Instance.CurrentTurn == EColor.WHITE) && (targetRow == 7)
               || (TurnController.Instance.CurrentTurn == EColor.BLACK) && (targetRow == 0);
    }
    
    /// <summary>
    /// Returns available squares to the right and left.
    /// </summary>
    public static (Move[], Move[]) CheckHorizontalMoves(ChessPiece chessPiece, Square refSquare, int distance, EColor currentPlayerColor, bool controlCheck)
    {
        if (controlCheck)
        {
            DeletePieceFromBoard(chessPiece);

            if (ChessAPI.IsCheck())
            {
                AddPieceToBoard(chessPiece);
                return (Array.Empty<Move>(), Array.Empty<Move>());
            }

            AddPieceToBoard(chessPiece);
        }
        
        List<Move> horizontalMoves = new List<Move>();
        List<Move> horizontalCaptureMoves = new List<Move>();

        int refSquareRow = refSquare.Row;
        int refSquareCol = refSquare.Col;

        Square currentSquare;

        // Check to the right
        for (int i = 1; i <= distance; i++)
        {
            if (IsInsideBounds(refSquareRow, refSquareCol + i))
            {
                currentSquare = ChessBoard.Instance.Board[refSquareRow][refSquareCol + i];

                if (currentSquare.IsSquareEmpty())
                {
                    Move legalMove = new Move(GetSquareNotation(chessPiece, currentSquare),
                        refSquare, currentSquare, chessPiece,
                        null);
                    
                    horizontalMoves.Add(legalMove);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    horizontalMoves.Add(captureMove);
                    horizontalCaptureMoves.Add(captureMove);
                    break;
                }
                else
                {
                    break; // Stop checking in this direction
                }
            }
        }

        // Check to the left
        for (int i = 1; i <= distance; i++)
        {
            if (IsInsideBounds(refSquareRow, refSquareCol - i))
            {
                currentSquare = ChessBoard.Instance.Board[refSquareRow][refSquareCol - i];

                if (currentSquare.IsSquareEmpty())
                {
                    Move legalMove = new Move(GetSquareNotation(chessPiece, currentSquare),
                        refSquare, currentSquare, chessPiece,
                        null);
                    
                    horizontalMoves.Add(legalMove);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    horizontalMoves.Add(captureMove);
                    horizontalCaptureMoves.Add(captureMove);
                    break;
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
    public static (Move[], Move[]) CheckVerticalMoves(ChessPiece chessPiece, Square refSquare, int distance, EColor currentPlayerColor, bool controlCheck)
    {
        if (controlCheck)
        {
            DeletePieceFromBoard(chessPiece);

            if (ChessAPI.IsCheck())
            {
                AddPieceToBoard(chessPiece);
                return (Array.Empty<Move>(), Array.Empty<Move>());
            }

            AddPieceToBoard(chessPiece);
        }
        
        List<Move> verticalMoves = new List<Move>();
        List<Move> verticalCaptureMoves = new List<Move>();

        int refSquareRow = refSquare.Row;
        int refSquareCol = refSquare.Col;

        Square currentSquare;

        // Check downward
        for (int i = 1; i <= distance; i++)
        {
            if (IsInsideBounds(refSquareRow + i, refSquareCol))
            {
                currentSquare = ChessBoard.Instance.Board[refSquareRow + i][refSquareCol];

                if (currentSquare.IsSquareEmpty())
                {
                    Move legalMove = new Move(GetSquareNotation(chessPiece, currentSquare),
                        refSquare, currentSquare, chessPiece,
                        null);
                    
                    verticalMoves.Add(legalMove);
                }
                else if (chessPiece.EChessPiece != EChessPiece.PAWN &&
                         currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    verticalMoves.Add(captureMove);
                    verticalCaptureMoves.Add(captureMove);
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        // Check upward
        for (int i = 1; i <= distance; i++)
        {
            if (IsInsideBounds(refSquareRow - i, refSquareCol))
            {
                currentSquare = ChessBoard.Instance.Board[refSquareRow - i][refSquareCol];

                if (currentSquare.IsSquareEmpty())
                {
                    Move legalMove = new Move(GetSquareNotation(chessPiece, currentSquare),
                        refSquare, currentSquare, chessPiece,
                        null);
                    
                    verticalMoves.Add(legalMove);
                }
                else if (chessPiece.EChessPiece != EChessPiece.PAWN &&
                         currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    verticalMoves.Add(captureMove);
                    verticalCaptureMoves.Add(captureMove);
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        return (verticalMoves.ToArray(), verticalCaptureMoves.ToArray());
    }
    
    
    public static (Move[], Move[]) CheckDiagonalMoves(ChessPiece chessPiece, Square refSquare, int distance, EColor currentPlayerColor, bool controlCheck)
    {
        if (controlCheck)
        {
            DeletePieceFromBoard(chessPiece);

            if (ChessAPI.IsCheck())
            {
                AddPieceToBoard(chessPiece);
                return (Array.Empty<Move>(), Array.Empty<Move>());
            }

            AddPieceToBoard(chessPiece);   
        }
        
        List<Move> diagonalMoves = new List<Move>();
        List<Move> diagonalCaptureMoves = new List<Move>();

        int refSquareRow = refSquare.Row;
        int refSquareCol = refSquare.Col;

        Square currentSquare;

        // Check to the upper left
        for (int i = 1; i <= distance; i++)
        {
            if (IsInsideBounds(refSquareRow - i, refSquareCol - i))
            {
                currentSquare = ChessBoard.Instance.Board[refSquareRow - i][refSquareCol - i];

                if (currentSquare.IsSquareEmpty())
                {
                    Move legalMove = new Move(GetSquareNotation(chessPiece, currentSquare),
                        refSquare, currentSquare, chessPiece,
                        null);
                    
                    diagonalMoves.Add(legalMove);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    diagonalMoves.Add(captureMove);
                    diagonalCaptureMoves.Add(captureMove);
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        // Check to the upper right
        for (int i = 1; i <= distance; i++)
        {
            if (IsInsideBounds(refSquareRow - i, refSquareCol + i))
            {
                currentSquare = ChessBoard.Instance.Board[refSquareRow - i][refSquareCol + i];

                if (currentSquare.IsSquareEmpty())
                {
                    Move legalMove = new Move(GetSquareNotation(chessPiece, currentSquare),
                        refSquare, currentSquare, chessPiece,
                        null);
                    
                    diagonalMoves.Add(legalMove);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    diagonalMoves.Add(captureMove);
                    diagonalCaptureMoves.Add(captureMove);
                    break; 
                }
                else
                {
                    break; 
                }
            }
        }

        // Check to the lower left
        for (int i = 1; i <= distance; i++)
        {
            if (IsInsideBounds(refSquareRow + i, refSquareCol - i))
            {
                currentSquare = ChessBoard.Instance.Board[refSquareRow + i][refSquareCol - i];

                if (currentSquare.IsSquareEmpty())
                {
                    Move legalMove = new Move(GetSquareNotation(chessPiece, currentSquare),
                        refSquare, currentSquare, chessPiece,
                        null);
                    
                    diagonalMoves.Add(legalMove);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    diagonalMoves.Add(captureMove);
                    diagonalCaptureMoves.Add(captureMove);
                    break; 
                }
                else
                {
                    break; 
                }
            }
        }

        // Check to the lower right
        for (int i = 1; i <= distance; i++)
        {
            if (IsInsideBounds(refSquareRow + i, refSquareCol + i))
            {
                currentSquare = ChessBoard.Instance.Board[refSquareRow + i][refSquareCol + i];

                if (currentSquare.IsSquareEmpty())
                {
                    Move legalMove = new Move(GetSquareNotation(chessPiece, currentSquare),
                        refSquare, currentSquare, chessPiece,
                        null);
                    
                    diagonalMoves.Add(legalMove);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    diagonalMoves.Add(captureMove);
                    diagonalCaptureMoves.Add(captureMove);
                    break;
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
    public static (Move[], Move[]) CheckAroundMoves(ChessPiece chessPiece, Square refSquare, int distance, EColor currentPlayerColor, bool controlCheck)
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
                                null);
                            
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
                                currentSquare.ChessPiece, isCaptured:true);
                            
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
        
        if (controlCheck)
        {
            bool itemRemoved = true;
            
            for (int i = 0; i < checkAroundMoves.Count;)
            {
                Move move = checkAroundMoves[i];
                
                // Do Move 
                move.InitialSquare.ChessPiece = null;
                chessPiece.Square = move.TargetSquare;
                move.TargetSquare.ChessPiece = chessPiece;
                
                if (move.IsCaptured)
                {
                    ChessPiece capturedPiece = move.CapturedChessPiece;
                    
                    if (capturedPiece.EColor == EColor.WHITE)
                    {
                        ChessPieceSpawner.Instance.WhitePieces.Remove(capturedPiece);
                    }
                    else
                    {
                        ChessPieceSpawner.Instance.BlackPieces.Remove(capturedPiece);
                    }

                    capturedPiece.Square.ChessPiece = null;
                }
                
                if (ChessAPI.IsCheck())
                {
                    checkAroundMoves.RemoveAt(i);

                    if (move.IsCaptured)
                    {
                        checkAroundCaptureMoves.Remove(move);
                    }
                }
                else
                {
                    itemRemoved = false;   
                }
                
                move.InitialSquare.ChessPiece = chessPiece;
                chessPiece.Square = move.InitialSquare;
                move.TargetSquare.ChessPiece = null;
            
                if (move.IsCaptured)
                {
                    ChessPiece capturedPiece = move.CapturedChessPiece;
                    
                    if (capturedPiece.EColor == EColor.WHITE)
                    {
                        ChessPieceSpawner.Instance.WhitePieces.Add(capturedPiece);
                    }
                    else
                    {
                        ChessPieceSpawner.Instance.BlackPieces.Add(capturedPiece);
                    }

                    capturedPiece.Square.ChessPiece = capturedPiece;
                }

                if (!itemRemoved)
                {
                    itemRemoved = true;
                    i++;
                }
            }
        }
        
        return (checkAroundMoves.ToArray(), checkAroundCaptureMoves.ToArray());
    }


    /// <summary>
    /// 
    /// </summary>
    public static (Move[], Move[]) CheckLMoves(ChessPiece chessPiece, Square refSquare, EColor currentPlayerColor, bool controlCheck)
    {
        if (controlCheck)
        {
            DeletePieceFromBoard(chessPiece);

            if (ChessAPI.IsCheck())
            {
                AddPieceToBoard(chessPiece);
                return (Array.Empty<Move>(), Array.Empty<Move>());
            }

            AddPieceToBoard(chessPiece);
        }
        
        List<Move> LMoves = new List<Move>();
        List<Move> LCaptureMoves = new List<Move>();
    
        int refSquareRow = refSquare.Row;
        int refSquareCol = refSquare.Col;
        
        int[][] knightMoves = 
        {
            new int[] { 1, 2 },
            new int[] { 2, 1 },
            new int[] { -1, 2 },
            new int[] { -2, 1 },
            new int[] { 1, -2 },
            new int[] { 2, -1 },
            new int[] { -1, -2 },
            new int[] { -2, -1 }
        };
        
        foreach (var move in knightMoves)
        {
            int newRow = refSquareRow + move[0];
            int newCol = refSquareCol + move[1];
            
            if (IsInsideBounds(newRow, newCol))
            {
                Square targetSquare = ChessBoard.Instance.Board[newRow][newCol];
                
                if (targetSquare.IsSquareEmpty())
                {
                    Move legalMove = new Move(GetSquareNotation(chessPiece, targetSquare),
                        refSquare, targetSquare, chessPiece,
                        null);
                    
                    LMoves.Add(legalMove);
                }
                
                if (!targetSquare.IsSquareEmpty() && targetSquare.ChessPiece.EColor != currentPlayerColor)
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, targetSquare, targetSquare.ChessPiece),
                        refSquare, targetSquare, chessPiece,
                        targetSquare.ChessPiece, isCaptured:true);
                    
                    LMoves.Add(captureMove);
                    LCaptureMoves.Add(captureMove); 
                }
            }
        }

        return (LMoves.ToArray(), LCaptureMoves.ToArray());
    }
    
    public static (Move[], Move[]) CheckPawnMoves(ChessPiece chessPiece, Square refSquare, EColor currentPlayerColor, bool controlCheck)
    {
        if (controlCheck)
        {
            DeletePieceFromBoard(chessPiece);

            if (ChessAPI.IsCheck())
            {
                AddPieceToBoard(chessPiece);
                return (Array.Empty<Move>(), Array.Empty<Move>());
            }

            AddPieceToBoard(chessPiece);
        }
        
        var chessPieceTypes = Enum.GetValues(typeof(EChessPiece));
        
        List<Move> pawnMoves = new List<Move>();
        List<Move> pawnCaptureMoves = new List<Move>();

        int refSquareRow = refSquare.Row;
        int refSquareCol = refSquare.Col;

        int forwardDirection = (currentPlayerColor == EColor.WHITE) ? 1 : -1;

        // Check one square forward
        int targetRow = refSquareRow + forwardDirection;

        Square targetSquare = ChessBoard.Instance.Board[targetRow][refSquareCol];

        bool isPromotion = false;
        
        if (IsInsideBounds(targetRow, refSquareCol) && targetSquare.IsSquareEmpty())
        {
            isPromotion = IsPromotion(targetSquare.Row);

            if (isPromotion)
            {
                chessPieceTypes = Enum.GetValues(typeof(EChessPiece));

                foreach (var chessPieceType in chessPieceTypes)
                {
                    if ((EChessPiece)chessPieceType == EChessPiece.DEFAULT ||
                        (EChessPiece)chessPieceType == EChessPiece.PAWN ||
                        (EChessPiece)chessPieceType == EChessPiece.KING)
                    {
                        continue;
                    }
                    
                    Move legalMove = new Move(GetSquareNotation(chessPiece, targetSquare),
                        refSquare, targetSquare, chessPiece,
                        null, isPromotion:true, promotionType:(EChessPiece)chessPieceType);
                    
                    pawnMoves.Add(legalMove);
                }
            }
            else
            {
                Move legalMove = new Move(GetSquareNotation(chessPiece, targetSquare),
                    refSquare, targetSquare, chessPiece,
                    null);
                    
                pawnMoves.Add(legalMove);
            }
        }
        
        // Check two squares forward from starting position
        int startingRow = (currentPlayerColor == EColor.WHITE) ? 1 : 6;
        
        if (refSquareRow == startingRow && targetSquare.IsSquareEmpty())
        {
            int twoSquaresForward = refSquareRow + 2 * forwardDirection;
            
            targetSquare = ChessBoard.Instance.Board[twoSquaresForward][refSquareCol];
            
            if (IsInsideBounds(twoSquaresForward, refSquareCol) && targetSquare.IsSquareEmpty())
            {
                Move legalMove = new Move(GetSquareNotation(chessPiece, targetSquare),
                        refSquare, targetSquare, chessPiece,
                        null);
                
                pawnMoves.Add(legalMove);
            }
        }

        // Check diagonal captures
        int[] captureCols = { refSquareCol - 1, refSquareCol + 1 };
        
        foreach (int captureCol in captureCols)
        {
            if (IsInsideBounds(refSquareRow + forwardDirection, captureCol))
            {
                targetSquare = ChessBoard.Instance.Board[refSquareRow + forwardDirection][captureCol];
                
                if (!targetSquare.IsSquareEmpty() && targetSquare.ChessPiece.EColor != currentPlayerColor)
                {
                    isPromotion = false;
                    isPromotion = IsPromotion(targetSquare.Row);
                    
                    if (isPromotion)
                    {
                        chessPieceTypes = Enum.GetValues(typeof(EChessPiece));

                        foreach (var chessPieceType in chessPieceTypes)
                        {
                            if ((EChessPiece)chessPieceType == EChessPiece.DEFAULT ||
                                (EChessPiece)chessPieceType == EChessPiece.PAWN ||
                                (EChessPiece)chessPieceType == EChessPiece.KING)
                            {
                                continue;
                            }
                            
                            Move captureMove = new Move(GetSquareNotation(chessPiece, targetSquare),
                                refSquare, targetSquare, chessPiece,
                                targetSquare.ChessPiece, isCaptured:true, isPromotion:true, promotionType:(EChessPiece)chessPieceType);
                    
                            pawnMoves.Add(captureMove);
                            pawnCaptureMoves.Add(captureMove);
                        }
                    }
                    else
                    {
                        Move captureMove = new Move(GetSquareNotation(chessPiece, targetSquare),
                            refSquare, targetSquare, chessPiece,
                            targetSquare.ChessPiece, isCaptured:true);
                    
                        pawnMoves.Add(captureMove);
                        pawnCaptureMoves.Add(captureMove);
                    }
                }
            }
        }

        return (pawnMoves.ToArray(), pawnCaptureMoves.ToArray());
    }
    
}
