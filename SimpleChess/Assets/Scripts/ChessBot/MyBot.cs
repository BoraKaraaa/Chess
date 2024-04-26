using System.Collections.Generic;
using System;

public class MyBot : ChessBot
{
    private static readonly int[] PieceVals = { 0, 10, 30, 35, 50, 90, 900 };

    private static readonly int[] PiecePositionalVals =
    {
        -4, -4, -4, -3, -3, -4, -4, -4,
        -4, -4, -4, -3, -3, -4, -4, -4,
        -4, -2, -2, -3, -3, -2, -2, -4,
        -4, -2, -1,  0,  0, -1, -2, -4,
        -4, -2, -1,  0,  0, -1, -2, -4,
        -4, -2, -2, -3, -3, -2, -2, -4,
        -4, -4, -4, -3, -3, -4, -4, -4,
        -4, -4, -4, -3, -3, -4, -4, -4
    };
    
    private class MoveNode
    {
        public Move Move;
        public readonly MoveNode ParentMoveNode;
        public MoveNode[] ChildMoves;
        public int HeuristicVal;
        public readonly int Depth;

        public MoveNode() { }
        public MoveNode(MoveNode[] childMoves, MoveNode parentMoveNode, Move move, int heuristicVal, int depth)
        {
            HeuristicVal = heuristicVal;
            Move = move;
            ChildMoves = childMoves;
            ParentMoveNode = parentMoveNode;
            Depth = depth;
        }
    }

    private const int MAX_VAL = 9999999;
    private const int MIN_VAL = -9999999;
    
    private const int ABSOLUTE_WIN = 30;

    private const int MIN_CALC_MOVE = 4;

    private ChessGameManager localChessGameManager;
    
    public override Move BestMove(ChessGameManager chessGameManager)
    {
        localChessGameManager = chessGameManager;
        
        int depth = CalculateDynamicDepth();
        return CalculateNDepthMoves(depth); // + (int)(2 * ChessAPI.GetLateGameRate()));
    }
    
    private int CalculateDynamicDepth()
    {
        int totalPieces = ChessAPI.GetMyPieces().Count + ChessAPI.GetOpponentPieces().Count;
        if (totalPieces < 12) return MIN_CALC_MOVE + 2; 
        if (totalPieces < 20) return MIN_CALC_MOVE + 1;
        return MIN_CALC_MOVE;
    }
    
    private Move CalculateNDepthMoves(int depth)
    {
        MoveNode rootMove = new MoveNode(null, new MoveNode(), new Move(), 0, 0);
        MoveNode resMoveNode = MiniMax(rootMove, depth, MIN_VAL, MAX_VAL, true);
        
        while (resMoveNode.Depth != 1)
        {
            resMoveNode = resMoveNode.ParentMoveNode;
        }
        
        return resMoveNode.Move;
    }
    
    
    private void GenerateChildrenMoveNodes(MoveNode parentMoveNode)
    {
        (Move[], Move[]) allMoves = localChessGameManager.GetLegalAndCaptureMoves();

        parentMoveNode.ChildMoves = new MoveNode[allMoves.Item1.Length];

        int lastIndex = 0;
        
        for (int i = 0; i < allMoves.Item2.Length; i++)
        {
            MoveNode childNode = new MoveNode(null, parentMoveNode, allMoves.Item2[i],
                parentMoveNode.ParentMoveNode.HeuristicVal, parentMoveNode.Depth + 1);
            
            childNode.HeuristicVal += CalculatePreHeuristicValue(childNode);
            
            parentMoveNode.ChildMoves[i] = childNode;
            lastIndex = i + 1;
        }
        
        for (int i = 0; i < allMoves.Item1.Length; i++)
        {
            bool sameMoveIncluded = false;
            
            for (int j = 0; j < allMoves.Item2.Length; j++)
            {
                if (allMoves.Item2[j].Equals(allMoves.Item1[i]))
                {
                    sameMoveIncluded = true;
                    break;
                }
            }
            
            if (!sameMoveIncluded)
            {
                MoveNode childNode = new MoveNode(null, parentMoveNode, allMoves.Item1[i],
                    parentMoveNode.ParentMoveNode.HeuristicVal, parentMoveNode.Depth + 1);
                
                childNode.HeuristicVal += CalculatePreHeuristicValue(childNode);
                
                parentMoveNode.ChildMoves[lastIndex++] = childNode;
            }
        }
    }
    
