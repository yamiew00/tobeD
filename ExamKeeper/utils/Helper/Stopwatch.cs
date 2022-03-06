using System.Diagnostics;
using System.Text;

namespace Utils {
    public class TimeTester {
        public TimeTester(bool start = true) {
            if (start) {
                Start();
            }
        }
        private Stopwatch sw = new Stopwatch();
        private StringBuilder log = new StringBuilder();
        public string message { get { return log.ToString(); } }

        public void Start() {
            sw.Start();
        }
        public void Record(string message = "") {
            sw.Stop();
            log.AppendLine($"[{sw.ElapsedMilliseconds}/ms] {message}");
            sw.Restart();
        }
        public void Stop() {
            sw.Stop();
        }
    }
}