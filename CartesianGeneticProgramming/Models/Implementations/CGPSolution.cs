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

using System.Linq;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Optimization;
using HeuristicLab.Problems.DataAnalysis;

namespace CartesianGeneticProgramming {
  /// <summary>
  /// Represents a CGP (model + data) and attributes of the solution like accuracy and complexity
  /// </summary>
  [StorableType("7d789971-ba8b-4224-8f7a-a76dc9766ac2")]
  [Item(Name = "CGPSolution", Description = "Represents a CGP symbolic regression solution (model + data) and attributes of the solution like accuracy and complexity.")]
  public sealed class CGPSolution : RegressionSolution, ICGPSolution {
    private const string ModelActiveNodesName = "Model Active Nodes";
    private const string ModelInactiveNodesName = "Model Inactive Nodes";

    private const string EstimationLimitsResultsResultName = "Estimation Limits Results";
    private const string EstimationLimitsResultName = "Estimation Limits";
    private const string TrainingUpperEstimationLimitHitsResultName = "Training Upper Estimation Limit Hits";
    private const string TestLowerEstimationLimitHitsResultName = "Test Lower Estimation Limit Hits";
    private const string TrainingLowerEstimationLimitHitsResultName = "Training Lower Estimation Limit Hits";
    private const string TestUpperEstimationLimitHitsResultName = "Test Upper Estimation Limit Hits";
    private const string TrainingNaNEvaluationsResultName = "Training NaN Evaluations";
    private const string TestNaNEvaluationsResultName = "Test NaN Evaluations";

    private const string ModelBoundsResultName = "Model Bounds";

    public new ICGPModel Model {
      get { return (ICGPModel)base.Model; }
      set { base.Model = value; }
    }
    ICGPDataAnalysisModel ICGPDataAnalysisSolution.Model {
      get { return (ICGPDataAnalysisModel)base.Model; }
    }
    public int ModelActiveNodes {
      get { return ((IntValue)this[ModelActiveNodesName].Value).Value; }
      private set { ((IntValue)this[ModelActiveNodesName].Value).Value = value; }
    }

    public int ModelInactiveNodes {
      get { return ((IntValue)this[ModelInactiveNodesName].Value).Value; }
      private set { ((IntValue)this[ModelInactiveNodesName].Value).Value = value; }
    }

    private ResultCollection EstimationLimitsResultCollection {
      get { return (ResultCollection)this[EstimationLimitsResultsResultName].Value; }
    }
    public DoubleLimit EstimationLimits {
      get { return (DoubleLimit)EstimationLimitsResultCollection[EstimationLimitsResultName].Value; }
    }

    public int TrainingUpperEstimationLimitHits {
      get { return ((IntValue)EstimationLimitsResultCollection[TrainingUpperEstimationLimitHitsResultName].Value).Value; }
      private set { ((IntValue)EstimationLimitsResultCollection[TrainingUpperEstimationLimitHitsResultName].Value).Value = value; }
    }
    public int TestUpperEstimationLimitHits {
      get { return ((IntValue)EstimationLimitsResultCollection[TestUpperEstimationLimitHitsResultName].Value).Value; }
      private set { ((IntValue)EstimationLimitsResultCollection[TestUpperEstimationLimitHitsResultName].Value).Value = value; }
    }
    public int TrainingLowerEstimationLimitHits {
      get { return ((IntValue)EstimationLimitsResultCollection[TrainingLowerEstimationLimitHitsResultName].Value).Value; }
      private set { ((IntValue)EstimationLimitsResultCollection[TrainingLowerEstimationLimitHitsResultName].Value).Value = value; }
    }
    public int TestLowerEstimationLimitHits {
      get { return ((IntValue)EstimationLimitsResultCollection[TestLowerEstimationLimitHitsResultName].Value).Value; }
      private set { ((IntValue)EstimationLimitsResultCollection[TestLowerEstimationLimitHitsResultName].Value).Value = value; }
    }
    public int TrainingNaNEvaluations {
      get { return ((IntValue)EstimationLimitsResultCollection[TrainingNaNEvaluationsResultName].Value).Value; }
      private set { ((IntValue)EstimationLimitsResultCollection[TrainingNaNEvaluationsResultName].Value).Value = value; }
    }
    public int TestNaNEvaluations {
      get { return ((IntValue)EstimationLimitsResultCollection[TestNaNEvaluationsResultName].Value).Value; }
      private set { ((IntValue)EstimationLimitsResultCollection[TestNaNEvaluationsResultName].Value).Value = value; }
    }

