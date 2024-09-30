using MusicAI.Infrastructure.Data.Configurations;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace MusicAI.Services.MusicService
{
    public class ProcessPoolManager
    {
        private static readonly ConcurrentQueue<Process> _processPool = new ConcurrentQueue<Process>();
        // Limit the pool to (number of parallel core cpre) * (max process per core)
        private static readonly SemaphoreSlim _poolSemaphore = new SemaphoreSlim(AnthemConfig.MaxCoreParallelism * AnthemConfig.MaxCuncurrentRequestPerCore);
        private static readonly int _maxPoolSize = AnthemConfig.MaxCoreParallelism * AnthemConfig.MaxCuncurrentRequestPerCore;
        static ProcessPoolManager()
        {
            InitializeProcessPool();
        }

        private static void InitializeProcessPool()
        {
            Console.WriteLine("initialize process started at :" + DateTime.Now.ToString());
            for (int i = 0; i < _maxPoolSize; i++)
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = AnthemConfig.anthemScorePath, // Adjust path to your AnthemScore executable
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };
                _processPool.Enqueue(process);
            }
            Console.WriteLine("initialize process finished at :" + DateTime.Now.ToString());
        }

        public static async Task<Process> GetProcessAsync()
        {
            // control count of proceess supposed to be added to pool
            await _poolSemaphore.WaitAsync();
            // this is a thread safe data structure that handle race condition bt itself
            if (_processPool.TryDequeue(out var process))
            {
                return process;
            }
            else
            {
                throw new InvalidOperationException("No available processes in the pool.");
            }
        }

        public static void ReleaseProcess(Process process)
        {
            _processPool.Enqueue(process);
            _poolSemaphore.Release();


        }
    }
}
