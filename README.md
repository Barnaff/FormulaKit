# Formula Kit

Formula Kit is a lightweight expression authoring framework for Unity projects. It allows designers to author readable formulas that can be loaded from JSON files or registered at runtime, and comes with an editor window that accelerates iteration.

## Features

- Runtime parser that evaluates arithmetic, logical, and conditional expressions.
- Deterministic evaluation backed by a node graph representation.
- JSON loader utilities so formulas can be stored outside of compiled code.
- Editor tooling for prototyping, including syntax highlighting and evaluation helpers.
- Test assembly demonstrating expected behaviours.

## Getting Started

1. Add the Git URL of this repository to your Unity project's Package Manager (Unity 2021.3 or newer).
2. Open the Formula Builder window via **Tools → Formula Framework → Formula Builder**.
3. Use the built-in examples or load formulas from JSON to experiment.

Refer to the documentation in `Documentation~/FormulaKit.md` for an overview of the runtime API and editor tooling.

## Samples

The package ships with an optional sample under **Formula Builder Quickstart** that walks through the editor workflow. Import it through the Unity Package Manager window.

## License

Formula Kit is provided under the MIT License. See `LICENSE.md` for details.
