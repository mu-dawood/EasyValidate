using System;
using System.Diagnostics;
using System.Threading;

namespace EasyValidate.Generator
{

    internal static class DebuggerUtil
    {
        internal static void AttachDebugger()
        {
#if DEBUG

            if (Debugger.IsAttached)
                return;

            TryAttachDebugger();

            // wait for debugger to be attached (up to 30s)
            // this leaves time to manually attach it on linux or if the automatic attach didn't work
            for (var i = 0; i < 30 && !Debugger.IsAttached; i++)
            {
                // Use diagnostics or supported logging mechanisms instead of Console
                Debugger.Log(0, "Debug", $"Waiting for debugger to attach... {30 - i}s remaining\n");
                Thread.Sleep(1000);
            }
#endif
        }

        private static void TryAttachDebugger()
        {
            try
            {
                Debugger.Launch();
            }
            catch (Exception ex)
            {
                Debugger.Log(0, "Error", $"Debugger attachment failed: {ex.Message}\n");
            }
        }
        public static void Log(string message)
        {
#if DEBUG
            Debugger.Log(0, "Debug", $"[DEBUG] {message}\n");
            System.Diagnostics.Debug.WriteLine($"[DEBUG] {message}");
#endif
        }
    }
}