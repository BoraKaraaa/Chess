using UnityEngine;
using System;

public static class ChessBoardAPI
{
    private const string x = "x";
    private const string kingSideCastleNotation = "0-0";
    private const string queenSideCastleNotation = "0-0-0";
    
    static int[][] knightMoves = 
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
    
    public static bool IsInsideBounds(int row, int col) 
    {
        return (row >= 0 && row < ChessBoard.Instance.Height) && (col >= 0 && col < ChessBoard.Instance.Width);
    }
    
    public static bool IsSquareEmpty(int row, int col)
    {
        return ChessBoard.Instance.Board[row][col].IsSquareEmpty();
    }
    
    public static bool IsSquareUnderThreat(int row, int col)
    {
        Square targetSquare = ChessBoard.Instance.Board[row][col];
        
        foreach (var oppChessPiece in ChessAPI.GetOpponentPieces())
        {
            if (oppChessPiece.CanThreatSquare(targetSquare))
            {
                return true;
            }
        }

        return false;
    }
    
    public static float GetTwoSquareHypotenuseDistance(Square square1, Square square2)
    {
        return Mathf.Sqrt(Mathf.Pow((square1.Row - square2.Row), 2) + Mathf.Pow((square1.Col - square2.Col), 2));
    }
    
    public static EChessPieceSpec GetChessPieceSpec(ChessPiece chessPiece)
    {
        if (chessPiece == null) return new EChessPieceSpec();
        return new EChessPieceSpec(chessPiece.EColor, chessPiece.EChessPiece, chessPiece.PieceIndex);
    }
    
    public static int GetSquare1DRep(Square square)
    {
        return 8 * square.Row + square.Col;
    }
    
    public static string GetSquareNotation(ChessPiece chessPiece, Square square)
    {
        return chessPiece.GetChessPieceNotationChar() + square.SquareNotation;
    }
    
    public static string GetSquareNotation(ChessPiece chessPiece, Square square, ChessPiece capturedPiece)
    {
        return chessPiece.GetChessPieceNotationChar() + x + square.SquareNotation;
    }

    public static string GerKingSideCastleNotation()
    {
        return kingSideCastleNotation;
    }
    
    public static string GerQueenSideCastleNotation()
    {
        return queenSideCastleNotation;
    }
    
    public static Square GetKingSideCastleKingSquare()
    {
        int row = (TurnController.Instance.CurrentTurn == EColor.WHITE) ? 0 : 7;
        return ChessBoard.Instance.Board[row][6];
    }

    public static Square GetQueenSideCastleKingSquare()
    {
        int row = (TurnController.Instance.CurrentTurn == EColor.WHITE) ? 0 : 7;
        return ChessBoard.Instance.Board[row][2];
    }
    
    public static Square GetKingSideCastleRookSquare()
    {
        int row = (TurnController.Instance.CurrentTurn == EColor.WHITE) ? 0 : 7;
        return ChessBoard.Instance.Board[row][5];
    }

    public static Square GetQueenSideCastleRookSquare()
    {
        int row = (TurnController.Instance.CurrentTurn == EColor.WHITE) ? 0 : 7;
        return ChessBoard.Instance.Board[row][3];
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

    private static bool IsInvalidMove(ref Move move)
    {
        ChessAPI.MakeAbstractMove(ref move);
        if (ChessAPI.IsCheckOpp())
        {
            ChessAPI.UndoAbstractMove(ref move);
            return true;
        }
        ChessAPI.UndoAbstractMove(ref move);
        return false;
    }
    
    /// <summary>
    /// Returns available squares to the right and left.
    /// </summary>
    public static void CheckHorizontalMoves(ref Span<Move> legalMoves, ref int legalMoveIndex,
        ref Span<Move> captureMoves, ref int captureMoveIndex, ChessPiece chessPiece, Square refSquare, 
        int distance, EColor currentPlayerColor)
    {
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
                    Move legalMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare), 
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(null));

                    if (IsInvalidMove(ref legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref legalMove))
                    {
                        legalMove.IsChecked = true;
                    }

