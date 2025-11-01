using System;
using System.Collections.Generic;

namespace FormulaKit.Runtime
{
    /// <summary>
    /// handles formula registration and caching
    /// /// </summary>
    public class FormulaLoader
    {
        private readonly Dictionary<string, Formula> _formulaCache = new();
        private readonly FormulaParser _parser = new();

        /// <summary>
        /// Register a formula from a string expression
        /// </summary>
        public bool RegisterFormula(string id, string expression)
        {
            try
            {
                var formula = _parser.Parse(expression);
                _formulaCache[id] = formula;
                return true;
            }
            catch (Exception e)
            {
                OnError?.Invoke($"Failed to register formula '{id}': {e.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Get a cached formula by ID
        /// </summary>
        public Formula GetFormula(string id)
        {
            _formulaCache.TryGetValue(id, out Formula formula);
            return formula;
        }

        /// <summary>
        /// Check if a formula exists
        /// </summary>
        public bool HasFormula(string id)
        {
            return _formulaCache.ContainsKey(id);
        }

        /// <summary>
        /// Get all required inputs for a formula
        /// </summary>
        public HashSet<string> GetRequiredInputs(string id)
        {
            return _formulaCache.TryGetValue(id, out var formula) ? formula.RequiredInputs : new HashSet<string>();
        }

        /// <summary>
        /// Get all registered formula IDs
        /// </summary>
        public IEnumerable<string> GetAllFormulaIds()
        {
            return _formulaCache.Keys;
        }

        /// <summary>
        /// Get formula count
        /// </summary>
        public int GetFormulaCount()
        {
            return _formulaCache.Count;
        }

        /// <summary>
        /// Remove a formula
        /// </summary>
        public bool RemoveFormula(string id)
        {
            return _formulaCache.Remove(id);
        }

        /// <summary>
        /// Clear all formulas
        /// </summary>
        public void ClearAll()
        {
            _formulaCache.Clear();
            OnLog?.Invoke("All formulas cleared");
        }

        /// <summary>
        /// Get formula expression string
        /// </summary>
        public string GetFormulaExpression(string id)
        {
            var formula = GetFormula(id);
            return formula?.Expression;
        }

        // Events for logging and error handling
        public event Action<string> OnLog;
        public event Action<string> OnError;
    }
}