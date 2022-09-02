using System;
using System.Collections.Generic;
using CartesianGeneticProgramming.Models;

namespace CartesianGeneticProgramming.Interpreter.Math {
  public enum OpCode : byte {
    ConstA = 0,
    Add = 1,
    Sub = 2,
    Mul = 3,
    Div = 4,
    Sin = 5,
    Cos = 6,
    Tan = 7,
    Log = 8,
    Exp = 9,
    Pow = 10,
    Cube = 11,
    Sqrt = 12,
    Square = 13,
    Root = 14,
    CubeRoot = 15,
  };

  public static class OpCodes {
    // constants for API compatibility only
    public const byte ConstA = (byte)OpCode.ConstA;
    public const byte Add = (byte)OpCode.Add;
    public const byte Sub = (byte)OpCode.Sub;
    public const byte Mul = (byte)OpCode.Mul;
    public const byte Div = (byte)OpCode.Div;
    public const byte Sin = (byte)OpCode.Sin;
    public const byte Cos = (byte)OpCode.Cos;
    public const byte Tan = (byte)OpCode.Tan;
    public const byte Log = (byte)OpCode.Log;
    public const byte Exp = (byte)OpCode.Exp;
    public const byte Pow = (byte)OpCode.Pow;
    public const byte Cube = (byte)OpCode.Cube;
    public const byte Sqrt = (byte)OpCode.Sqrt;
    public const byte Square = (byte)OpCode.Square;
    public const byte Root = (byte)OpCode.Root;
    public const byte CubeRoot = (byte)OpCode.CubeRoot;


    private static Dictionary<int, string> numberToString = new Dictionary<int, string>() {
      [0] = "const",
      [1] = "+",
      [2] = "-",
      [3] = "*",
      [4] = "/",
      [5] = "sin",
      [6] = "cos",
      [7] = "tan",
      [8] = "log",
      [9] = "exp",
      [10] = "pow",
      [11] = "^3",
      [12] = "sqrt",
      [13] = "^2",
      [14] = "root",
      [15] = "cubeRoot",
    };

    private static Dictionary<int, OpCode> numberToOpCode = new Dictionary<int, OpCode>() {
      { 0, OpCode.ConstA },
      { 1, OpCode.Add },
      { 2, OpCode.Sub },
      { 3, OpCode.Mul },
      { 4, OpCode.Div },
      { 5, OpCode.Sin },
      { 6, OpCode.Cos },
      { 7, OpCode.Tan},
      { 8, OpCode.Log },
      { 9, OpCode.Exp },
      { 10, OpCode.Pow },
      { 11, OpCode.Cube },
      { 12, OpCode.Sqrt },
      { 13, OpCode.Square },
      { 14, OpCode.Root },
      { 15, OpCode.CubeRoot },
    };

    private static Dictionary<int, int> numberToArity = new Dictionary<int, int>() {
      { 0, 1 },
      { 1, 2 },
      { 2, 2 },
      { 3, 2 },
      { 4, 2 },
      { 5, 1 },
      { 6, 1 },
      { 7, 1},
      { 8, 1 },
      { 9, 2 },
      { 10, 2 },
      { 11, 1 },
      { 12, 1 },
      { 13, 1 },
      { 14, 2 },
      { 15, 1 },
    };

    public static OpCode MapNodeToOpCode(Node node) {
      if (numberToOpCode.TryGetValue(node.FunctionNumber, out OpCode opCode)) return opCode;
      else throw new NotSupportedException("Symbol: " + node.Name + " (id = " + node.Id + ")");
    }

    public static string MapNodeToString(Node node) {
      if (numberToString.TryGetValue(node.FunctionNumber, out string functionString)) return functionString;
      else throw new NotSupportedException("Symbol: " + node.Name + " (id = " + node.Id + ")");
    }

    public static int MapNodeToArity(Node node) {
      if (node.Type == NodeType.OUTPUT) return 1;
      if (node.Type == NodeType.INPUT) return 0;
      if (numberToArity.TryGetValue(node.FunctionNumber, out int arity)) return arity;
      else throw new NotSupportedException("Symbol: " + node.Name + " (id = " + node.Id + ")");
    }
  }
}