    private bool IsWhitePlayingByDepth(int depth)
    {
        return (EColor == EColor.WHITE) ? depth % 2 == 1 : depth % 2 == 0;
    }

    private EColor GetPlayerColorByDepth(int depth)
    {
        if (IsWhitePlayingByDepth(depth))
        {
            return EColor.WHITE;
        }
        else
        {
            return EColor.BLACK;
        }
    }

    private bool IsIPlaying(int depth)
    {
        return EColor == GetPlayerColorByDepth(depth);
    }
    
    private int CalculatePreHeuristicValue(MoveNode moveNode)
    {
        int preHeuristicVal = 0;
        
        if (moveNode.Move.IsPromotion)
        {
            preHeuristicVal += IsIPlaying(moveNode.Depth) ? 60 : -60;
        }
        
        if (moveNode.Move.IsCastles)
        {
            preHeuristicVal += IsIPlaying(moveNode.Depth) ? 15 : -15;
        }
        
        if (moveNode.Move.MovedChessPiece.EChessPiece == EChessPiece.QUEEN)
        {
            if (TurnController.Instance.TotalMoveCount < 8)
            {
                preHeuristicVal += IsIPlaying(moveNode.Depth) ? -15 : 15;
            }
        }
        
        return preHeuristicVal;
    }

    private int GetPushKingCornerHeuristic()
    {
        int myPiecesCount = ChessAPI.GetMyPieces().Count;
        int opponentPiecesCount = ChessAPI.GetOpponentPieces().Count;
        
        if (!(myPiecesCount <= 5 || opponentPiecesCount <= 5))
        {
            return 0;
        }
        
        King oppKing = (King)ChessAPI.GetOpponentKing();
        return (int)((8 - Math.Min(7 - oppKing.Square.Row, oppKing.Square.Row) + Math.Min(7 - oppKing.Square.Col, oppKing.Square.Col)) 
                     * 5 * ChessAPI.GetLateGameRate() * ChessAPI.GetLateGameRate());
    }
    
