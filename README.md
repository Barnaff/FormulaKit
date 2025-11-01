Formula Kit is a lightweight expression authoring framework for Unity projects. It allows designers to author readable formulas
that can be provided as plain strings from any source (ScriptableObjects, TextAssets, remote configs, etc.) and comes with an
editor window that accelerates iteration.

## Table of Contents

- [API Overview](#api-overview)
  - [Inline Evaluation](#inline-evaluation)
  - [Fluent Builder](#fluent-builder)
- [Advanced Usage](#advanced-usage)
  - [Using FormulaLoader and FormulaRunner](#using-formulaloader-and-formularunner)
  - [Caching Strategies](#caching-strategies)
- [Supported Operations](#supported-operations)
  - [Expression Syntax](#expression-syntax)
  - [Built-in Functions](#built-in-functions)
  - [Random Helpers](#random-helpers)
- [Features](#features)
- [Installation](#installation)
  - [Add via Unity Package Manager](#add-via-unity-package-manager)
  - [Install by Editing `manifest.json`](#install-by-editing-manifestjson)
- [Editor Tooling](#editor-tooling)
- [Samples](#samples)
- [Additional Resources](#additional-resources)
- [License](#license)

## API Overview

Formula Kit ships with a static `FormulaAPI` that offers the quickest way to evaluate expressions. Expressions can be evaluated
inline or via a fluent builder that caches parsed formulas for reuse.

### Inline Evaluation

```csharp
using System.Collections.Generic;
using FormulaFramework;

var inputs = new Dictionary<string, float>
{
    ["baseDamage"] = 10f,
    ["strength"] = 5f
};

float total = FormulaAPI.Run("baseDamage * (1 + strength * 0.1f)", inputs);
```

- Formulas return `float` values.
- Input variables that are not supplied default to `0`.
- A deterministic cache identifier is generated automatically so repeated calls reuse the parsed formula.
- Expressions are plain strings, so you can source them from any Unity asset, configuration system, or even download them at
  runtime before evaluation.

#### Static API Examples

`FormulaAPI.Run(expression, inputs)` is versatile enough to cover many gameplay scenarios. The following snippets each supply
the inputs that matter for a specific expression:

```csharp
// Arithmetic scaling
var damageInputs = new Dictionary<string, float>
{
    ["baseDamage"] = 18f,
    ["strength"] = 12f
};

float scaledDamage = FormulaAPI.Run("baseDamage * (1 + strength * 0.05)", damageInputs);
```

```csharp
// Built-in helpers and clamping
var energyInputs = new Dictionary<string, float>
{
    ["currentEnergy"] = 45f,
    ["regen"] = 10f,
    ["deltaTime"] = 0.5f,
    ["maxEnergy"] = 100f
};

float energyTick = FormulaAPI.Run("clamp(currentEnergy + regen * deltaTime, 0, maxEnergy)", energyInputs);
```

```csharp
// Branching with the ternary operator and random rolls
var critInputs = new Dictionary<string, float>
{
    ["critChance"] = 0.25f,
    ["critMultiplier"] = 2f
};

float critRoll = FormulaAPI.Run("randf(1) < critChance ? critMultiplier : 1", critInputs);
```

```csharp
// Function composition for vector math
var travelInputs = new Dictionary<string, float>
{
    ["dx"] = 3f,
    ["dy"] = 4f,
    ["distanceWeight"] = 0.6f
};

float travelCost = FormulaAPI.Run("sqrt(dx * dx + dy * dy) * distanceWeight", travelInputs);
```

```csharp
// Scoped variables and expressions via `let`
var supportInputs = new Dictionary<string, float>
{
    ["baseDamage"] = 18f,
    ["spirit"] = 30f,
    ["targetSpirit"] = 24f
};

float supportHeal = FormulaAPI.Run(
    "let bonus = max(0, (spirit - targetSpirit) * 0.25) in baseDamage + bonus",
    supportInputs);
```

Each call highlights a different facet of the expression language, giving you a starting point for arithmetic chains, helper
functions, branching logic, and multi-step calculations.

### Fluent Builder

```csharp
using FormulaFramework;

float critical = FormulaAPI
    .Run("(baseDamage + bonus) * crit")
    .Set("baseDamage", 12f)
    .Set("bonus", 3f)
    .Set("crit", 1.5f)
    .Evaluate();
```

The builder lets you populate inputs incrementally. Call `WithCache("myKey")` instead of `Evaluate()` to provide a custom cache
identifier when you want to share the parsed expression between systems.

## Features

- Runtime parser that evaluates arithmetic, logical, and conditional expressions.
- Deterministic evaluation backed by a node graph representation.
- Lightweight runtime APIs that accept string formulas from any source in your project.
- Editor tooling for prototyping, including syntax highlighting and evaluation helpers.
- Test assembly demonstrating expected behaviours.

## Installation

Formula Kit is distributed as a Unity package that can be installed directly from a Git URL. Unity 2021.3 LTS or newer is required.

### Add via Unity Package Manager

1. Open your Unity project.
2. Navigate to **Window → Package Manager**.
3. Click the **+** button in the top-left corner of the Package Manager window.
4. Select **Add package from git URL...**.
5. Paste the repository URL:

   ```
   https://github.com/aornelas07/FormulaKit.git
   ```

6. Press **Add**. Unity downloads the package and registers it inside your project.

### Install by Editing `manifest.json`

If you prefer to edit your project's `Packages/manifest.json` manually, add an entry under the `dependencies` section:

```json
{
  "dependencies": {
    "com.aornelas.formulakit": "https://github.com/aornelas07/FormulaKit.git",
    "com.unity.modules.ui": "1.0.0",
    "com.unity.modules.uielements": "1.0.0"
  }
}
```

Save the file and Unity will fetch the package the next time the editor refreshes packages.

## Advanced Usage

### Using FormulaLoader and FormulaRunner

For projects that manage many expressions, instantiate `FormulaLoader` and `FormulaRunner` directly. Register formulas in code
or inject string expressions from whichever content pipeline you use (ScriptableObjects, TextAssets, remote configs, etc.) and
evaluate them by ID.

```csharp
using FormulaKit.Runtime;
using System.Collections.Generic;

var loader = new FormulaLoader();
loader.RegisterFormula("heal", "baseHeal + spirit * 0.5f");
loader.RegisterFormula("bossPhase", bossPhaseFormulaTextAsset.text);

var runner = new FormulaRunner(loader);
var damage = runner.Evaluate("damage", new Dictionary<string, float>
{
    ["baseDamage"] = 10f,
    ["strength"] = 5f
});
```

Mix and match formulas registered in code and formulas sourced from external content such as TextAssets, ScriptableObjects, or
remote configuration payloads. Each formula is referenced by the string identifier used during registration.

### Caching Strategies

- `FormulaAPI.Run(expression).WithCache("id")` stores the parsed expression under a custom key so other systems can reuse it.
- Call `FormulaRunner.PrepareFormula("damage")` to pool input dictionaries for hot paths before entering a tight loop.
- Use `FormulaRunner.UseInputPooling = false` when you prefer to allocate fresh dictionaries for every evaluation.
- `FormulaAPI.ClearCache()` removes all cached expressions and pooled inputs.

## Supported Operations

### Expression Syntax

- Arithmetic: `+`, `-`, `*`, `/`, `%`, and exponentiation `^`.
- Unary operations: prefix `+`, prefix `-`, and logical negation `!`.
- Comparisons: `<`, `<=`, `>`, `>=`, `==`, `!=`.
- Logical operators: `&&`, `||`.
- Conditional operator: `condition ? whenTrue : whenFalse`.
- Statements: `let` declarations, assignments (`=`, `+=`, `-=`, `*=`, `/=`), `if`/`else`, and block scopes `{ ... }`.

### Built-in Functions

Unary helpers:

`sqrt`, `abs`, `floor`, `ceil`, `round`, `sin`, `cos`, `tan`, `log`, `exp`, `clamp01`, `sign`, `negative`, `acos`, `asin`, `atan`.

Multi-argument helpers:

`min(a, b)`, `max(a, b)`, `clamp(value, min, max)`, `lerp(a, b, t)`, `pow(a, b)`.

### Random Helpers

- `random()` returns a float in `[0, 1]`.
- `rand(max)` returns an integer from `0` to `max - 1`.
- `randf(max)` returns a float in `[0, max)`.

## Editor Tooling

Open the Formula Builder via **Tools → Formula Framework → Formula Builder**. The window provides:

<img width="600" height="892" alt="image" src="https://github.com/user-attachments/assets/66960947-e611-40f9-bf3b-61e4b57fd5e7" />

- An advanced expression editor with syntax highlighting and inline validation.
- Auto-detection of input variables and quick entry of sample values.
- Real-time evaluation so designers can test calculations before committing them to code.
- A curated library of function snippets for common arithmetic operations.
- Built-in examples that populate the editor and auto-fill inputs.

The editor works without any additional setup once the package is installed.

## Samples

The package ships with an optional sample under **Formula Builder Quickstart** that walks through the editor workflow and includes
ready-made formulas covering common gameplay systems. Import it through the Unity Package Manager window to explore the examples.

## Additional Resources

- Tests under the `Tests/` folder demonstrate expected runtime behaviours.
- Submit issues and feature requests through the repository issue tracker.

## License

Formula Kit is provided under the MIT License. See `LICENSE.md` for details.
