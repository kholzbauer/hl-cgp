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
using System.Collections.Generic;
using System.Linq;
using CartesianGeneticProgramming.Interpreter;
using CartesianGeneticProgramming.Interpreter.Math;
using CartesianGeneticProgramming.Models;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;
using HeuristicLab.Problems.DataAnalysis;

namespace CartesianGeneticProgramming {
  [StorableType("2352bf86-3064-4542-a45c-7b246884a876")]
  [Item("MathInterpreter", "Interpreter that interprets a graph with mathematical expressions.")]
  public class MathInterpreter : ParameterizedNamedItem, ICGPInterpreter {
    private const string EvaluatedSolutionsParameterName = "EvaluatedSolutions";

    #region parameter properties
    public IFixedValueParameter<IntValue> EvaluatedSolutionsParameter {
      get { return (IFixedValueParameter<IntValue>)Parameters[EvaluatedSolutionsParameterName]; }
    }
    #endregion

    #region properties
    public int EvaluatedSolutions {
      get { return EvaluatedSolutionsParameter.Value.Value; }
      set { EvaluatedSolutionsParameter.Value.Value = value; }
    }
    #endregion

    [StorableConstructor]
    protected MathInterpreter(StorableConstructorFlag _) : base(_) { }
    protected MathInterpreter(MathInterpreter original, Cloner cloner) : base(original, cloner) {
    }
    public MathInterpreter() {
      Parameters.Add(new FixedValueParameter<IntValue>(EvaluatedSolutionsParameterName, "A counter for the total number of solutions the interpreter has evaluated", new IntValue(0)));
    }

    public string GetFunctionString(Node node) {
      return OpCodes.MapNodeToString(node);
    }

    public string GetSolutionString(Graph graph) {
      string solution_string = "";
      solution_string = GetSolutionStringRec(graph, graph.Nodes[graph.Output.Inputs.ElementAt(0)], solution_string);
      solution_string += $"={graph.Output.Name}";
      return solution_string;
    }

    private string GetSolutionStringRec(Graph graph, Node node, string s) {

      if (node.Type == NodeType.NODE) {
        int arity = OpCodes.MapNodeToArity(node);
        if (arity == 1) {
          s = $"{node.Name}({GetSolutionStringRec(graph, graph.Nodes[node.Inputs.ElementAt(0)], s)}) ";

        } else {
          s = $"({GetSolutionStringRec(graph, graph.Nodes[node.Inputs.ElementAt(0)], s)})  {node.Name} ({GetSolutionStringRec(graph, graph.Nodes[node.Inputs.ElementAt(1)], s)}) ";
        }
      } else {
        s += node.Name;
      }

      return s;
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new MathInterpreter(this, cloner);
    }

    public int GetNumberOfFunctions() {
      return Enum.GetNames(typeof(OpCode)).Length;
    }

    private readonly object syncRoot = new object();
    public IEnumerable<double> GetGraphValues(Graph graph, IDataset dataset, IEnumerable<int> rows) {
      if (!rows.Any()) return Enumerable.Empty<double>();

      lock (syncRoot) {
        EvaluatedSolutions++; // increment the evaluated solutions counter
      }

      var provider = MathProviderFactory.CreateProvider<double>();

      return rows.Select(row => Evaluate(dataset, row, graph, provider));
    }

    private double Evaluate(IDataset dataset, int row, Graph graph, IMathProvider<double> provider) {
      var result = GetNodeResult(graph.Nodes[graph.Output.Inputs.FirstOrDefault()], dataset, row, graph, provider);
      if (double.IsNaN(result) || double.IsInfinity(result)) {
        result = double.MaxValue;
      }
      return result;
    }

    private double GetNodeResult(Node node, IDataset dataset, int row, Graph graph, IMathProvider<double> provider) {
      try {
        if (node.Type == NodeType.NODE) {
          var arity = OpCodes.MapNodeToArity(node);

          var leftHS = GetNodeResult(graph.Nodes[node.Inputs.ElementAt(0)], dataset, row, graph, provider);
          double rightHS = 0.0;
          if (arity == 2)
            rightHS = GetNodeResult(graph.Nodes[node.Inputs.ElementAt(1)], dataset, row, graph, provider);

          return provider.Apply(OpCodes.MapNodeToOpCode(node)).Invoke(leftHS, rightHS);
        }
        return dataset.GetReadOnlyDoubleValues(node.Name)[row];
      } catch (ConstraintViolationException ex) {
        throw ex;
      } catch (Exception ex) {
        throw ex;
      }
      return dataset.GetReadOnlyDoubleValues(node.Name)[row];
    }

    #region IStatefulItem
    public void InitializeState() {
      EvaluatedSolutions = 0;
    }

    public void ClearState() { }
    #endregion
  }
}
