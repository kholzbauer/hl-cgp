using System.Collections.Generic;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;

namespace CartesianGeneticProgramming.Models {

  [StorableType("8b16ef62-0022-40ff-8b34-af3717ed7b10")]
  public enum NodeType {
    NODE,
    INPUT,
    OUTPUT
  }

  [Item("Node", "Represents node in the CGP graph.")]
  [StorableType("9567cf3a-2f0b-4d68-811e-e367819e6833")]
  public class Node : DeepCloneable {
    [Storable]
    public int Id { get; set; }

    [Storable]
    public string Name { get; set; }

    [Storable]
    public int FunctionNumber { get; set; }

    [Storable]
    public bool IsActive { get; set; } = false;

    [Storable]
    public NodeType Type { get; set; } = NodeType.NODE;

    [Storable]
    public List<int> Inputs { get; set; }

    public Node(int id, NodeType type) {
      Id = id;
      Type = type;

      Inputs = new List<int>();
    }

    #region item cloning and persistence
    [StorableConstructor]
    private Node(StorableConstructorFlag _) : base(_) { }
    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() { }

    private Node(Node original, Cloner cloner)
      : base(original, cloner) {
      Inputs = original.Inputs;
      Id = original.Id;
      Name = original.Name;
      FunctionNumber = original.FunctionNumber;
      IsActive = original.IsActive;
      Type = original.Type;
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new Node(this, cloner);
    }
    #endregion
  }

  public class NodeFactory {
    public static Node Create(ref int id, NodeType type) {
      Node node = new Node(id++, type);

      if (type == NodeType.INPUT) { } //no special needs

      if (type == NodeType.OUTPUT) {
        node.IsActive = true;
      }
      if (type == NodeType.NODE) { } //no special needs

      return node;
    }
  }
}
