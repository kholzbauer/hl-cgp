using System;

namespace CartesianGeneticProgramming.Interpreter {
  /*
   * Custom exception that indicates that a constraint was not met
   */
  public class ConstraintViolationException : Exception {
    public ConstraintViolationException() { }

    public ConstraintViolationException(string message) : base(message) { }
  }
}
