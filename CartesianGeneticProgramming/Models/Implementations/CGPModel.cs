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
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Problems.DataAnalysis;

namespace CartesianGeneticProgramming {
  /// <summary>
  /// Represents a CGP symbolic regression model
  /// </summary>
  [StorableType("7b826155-ce25-4ee1-9b43-5bc7f76b4511")]
  [Item(Name = "CGP Symbolic Regression Model", Description = "Represents a CGP symbolic regression model.")]
  public class CGPModel : CGPDataAnalysisModel, ICGPModel {
    [Storable]
    private string targetVariable;
    public string TargetVariable {
      get { return targetVariable; }
      set {
        if (string.IsNullOrEmpty(value) || targetVariable == value) return;
        targetVariable = value;
        OnTargetVariableChanged(this, EventArgs.Empty);
      }
    }

    [StorableConstructor]
    protected CGPModel(StorableConstructorFlag _) : base(_) {
      targetVariable = string.Empty;
    }

    protected CGPModel(CGPModel original, Cloner cloner)
      : base(original, cloner) {
      this.targetVariable = original.targetVariable;
    }

    public CGPModel(string targetVariable, Graph graph,
      ICGPInterpreter interpreter,
      double lowerEstimationLimit = double.MinValue, double upperEstimationLimit = double.MaxValue)
      : base(graph, interpreter, lowerEstimationLimit, upperEstimationLimit) {
      this.targetVariable = targetVariable;
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new CGPModel(this, cloner);
    }

    public IEnumerable<double> GetEstimatedValues(IDataset dataset, IEnumerable<int> rows) {
      return Interpreter.GetGraphValues(Graph, dataset, rows)
        .LimitToRange(LowerEstimationLimit, UpperEstimationLimit);
    }

    public ICGPSolution CreateRegressionSolution(IRegressionProblemData problemData) {
      return new CGPSolution(this, new RegressionProblemData(problemData));
    }
    IRegressionSolution IRegressionModel.CreateRegressionSolution(IRegressionProblemData problemData) {
      return CreateRegressionSolution(problemData);
    }

    public virtual bool IsProblemDataCompatible(IRegressionProblemData problemData, out string errorMessage) {
      return RegressionModel.IsProblemDataCompatible(this, problemData, out errorMessage);
    }

    public override bool IsProblemDataCompatible(IDataAnalysisProblemData problemData, out string errorMessage) {
      if (problemData == null) throw new ArgumentNullException("problemData", "The provided problemData is null.");
      var regressionProblemData = problemData as IRegressionProblemData;
      if (regressionProblemData == null)
        throw new ArgumentException("The problem data is not compatible with this symbolic regression model. Instead a " + problemData.GetType().GetPrettyName() + " was provided.", "problemData");
      return IsProblemDataCompatible(regressionProblemData, out errorMessage);
    }

    #region events
    public event EventHandler TargetVariableChanged;
    private void OnTargetVariableChanged(object sender, EventArgs args) {
      var changed = TargetVariableChanged;
      if (changed != null)
        changed(sender, args);
    }
    #endregion
  }
}
