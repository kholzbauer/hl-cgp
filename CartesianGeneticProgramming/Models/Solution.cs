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

using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;

namespace CartesianGeneticProgramming {
  /// <summary>
  /// Represents a CGP solution which contains its quality and the result graph.
  /// </summary>
  [Item("Solution", "Represents a CGP solution which contains its quality and the result graph.")]
  [StorableType("587ded6c-49ad-4e29-b03e-86820b846991")]
  public sealed class Solution : Item {
    [Storable]
    public Graph Graph { get; private set; }
    [Storable]
    public double Quality { get; private set; }

    public Solution(Graph graph, double quality) {
      this.Graph = graph;
      this.Quality = quality;
    }

    #region item cloning and persistence
    [StorableConstructor]
    private Solution(StorableConstructorFlag _) : base(_) { }
    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() { }

    private Solution(Solution original, Cloner cloner)
      : base(original, cloner) {
      Graph = cloner.Clone(original.Graph);
      Quality = original.Quality;
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new Solution(this, cloner);
    }
    #endregion
  }
}
