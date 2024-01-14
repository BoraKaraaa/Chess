
using Unity.VisualScripting;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override (Move[], Move[]) GetLegalMoves()
    {
        return ChessBoardAPI.CheckPawnMoves(this, square, eColor);
    }

    public override bool CanThreatSquare(Square targetSquare)
    {
        return ChessBoardAPI.CanPawnThreat(square, targetSquare);
    }

    protected override void OnAfterMoveCustomAction(Move move)
    {
        base.OnAfterMoveCustomAction(move);
        
        if (move.IsPromotion)
        {
            move.TargetSquare.ChessPiece = null;
            square = null;

            ChessPiece promotedPiece = null;
            Sprite promotedPieceSprite = null;
            
            switch (move.PromotionType)
            {
                case EChessPiece.KNIGHT:
                    promotedPiece = this.AddComponent<Knight>();
                    promotedPiece.EChessPiece = EChessPiece.KNIGHT;
                    promotedPieceSprite = (eColor == EColor.WHITE)
                        ? ChessPieceRefs.Instance.WhiteKnightSprite
                        : ChessPieceRefs.Instance.BlackKnightSprite;
                    break;
                case EChessPiece.BISHOP:
                    promotedPiece = this.AddComponent<Bishop>();
                    promotedPiece.EChessPiece = EChessPiece.BISHOP;
                    promotedPieceSprite = (eColor == EColor.WHITE)
                        ? ChessPieceRefs.Instance.WhiteBishopSprite
                        : ChessPieceRefs.Instance.BlackBishopSprite;
                    break;
                case EChessPiece.ROOK:
                    promotedPiece = this.AddComponent<Rook>();
                    promotedPiece.EChessPiece = EChessPiece.ROOK;
                    promotedPieceSprite = (eColor == EColor.WHITE)
                        ? ChessPieceRefs.Instance.WhiteRookSprite
                        : ChessPieceRefs.Instance.BlackRookSprite;
                    break;
                case EChessPiece.QUEEN:
                    promotedPiece = this.AddComponent<Queen>();
                    promotedPiece.EChessPiece = EChessPiece.QUEEN;
                    promotedPieceSprite = (eColor == EColor.WHITE)
                        ? ChessPieceRefs.Instance.WhiteQueenSprite
                        : ChessPieceRefs.Instance.BlackQueenSprite;
                    break;
            }

            promotedPiece.name = "" + eColor + eChessPiece;
            promotedPiece.EColor = this.EColor;
            promotedPiece.Square = move.TargetSquare;
            
            GetComponentInChildren<SpriteRenderer>().sprite = promotedPieceSprite;

            if (eColor == EColor.WHITE)
            {
                ChessPieceSpawner.Instance.WhitePieces.Add(promotedPiece);
                ChessPieceSpawner.Instance.WhitePieces.Remove(this);
            }
            else
            {
                ChessPieceSpawner.Instance.BlackPieces.Add(promotedPiece);
                ChessPieceSpawner.Instance.BlackPieces.Remove(this);
            }
            
            Destroy(this);
        }
    }
    
    public override string GetChessPieceNotationChar()
    {
        return "";
    }
}
