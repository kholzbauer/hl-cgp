using HeuristicLab.PluginInfrastructure;

namespace CartesianGeneticProgramming {
  [Plugin("CartesianGeneticProgramming", "Provides classes for cartesian genetic programming (CGP).", "1.0.0.0")]
  [PluginFile("CartesianGeneticProgramming-1.0.dll", PluginFileType.Assembly)] // each plugin represents a collection of files. The minimum is one file; the assembly.

  [PluginDependency("HeuristicLab.Analysis", "3.3")]
  [PluginDependency("HeuristicLab.Collections", "3.3")]
  [PluginDependency("HeuristicLab.Common", "3.3")]
  [PluginDependency("HeuristicLab.Common.Resources", "3.3")]
  [PluginDependency("HeuristicLab.Core", "3.3")]
  [PluginDependency("HeuristicLab.Data", "3.3")]
  [PluginDependency("HeuristicLab.Encodings.IntegerVectorEncoding", "3.3")]
  [PluginDependency("HeuristicLab.Operators", "3.3")]
  [PluginDependency("HeuristicLab.Optimization", "3.3")]
  [PluginDependency("HeuristicLab.Parameters", "3.3")]
  [PluginDependency("HeuristicLab.Problems.Binary", "3.3")]
  [PluginDependency("HeuristicLab.Problems.DataAnalysis", "3.4")]
  [PluginDependency("HeuristicLab.Problems.DataAnalysis.Symbolic", "3.4")]
  [PluginDependency("HeuristicLab.Problems.DataAnalysis.Symbolic.Regression", "3.4")]
  [PluginDependency("HeuristicLab.Problems.Instances", "3.3")]
  [PluginDependency("HeuristicLab.Persistence", "3.3")]
  [PluginDependency("HeuristicLab.Random", "3.3")]
  public class Plugin : PluginBase {
  }
}
