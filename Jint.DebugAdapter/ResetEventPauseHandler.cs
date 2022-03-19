namespace Jint.DebugAdapter
{
    public class ResetEventPauseHandler : IPauseHandler
    {
        private readonly ManualResetEvent waitForContinue = new(false);

        public void Pause()
        {
            // Pause the thread until waitForContinue is set
            waitForContinue.WaitOne();
            waitForContinue.Reset();
        }

        public void Resume()
        {
            waitForContinue.Set();
        }
    }
}
