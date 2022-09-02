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
 * 
 * Author: Sabine Winkler
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using CartesianGeneticProgramming.Interpreter.Math;
using CartesianGeneticProgramming.Models;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Encodings.IntegerVectorEncoding;

namespace CartesianGeneticProgramming {
  /// <summary>
  /// Implementation of the IGenotypeToPhenotypeMapper interface
  /// </summary>
  [StorableType("4fea97cb-aada-48ca-84b9-b91509c4df1c")]
  public class GenotypeToPhenotypeMapper : IntegerVectorOperator, IGenotypeToPhenotypeMapper {

    [StorableConstructor]
    protected GenotypeToPhenotypeMapper(StorableConstructorFlag _) : base(_) { }
    protected GenotypeToPhenotypeMapper(GenotypeToPhenotypeMapper original, Cloner cloner) : base(original, cloner) { }
    public GenotypeToPhenotypeMapper() { }

    /// <summary>
    /// Maps a genotype (an integer vector) to a phenotype (a graph).
    /// </summary>
    /// <param name="n_inputs">number of inputs</param>
    /// <param name="n_outputs">number of outputs</param>
    /// <param name="n_arity">arity of each node (function)</param>
    /// <param name="n_rows">number of rows</param>
    /// <param name="n_cols">number of columns</param>
    /// <returns>phenotype (a graph)</returns>
    public Graph Map(int n_inputs, int n_outputs, int n_arity, int n_rows, int n_cols, IntegerVector genotype) {
      Graph g = new Graph(n_rows, n_cols);

      int currentId = 0;

      for (int i = 0; i < n_inputs; i++) {
        Node node = NodeFactory.Create(ref currentId, NodeType.INPUT);
        g.AddNode(node);
        g.Inputs.Add(node);
      }

      int currentThresholdNode = n_inputs;
      for (int i = 0, n_node = 0; i < (genotype.Length - n_outputs); i += (n_arity + 1), n_node++) {
        Node node = NodeFactory.Create(ref currentId, NodeType.NODE);
        node.FunctionNumber = genotype[i];

        if (n_node > 0 && n_node % n_rows == 0) {
          currentThresholdNode += n_rows;
        }

        var test = new List<int>() {
           genotype[i + 1] >= currentThresholdNode ?
            genotype[i + 1] - currentThresholdNode :
            genotype[i + 1]
            ,
            genotype[i + 2] >= currentThresholdNode ?
            genotype[i + 2] - currentThresholdNode :
            genotype[i + 2]};

        node.Inputs = new List<int>();
        for (int j = 0; j < n_arity; j++) {
          node.Inputs.Add(genotype[i + (j + 1)] >= currentThresholdNode ?
            genotype[i + (j + 1)] - currentThresholdNode :
            genotype[i + (j + 1)]);
        }

        if (!node.Inputs.SequenceEqual(test)) {
          Console.WriteLine("Hlelo");
        }

        g.AddNode(node);
      }

      Node outputNode = NodeFactory.Create(ref currentId, NodeType.OUTPUT);
      int index = genotype.Length - n_outputs;
      outputNode.Inputs = new List<int>() {
        genotype[index] >= outputNode.Id ?
          genotype[index] - outputNode.Id :
          genotype[index]};

      g.AddNode(outputNode);
      g.Output = outputNode;

      ActivateNodes(g.Nodes, outputNode);

      return g;
    }

    void ActivateNodes(Dictionary<int, Node> nodes, Node node) {
      node.IsActive = true;

      int arity = OpCodes.MapNodeToArity(node);
      for (int i = 0; i < arity; i++) {
        ActivateNodes(nodes, nodes[node.Inputs.ElementAt(i)]);
      }
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new GenotypeToPhenotypeMapper(this, cloner);
    }
  }
}
