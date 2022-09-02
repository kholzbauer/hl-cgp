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

using System.Collections.Generic;
using System.Linq;
using CartesianGeneticProgramming.Models;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;

namespace CartesianGeneticProgramming {
  /// <summary>
  /// Represents a cgp graph which can be visualized in the GUI.
  /// </summary>
  [Item("Graph", "Represents a cgp graph.")]
  [StorableType("b1765727-abc2-47b6-a0ae-93899ee525c0")]
  public class Graph : Item {

    [Storable]
    public Node Output { get; set; }

    [Storable]
    public List<Node> Inputs { get; set; } //variable depending on problem

    [Storable]
    public Dictionary<int, Node> Nodes { get; set; }

    [Storable]
    public string SolutionString { get; set; }

    [Storable]
    public int Rows { get; set; }

    [Storable]
    public int Columns { get; set; }

    public Graph(int rows, int cols) {
      Rows = rows;
      Columns = cols;

      this.Nodes = new Dictionary<int, Node>();
      this.Inputs = new List<Node>();
    }

    public void AddNode(Node node) {
      Nodes.Add(node.Id, node);
    }

    public int GetActiveNodes() {
      return this.Nodes.Where(n => n.Value.IsActive).Count();
    }

    #region item cloning and persistence
    [StorableConstructor]
    protected Graph(StorableConstructorFlag _) : base(_) { }
    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() { }

    public Graph() : base() { }

    protected Graph(Graph original, Cloner cloner)
      : base(original, cloner) {
      Nodes = original.Nodes;
      Output = original.Output;
      Inputs = original.Inputs;
      SolutionString = original.SolutionString;
      Rows = original.Rows;
      Columns = original.Columns;
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new Graph(this, cloner);
    }
    #endregion
  }
}
