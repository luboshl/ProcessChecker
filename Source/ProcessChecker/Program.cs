using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ProcessChecker
{
  public class Program
  {
    static void Main(string[] args)
    {
      string processName = args.FirstOrDefault()?.Trim();

      if (string.IsNullOrWhiteSpace(processName))
        throw new ApplicationException("Process name not set.");

      using (var mutex = new Mutex(false, $"CheckProgramIsRunning_{processName}"))
      {
        if (!mutex.WaitOne(TimeSpan.FromSeconds(1), false))
        {
          // jina instance uz bezi, konec
          return;
        }

        RuchCheck(processName);
      }
    }

    private static void RuchCheck(string processName)
    {
      var processes = Process.GetProcessesByName(processName);

      if (!processes.Any())
      {
        using (var form = new Form { TopMost = true})
        {
          MessageBox.Show(form, $"Process '{processName}' is not running.", "Process Checker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
      }
    }
  }
}
