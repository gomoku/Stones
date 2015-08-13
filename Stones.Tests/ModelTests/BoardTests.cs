using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Stones.Model;

namespace Stones.Tests.ModelTests {
    [TestFixture]
    public class BoardTests {

        [Test, ExpectedException(typeof(ArgumentException))]
        public void Ctor_NegativeSize() {
            new Board(-1);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void Ctor_ZeroSize() {
            new Board(0);
        }

        [Test]
        public void Ctor() {
            Assert.AreEqual(2, new Board(2).Size);
        }

        [Test]
        public void IsOutside() {
            Board b = new Board(3);

            // Outside bounds
            Assert.IsTrue(b.IsOutside(1, -1));
            Assert.IsTrue(b.IsOutside(3, 1));
            Assert.IsTrue(b.IsOutside(1, 3));
            Assert.IsTrue(b.IsOutside(-1, 1));

            // Inside bounds
            Assert.IsFalse(b.IsOutside(0, 0));
            Assert.IsFalse(b.IsOutside(2, 2));

            Assert.IsFalse(b.IsOutside(1, 1));
        }

        [Test]
        public void GetStone() {
            Board board = new Board(3);

            board.PutStone(CellState.Black, 1, 1);
            board.PutStone(CellState.White, 2, 2);            

            Assert.AreEqual(CellState.None, board.GetStone(0, 0));
            Assert.AreEqual(CellState.Black, board.GetStone(1, 1));
            Assert.AreEqual(CellState.White, board.GetStone(2, 2));
            Assert.AreEqual(CellState.None, board.GetStone(-1, -1));
        }    

        [Test, ExpectedException(typeof(ArgumentException))]
        public void PutStone_Outside() {
            new Board(1).PutStone(CellState.Black, 3, 3);
        }
    }
}
