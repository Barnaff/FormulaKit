# Formula Kit

Formula Kit is a lightweight expression authoring framework for Unity projects. It allows designers to author readable formulas
that can be loaded from JSON files or registered at runtime, and comes with an editor window that accelerates iteration.

## Table of Contents

- [Features](#features)
- [Installation](#installation)
  - [Add via Unity Package Manager](#add-via-unity-package-manager)
  - [Install by Editing `manifest.json`](#install-by-editing-manifestjson)
- [Quick Start](#quick-start)
  - [Create and Evaluate a Formula](#create-and-evaluate-a-formula)
  - [Load Formulas from JSON](#load-formulas-from-json)
- [Editor Tooling](#editor-tooling)
- [Samples](#samples)
- [Additional Resources](#additional-resources)
- [License](#license)

## Features

- Runtime parser that evaluates arithmetic, logical, and conditional expressions.
- Deterministic evaluation backed by a node graph representation.
- JSON loader utilities so formulas can be stored outside of compiled code.
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

## Quick Start

### Create and Evaluate a Formula

The runtime API revolves around two types: `FormulaLoader` stores the text of each formula, and `FormulaRunner` evaluates them.

```csharp
using FormulaKit.Runtime;

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

- Formulas return `float` values.
- Input variables that are not supplied default to `0`.
- Use standard arithmetic, comparison, logical operators, and ternary expressions.

### Load Formulas from JSON

`FormulaJsonLoader` supports importing formulas from external files so that designers can tweak values without a code change.

```json
{
  "damage": "baseDamage * (1 + strength * 0.1)",
  "heal": "baseHeal + spirit * 0.5"
}
```

Load the JSON text (from a TextAsset, streaming assets, addressable, etc.) and pass it to `FormulaJsonLoader.LoadJson`:

```csharp
using FormulaKit.Runtime;

TextAsset formulasAsset = Resources.Load<TextAsset>("FormulaExamples");

var loader = new FormulaLoader();
FormulaJsonLoader.LoadJson(formulasAsset.text, loader);

var runner = new FormulaRunner(loader);
var result = runner.Evaluate("heal", new Dictionary<string, float>
{
    ["baseHeal"] = 25f,
    ["spirit"] = 12f
});
```

You can mix and match formulas registered in code and formulas loaded from JSON. Each formula is referenced by the identifier
supplied when it was registered.

## Editor Tooling

Open the Formula Builder via **Tools → Formula Framework → Formula Builder**. The window provides:

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

- API and workflow documentation lives in `Documentation~/FormulaKit.md`.
- Tests under the `Tests/` folder demonstrate expected runtime behaviours.
- Submit issues and feature requests through the repository issue tracker.

## License

Formula Kit is provided under the MIT License. See `LICENSE.md` for details.
