#region License Information
/* HeuristicLab
 * Copyright (C) Heuristic and Evolutionary Algorithms Laboratory (HEAL)
 *
 * This file is part of HeuristicLab.
 *
 * HeuristicLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HeuristicLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using System;
using CartesianGeneticProgramming.Interpreter.Math;

namespace CartesianGeneticProgramming {

  public abstract class IMathProvider<T> {
    public abstract T Divide(T a, T b);
    public abstract T Multiply(T a, T b);
    public abstract T Add(T a, T b);
    public abstract T Substract(T a, T b);

    public abstract T Sin(T a, T b);
    public abstract T Cos(T a, T b);
    public abstract T Tan(T a, T b);

    public abstract T Pow(T a, T b);
    public abstract T Cube(T a, T b);

    public abstract T Square(T a, T b);
    public abstract T Root(T a, T b);
    public abstract T Sqrt(T a, T b);
    public abstract T CubeRoot(T a, T b);

    public abstract T Exp(T a, T b);
    public abstract T Log(T a, T b);

    public abstract T ConstA(T a, T b);

    public abstract T ConstB(T a, T b);

    public abstract T Abs(T a);
    public abstract double Normalize(T a);

    /* //lang version 8 */
    public Func<T, T, T> Apply(OpCode opCode) {
      switch (opCode) {
        case OpCode.Add:
          return Add;
        case OpCode.Sub:
          return Substract;
        case OpCode.Mul:
          return Multiply;
        case OpCode.Div:
          return Divide;
        case OpCode.Sin:
          return Sin;
        case OpCode.Cos:
          return Cos;
        case OpCode.Tan:
          return Tan;
        case OpCode.Pow:
          return Pow;
        case OpCode.Cube:
          return Cube;
        case OpCode.Square:
          return Square;
        case OpCode.Root:
          return Root;
        case OpCode.Sqrt:
          return Sqrt;
        case OpCode.CubeRoot:
          return CubeRoot;
        case OpCode.Exp:
          return Exp;
        case OpCode.Log:
          return Log;
        case OpCode.ConstA:
          return ConstA;
      }
      throw new NotSupportedException();
    }

  }

  public class MathProviderFactory {
    public static IMathProvider<T> CreateProvider<T>() {
      if (typeof(T) == typeof(double)) {
        return new DoubleMathProvider() as IMathProvider<T>;
      }

      throw new InvalidOperationException();
    }
  }

  class DoubleMathProvider : IMathProvider<double> {
    public override double Divide(double a, double b) {
      return a / b;
    }

    public override double Multiply(double a, double b) {
      return a * b;
    }

    public override double Add(double a, double b) {
      return a + b;
    }


    public override double Substract(double a, double b) {
      return a - b;
    }

    public override double Abs(double a) {
      return Math.Abs(a);
    }

    public override double Normalize(double a) {
      return a;
    }

    public override double Sin(double a, double b) {
      return Math.Sin(a);
    }

    public override double Cos(double a, double b) {
      return Math.Cos(a);
    }

    public override double Tan(double a, double b) {
      return Math.Tan(a);
    }

    //may return NaN for negative exponents
    public override double Pow(double a, double b) {
      return Math.Pow(a, b);
    }

    public override double Cube(double a, double b) {
      return Math.Pow(a, 3);
    }

    public override double Square(double a, double b) {
      return a * a;
    }

    public override double Root(double a, double b) {
      return Math.Pow(a, 1.0 / b);
    }

    public override double Sqrt(double a, double b) {
      return Math.Sqrt(a);
    }

    public override double CubeRoot(double a, double b) {
      return Math.Pow(a, 1.0 / 3);
    }

    public override double Exp(double a, double b) {
      return Math.Exp(a);
    }

    public override double Log(double a, double b) {
      return Math.Log(a);
    }

    public override double ConstA(double a, double b) {
      return a;
    }

    public override double ConstB(double a, double b) {
      return b;
    }
  }
}