    public IntervalCollection ModelBoundsCollection {
      get {
        if (!ContainsKey(ModelBoundsResultName)) return null;
        return (IntervalCollection)this[ModelBoundsResultName].Value;
      }
      private set {
        if (ContainsKey(ModelBoundsResultName)) {
          this[ModelBoundsResultName].Value = value;
        } else {
          Add(new Result(ModelBoundsResultName, "Results concerning the derivation of symbolic regression solution", value));
        }
      }
    }



    [StorableConstructor]
    private CGPSolution(StorableConstructorFlag _) : base(_) { }
    private CGPSolution(CGPSolution original, Cloner cloner)
      : base(original, cloner) {
    }
    public CGPSolution(ICGPModel model, IRegressionProblemData problemData)
      : base(model, problemData) {

      Add(new Result(ModelActiveNodesName, "Number of active nodes in the model.", new IntValue()));
      Add(new Result(ModelInactiveNodesName, "Number of inactive nodes in the model.", new IntValue()));

      ResultCollection estimationLimitResults = new ResultCollection();
      estimationLimitResults.Add(new Result(EstimationLimitsResultName, "", new DoubleLimit()));
      estimationLimitResults.Add(new Result(TrainingUpperEstimationLimitHitsResultName, "", new IntValue()));
      estimationLimitResults.Add(new Result(TestUpperEstimationLimitHitsResultName, "", new IntValue()));
      estimationLimitResults.Add(new Result(TrainingLowerEstimationLimitHitsResultName, "", new IntValue()));
      estimationLimitResults.Add(new Result(TestLowerEstimationLimitHitsResultName, "", new IntValue()));
      estimationLimitResults.Add(new Result(TrainingNaNEvaluationsResultName, "", new IntValue()));
      estimationLimitResults.Add(new Result(TestNaNEvaluationsResultName, "", new IntValue()));
      Add(new Result(EstimationLimitsResultsResultName, "Results concerning the estimation limits of symbolic regression solution", estimationLimitResults));

      RecalculateResults();
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new CGPSolution(this, cloner);
    }

    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      if (!ContainsKey(EstimationLimitsResultsResultName)) {
        ResultCollection estimationLimitResults = new ResultCollection();
        estimationLimitResults.Add(new Result(EstimationLimitsResultName, "", new DoubleLimit()));
        estimationLimitResults.Add(new Result(TrainingUpperEstimationLimitHitsResultName, "", new IntValue()));
        estimationLimitResults.Add(new Result(TestUpperEstimationLimitHitsResultName, "", new IntValue()));
        estimationLimitResults.Add(new Result(TrainingLowerEstimationLimitHitsResultName, "", new IntValue()));
        estimationLimitResults.Add(new Result(TestLowerEstimationLimitHitsResultName, "", new IntValue()));
        estimationLimitResults.Add(new Result(TrainingNaNEvaluationsResultName, "", new IntValue()));
        estimationLimitResults.Add(new Result(TestNaNEvaluationsResultName, "", new IntValue()));
        Add(new Result(EstimationLimitsResultsResultName, "Results concerning the estimation limits of symbolic regression solution", estimationLimitResults));
        CalculateResults();
      }
    }

    protected override void RecalculateResults() {
      base.RecalculateResults();
      CalculateResults();
    }

    private void CalculateResults() {
      ModelActiveNodes = Model.Graph.Nodes.Where(n => n.Value.IsActive).Count();
      ModelInactiveNodes = Model.Graph.Nodes.Where(n => !n.Value.IsActive).Count();

      EstimationLimits.Lower = Model.LowerEstimationLimit;
      EstimationLimits.Upper = Model.UpperEstimationLimit;

      TrainingUpperEstimationLimitHits = EstimatedTrainingValues.Count(x => x.IsAlmost(Model.UpperEstimationLimit));
      TestUpperEstimationLimitHits = EstimatedTestValues.Count(x => x.IsAlmost(Model.UpperEstimationLimit));
      TrainingLowerEstimationLimitHits = EstimatedTrainingValues.Count(x => x.IsAlmost(Model.LowerEstimationLimit));
      TestLowerEstimationLimitHits = EstimatedTestValues.Count(x => x.IsAlmost(Model.LowerEstimationLimit));
      TrainingNaNEvaluations = Model.Interpreter.GetGraphValues(Model.Graph, ProblemData.Dataset, ProblemData.TrainingIndices).Count(double.IsNaN);
      TestNaNEvaluations = Model.Interpreter.GetGraphValues(Model.Graph, ProblemData.Dataset, ProblemData.TestIndices).Count(double.IsNaN);
    }
  }
}
