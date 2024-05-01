
public struct Move
{
    public int InitialSquareIndex; 
    public int TargetSquareIndex; 

    public Square InitialSquare
    {
        get => ChessBoard.Instance.Board[InitialSquareIndex / 8][InitialSquareIndex % 8];
        set => InitialSquareIndex = 8 * value.Row + value.Col;
    }

    public Square TargetSquare
    {
        get => ChessBoard.Instance.Board[TargetSquareIndex / 8][TargetSquareIndex % 8];
        set => TargetSquareIndex = 8 * value.Row + value.Col;
    }
    
    public EChessPieceSpec MovedChessPieceSpec;
    public EChessPieceSpec CapturedChessPieceSpec;

    public ChessPiece MovedChessPiece
    {
        get => ChessPieceSpawner.Instance.AllPieces[MovedChessPieceSpec];
    }

    public ChessPiece CapturedChessPiece
    {
        get => ChessPieceSpawner.Instance.AllPieces[CapturedChessPieceSpec];
    }
    
    public EChessPieceSpec CastleRookSpec;

    public ChessPiece CastleRook
    {
        get => ChessPieceSpawner.Instance.AllPieces[CastleRookSpec];
    }
    
    public bool IsChecked;
    public bool IsCaptured;
    public bool IsCastles;
    public bool IsCastlesKingSide;
    public bool IsPromotion;
    public bool IsEnpassant;
    
    public EChessPiece PromotionType;
    
    public bool IsNull;

    public Move(int initialSquareIndex, int targetSquareIndex, EChessPieceSpec movedChessPiece,
        EChessPieceSpec capturedChessPiece, EChessPieceSpec castleRookSpec = new EChessPieceSpec(), bool isChecked = false, bool isCaptured = false, 
        bool isCastles = false, bool isCastlesKingSide = false, bool isPromotion = false, bool isEnpassant = false, 
        EChessPiece promotionType = EChessPiece.NONE)
    {
        IsNull = false;
        
        InitialSquareIndex = initialSquareIndex;
        TargetSquareIndex = targetSquareIndex;
        MovedChessPieceSpec = movedChessPiece;
        CapturedChessPieceSpec = capturedChessPiece;
        CastleRookSpec = castleRookSpec;
        IsChecked = isChecked;
        IsCaptured = isCaptured;
        IsCastles = isCastles;
        IsCastlesKingSide = isCastlesKingSide;
        IsPromotion = isPromotion;
        IsEnpassant = isEnpassant;
        PromotionType = promotionType;
    }

    public Move(bool isNull)
    {
        IsNull = isNull;
        
        InitialSquareIndex = 0;
        TargetSquareIndex = 0;
        MovedChessPieceSpec = new EChessPieceSpec();
        CapturedChessPieceSpec = new EChessPieceSpec();
        CastleRookSpec = new EChessPieceSpec();
        IsChecked = false;
        IsCaptured = false;
        IsCastles = false;
        IsCastlesKingSide = false;
        IsPromotion = false;
        IsEnpassant = false;
        PromotionType = EChessPiece.NONE;
    }
}
