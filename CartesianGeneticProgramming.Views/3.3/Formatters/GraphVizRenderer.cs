using System;
using System.Diagnostics;
using System.IO;

namespace CartesianGeneticProgramming.Views {
  public class GraphVizRenderer {
    private static Graph CurrentGraph;

    public GraphVizRenderer() { }

    public static bool RenderGraph(Graph graph) {
      if (CurrentGraph == null || !CurrentGraph.Equals(graph)) {
        GenerateBitmap(CGPGraphvizGridFormatter.Format(graph, true), "cgp");
        GenerateBitmap(CGPGraphvizGridFormatter.Format(graph, false), "cgp_simplified");

        CurrentGraph = graph;

        return true;
      }
      return false;
    }

    private static void GenerateBitmap(string dotString, string filename) {
      string inputFile = $"{filename}.dot";
      string outputFile = $"{filename}.bmp";

      File.WriteAllText(inputFile, dotString);

      ProcessStartInfo startInfo = new ProcessStartInfo();
      startInfo.CreateNoWindow = true;
      startInfo.UseShellExecute = false;
      startInfo.FileName = @"dot\dot.exe";
      startInfo.WindowStyle = ProcessWindowStyle.Hidden;
      startInfo.Verb = "runas";

      startInfo.Arguments = $"dot -Tbmp {inputFile} -o {outputFile}";
      try {
        using (Process exeProcess = Process.Start(startInfo)) {
          exeProcess.WaitForExit();
        }
      } catch (Exception ex) {
        Console.WriteLine(ex);
        // Log error.
      }

    }
  }
}
