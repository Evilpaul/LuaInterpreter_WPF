using MoonSharp.Interpreter;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace LuaInterpreter_WPF
{
    class Worker
	{
		private Thread t_work;
		private Stopwatch sw;

		public Worker()
		{
			sw = new Stopwatch();

			Script.DefaultOptions.DebugPrint = s =>
			{
				Logger.Instance.LogExecutionInfo(sw.ElapsedTicks, s);
			};

            Logger.Instance.LogInfo("MoonSharp " + Script.VERSION);
            Logger.Instance.LogInfo("Lua " + Script.LUA_VERSION);
        }

		public async void doScript()
		{
			Task task = runScript(InputHandler.Instance.Text);
			if (task == await Task.WhenAny(task, Task.Delay(Timeout.Instance.Value)))
			{
				await task;
			}
			else
			{
				abortScript();
			}
		}

		private Task runScript(string code)
		{
			Logger.Instance.ClearLog();

			return Task.Run(() =>
			{
				t_work = Thread.CurrentThread;

				try
				{
					sw.Restart();
					DynValue res = Script.RunString(code);
					if (res.Type != DataType.Void)
                        Logger.Instance.LogExecutionResult("Return value : " + res.ToString());
				}
				catch (InterpreterException ex)
				{
                    Logger.Instance.LogWarning("Interpreter exception : " +  ex.DecoratedMessage);
				}
				catch (Exception ex)
				{
                    Logger.Instance.LogWarning("Execution exception : " + ex.Message);
				}
				finally
				{
					sw.Stop();
                    Logger.Instance.LogInfo("Script completed in : " + sw.ElapsedMilliseconds + "ms");
				}
			});
		}

		private void abortScript()
		{
			t_work.Abort();
		}
	}
}
