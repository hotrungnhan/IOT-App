global using Microsoft.VisualStudio.TestTools.UnitTesting;

using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
