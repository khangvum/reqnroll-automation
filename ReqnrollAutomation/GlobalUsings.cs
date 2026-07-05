global using NUnit.Framework;
global using OpenQA.Selenium;
global using Reqnroll;
global using Assert = NUnit.Framework.Assert;

// Parallel execution
// ExecutionScope.MethodLevel    - Every scenario runs in parallel (fastest, requires fully isolated scenarios)
// ExecutionScope.ClassLevel     - Features run in parallel, but scenarios within a feature run sequentially (slower, but allows sharing state between scenarios)
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]