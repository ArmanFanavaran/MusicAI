using MusicAI.Contracts.Enums;
using MusicAI.Infrastructure.Data.Configurations;
using MusicAI.Services.Logs;
using System.Diagnostics;

namespace MusicAI.Services.MusicService
{
    public class AnthemScoreCmdServiceTest
    {
        NLog.ILogger logger = MyNLog.GetLogger(LoggerType.InfoLogger);
        #region test
        //public async Task TestMultipleProcess()
        //{
        //    string[] inputFiles = Directory.GetFiles(AnthemConfig.inPutDir, "*.mp3");
        //    // Create a list of tasks to run AnthemScore on each file
        //    List<Task> tasks = new List<Task>();
        //    foreach (var inputFile in inputFiles)
        //    {
        //        string outputFile = Path.Combine(AnthemConfig.outPutDir, Path.GetFileNameWithoutExtension(inputFile) + ".musicxml");
        //        tasks.Add(ProcessAudio(AnthemConfig.anthemScorePath, inputFile, outputFile));
        //    }
        //    var now = DateTime.Now;
        //    logger.Info("started anthem task with " + inputFiles.Length + " files at : " + now.ToString());
        //    // Run all tasks simultaneously and wait for them to complete
        //    await Task.WhenAll(tasks);
        //    var then = DateTime.Now;
        //    var seconds = then.Subtract(now).TotalSeconds;
        //    logger.Info("finished anthem task at : " + then.ToString() + " i took " + seconds + " seconds");
        //}
        public async Task TestMultipleProcessV2()
        {
            string[] inputFiles = Directory.GetFiles(AnthemConfig.inPutDir, "*.mp3");

            var now = DateTime.Now;
            logger.Info($"Started anthem task with {inputFiles.Length} files at: {now}");

            var tasks = inputFiles.Select(inputFile =>
            {
                return Task.Run(async () =>
                {
                    string outputFile = Path.Combine(AnthemConfig.outPutDir, Path.GetFileNameWithoutExtension(inputFile) + ".musicxml");
                    var process = await ProcessPoolManager.GetProcessAsync();

                    try
                    {
                        await ProcessAudio(process, inputFile, outputFile);
                    }
                    finally
                    {
                        ProcessPoolManager.ReleaseProcess(process);
                    }
                });
            }).ToArray();

            await Task.WhenAll(tasks);

            var then = DateTime.Now;
            var seconds = then.Subtract(now).TotalSeconds;
            logger.Info($"Finished anthem task at: {then}, it took {seconds} seconds");
        }

        public async Task TestMultipleProcessV3()
        {
            string[] inputFiles = Directory.GetFiles(AnthemConfig.inPutDir, "*.mp3");
            // Group input files into batches of 50
            var batches = inputFiles
                .Select((file, index) => new { file, index })
                .GroupBy(x => x.index / AnthemConfig.MaxCuncurrentRequestPerCore)
                .Select(g => g.Select(x => x.file).ToArray())
                .ToList();
            var now = DateTime.Now;
            logger.Info($"Started anthem task with {inputFiles.Length} files at: {now}");

            // Use Parallel.ForEach with limited parallelism
            Parallel.ForEach(batches,
                new ParallelOptions { MaxDegreeOfParallelism = AnthemConfig.MaxCoreParallelism },
                batch =>
                {
                    // Create and run tasks for each input file
                    var tasks = batch.Select(async (inputFile, index) =>
                    {
                        // Introduce a delay of one second between each task
                        await Task.Delay(TimeSpan.FromSeconds(index));
                        return Task.Run(async () =>
                        {
                            string outputFile = Path.Combine(AnthemConfig.outPutDir, Path.GetFileNameWithoutExtension(inputFile) + ".musicxml");
                            var process = await ProcessPoolManager.GetProcessAsync();

                            try
                            {
                                await ProcessAudio(process, inputFile, outputFile);
                            }
                            finally
                            {
                                ProcessPoolManager.ReleaseProcess(process);
                            }
                        });
                    }).ToArray();

                    Task.WhenAll(tasks).GetAwaiter().GetResult(); // Block here to wait for all tasks to complete
                });

            var then = DateTime.Now;
            var seconds = then.Subtract(now).TotalSeconds;
            logger.Info($"Finished anthem task at: {then}, it took {seconds} seconds");
        }

        public async Task TestMultipleProcessV3_1()
        {
            string[] inputFiles = Directory.GetFiles(AnthemConfig.inPutDir, "*.mp3");
            // Group input files into batches of 50
            var batches = inputFiles
                .Select((file, index) => new { file, index })
                .GroupBy(x => x.index / AnthemConfig.MaxCuncurrentRequestPerCore)
                .Select(g => g.Select(x => x.file).ToArray())
                .ToList();
            var now = DateTime.Now;
            logger.Info($"Started anthem task with {inputFiles.Length} files at: {now}");

            // Use Parallel.ForEach with limited parallelism
            Parallel.ForEach(batches,
                new ParallelOptions { MaxDegreeOfParallelism = AnthemConfig.MaxCoreParallelism },
                batch =>
                {
                    Task[] tasks = new Task[batch.Length];
                    // Create and run tasks for each input file
                    for (int i = 0; i < batch.Length; i++)
                    {
                        tasks[i] = Task.Run(async () =>
                        {
                            string outputFile = Path.Combine(AnthemConfig.outPutDir, Path.GetFileNameWithoutExtension(batch[i]) + ".musicxml");
                            var process = await ProcessPoolManager.GetProcessAsync();
                            try
                            {
                                await ProcessAudio(process, batch[i], outputFile);
                            }
                            finally
                            {
                                ProcessPoolManager.ReleaseProcess(process);
                            }
                        });
                    }
                    Task.WhenAll(tasks).GetAwaiter().GetResult(); // Block here to wait for all tasks to complete
                });

            var then = DateTime.Now;
            var seconds = then.Subtract(now).TotalSeconds;
            logger.Info($"Finished anthem task at: {then}, it took {seconds} seconds");
        }
        #endregion


        public async Task ProcessAudio(Process process, string inputFile, string outputFile)
        {
            process.StartInfo.Arguments = $"\"{inputFile}\" -a -x \"{outputFile}\"";

            Console.WriteLine($"Started processing: {inputFile} at: {DateTime.Now}");

            process.Start();
            await process.WaitForExitAsync();

            Console.WriteLine($"Finished processing: {inputFile} at: {DateTime.Now}");

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (!string.IsNullOrEmpty(error) || output.Contains("Error:"))
            {
                logger.Debug($"Error: {output + error}");
                throw new Exception(output + error);
            }
            else
            {
                logger.Debug($"Output: {output}");
            }
        }


    }
}
