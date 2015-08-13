using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stones.Model;

namespace Stones {
    public partial class Form1 : Form {
        const int BoardSize = 15;
        const int CellSize = 25;
        const int GameSize = 5;
        const CellState UserColor = CellState.White;

        Board Board;
        Logic Logic;

        public Form1() {
            InitializeComponent();

            SetClientSizeCore(CellSize * BoardSize, CellSize * BoardSize);
            Board = new Board(BoardSize);
            Logic = new SimpleLogic(Board, GameSize);
        }

        // Drawing

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            DrawBoard(e.Graphics);
        }

        void DrawBoard(Graphics g) {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            foreach(Point location in Board.EnumerateCells()) {
                DrawCell(g, location);
                if(Board.GetStone(location) == CellState.White)
                    DrawWhiteStone(g, location);
                if(Board.GetStone(location) == CellState.Black)
                    DrawBlackStone(g, location);
            }
            
        }

        void DrawCell(Graphics g, Point location) {
            Rectangle rect = CreateCellRect(location);
            g.FillRectangle(Brushes.Gray, rect);
            g.DrawRectangle(Pens.Silver, rect);
        }

        void DrawWhiteStone(Graphics g, Point location) {
            Rectangle rect = CreateStoneRect(location);
            g.FillEllipse(Brushes.White, rect);
            g.DrawEllipse(Pens.Black, rect);
        }

        void DrawBlackStone(Graphics g, Point location) {
            g.FillEllipse(Brushes.Black, CreateStoneRect(location));
        }

        Rectangle CreateCellRect(Point location) {
            return new Rectangle(location.X * CellSize, location.Y * CellSize, CellSize, CellSize);
        }

        Rectangle CreateStoneRect(Point location) {
            Rectangle result = CreateCellRect(location);
            result.Inflate(-3, -3);
            return result;
        }

        // Action

        protected override void OnMouseClick(MouseEventArgs e) {
            base.OnMouseClick(e);
            if(e.Button == MouseButtons.Left) {
                Point cellCoords = new Point(e.X / CellSize, e.Y / CellSize);
                if(Board.GetStone(cellCoords) == CellState.None) {
                    MakeMove(cellCoords, UserColor);

                    CellState opponentColor = Logic.GetOpponentColor(UserColor);
                    Point opponentLocation = Logic.SelectBestCell(opponentColor);
                    if(opponentLocation == Logic.Draw) {
                        MessageBox.Show("Draw!");
                        Board.Clear();
                        Refresh();
                    } else {
                        MakeMove(opponentLocation, opponentColor);
                    }
                }
            }
        }

        void MakeMove(Point location, CellState color) {
            Board.PutStone(color, location);
            Refresh();
            if(Logic.HaveVictoryAt(location, color)) {
                MessageBox.Show(color + " wins!");
                Board.Clear();
                Refresh();
            }
        }
    }
}
