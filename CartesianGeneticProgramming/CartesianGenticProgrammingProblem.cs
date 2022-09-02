using System;
using System.Collections.Generic;
using System.Linq;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Encodings.IntegerVectorEncoding;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Problems.DataAnalysis;
using HeuristicLab.Problems.Instances;

namespace CartesianGeneticProgramming {
  [Item(Name = "Cartesian Gentic Programming Problem", Description = "Represents cartesian genetic programming for single objective symbolic regression problems.")]
  [StorableType("8cb6ac0f-90a5-4a61-9bad-8614222f507b")]
  [Creatable(CreatableAttribute.Categories.GeneticProgrammingProblems, Priority = 100)]
  public class CartesianGenticProgrammingProblem : SingleObjectiveBasicProblem<IntegerVectorEncoding>, IStatefulItem, IRegressionProblem, IProblemInstanceConsumer<IRegressionProblemData>, IProblemInstanceExporter<IRegressionProblemData> {
    public override bool Maximization => false;

    private int arity = 2;
    private int outputs = 1;

    #region names
    private const string LevelsBackParameterName = "LevelsBack";
    private const string RowsParameterName = "Rows";
    private const string ColumnsParameterName = "Columns";
    private const string GenotypeToPhenotypeMapperParameterName = "GenotypeToPhenotypeMapper";
    private const string InterpreterParameterName = "Interpreter";
    private const string ProblemDataParameterName = "ProblemData";

    #endregion

    #region Parameter Properties
    public IValueParameter<IntValue> LevelsBackParameter {
      get { return (IValueParameter<IntValue>)Parameters[LevelsBackParameterName]; }
    }
    public IValueParameter<IntValue> RowsParameter {
      get { return (IValueParameter<IntValue>)Parameters[RowsParameterName]; }
    }
    public IValueParameter<IntValue> ColumnsParameter {
      get { return (IValueParameter<IntValue>)Parameters[ColumnsParameterName]; }
    }

    public IValueParameter<IGenotypeToPhenotypeMapper> GenotypeToPhenotypeMapperParameter {
      get { return (IValueParameter<IGenotypeToPhenotypeMapper>)Parameters[GenotypeToPhenotypeMapperParameterName]; }
    }

    public IValueParameter<ICGPInterpreter> InterpreterParameter {
      get { return (IValueParameter<ICGPInterpreter>)Parameters[InterpreterParameterName]; }
    }

    public IValueParameter<IRegressionProblemData> ProblemDataParameter {
      get { return (IValueParameter<IRegressionProblemData>)Parameters[ProblemDataParameterName]; }
    }
    #endregion

    #region Properties
    public IntValue LevelsBack {
      get { return LevelsBackParameter.Value; }
      set { LevelsBackParameter.Value = value; }
    }
    public IntValue Rows {
      get { return RowsParameter.Value; }
      set { RowsParameter.Value = value; }
    }
    public IntValue Columns {
      get { return ColumnsParameter.Value; }
      set { ColumnsParameter.Value = value; }
    }

    public IGenotypeToPhenotypeMapper Mapper {
      get { return GenotypeToPhenotypeMapperParameter.Value; }
      set { GenotypeToPhenotypeMapperParameter.Value = value; }
    }

    public ICGPInterpreter Interpreter {
      get { return InterpreterParameter.Value; }
      set { InterpreterParameter.Value = value; }
    }

    public IRegressionProblemData ProblemData {
      get { return ProblemDataParameter.Value; }
      set { ProblemDataParameter.Value = value; }
    }

    IParameter IDataAnalysisProblem.ProblemDataParameter { get { return ProblemDataParameter; } }

    IDataAnalysisProblemData IDataAnalysisProblem.ProblemData { get { return ProblemData; } }
    #endregion

    public CartesianGenticProgrammingProblem() : base() {
      Parameters.Add(new ValueParameter<IRegressionProblemData>(ProblemDataParameterName, "The data for the regression problem", new RegressionProblemData()));

      Parameters.Add(new ValueParameter<IntValue>(LevelsBackParameterName, "Number of columns before itself the node can get its inputs from.", new IntValue(30)));
      Parameters.Add(new ValueParameter<IntValue>(RowsParameterName, "The number of rows for the grid of nodes.", new IntValue(1)));
      Parameters.Add(new ValueParameter<IntValue>(ColumnsParameterName, "The number of columns for the grid of nodes.", new IntValue(30)));

      Parameters.Add(new ValueParameter<IGenotypeToPhenotypeMapper>(GenotypeToPhenotypeMapperParameterName, "Maps the genotype (an integer vector) to the phenotype (a graph).", new GenotypeToPhenotypeMapper()));
      Parameters.Add(new ValueParameter<ICGPInterpreter>(InterpreterParameterName, "The interpreter to evaluate the graph.", new MathInterpreter()));

      BestKnownQuality = 0;

      CreateEncoding();

      RegisterEventHandlers();
    }

