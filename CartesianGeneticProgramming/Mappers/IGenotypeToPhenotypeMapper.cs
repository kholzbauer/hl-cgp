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

using HEAL.Attic;
using HeuristicLab.Encodings.IntegerVectorEncoding;

namespace CartesianGeneticProgramming {
  [StorableType("77b1d25c-58ec-48cb-8553-41f6ba9c1368")]
  /// <summary>
  /// IGenotypeToPhenotypeMapper
  /// </summary>
  public interface IGenotypeToPhenotypeMapper : IIntegerVectorOperator {
    Graph Map(int n_inputs, int n_outputs, int levelsBack, int rows, int cols, IntegerVector genotype);
  }
}
