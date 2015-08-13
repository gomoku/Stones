using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Stones.Model;
using System.Drawing;

namespace Stones.Tests {

    [TestFixture]
    public class LogicTests {

        class LogicStub : Logic {
            public LogicStub(Board board, int gameSize)
                : base(board, gameSize) {

            }

            public override Point SelectBestCell(CellState color) {
                throw new NotImplementedException();
            }
        }

        [Test,ExpectedException(typeof(ArgumentNullException))]
        public void BoardIsNull() {
            new LogicStub(null, 5);
        }

        [Test,ExpectedException(typeof(ArgumentException))]
        public void InvalidGameSize() {
            new LogicStub(new Board(3), 1);
        }

        [Test]
        public void HaveVictoryAt() {
            Board board = new Board(5);
            board.PutStone(CellState.Black, 0, 2);
            board.PutStone(CellState.White, 1, 2);
            board.PutStone(CellState.White, 3, 2);

            Logic logic = new LogicStub(board, 5);

            Assert.IsFalse(logic.HaveVictoryAt(2, 2, CellState.White));

            board.PutStone(CellState.White, 4, 2);
            Assert.IsFalse(logic.HaveVictoryAt(2, 2, CellState.White));

            board.PutStone(CellState.White, 0, 2);
            Assert.IsFalse(logic.HaveVictoryAt(2, 2, CellState.White));

            board.PutStone(CellState.White, 2, 2);
            Assert.IsTrue(logic.HaveVictoryAt(2, 2, CellState.White));
            Assert.IsFalse(logic.HaveVictoryAt(2, 1, CellState.White));
            Assert.IsFalse(logic.HaveVictoryAt(2, 2, CellState.Black));
        }

        [Test]
        public void GetOpponentColor() {
            Logic logic = new LogicStub(new Board(1), 2);
            Assert.AreEqual(CellState.Black, logic.GetOpponentColor(CellState.White));
            Assert.AreEqual(CellState.White, logic.GetOpponentColor(CellState.Black));
        }
    }
}