    [StorableConstructor]
    public CartesianGenticProgrammingProblem(StorableConstructorFlag _) : base(_) { }

    public CartesianGenticProgrammingProblem(CartesianGenticProgrammingProblem original, Cloner cloner) : base(original, cloner) {
      // Don't forget to call the cloning ctor of the base class
      // This class does not have fields, therefore we don't need to actually clone anything
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new CartesianGenticProgrammingProblem(this, cloner);
    }

    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      RegisterEventHandlers();
    }

    private void RegisterEventHandlers() {
      RowsParameter.ValueChanged += new EventHandler(GridParameter_ValueChanged);
      if (RowsParameter.Value != null) RowsParameter.Value.ValueChanged += new EventHandler(Grid_Changed);

      ColumnsParameter.ValueChanged += new EventHandler(GridParameter_ValueChanged);
      if (ColumnsParameter.Value != null) ColumnsParameter.Value.ValueChanged += new EventHandler(Grid_Changed);

      LevelsBackParameter.ValueChanged += new EventHandler(GridParameter_ValueChanged);
      if (LevelsBackParameter.Value != null) LevelsBackParameter.Value.ValueChanged += new EventHandler(Grid_Changed);
    }

    private void GridParameter_ValueChanged(object sender, EventArgs e) {
      RowsParameter.Value.ValueChanged += new EventHandler(Grid_Changed);
      ColumnsParameter.Value.ValueChanged += new EventHandler(Grid_Changed);
      LevelsBackParameter.Value.ValueChanged += new EventHandler(Grid_Changed);

      OnGridChanged();
      OnReset();
    }

    private void Grid_Changed(object sender, EventArgs e) {
      CreateEncoding();

      OnReset();
    }

    public event EventHandler GridChanged;
    protected virtual void OnGridChanged() {
      var handler = GridChanged;
      if (handler != null) handler(this, EventArgs.Empty);
    }

    private void CreateEncoding() {
      var problemData = ProblemData;
      if (problemData.AllowedInputVariables.Any(name => !problemData.Dataset.VariableHasType<double>(name))) throw new NotSupportedException("Categorical variables are not supported");
      var inputVariables = problemData.InputVariables.Where(v => problemData.InputVariables.ItemChecked(v) && v.Value != problemData.TargetVariable);
      var outputVariable = problemData.TargetVariable;

      int n_rows = Rows.Value;
      int n_columns = Columns.Value;
      int n_nodes = n_rows * n_columns;

      int n_inputs = inputVariables.Count(); //min node number = n_inputs - 1
      int n_outputs = outputs; //vector has to be enlenghted by this //TODO: dynamic?

      int n_arity = arity; //how many inputs can each node have
      int n_functions = Interpreter.GetNumberOfFunctions();
      int n_levelsback = LevelsBack.Value;

      int length = n_nodes * (n_arity + 1) + n_outputs;

      var mins = new List<int>();
      var maxs = new List<int>();

      //levels-back defines how many columns before we can get input from so the bounds have to be set accordingly
      for (int i = 0; i < n_columns; i++) {
        for (int j = 0; j < n_rows; j++) {
          //Function index - [0-n_functions]
          mins.Add(0);
          maxs.Add(n_functions);

          //input nrs - [current_column - levels_back; current_columns - 1] (feed-forward)
          int min_node = 0;
          int max_node = i * n_rows + n_inputs;

          //levelsback takes effect
          if (i > n_levelsback) {
            min_node = (i - n_levelsback) * n_rows + n_inputs;
            max_node = max_node + n_inputs; //everything above our current maps to inputs in the end
          }

          for (int k = 0; k < n_arity; k++) {
            mins.Add(min_node); //min = one columns before current - levelsback
            maxs.Add(max_node); //max = one column before current
          }
        }
      }

      //output => output is "reserved"... last n_outputs fields in vector define where output comes from
      int min_output = 0;
      int max_output = n_nodes + n_inputs + n_inputs;
      for (int i = 0; i < n_outputs; i++) {
        if (n_columns > n_levelsback) {
          min_output = (n_columns - n_levelsback) * n_rows + n_inputs;
        }

        mins.Add(min_output); //output can be connected to input directly
        maxs.Add(max_output);
      }

      //max value in encoding = number of nodes
      Encoding = new IntegerVectorEncoding("i", length, mins, maxs);
    }

    Graph MapVector(IntegerVector vector) {
      var problemData = ProblemData;

      var inputs = problemData.InputVariables.Where(v => problemData.InputVariables.ItemChecked(v) && v.Value != problemData.TargetVariable);

      Graph g = Mapper.Map(inputs.Count(), outputs, arity, Rows.Value, Columns.Value, vector);

      g.Output.Name = problemData.TargetVariable;
      for (int i = 0; i < inputs.Count(); i++) {
        g.Inputs[i].Name = inputs.ElementAt(i).Value;
      }

      return g;
    }

