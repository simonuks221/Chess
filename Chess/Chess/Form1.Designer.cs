namespace Chess
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ChessBoard = new System.Windows.Forms.Panel();
            this.MoveLabel = new System.Windows.Forms.Label();
            this.ResetButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ChessBoard
            // 
            this.ChessBoard.Location = new System.Drawing.Point(50, 50);
            this.ChessBoard.Name = "ChessBoard";
            this.ChessBoard.Size = new System.Drawing.Size(400, 400);
            this.ChessBoard.TabIndex = 0;
            // 
            // MoveLabel
            // 
            this.MoveLabel.AutoSize = true;
            this.MoveLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.MoveLabel.Location = new System.Drawing.Point(359, 9);
            this.MoveLabel.Name = "MoveLabel";
            this.MoveLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MoveLabel.Size = new System.Drawing.Size(67, 31);
            this.MoveLabel.TabIndex = 1;
            this.MoveLabel.Text = "Text";
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(422, 456);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 2;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 486);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.MoveLabel);
            this.Controls.Add(this.ChessBoard);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ChessBoard;
        private System.Windows.Forms.Label MoveLabel;
        private System.Windows.Forms.Button ResetButton;
    }
}

