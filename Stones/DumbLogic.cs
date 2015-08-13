using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stones.Model;
using System.Drawing;

namespace Stones {
    class DumbLogic : Logic {

        public DumbLogic(Board board, int gameSize)
            : base(board, gameSize) {

        }

        public override Point SelectBestCell(CellState color) {
            foreach(Point attempt in Board.EnumerateCells()) {
                if(Board.GetStone(attempt) == CellState.None)
                    return attempt;
            }
            return Draw;
        }
    }
}