    private int CalculatePositionValue(MoveNode moveNode, out bool isCheck)
    {
        int totalHeuristicVal = moveNode.HeuristicVal;

        isCheck = false;
        
        if (localChessGameManager.IsCheckMate())
        {
            if (IsIPlaying(moveNode.Depth))
            {
                return MAX_VAL - 10;
            }

            return MIN_VAL + 10;
        }
        
        if (moveNode.Move.IsChecked && IsIPlaying(moveNode.Depth))
        {
            isCheck = true;
            totalHeuristicVal += 3;
        }
        
        
        totalHeuristicVal += IsIPlaying(moveNode.Depth) ? GetPushKingCornerHeuristic() : -GetPushKingCornerHeuristic();   
        
        
        List<ChessPiece> chessPieces = null;
        List<ChessPiece> oppChessPieces = null;
            
        if (IsIPlaying(moveNode.Depth))
        {
            chessPieces = ChessAPI.GetMyPieces();
            oppChessPieces = ChessAPI.GetOpponentPieces();
        }
        else
        {
            chessPieces = ChessAPI.GetOpponentPieces();
            oppChessPieces = ChessAPI.GetMyPieces();
        }
    
        
        foreach (var chessPiece in chessPieces)
        {
            if (chessPiece.EChessPiece == EChessPiece.PAWN)
            {
                if (IsIPlaying(moveNode.Depth))
                {
                    totalHeuristicVal += chessPiece.EColor == EColor.WHITE ? chessPiece.Square.Row / 2 : (7 
                        - chessPiece.Square.Row) / 2;   
                }
                else
                {
                    totalHeuristicVal += chessPiece.EColor == EColor.WHITE ? -chessPiece.Square.Row / 2 : - ((7
                        - chessPiece.Square.Row) / 2);   
                }
            }
            
            totalHeuristicVal += IsIPlaying(moveNode.Depth) ? PieceVals[(int)chessPiece.EChessPiece] :
                -PieceVals[(int)chessPiece.EChessPiece];

            totalHeuristicVal += IsIPlaying(moveNode.Depth)
                ? PiecePositionalVals[chessPiece.Square.Row * 8 + chessPiece.Square.Col]
                : -PiecePositionalVals[chessPiece.Square.Row * 8 + chessPiece.Square.Col];
        }

        foreach (var oppChessPiece in oppChessPieces)
        {
            if (oppChessPiece.EChessPiece == EChessPiece.PAWN)
            {
                if (IsIPlaying(moveNode.Depth))
                {
                    totalHeuristicVal += oppChessPiece.EColor == EColor.WHITE ? -oppChessPiece.Square.Row / 2 : -((7 
                        - oppChessPiece.Square.Row) / 2);   
                }
                else
                {
                    totalHeuristicVal += oppChessPiece.EColor == EColor.WHITE ? oppChessPiece.Square.Row / 2 : (7
                        - oppChessPiece.Square.Row) / 2;   
                }
            }
            
            totalHeuristicVal += IsIPlaying(moveNode.Depth) ? -PieceVals[(int)oppChessPiece.EChessPiece] :
                PieceVals[(int)oppChessPiece.EChessPiece];

            totalHeuristicVal += IsIPlaying(moveNode.Depth)
                ? -PiecePositionalVals[oppChessPiece.Square.Row * 8 + oppChessPiece.Square.Col]
                : PiecePositionalVals[oppChessPiece.Square.Row * 8 + oppChessPiece.Square.Col];
        }
            
        if (localChessGameManager.IsDraw())
        {
            if (IsIPlaying(moveNode.Depth))
            {
                if (totalHeuristicVal > ABSOLUTE_WIN)
                {
                    totalHeuristicVal -= 500;
                }

                totalHeuristicVal += 500;
            }
            else
            {
                if (totalHeuristicVal < -ABSOLUTE_WIN)
                {
                    totalHeuristicVal += 500;
                }

                totalHeuristicVal -= 500;   
            }
        }
        
        return totalHeuristicVal;
    }
    
    
    private MoveNode MiniMax(MoveNode moveNode, int depth, int alpha, int beta, bool maximizingPlayer)
    {
        bool isCheckMate = localChessGameManager.IsCheckMate();
        bool isDraw = localChessGameManager.IsDraw();

        if (depth == 0 || isCheckMate || isDraw)
        {
            moveNode.HeuristicVal = CalculatePositionValue(moveNode, out bool isCheck);

            if (isCheckMate)
            {
                moveNode.HeuristicVal += depth;
                return moveNode;
            }

            if (isDraw)
            {
                return moveNode;
            }

            if (!isCheck)
            {
                return moveNode;
            }

            depth = 1;
        }
        
        GenerateChildrenMoveNodes(moveNode);
        
        MoveNode localMoveNode = new MoveNode();

        if (maximizingPlayer)
        {
            localMoveNode.HeuristicVal = MIN_VAL;
            
            foreach (var childMove in moveNode.ChildMoves)
            {
                localChessGameManager.MakeAbstractMove(ref childMove.Move);
                MoveNode resMoveNode = MiniMax(childMove, depth - 1, alpha, beta, false);
                localChessGameManager.UndoAbstractMove(ref childMove.Move);
                
                if (localMoveNode.HeuristicVal < resMoveNode.HeuristicVal)
                {
                    localMoveNode = resMoveNode;
                }
                
                alpha = Math.Max(alpha, localMoveNode.HeuristicVal);
                
                if (localMoveNode.HeuristicVal >= beta)
                {
                    break;
                }
            }
        }
        else
        {
            localMoveNode.HeuristicVal = MAX_VAL;
            
            foreach (var childMove in moveNode.ChildMoves)
            {
                localChessGameManager.MakeAbstractMove(ref childMove.Move);
                MoveNode resMoveNode = MiniMax(childMove, depth - 1, alpha, beta, true);
                localChessGameManager.UndoAbstractMove(ref childMove.Move);
                
                if (localMoveNode.HeuristicVal > resMoveNode.HeuristicVal)
                {
                    localMoveNode = resMoveNode;
                }
                
                beta = Math.Min(beta, localMoveNode.HeuristicVal);
                
                if (localMoveNode.HeuristicVal <= alpha)
                {
                    break;
                }
            }
        }
        
        return localMoveNode;
    }
}