    public override double Evaluate(Individual individual, IRandom random) {
      var vector = individual.IntegerVector();

      Graph graph = MapVector(vector);

      var estimatedTraining = Interpreter.GetGraphValues(graph, ProblemData.Dataset, ProblemData.TrainingIndices);
      var originalTraining = ProblemData.TargetVariableTrainingValues;

      //File.WriteAllText("test.txt", CGPGraphvizGridFormatter.Format(graph, Interpreter, true));
      //File.WriteAllText("test_2.txt", CGPGraphvizGridFormatter.Format(graph, Interpreter, false));

      double result = double.MaxValue;
      try {
        OnlineCalculatorError errorState;
        //result = OnlineMaxAbsoluteErrorCalculator.Calculate(originalTraining, estimatedTraining, out errorState);
        //result = OnlineNormalizedMeanSquaredErrorCalculator.Calculate(originalTraining, estimatedTraining, out errorState);
        result = OnlineMeanSquaredErrorCalculator.Calculate(originalTraining, estimatedTraining, out errorState);
        if (double.IsInfinity(result)) {
          result = double.MaxValue;
        }
      } catch (Exception ex) {
        throw ex;
      }

      return result;
    }

    public override void Analyze(Individual[] individuals, double[] qualities, ResultCollection results, IRandom random) {
      const string bestSolutionResultName = "Best training solution";
      const string bestRawSolutionResultName = "Best training solution (raw)";
      const string solutionLengthName = "Solution Length";
      const string bestCGPSolutionResultName = "Best training solution (analysis)";
      const string bestSolutionQuality = "Best training solution quality";

      var bestQuality = Maximization ? qualities.Max() : qualities.Min();
      var bestIdx = Array.IndexOf(qualities, bestQuality);

      var graph = MapVector(individuals[bestIdx].IntegerVector());
      graph = AnnotateGraph(graph);

      var model = new CGPModel(graph.Output.Name, graph, Interpreter);
      var cgpSolution = new CGPSolution(model, ProblemData);

      if (!results.ContainsKey(bestRawSolutionResultName)) {
        results.Add(new Result(bestRawSolutionResultName, individuals[bestIdx].IntegerVector()));
      }
      if (!results.ContainsKey(solutionLengthName)) {
        results.Add(new Result(solutionLengthName, new IntValue(individuals[bestIdx].IntegerVector().Length)));
      }
      if (!results.ContainsKey(bestCGPSolutionResultName)) {
        results.Add(new Result(bestCGPSolutionResultName, cgpSolution));
      }
      if (!results.ContainsKey(bestSolutionQuality)) {
        results.Add(new Result(bestSolutionQuality, new DoubleValue(bestQuality)));
      }

      if (!results.ContainsKey(bestSolutionResultName)) {
        results.Add(new Result(bestSolutionResultName, new Solution(graph, bestQuality)));
      }
      if ((((Solution)results[bestSolutionResultName].Value).Quality < qualities[bestIdx]) ||
                 IsBetter((CGPSolution)results[bestCGPSolutionResultName].Value, cgpSolution)) {
        results[bestSolutionResultName].Value = new Solution(graph, bestQuality);
        results[bestRawSolutionResultName].Value = individuals[bestIdx].IntegerVector();
        results[solutionLengthName].Value = new IntValue(individuals[bestIdx].IntegerVector().Length);
        results[bestCGPSolutionResultName].Value = cgpSolution;
        results[bestSolutionQuality].Value = new DoubleValue(bestQuality);
      }
    }

    private Graph AnnotateGraph(Graph graph) {
      foreach (var node in graph.Nodes.Values.Where(n => n.Type == Models.NodeType.NODE)) {
        node.Name = Interpreter.GetFunctionString(node);
      }

      graph.SolutionString = Interpreter.GetSolutionString(graph);

      return graph;
    }

    private bool IsBetter(CGPSolution oldSolution, CGPSolution newSolution) {
      if (oldSolution.TestMeanSquaredError > newSolution.TestMeanSquaredError &&
          oldSolution.TrainingMeanSquaredError > newSolution.TrainingMeanSquaredError)
        return true;
      if (oldSolution.TestRSquared < newSolution.TestRSquared &&
          oldSolution.TrainingRSquared < newSolution.TrainingRSquared)
        return true;
      if (oldSolution.TestRelativeError > newSolution.TestMeanSquaredError &&
          oldSolution.TrainingRelativeError > newSolution.TrainingRelativeError)
        return true;
      return false;
    }

    public void Load(IRegressionProblemData data) {
      this.ProblemData = data;
      OnProblemDataChanged();
    }

    public IRegressionProblemData Export() {
      return ProblemData;
    }

    #region events

    public event EventHandler ProblemDataChanged;

    private void OnProblemDataChanged() {
      CreateEncoding();

      var handler = ProblemDataChanged;
      if (handler != null)
        handler(this, EventArgs.Empty);
    }

    public void InitializeState() { ClearState(); }
    public void ClearState() {

    }
    #endregion
  }
}
