using System.Collections.Generic;

namespace FormulaKit.Runtime.Nodes
{
    /// <summary>
    /// Variable reference node
    /// </summary>
    public class VariableNode : IFormulaNode
    {
        private readonly string _variableName;

        public VariableNode(string variableName)
        {
            _variableName = variableName;
        }

        public float Evaluate(Dictionary<string, float> inputs)
        {
            return inputs.GetValueOrDefault(_variableName, 0f);
        }
    }
}