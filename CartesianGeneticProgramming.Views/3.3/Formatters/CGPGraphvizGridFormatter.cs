using System.Collections.Generic;
using System.Linq;
using System.Text;
using CartesianGeneticProgramming.Interpreter.Math;
using CartesianGeneticProgramming.Models;
using HeuristicLab.Common;
using HeuristicLab.Core;

namespace CartesianGeneticProgramming.Views {
  [Item("GraphViz CGP Grid Formatter", "Formatter for CGP graph for grid visualization with GraphViz.")]
  public sealed class CGPGraphvizGridFormatter : NamedItem {

    private const string activeColor = "#b0c4de";
    private const string inactiveColor = "#d5d6d8";
    private const string inputColor = "#EBF3FF";
    private const string outputColor = "#a8d4c6";
    private const string lhsColor = "#000000";
    private const string rhsColor = "#000000";

    private static Graph Graph { get; set; }

    public CGPGraphvizGridFormatter() {
    }
    private CGPGraphvizGridFormatter(CGPGraphvizGridFormatter original, Cloner cloner)
      : base(original, cloner) {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new CGPGraphvizGridFormatter(this, cloner);
    }

    public static string Format(Graph cgpGraph, bool displayInactive) {
      Graph = cgpGraph;

      StringBuilder strBuilder = new StringBuilder();
      strBuilder.AppendLine("digraph {");
      strBuilder.AppendLine("labelloc=\"t\"");
      strBuilder.AppendLine("rankdir=\"LR\"");

      foreach (Node node in cgpGraph.Nodes.Values) {
        if (!displayInactive && !node.IsActive) {
          strBuilder.AppendLine(DeclareNode(node, true));
        } else {
          strBuilder.AppendLine(DeclareNode(node, false));
        }
      }


      List<int> activeNodes = cgpGraph.Nodes.Values.Where(n => n.IsActive).Select(n => n.Id).ToList();
      //total nodes - output nodes
      for (int i = 0; i < cgpGraph.Columns; i++) {
        //special case single row graph => no invis for active node columns
        if (cgpGraph.Rows == 1 && activeNodes.Contains(cgpGraph.Inputs.Count + (i * cgpGraph.Rows))) {
          strBuilder.AppendLine(DeclareColumn(cgpGraph.Inputs.Count + (i * cgpGraph.Rows), cgpGraph.Rows, false));
        } else {
          strBuilder.AppendLine(DeclareColumn(cgpGraph.Inputs.Count + (i * cgpGraph.Rows), cgpGraph.Rows, !displayInactive));
        }
      }

      strBuilder.AppendLine();

      for (int i = 0; i < cgpGraph.Rows; i++) {
        strBuilder.AppendLine(DeclareRows(cgpGraph.Inputs.Count + i, cgpGraph.Rows, cgpGraph.Columns));
      }

      strBuilder.AppendLine();

      //draw connections
      foreach (Node node in cgpGraph.Nodes.Values) {
        if ((!displayInactive && node.IsActive) || displayInactive) {
          int position = 0;
          int arity = OpCodes.MapNodeToArity(node);

          for (int i = 0; i < arity; i++) {
            strBuilder.AppendLine(DeclareConnection(node.Inputs[i], node.Id, position));
            position++;
          }
        }
      }

      strBuilder.AppendLine($"label=\"{Graph.SolutionString}\"");

      strBuilder.AppendLine("}");
      return strBuilder.ToString();
    }

    private static string DeclareRows(int start, int rows, int cols) {
      StringBuilder strBuilder = new StringBuilder();

      for (int i = 0; i < cols - 1; i++) {
        strBuilder.Append($"node{start + (i * rows)} -> ");
      }
      strBuilder.Append($"node{start + ((cols - 1) * rows)}");

      strBuilder.Append($"[ weight = {rows * cols * 10} ][ style = invis ]");

      return strBuilder.ToString();
    }

    private static string DeclareColumn(int start, int rows, bool invisible) {
      StringBuilder strBuilder = new StringBuilder();

      strBuilder.Append("{rank = same; ");

      for (int i = start; i < (start + rows) - 1; i++) {
        strBuilder.Append($"node{i} -> ");
      }
      strBuilder.Append($"node{(start + rows) - 1}");

      if (invisible) {
        strBuilder.Append("[ style = invis ]");
      }

      strBuilder.Append("}");

      return strBuilder.ToString();
    }

    private static string DeclareNode(Node node, bool invisible) {
      StringBuilder strBuilder = new StringBuilder();

      strBuilder.Append($"node{node.Id}[label=\"");

      if (node.Type == NodeType.INPUT ||
          node.Type == NodeType.OUTPUT) {
        strBuilder.Append($"{node.Name}\", fillcolor =\"{(node.Type == NodeType.INPUT ? inputColor : outputColor)}\", style=\" {(invisible ? "invis" : "filled")}\"]");
      } else {
        strBuilder.Append($"{node.Name}\", fillcolor=\"{(node.IsActive ? activeColor : inactiveColor)}\", style=\" {(invisible ? "invis" : "filled")}\"]");
      }

      return strBuilder.ToString();
    }

    private static string DeclareConnection(int sourceId, int destinationId, int position) {
      return $"node{sourceId}:e -> node{destinationId}:w[color=\"{(position == 0 ? lhsColor : rhsColor)}\", label=\"{(position == 0 ? "0" : "1")}\"]";
    }
  }
}
