using System.Collections.Generic;
using UnityEngine;
using System;

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

    private static bool IsInvalidMove(Move move)
    {
        ChessAPI.MakeAbstractMove(move);
        if (ChessAPI.IsCheckMe())
        {
            ChessAPI.UndoAbstractMove(move);
            return true;
        }
        ChessAPI.UndoAbstractMove(move);
        return false;
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

                    if (IsInvalidMove(legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    horizontalMoves.Add(legalMove);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    if (IsInvalidMove(captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
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
                    
                    if (IsInvalidMove(legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    horizontalMoves.Add(legalMove);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    if (IsInvalidMove(captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
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
    public static (Move[], Move[]) CheckVerticalMoves(ChessPiece chessPiece, Square refSquare, int distance, EColor currentPlayerColor)
    {
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
                    
                    if (IsInvalidMove(legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    verticalMoves.Add(legalMove);
                }
                else if (chessPiece.EChessPiece != EChessPiece.PAWN &&
                         currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    if (IsInvalidMove(captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
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
                    
                    if (IsInvalidMove(legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    verticalMoves.Add(legalMove);
                }
                else if (chessPiece.EChessPiece != EChessPiece.PAWN &&
                         currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    if (IsInvalidMove(captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
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
    
    
    public static (Move[], Move[]) CheckDiagonalMoves(ChessPiece chessPiece, Square refSquare, int distance, EColor currentPlayerColor)
    {
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
                    
                    if (IsInvalidMove(legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    diagonalMoves.Add(legalMove);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    if (IsInvalidMove(captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
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
                    
                    if (IsInvalidMove(legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    diagonalMoves.Add(legalMove);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    if (IsInvalidMove(captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
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
                    
                    if (IsInvalidMove(legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    diagonalMoves.Add(legalMove);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    if (IsInvalidMove(captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
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
                    
                    if (IsInvalidMove(legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    diagonalMoves.Add(legalMove);
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, currentSquare, currentSquare.ChessPiece),
                        refSquare, currentSquare, chessPiece,
                        currentSquare.ChessPiece, isCaptured:true);
                    
                    if (IsInvalidMove(captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
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
    public static (Move[], Move[]) CheckKingMoves(ChessPiece chessPiece, Square refSquare, int distance, EColor currentPlayerColor)
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
                            
                            if (IsInvalidMove(legalMove))
                            {
                                continue;
                            }
                            
                            if (ChessAPI.IsMoveCheck(legalMove))
                            {
                                legalMove.IsChecked = true;
                            }
                            
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
                            
                            if (IsInvalidMove(captureMove))
                            {
                                continue;
                            }
                            
                            if (ChessAPI.IsMoveCheck(captureMove))
                            {
                                captureMove.IsChecked = true;
                            }
                            
                            checkAroundMoves.Add(captureMove);
                            checkAroundCaptureMoves.Add(captureMove);
                        }
                    }
                }
            }
        }
        
        // Check for castling
        /*
        if (ChessAPI.CanKingCastle(currentPlayerColor))
        {
            // Check kingside castling
            Square kingsideRookSquare = ChessBoard.Instance.Board[refTileRow][7];
            if (kingsideRookSquare != null && kingsideRookSquare.ChessPiece != null
                                           && kingsideRookSquare.ChessPiece.EChessPiece == EChessPiece.ROOK
                                           && kingsideRookSquare.ChessPiece.EColor == currentPlayerColor
                                           && ChessAPI.IsClearPathForCastling(refSquare, kingsideRookSquare))
            {
                Move kingsideCastleMove = new Move(GetSquareNotation(chessPiece, refSquare, kingsideRookSquare),
                    refSquare, kingsideRookSquare, chessPiece,
                    kingsideRookSquare.ChessPiece, isCastle: true);

                if (!IsInvalidMove(kingsideCastleMove) && !ChessAPI.IsMoveCheck(kingsideCastleMove))
                {
                    checkAroundMoves.Add(kingsideCastleMove);
                }
            }

            // Check queenside castling
            Square queensideRookSquare = ChessBoard.Instance.Board[refTileRow][0];
            if (queensideRookSquare != null && queensideRookSquare.ChessPiece != null
                                            && queensideRookSquare.ChessPiece.EChessPiece == EChessPiece.ROOK
                                            && queensideRookSquare.ChessPiece.EColor == currentPlayerColor
                                            && ChessAPI.IsClearPathForCastling(refSquare, queensideRookSquare))
            {
                Move queensideCastleMove = new Move(GetSquareNotation(chessPiece, refSquare, queensideRookSquare),
                    refSquare, queensideRookSquare, chessPiece,
                    queensideRookSquare.ChessPiece, isCastle: true);

                if (!IsInvalidMove(queensideCastleMove) && !ChessAPI.IsMoveCheck(queensideCastleMove))
                {
                    checkAroundMoves.Add(queensideCastleMove);
                }
            }
        }
        */
        
        return (checkAroundMoves.ToArray(), checkAroundCaptureMoves.ToArray());
    }


    /// <summary>
    /// 
    /// </summary>
    public static (Move[], Move[]) CheckLMoves(ChessPiece chessPiece, Square refSquare, EColor currentPlayerColor)
    {
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
                    
                    if (IsInvalidMove(legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    LMoves.Add(legalMove);
                }
                
                if (!targetSquare.IsSquareEmpty() && targetSquare.ChessPiece.EColor != currentPlayerColor)
                {
                    Move captureMove = new Move(GetSquareNotation(chessPiece, targetSquare, targetSquare.ChessPiece),
                        refSquare, targetSquare, chessPiece,
                        targetSquare.ChessPiece, isCaptured:true);
                    
                    if (IsInvalidMove(captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
                    LMoves.Add(captureMove);
                    LCaptureMoves.Add(captureMove); 
                }
            }
        }

        return (LMoves.ToArray(), LCaptureMoves.ToArray());
    }
    
    public static (Move[], Move[]) CheckPawnMoves(ChessPiece chessPiece, Square refSquare, EColor currentPlayerColor)
    {
        var chessPieceTypes = Enum.GetValues(typeof(EChessPiece));

        List<Move> pawnMoves = new List<Move>();
        List<Move> pawnCaptureMoves = new List<Move>();

        int refSquareRow = refSquare.Row;
        int refSquareCol = refSquare.Col;

        int forwardDirection = (currentPlayerColor == EColor.WHITE) ? 1 : -1;

        // Check one square forward
        int targetRow = refSquareRow + forwardDirection;

        Square targetSquare = null;

        if (IsInsideBounds(targetRow, refSquareCol))
        {
            targetSquare = ChessBoard.Instance.Board[targetRow][refSquareCol];
        }
        else
        {
            return (Array.Empty<Move>(), Array.Empty<Move>());
        }

        bool isPromotion = false;

        if (targetSquare.IsSquareEmpty())
        {
            isPromotion = IsPromotion(targetSquare.Row);

            if (isPromotion)
            {
                chessPieceTypes = Enum.GetValues(typeof(EChessPiece));

                foreach (var chessPieceType in chessPieceTypes)
                {
                    if ((EChessPiece)chessPieceType == EChessPiece.NONE ||
                        (EChessPiece)chessPieceType == EChessPiece.PAWN ||
                        (EChessPiece)chessPieceType == EChessPiece.KING)
                    {
                        continue;
                    }

                    Move legalMove = new Move(GetSquareNotation(chessPiece, targetSquare),
                        refSquare, targetSquare, chessPiece,
                        null, isPromotion: true, promotionType: (EChessPiece)chessPieceType);

                    if (IsInvalidMove(legalMove))
                    {
                        continue;
                    }

                    if (ChessAPI.IsMoveCheck(legalMove))
                    {
                        legalMove.IsChecked = true;
                    }

                    pawnMoves.Add(legalMove);
                }
            }
            else
            {
                Move legalMove = new Move(GetSquareNotation(chessPiece, targetSquare),
                    refSquare, targetSquare, chessPiece,
                    null);

                if (!IsInvalidMove(legalMove))
                {
                    if (ChessAPI.IsMoveCheck(legalMove))
                    {
                        legalMove.IsChecked = true;
                    }

                    pawnMoves.Add(legalMove);
                }
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

                if (!IsInvalidMove(legalMove))
                {
                    if (ChessAPI.IsMoveCheck(legalMove))
                    {
                        legalMove.IsChecked = true;
                    }

                    pawnMoves.Add(legalMove);
                }
            }
        }

        // Check diagonal captures
        int[] captureCols = { refSquareCol - 1, refSquareCol + 1 };

        Square enPassantSquare = CalculateEnPassantSquare();
        
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
                            if ((EChessPiece)chessPieceType == EChessPiece.NONE ||
                                (EChessPiece)chessPieceType == EChessPiece.PAWN ||
                                (EChessPiece)chessPieceType == EChessPiece.KING)
                            {
                                continue;
                            }

                            Move captureMove = new Move(GetSquareNotation(chessPiece, targetSquare),
                                refSquare, targetSquare, chessPiece,
                                targetSquare.ChessPiece, isCaptured: true, isPromotion: true, promotionType: (EChessPiece)chessPieceType);

                            if (IsInvalidMove(captureMove))
                            {
                                continue;
                            }

                            if (ChessAPI.IsMoveCheck(captureMove))
                            {
                                captureMove.IsChecked = true;
                            }

                            pawnMoves.Add(captureMove);
                            pawnCaptureMoves.Add(captureMove);
                        }
                    }
                    else
                    {
                        Move captureMove = new Move(GetSquareNotation(chessPiece, targetSquare),
                            refSquare, targetSquare, chessPiece,
                            targetSquare.ChessPiece, isCaptured: true);

                        if (IsInvalidMove(captureMove))
                        {
                            continue;
                        }

                        if (ChessAPI.IsMoveCheck(captureMove))
                        {
                            captureMove.IsChecked = true;
                        }

                        pawnMoves.Add(captureMove);
                        pawnCaptureMoves.Add(captureMove);
                    }
                }
                else if (targetSquare == enPassantSquare)
                {
                    Move enPassantMove = new Move(GetSquareNotation(chessPiece, targetSquare),
                        refSquare, targetSquare, chessPiece,
                        targetSquare.ChessPiece, isCaptured: true, isEnpassant: true);

                    if (!IsInvalidMove(enPassantMove))
                    {
                        if (ChessAPI.IsMoveCheck(enPassantMove))
                        {
                            enPassantMove.IsChecked = true;
                        }

                        pawnMoves.Add(enPassantMove);
                        pawnCaptureMoves.Add(enPassantMove);
                    }
                }
            }
        }

        return (pawnMoves.ToArray(), pawnCaptureMoves.ToArray());
    }

    public static bool CanHorizontallyThreat(Square initialSquare, Square targetSquare)
    {
        if (initialSquare.Row != targetSquare.Row)
        {
            return false;
        }
        
        int colDiff = targetSquare.Col - initialSquare.Col;
        int step = Math.Sign(colDiff);
        
        for (int col = initialSquare.Col + step; col != targetSquare.Col; col += step)
        {
            Square currentSquare = ChessBoard.Instance.Board[initialSquare.Row][col];
            
            if (currentSquare.ChessPiece != null)
            {
                return false;
            }
        }
        
        return true;
    }

    public static bool CanVerticallyThreat(Square initialSquare, Square targetSquare)
    {
        if (initialSquare.Col != targetSquare.Col)
        {
            return false;
        }
        
        int rowDiff = targetSquare.Row - initialSquare.Row;
        int step = Math.Sign(rowDiff);
        
        for (int row = initialSquare.Row + step; row != targetSquare.Row; row += step)
        {
            Square currentSquare = ChessBoard.Instance.Board[row][initialSquare.Col];
            
            if (currentSquare.ChessPiece != null)
            {
                return false;
            }
        }

        return true;
    }

    public static bool CanDiagonallyThreat(Square initialSquare, Square targetSquare)
    {
        int rowDiff = Math.Abs(initialSquare.Row - targetSquare.Row);
        int colDiff = Math.Abs(initialSquare.Col - targetSquare.Col);
        
        if (rowDiff != colDiff)
        {
            return false;
        }
        
        int rowStep = Math.Sign(targetSquare.Row - initialSquare.Row);
        int colStep = Math.Sign(targetSquare.Col - initialSquare.Col);
        
        for (int row = initialSquare.Row + rowStep, col = initialSquare.Col + colStep;
             row != targetSquare.Row;
             row += rowStep, col += colStep)
        {
            Square currentSquare = ChessBoard.Instance.Board[row][col];
            
            if (currentSquare.ChessPiece != null)
            {
                return false;
            }
        }
        
        return true;
    }

    public static bool CanKingThreat(Square initialSquare, Square targetSquare)
    {
        int rowDiff = Math.Abs(initialSquare.Row - targetSquare.Row);
        int colDiff = Math.Abs(initialSquare.Col - targetSquare.Col);
        return rowDiff <= 1 && colDiff <= 1;
    }

    public static bool CanKnightThreat(Square initialSquare, Square targetSquare)
    {
        int rowDiff = Math.Abs(initialSquare.Row - targetSquare.Row);
        int colDiff = Math.Abs(initialSquare.Col - targetSquare.Col);
        return (rowDiff == 1 && colDiff == 2) || (rowDiff == 2 && colDiff == 1);
    }

    public static bool CanPawnThreat(Square initialSquare, Square targetSquare)
    {
        int rowDiff = Math.Abs(initialSquare.Row - targetSquare.Row);
        int colDiff = Math.Abs(initialSquare.Col - targetSquare.Col);
        
        if (initialSquare.ChessPiece.EColor == EColor.WHITE)
        {
            if (rowDiff == 1 && colDiff == 1 && targetSquare.Row == initialSquare.Row + 1)
            {
                return true;
            }
        }
        else
        {
            if (rowDiff == 1 && colDiff == 1 && targetSquare.Row == initialSquare.Row - 1)
            {
                return true;
            }
        }

        // Check if en passant is possible
        Square enPassantSquare = CalculateEnPassantSquare();
        
        if (enPassantSquare != null)
        {
            int enPassantRowDiff = Math.Abs(initialSquare.Row - enPassantSquare.Row);
            int enPassantColDiff = Math.Abs(initialSquare.Col - enPassantSquare.Col);

            if (enPassantRowDiff == 1 && enPassantColDiff == 1 &&
                targetSquare.Row == enPassantSquare.Row && targetSquare.Col == enPassantSquare.Col)
            {
                return true;
            }
        }

        return false;
    }
    
    private static Square CalculateEnPassantSquare()
    {
        Move lastMove = ChessAPI.GetLastMove();

        if (lastMove != null && lastMove.MovedChessPiece is Pawn &&
            Math.Abs(lastMove.InitialSquare.Row - lastMove.TargetSquare.Row) == 2)
        {
            if (lastMove.TargetSquare.Row == lastMove.InitialSquare.Row &&
                Math.Abs(lastMove.TargetSquare.Col - lastMove.InitialSquare.Col) == 1)
            {
                int enPassantRow = lastMove.TargetSquare.Row + (lastMove.InitialSquare.Row < lastMove.TargetSquare.Row ? 1 : -1);
                return ChessBoard.Instance.Board[enPassantRow][lastMove.TargetSquare.Col];
            }
        }

        return null;
    }
}
