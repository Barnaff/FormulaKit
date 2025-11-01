# Formula Kit Documentation

## Overview

Formula Kit enables designers and engineers to describe gameplay calculations with a small expression language. Expressions are parsed into an abstract syntax tree and executed deterministically at runtime.

The package is split into three assemblies:

- **FormulaKit** – runtime types for defining and running formulas.
- **FormulaKit.Editor.Tools** – editor utilities, including the Formula Builder window.
- **FormulaKit.Editor.Tests** – optional editor tests (enabled when `UNITY_INCLUDE_TESTS` is defined).

## Runtime Usage

1. Create a `FormulaLoader` and register formulas using either `RegisterFormula` or by loading a JSON payload with `FormulaJsonLoader`.
2. Construct a `FormulaRunner` with the loader instance.
3. Call `Evaluate(formulaId, inputs)` to produce a result. Inputs are supplied as a dictionary mapping variable names to floats.

```csharp
var loader = new FormulaLoader();
loader.RegisterFormula("damage", "baseDamage * (1 + strength * 0.1)");

var runner = new FormulaRunner(loader);
var inputs = new Dictionary<string, float>
{
    ["baseDamage"] = 10f,
    ["strength"] = 5f
};

float total = runner.Evaluate("damage", inputs);
```

### Loading from JSON

`FormulaJsonLoader` supports importing formulas from external files so that designers can tweak values without a code change. The JSON schema maps identifiers to expression strings.

```json
{
  "damage": "baseDamage * (1 + strength * 0.1)",
  "heal": "baseHeal + spirit * 0.5"
}
```

Load the file at runtime using Unity's asset systems or any other IO method, then pass the raw string into `LoadJson`.

## Formula Builder Window

Open the Formula Builder via **Tools → Formula Framework → Formula Builder**. The window provides:

- An advanced editor with syntax highlighting.
- Auto-detection of input variables.
- Evaluation of formulas using sample values.
- A curated list of ready-to-use function snippets.
- Built-in examples that populate the editor and auto-fill inputs.

The example formulas are compiled into the package so they work in any project without additional setup.

## Samples

Import the **Formula Builder Quickstart** sample to explore typical workflows and see the formulas used in the built-in examples.

## Requirements

- Unity 2021.3 LTS or newer.
- .NET Standard 2.1 compatible scripting backend.

## Support

Issues and feature requests can be submitted through the repository issue tracker.