                    legalMoves[legalMoveIndex++] = legalMove;
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare),
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(currentSquare.ChessPiece),
                        isCaptured:true);
                    
                    if (IsInvalidMove(ref captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref captureMove))
                    {
                        captureMove.IsChecked = true;
                    }

                    legalMoves[legalMoveIndex++] = captureMove;
                    captureMoves[captureMoveIndex++] = captureMove;
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
                    Move legalMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare),
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(null));
                    
                    if (IsInvalidMove(ref legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    legalMoves[legalMoveIndex++] = legalMove;
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare), 
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(currentSquare.ChessPiece),
                        isCaptured:true);
                    
                    if (IsInvalidMove(ref captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
                    legalMoves[legalMoveIndex++] = captureMove;
                    captureMoves[captureMoveIndex++] = captureMove;
                    break;
                }
                else
                {
                    break;
                }
            }
        }
    }
    
    /// <summary>
    /// Returns available squares to the up and down.
    /// </summary>
    public static void CheckVerticalMoves(ref Span<Move> legalMoves, ref int legalMoveIndex,
        ref Span<Move> captureMoves, ref int captureMoveIndex, ChessPiece chessPiece, Square refSquare, 
        int distance, EColor currentPlayerColor)
    {
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
                    Move legalMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare), 
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(null));
                    
                    if (IsInvalidMove(ref legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref legalMove))
                    {
                        legalMove.IsChecked = true;
                    }

                    legalMoves[legalMoveIndex++] = legalMove;
                }
                else if (chessPiece.EChessPiece != EChessPiece.PAWN &&
                         currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare),
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(currentSquare.ChessPiece),
                        isCaptured:true);
                    
                    if (IsInvalidMove(ref captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
                    legalMoves[legalMoveIndex++] = captureMove;
                    captureMoves[captureMoveIndex++] = captureMove;
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
                    Move legalMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare),
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(null));
                    
                    if (IsInvalidMove(ref legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    legalMoves[legalMoveIndex++] = legalMove;
                }
                else if (chessPiece.EChessPiece != EChessPiece.PAWN &&
                         currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare), 
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(currentSquare.ChessPiece),
                        isCaptured:true);
                    
                    if (IsInvalidMove(ref captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
                    legalMoves[legalMoveIndex++] = captureMove;
                    captureMoves[captureMoveIndex++] = captureMove;
                    break;
                }
                else
                {
                    break;
                }
            }
        }
    }
    
    
    public static void CheckDiagonalMoves(ref Span<Move> legalMoves, ref int legalMoveIndex,
        ref Span<Move> captureMoves, ref int captureMoveIndex, ChessPiece chessPiece, Square refSquare, 
        int distance, EColor currentPlayerColor)
    {
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
                    Move legalMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare),
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(null));
                    
                    if (IsInvalidMove(ref legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref legalMove))
                    {
                        legalMove.IsChecked = true;
                    }

                    legalMoves[legalMoveIndex++] = legalMove;
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare), 
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(currentSquare.ChessPiece),
                        isCaptured:true);
                    
                    if (IsInvalidMove(ref captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
                    legalMoves[legalMoveIndex++] = captureMove;
                    captureMoves[captureMoveIndex++] = captureMove;
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
                    Move legalMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare),
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(null));
                    
                    if (IsInvalidMove(ref legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    legalMoves[legalMoveIndex++] = legalMove;
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare), 
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(currentSquare.ChessPiece),
                        isCaptured:true);
                    
                    if (IsInvalidMove(ref captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
                    legalMoves[legalMoveIndex++] = captureMove;
                    captureMoves[captureMoveIndex++] = captureMove;
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
                    Move legalMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare), 
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(null));
                    
                    if (IsInvalidMove(ref legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    legalMoves[legalMoveIndex++] = legalMove;
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare),
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(currentSquare.ChessPiece),
                        isCaptured:true);
                    
                    if (IsInvalidMove(ref captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
                    legalMoves[legalMoveIndex++] = captureMove;
                    captureMoves[captureMoveIndex++] = captureMove;
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
                    Move legalMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare),
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(null));
                    
                    if (IsInvalidMove(ref legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    legalMoves[legalMoveIndex++] = legalMove;
                }
                else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                {
                    Move captureMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare),
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(currentSquare.ChessPiece),
                        isCaptured:true);
                    
                    if (IsInvalidMove(ref captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
                    legalMoves[legalMoveIndex++] = captureMove;
                    captureMoves[captureMoveIndex++] = captureMove;
                    break;
                }
                else
                {
                    break; 
                }
            }
        }
    }
    
    /// <summary>
    /// Returns all the tiles around.
    /// </summary>
    public static void CheckKingMoves(ref Span<Move> legalMoves, ref int legalMoveIndex,
        ref Span<Move> captureMoves, ref int captureMoveIndex, ChessPiece chessPiece, Square refSquare, 
        int distance, EColor currentPlayerColor)
    {
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
                            Move legalMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare),
                                GetChessPieceSpec(chessPiece), GetChessPieceSpec(null));
                            
                            if (IsInvalidMove(ref legalMove))
                            {
                                continue;
                            }
                            
                            if (ChessAPI.IsMoveCheck(ref legalMove))
                            {
                                legalMove.IsChecked = true;
                            }
                            
                            legalMoves[legalMoveIndex++] = legalMove;
                        }
                    }
                    else if (currentSquare.ChessPiece.EColor != currentPlayerColor) // Capture
                    {
                        if (i == refTileRow - distance || i == refTileRow + distance
                                                       || j == refTileCol - distance 
                                                       || j == refTileCol + distance)
                        {
                            Move captureMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(currentSquare),
                                GetChessPieceSpec(chessPiece), GetChessPieceSpec(currentSquare.ChessPiece),
                                isCaptured:true);
                            
                            if (IsInvalidMove(ref captureMove))
                            {
                                continue;
                            }
                            
                            if (ChessAPI.IsMoveCheck(ref captureMove))
                            {
                                captureMove.IsChecked = true;
                            }
                            
                            legalMoves[legalMoveIndex++] = captureMove;
                            captureMoves[captureMoveIndex++] = captureMove;
                        }
                    }
                }
            }
        }
        
        // Check for castling
        if (ChessAPI.CanKingCastle())
        {
            if (ChessAPI.IsShortCastlingPossible())
            {
                Square kingsideRookSquare = GetKingSideCastleKingSquare();
                
                Move kingsideCastleMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(kingsideRookSquare),
                    GetChessPieceSpec(chessPiece), GetChessPieceSpec(null),
                    castleRookSpec:GetChessPieceSpec(ChessAPI.GetRightRook()), isCastles: true, isCastlesKingSide:true);
                    
                if (!IsInvalidMove(ref kingsideCastleMove))
                {
                    legalMoves[legalMoveIndex++] = kingsideCastleMove;
                }
            }
            
            if (ChessAPI.IsLongCastlingPossible())
            {
                Square queensideRookSquare = GetQueenSideCastleKingSquare();
                
                Move queensideCastleMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(queensideRookSquare), 
                    GetChessPieceSpec(chessPiece), GetChessPieceSpec(null),
                    castleRookSpec:GetChessPieceSpec(ChessAPI.GetLeftRook()), isCastles: true);

                if (!IsInvalidMove(ref queensideCastleMove))
                {
                    legalMoves[legalMoveIndex++] = queensideCastleMove;
                }
            }
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public static void CheckLMoves(ref Span<Move> legalMoves, ref int legalMoveIndex,
        ref Span<Move> captureMoves, ref int captureMoveIndex, ChessPiece chessPiece, Square refSquare, EColor currentPlayerColor)
    {
        int refSquareRow = refSquare.Row;
        int refSquareCol = refSquare.Col;
        
        foreach (var move in knightMoves)
        {
            int newRow = refSquareRow + move[0];
            int newCol = refSquareCol + move[1];
            
            if (IsInsideBounds(newRow, newCol))
            {
                Square targetSquare = ChessBoard.Instance.Board[newRow][newCol];
                
                if (targetSquare.IsSquareEmpty())
                {
                    Move legalMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(targetSquare), 
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(null));
                    
                    if (IsInvalidMove(ref legalMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref legalMove))
                    {
                        legalMove.IsChecked = true;
                    }

                    legalMoves[legalMoveIndex++] = legalMove;
                }
                
                if (!targetSquare.IsSquareEmpty() && targetSquare.ChessPiece.EColor != currentPlayerColor)
                {
                    Move captureMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(targetSquare),
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(targetSquare.ChessPiece),
                        isCaptured:true);
                    
                    if (IsInvalidMove(ref captureMove))
                    {
                        continue;
                    }
                    
                    if (ChessAPI.IsMoveCheck(ref captureMove))
                    {
                        captureMove.IsChecked = true;
                    }
                    
                    legalMoves[legalMoveIndex++] = captureMove;
                    captureMoves[captureMoveIndex++] = captureMove;
                }
            }
        }
    }
    
    public static void CheckPawnMoves(ref Span<Move> legalMoves, ref int legalMoveIndex,
        ref Span<Move> captureMoves, ref int captureMoveIndex, ChessPiece chessPiece, Square refSquare, EColor currentPlayerColor)
    {
        var chessPieceTypes = Enum.GetValues(typeof(EChessPiece));

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
            return;
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

                    Move legalMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(targetSquare),
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(null),
                        isPromotion: true, promotionType: (EChessPiece)chessPieceType);

                    if (IsInvalidMove(ref legalMove))
                    {
                        continue;
                    }

                    if (ChessAPI.IsMoveCheck(ref legalMove))
                    {
                        legalMove.IsChecked = true;
                    }

                    legalMoves[legalMoveIndex++] = legalMove;
                }
            }
            else
            {
                Move legalMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(targetSquare), 
                    GetChessPieceSpec(chessPiece), GetChessPieceSpec(null));

                if (!IsInvalidMove(ref legalMove))
                {
                    if (ChessAPI.IsMoveCheck(ref legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    legalMoves[legalMoveIndex++] = legalMove;
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
                Move legalMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(targetSquare),
                    GetChessPieceSpec(chessPiece), GetChessPieceSpec(null));

                if (!IsInvalidMove(ref legalMove))
                {
                    if (ChessAPI.IsMoveCheck(ref legalMove))
                    {
                        legalMove.IsChecked = true;
                    }
                    
                    legalMoves[legalMoveIndex++] = legalMove;
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

                            Move captureMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(targetSquare),
                                GetChessPieceSpec(chessPiece), GetChessPieceSpec(targetSquare.ChessPiece),
                                isCaptured: true, isPromotion: true, promotionType: (EChessPiece)chessPieceType);

                            if (IsInvalidMove(ref captureMove))
                            {
                                continue;
                            }

                            if (ChessAPI.IsMoveCheck(ref captureMove))
                            {
                                captureMove.IsChecked = true;
                            }
                            
                            legalMoves[legalMoveIndex++] = captureMove;
                            captureMoves[captureMoveIndex++] = captureMove;
                        }
                    }
                    else
                    {
                        Move captureMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(targetSquare),
                            GetChessPieceSpec(chessPiece), GetChessPieceSpec(targetSquare.ChessPiece),
                            isCaptured: true);

                        if (IsInvalidMove(ref captureMove))
                        {
                            continue;
                        }

                        if (ChessAPI.IsMoveCheck(ref captureMove))
                        {
                            captureMove.IsChecked = true;
                        }
                        
                        legalMoves[legalMoveIndex++] = captureMove;
                        captureMoves[captureMoveIndex++] = captureMove;
                    }
                }
                else if (targetSquare == enPassantSquare)
                {
                    Move enPassantMove = new Move(GetSquare1DRep(refSquare), GetSquare1DRep(targetSquare),
                        GetChessPieceSpec(chessPiece), GetChessPieceSpec(targetSquare.ChessPiece),
                        isCaptured: true, isEnpassant: true);

                    if (!IsInvalidMove(ref enPassantMove))
                    {
                        if (ChessAPI.IsMoveCheck(ref enPassantMove))
                        {
                            enPassantMove.IsChecked = true;
                        }
                        
                        legalMoves[legalMoveIndex++] = enPassantMove;
                        captureMoves[captureMoveIndex++] = enPassantMove;
                    }
                }
            }
        }
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
        
        if (!lastMove.IsNull && lastMove.MovedChessPiece is Pawn &&
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
