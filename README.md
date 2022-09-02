# Cartesian Genetic Programming for HeuristicLab

This repository contains the source code and built sources for a prototype of Cartesian Genetic Programming (CGP) for symbolic regression as plugin for HeuristicLab (HL) (https://dev.heuristiclab.com/trac.fcgi/). The implementation was part of the master thesis for the department of Software Engineering at the FH Hagenberg.

> Note: This prototype is currently not under development and will not receive further updates

___

## Features

- HL Problem that encodes solution candidates as graphs
- Support of benchmark problems for symbolic regression
- Enhanced anaylsis data for symbolic regression solutions (e.g. model accuracy)
- GraphViz visualization for solutions
- Import and save graphs as bitmap image

___

## Building from sources
To build HL from sources, `revision 18148` of the `trunk` branch is needed. Then, the two folders `CartesianGeneticProgramming` and `CartesianGeneticProgramming.Views` have to be copied into the source folder. These two projects can then be added to the solution via Visual Studio. Upon compilation, the plugins also build to the `bin` folder and are automatically detected by HL and available in the GUI.

## Compiled version
To allow for easy testing of the prototype, a compiled version of HL containing the prototype is provided in the `build-sources` zip folder. Upon downloading and extracting the files, HL including CGP may be used.
___
