using System;
using System.Windows.Forms;


public enum ChessPieceColor { None, White, Black};

public enum ChessPiece {None, King, Queen, Rook, Bishop, Knight, Pawn};

public enum ChessBoardNodeColor { None, Selected, Kill};

public class ChessBoardNode
{
    public int locationX;
    public int locationY;

    public ChessPiece chessPiece;
    public ChessPieceColor chessPieceColor;

    public Button thisButton;

	public ChessBoardNode(int x, int y, Button _thisButton)
	{
        locationX = x;
        locationY = y;

        thisButton = _thisButton;

        ChangeColor(ChessBoardNodeColor.None);
	}

    public void ChangePiece(ChessPiece _chessPiece, ChessPieceColor _color) //Changes piece location
    {
        chessPiece = _chessPiece;
        chessPieceColor = _color;

        if(chessPiece == ChessPiece.None) //empty piece
        {
            thisButton.Text = "";
        }
        else
        {
            thisButton.Text = chessPiece.ToString();

            if(_color == ChessPieceColor.White)
            {
                thisButton.ForeColor = System.Drawing.Color.White;
            }
            else //Black
            {
                thisButton.ForeColor = System.Drawing.Color.Black;
            }
        }
        ChangeColor(ChessBoardNodeColor.None);
    }

    public void ChangeColor(ChessBoardNodeColor nodeColor) //Change color accordingly
    {
        switch(nodeColor){
            case ChessBoardNodeColor.None:
                if (locationX % 2 == 0)
                {
                    if (locationY % 2 == 0)
                    {
                        thisButton.BackColor = System.Drawing.Color.Wheat;
                    }
                    else
                    {
                        thisButton.BackColor = System.Drawing.Color.Gray;
                    }
                }
                else
                {
                    if (locationY % 2 == 0)
                    {
                        thisButton.BackColor = System.Drawing.Color.Gray;
                    }
                    else
                    {
                        thisButton.BackColor = System.Drawing.Color.Wheat;
                    }
                }
                break;
            case ChessBoardNodeColor.Kill:
                thisButton.BackColor = System.Drawing.Color.Red;
                break;
            case ChessBoardNodeColor.Selected:
                thisButton.BackColor = System.Drawing.Color.Yellow;
                break;
        }
    }
}
