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
using System.Drawing;
using System.Linq;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Problems.DataAnalysis;

namespace CartesianGeneticProgramming {
  /// <summary>
  /// Abstract base class for CGP data analysis models
  /// </summary>
  [StorableType("645d4609-bcc7-4ef4-91e9-a1e70590ccac")]
  public abstract class CGPDataAnalysisModel : DataAnalysisModel, ICGPDataAnalysisModel {
    public static new Image StaticItemImage {
      get { return HeuristicLab.Common.Resources.VSImageLibrary.Function; }
    }

    #region properties
    [Storable]
    private double lowerEstimationLimit;
    public double LowerEstimationLimit { get { return lowerEstimationLimit; } }
    [Storable]
    private double upperEstimationLimit;
    public double UpperEstimationLimit { get { return upperEstimationLimit; } }

    [Storable]
    private Graph graph;
    public Graph Graph {
      get { return graph; }
    }

    [Storable]
    private ICGPInterpreter interpreter;
    public ICGPInterpreter Interpreter {
      get { return interpreter; }
    }

    public override IEnumerable<string> VariablesUsedForPrediction {
      get {
        var variables =
          Graph.Inputs
            .Select(x => x.Name)
            .Distinct();

        return variables.OrderBy(x => x);
      }
    }

    #endregion

    [StorableConstructor]
    protected CGPDataAnalysisModel(StorableConstructorFlag _) : base(_) { }
    protected CGPDataAnalysisModel(CGPDataAnalysisModel original, Cloner cloner)
      : base(original, cloner) {
      this.graph = cloner.Clone(original.graph);
      this.interpreter = cloner.Clone(original.interpreter);
      this.lowerEstimationLimit = original.lowerEstimationLimit;
      this.upperEstimationLimit = original.upperEstimationLimit;
    }
    protected CGPDataAnalysisModel(Graph graph, ICGPInterpreter interpreter,
       double lowerEstimationLimit, double upperEstimationLimit)
      : base() {
      this.name = ItemName;
      this.description = ItemDescription;
      this.graph = graph;
      this.interpreter = interpreter;
      this.lowerEstimationLimit = lowerEstimationLimit;
      this.upperEstimationLimit = upperEstimationLimit;
    }
  }
}
