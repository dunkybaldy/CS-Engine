using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Diagnostics
{
    public class DiagnosticsController : IDisposable
    {
        private readonly ILogger<DiagnosticsController> _logger;
        private readonly Stopwatch _stopwatch;
        private readonly IDictionary<string, int> MethodErrors;

        public DiagnosticsController(ILogger<DiagnosticsController> logger, Stopwatch stopwatch)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
            MethodErrors = new Dictionary<string, int>();
            _stopwatch.Start();
        }

        public async Task DiagnoseTask(Task action, string methodName)
        {
            try
            {
                var beforeExecution = _stopwatch.ElapsedMilliseconds;
                await action;
                var executionTime = _stopwatch.ElapsedMilliseconds - beforeExecution;
                _logger.LogInformation("{MethodName} finished executing in {MethodExecutionTime} ms", methodName, executionTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong in {MethodName}", methodName);
                AddError(nameof(action));
            }
        }

        public async Task DiagnoseFunctionAction(Func<Action> action)
        {
            try
            {
                var beforeExecution = _stopwatch.ElapsedMilliseconds;
                await Task.FromResult(action.Invoke());
                var executionTime = _stopwatch.ElapsedMilliseconds - beforeExecution;
                _logger.LogInformation("{MethodName} finished executing in {MethodExecutionTime} ms", action.Method.Name, executionTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong in {MethodName}", action.Method.Name);
                AddError(action.Method.Name);
            }
        }

        public async Task DiagnoseAction(Action action)
        {
            try
            {
                var beforeExecution = _stopwatch.ElapsedMilliseconds;
                await Task.FromResult(new Task(action));
                var executionTime = _stopwatch.ElapsedMilliseconds - beforeExecution;
                _logger.LogInformation("{MethodName} finished executing in {MethodExecutionTime} ms", action.Method.Name, executionTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong in {MethodName}", action.Method.Name);
                AddError(action.Method.Name);
            }
        }

        private void AddError(string methodName)
        {
            if (MethodErrors.ContainsKey(methodName))
                MethodErrors[methodName] += 1;            
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _logger.LogInformation("Application running: {AppTime} seconds", _stopwatch.ElapsedMilliseconds / 1000);
            _logger.LogInformation("There are {ErrorCount} methods. Most errored method: {MostErroredMethod}", MethodErrors.Count,
                MethodErrors.OrderByDescending(x => x.Value).FirstOrDefault().Value);
        }
    }
}
