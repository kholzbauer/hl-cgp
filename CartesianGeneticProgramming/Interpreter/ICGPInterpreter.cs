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

using System.Collections.Generic;
using CartesianGeneticProgramming.Models;
using HEAL.Attic;
using HeuristicLab.Core;
using HeuristicLab.Problems.DataAnalysis;

namespace CartesianGeneticProgramming {
  [StorableType("d9f2ce8d-fc06-48ad-abc2-ee5b65587782")]
  /// <summary>
  /// Interface for interpreters of CGP Graphs
  /// </summary>
  public interface ICGPInterpreter : INamedItem, IStatefulItem {
    IEnumerable<double> GetGraphValues(Graph graph, IDataset dataset, IEnumerable<int> rows);
    int EvaluatedSolutions { get; set; }

    string GetFunctionString(Node node);

    int GetNumberOfFunctions();

    string GetSolutionString(Graph graph);
  }
}
