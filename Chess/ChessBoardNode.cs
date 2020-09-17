using Chess;
using System;
using System.Windows.Forms;

public enum ChessBoardNodeColor { None, Selected, Kill};

public class ChessBoardNode
{
    public int locationX;
    public int locationY;

    public CPClass chessPiece;

    public Button thisButton;

	public ChessBoardNode(int x, int y, Button _thisButton = null)
	{
        locationX = x;
        locationY = y;

        thisButton = _thisButton;

        ChangeColor(ChessBoardNodeColor.None);
	}

    public void ChangePiece(CPClass _chessPiece) //Changes piece location
    {
        chessPiece = _chessPiece;
        if(chessPiece != null)
        {
            chessPiece.x = locationX;
            chessPiece.y = locationY;
        }

        if (thisButton != null)
        {
            if (chessPiece == null) //empty piece
            {
                thisButton.Text = "";
            }
            else
            {
                thisButton.Text = chessPiece.CPName;

                if (chessPiece.pieceColor == ChessPieceColor.White)
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
    }

    public void ChangeColor(ChessBoardNodeColor nodeColor) //Change color accordingly
    {
        if(thisButton != null){
            switch (nodeColor) {
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
}
