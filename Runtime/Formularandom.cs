using System;

namespace FormulaKit.Runtime
{
    /// <summary>
    /// Random number provider for formulas
    /// Allows seeding for predictable/testable randomness
    /// </summary>
    public interface IRandomProvider
    {
        /// <summary>
        /// Get random integer between 0 (inclusive) and max (exclusive)
        /// </summary>
        int Next(int max);
        
        /// <summary>
        /// Get random float between 0 (inclusive) and max (exclusive)
        /// </summary>
        float NextFloat(float max);
        
        /// <summary>
        /// Get random float between 0.0 and 1.0
        /// </summary>
        float Value();
    }
    
    /// <summary>
    /// Default random provider using System.Random
    /// Thread-safe with ThreadStatic
    /// </summary>
    public class DefaultRandomProvider : IRandomProvider
    {
        [ThreadStatic]
        private static Random _random;
        
        private static Random Random => _random ??= new Random();

        public int Next(int max)
        {
            return max <= 0 ? 0 : Random.Next(max);
        }
        
        public float NextFloat(float max)
        {
            return (float)(Random.NextDouble() * max);
        }
        
        public float Value()
        {
            return (float)Random.NextDouble();
        }
    }
    
    /// <summary>
    /// Seeded random provider for predictable/testable randomness
    /// </summary>
    public class SeededRandomProvider : IRandomProvider
    {
        private readonly Random _random;
        
        public SeededRandomProvider(int seed)
        {
            _random = new Random(seed);
        }
        
        public int Next(int max)
        {
            return max <= 0 ? 0 : _random.Next(max);
        }
        
        public float NextFloat(float max)
        {
            return (float)(_random.NextDouble() * max);
        }
        
        public float Value()
        {
            return (float)_random.NextDouble();
        }
    }
    
    /// <summary>
    /// Fixed random provider for testing (always returns same values)
    /// </summary>
    public class FixedRandomProvider : IRandomProvider
    {
        private readonly float _fixedValue;
        
        public FixedRandomProvider(float fixedValue = 0.5f)
        {
            _fixedValue = fixedValue;
        }
        
        public int Next(int max)
        {
            return (int)(_fixedValue * max);
        }
        
        public float NextFloat(float max)
        {
            return _fixedValue * max;
        }
        
        public float Value()
        {
            return _fixedValue;
        }
    }
    
    /// <summary>
    /// Unity random provider using UnityEngine.Random
    /// Uses Unity's random system (not thread-safe)
    /// </summary>
    public class UnityRandomProvider : IRandomProvider
    {
        public int Next(int max)
        {
            return max <= 0 ? 0 : UnityEngine.Random.Range(0, max);
        }
        
        public float NextFloat(float max)
        {
            return UnityEngine.Random.Range(0f, max);
        }
        
        public float Value()
        {
            return UnityEngine.Random.value;
        }
    }
}