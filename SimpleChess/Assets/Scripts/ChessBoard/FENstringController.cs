using System.Text.RegularExpressions;
using System.Text;
using UnityEngine;
using System;
using TMPro;

public class FENstringController : Singleton<FENstringController>
{
    [SerializeField] private TMP_InputField inputField;

    public static bool IsFenStringValid = false;
    
    private StringBuilder fenBuilder = new StringBuilder();
    
    private void Awake()
    {
        if (inputField != null)
        {
            inputField.onEndEdit.AddListener(OnFenStringEntered);
        }
    }

    private void OnDestroy()
    {
        if (inputField != null)
        {
            inputField.onEndEdit.RemoveListener(OnFenStringEntered);
        }
    }

    public string GetCurrentBoardFenString()
    {
        fenBuilder.Clear();

        for (int row = 7; row >= 0; row--)
        {
            int emptySquareCount = 0;

            for (int col = 0; col < 8; col++)
            {
                Square square = ChessBoard.Instance.Board[col][row];
                ChessPiece piece = square.ChessPiece;

                if (piece != null)
                {
                    if (emptySquareCount > 0)
                    {
                        fenBuilder.Append(emptySquareCount);
                        emptySquareCount = 0;
                    }

                    char pieceChar = GetCharFromPiece(piece);
                    fenBuilder.Append(piece.EColor == EColor.WHITE ? char.ToUpper(pieceChar) : pieceChar);
                }
                else
                {
                    emptySquareCount++;
                }
            }

            if (emptySquareCount > 0)
            {
                fenBuilder.Append(emptySquareCount);
            }

            if (row > 0)
            {
                fenBuilder.Append("/");
            }
        }

        //fenBuilder.Append(" ");
        //fenBuilder.Append(TurnController.Instance.CurrentTurn == EColor.WHITE ? "w" : "b");
        //fenBuilder.Append(" KQkq - 0 1");

        return fenBuilder.ToString();
    }

    
    private void OnFenStringEntered(string fenString)
    {
        inputField.text = String.Empty;
        
        if (ValidateFenString(fenString))
        {
            IsFenStringValid = true;
            
            string[] fenParts = fenString.Split(' ');
            
            ChessPieceSpawner.Instance.ClearChessPieceRuntime();
            
            SetChessboardPosition(fenParts[0]);
        }
        else
        {
            Debug.LogError("Invalid FEN string");
        }
    }
    
    private bool ValidateFenString(string fenString)
    {
        string[] parts = fenString.Split(' ');

        if (!IsValidPiecePlacement(parts[0]))
        {
            return false;
        }

        return true;
    }

    private bool IsValidPiecePlacement(string piecePlacement)
    {
        // Piece placement validation: Check for correct piece types and row counts
        return IsValidRowCounts(piecePlacement);
    }

    private bool IsValidRowCounts(string piecePlacement)
    {
        // Check for 8 rows separated by '/' character
        string[] rows = piecePlacement.Split('/');
        if (rows.Length != 8)
        {
            return false;
        }

        // Check that each row has valid piece types and empty squares
        foreach (string row in rows)
        {
            int count = 0;
            foreach (char c in row)
            {
                if (char.IsDigit(c))
                {
                    count += int.Parse(c.ToString());
                }
                else
                {
                    count++;
                }
            }

            if (count != 8)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsValidColor(string color)
    {
        // Implement color validation: Should be "w" or "b"
        return color == "w" || color == "b";
    }

    private bool IsValidCastlingRights(string castlingRights)
    {
        // Implement castling rights validation: Check for valid castling rights format
        return Regex.IsMatch(castlingRights, @"^(-|[KQkq]+)$");
    }

    private bool IsValidEnPassantSquare(string enPassantSquare)
    {
        // Implement en passant square validation: Check for valid square format (e.g., "e3")
        return Regex.IsMatch(enPassantSquare, @"^[a-h][36]$|^-$");
    }

    private bool IsValidHalfMoveClock(string halfMoveClock)
    {
        // Implement half move clock validation: Check for valid positive integer format
        int value;
        return int.TryParse(halfMoveClock, out value) && value >= 0;
    }

    private bool IsValidFullMoveNumber(string fullMoveNumber)
    {
        // Implement full move number validation: Check for valid positive integer format
        int value;
        return int.TryParse(fullMoveNumber, out value) && value >= 1;
    }
    
    private void SetChessboardPosition(string position)
    {
        int row = 0;
        int col = 7;

        foreach (char c in position)
        {
            if (c == '/')
            {
                row++;
                col = 7;
            }
            else if (char.IsDigit(c))
            {
                col -= int.Parse(c.ToString());
            }
            else
            {
                EColor pieceColor = char.IsUpper(c) ? EColor.WHITE : EColor.BLACK;
                EChessPiece pieceType = GetPieceTypeFromChar(c);
                
                int symRow = 7 - row;
                int symCol = 7 - col;

                ChessPieceSpawner.Instance.SpawnChessPiece(pieceColor, pieceType, symCol, symRow);
                col--;
            }
        }
    }
    
    private char GetCharFromPiece(ChessPiece piece)
    {
        switch (piece.EChessPiece)
        {
            case EChessPiece.PAWN:
                return 'p';
            case EChessPiece.KNIGHT:
                return 'n';
            case EChessPiece.BISHOP:
                return 'b';
            case EChessPiece.ROOK:
                return 'r';
            case EChessPiece.QUEEN:
                return 'q';
            case EChessPiece.KING:
                return 'k';
            default:
                return ' ';
        }
    }
    
    private EChessPiece GetPieceTypeFromChar(char c)
    {
        switch (char.ToUpper(c))
        {
            case 'P':
                return EChessPiece.PAWN;
            case 'N':
                return EChessPiece.KNIGHT;
            case 'B':
                return EChessPiece.BISHOP;
            case 'R':
                return EChessPiece.ROOK;
            case 'Q':
                return EChessPiece.QUEEN;
            case 'K':
                return EChessPiece.KING;
            default:
                return EChessPiece.NONE;
        }
    }
}
