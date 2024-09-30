
using FFMpegCore;
using FFMpegCore.Pipes;
using MusicAI.Contracts.Enums;
using MusicAI.Services.Logs;
using System.Diagnostics;

namespace MusicAI.Services.MusicService
{
    public class AnthemScoreCMDService1
    {
        NLog.ILogger logger;
        string anthemScorePath;
        string outPutDir;
        string outPutFormat;
        string inPutDir;
        string inPutMp3Fornat;
        string inPutWavFornat;

        public AnthemScoreCMDService1(bool isDevelopment)
        {
            if (isDevelopment)
            {
                GlobalFFOptions.Configure(options => options.BinaryFolder = @"D:\Programs Downloads\ffmpeg-7.0.1-essentials_build\ffmpeg-7.0.1-essentials_build\bin\");
                inPutDir = "D:\\ArmanFanavaranParsRayaneh\\Projects\\bigProject\\MusicAi\\AIInPut\\";

            }
            else
            {
                GlobalFFOptions.Configure(options => options.BinaryFolder = @"C:\Users\Administrator\Desktop\ffmpeg-7.0.1-essentials_build\ffmpeg-7.0.1-essentials_build\bin\");
                inPutDir = "C:\\Users\\Administrator\\Desktop\\AIInput\\";
                anthemScorePath = "C:\\Users\\Administrator\\Desktop\\anthemScore2\\AnthemScore.exe";
                outPutDir = "C:\\Users\\Administrator\\Desktop\\AIOutput\\";
            }
            outPutFormat = ".musicxml";
            inPutWavFornat = ".wav";
            inPutMp3Fornat = ".mp3";
            logger = MyNLog.GetLogger(LoggerType.InfoLogger);
        }


        private async Task<string> SaveWavFile(byte[] audioData)
        {
            var newName = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss");
            string filePath = Path.Combine(inPutDir, newName + inPutWavFornat);
            //if (!Directory.Exists(inPutDir)) 
            //{
            //    Console.WriteLine("inout dir not exists");
            //    Console.WriteLine(inPutDir);
            //}
            //if (!File.Exists(inPutDir+"a.mp3"))
            //{
            //    Console.WriteLine("file not exists");
            //}
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                // WAV file header
                await fileStream.WriteAsync(new byte[] { 0x52, 0x49, 0x46, 0x46 }, 0, 4); // "RIFF"
                await fileStream.WriteAsync(BitConverter.GetBytes(36 + audioData.Length), 0, 4); // File size
                await fileStream.WriteAsync(new byte[] { 0x57, 0x41, 0x56, 0x45 }, 0, 4); // "WAVE"
                await fileStream.WriteAsync(new byte[] { 0x66, 0x6D, 0x74, 0x20 }, 0, 4); // "fmt "
                await fileStream.WriteAsync(BitConverter.GetBytes(16), 0, 4); // Subchunk1Size (16 for PCM)
                await fileStream.WriteAsync(BitConverter.GetBytes((short)1), 0, 2); // AudioFormat (1 for PCM)
                await fileStream.WriteAsync(BitConverter.GetBytes((short)1), 0, 2); // Number of channels (1 for mono)
                await fileStream.WriteAsync(BitConverter.GetBytes(44100), 0, 4); // Sample rate (44100)
                await fileStream.WriteAsync(BitConverter.GetBytes(44100 * 2), 0, 4); // Byte rate (SampleRate * NumChannels * BitsPerSample/8)
                await fileStream.WriteAsync(BitConverter.GetBytes((short)2), 0, 2); // Block align (NumChannels * BitsPerSample/8)
                await fileStream.WriteAsync(BitConverter.GetBytes((short)16), 0, 2); // Bits per sample (16)
                await fileStream.WriteAsync(new byte[] { 0x64, 0x61, 0x74, 0x61 }, 0, 4); // "data"
                await fileStream.WriteAsync(BitConverter.GetBytes(audioData.Length), 0, 4); // Subchunk2Size
                await fileStream.WriteAsync(audioData, 0, audioData.Length); // Audio data
            }
            return filePath;
        }

        private async Task<string> SaveFileAsItIs(byte[] audioData, string inputFormat)
        {
            var newName = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss");
            string filePath = Path.Combine(inPutDir, newName + inputFormat);
            await File.WriteAllBytesAsync(filePath, audioData);
            return filePath;
        }

        private async Task<string> SaveWebmToMp3Format(byte[] webmBytes)
        {
            using (var webmStream = new MemoryStream(webmBytes))
            using (var mp3Stream = new MemoryStream())
            {
                await ConvertWebMToMp3(webmStream, mp3Stream);

                // Now you can use the wavStream as needed, for example, sending it back to the client or saving it
                byte[] wavBytes = mp3Stream.ToArray();

                return await SaveFileAsItIs(wavBytes, inPutMp3Fornat);
            }
        }

        private async Task ConvertWebMToMp3(Stream inputStream, Stream outputStream)
        {
            var inputPipe = new StreamPipeSource(inputStream);
            var outputPipe = new StreamPipeSink(outputStream);
            var conversionProcess = FFMpegArguments
                .FromPipeInput(inputPipe, options => options.ForceFormat("webm"))
                .OutputToPipe(outputPipe, options =>
                    options.ForceFormat("mp3"));
            await conversionProcess.ProcessAsynchronously();
        }


        public async Task<string> ProcessAudio(string anthemScorePath, string inputFile, string outputFile)
        {
            //string anthemScorePath = @"C:\Path\To\AnthemScore.exe";
            //string inputFile = @"D:\Input\audio_chunk.mp3";
            //string outputFile = @"D:\Output\output_file.musicxml";

            // Run AnthemScore with the CLI options
            await RunAnthemScore(anthemScorePath, inputFile, outputFile);

            // Monitor output directory for the result
            MonitorOutputFile(outputFile);

            // Process the output file and receive .musicxml file
            return await ProcessOutputFile(outputFile);
        }

        public async Task<string> ProcessAudio(string inputFile)
        {
            // fileName
            string inputFileNameWithoutFormat = Path.GetFileNameWithoutExtension(inputFile);
            string outputFile = Path.Combine(outPutDir, inputFileNameWithoutFormat + outPutFormat);
            return await ProcessAudio(anthemScorePath, inputFile, outputFile);
        }

        public async Task<string> ProcessAudio(byte[] inputAudioData, bool wavFormat, bool mp3Fromat, bool isForTest = false)
        {
            if (isForTest)
            {
                //return await File.ReadAllTextAsync("C:\\Users\\Administrator\\Desktop\\AIOutput\\a.musicxml");
                if (wavFormat)
                {
                    await SaveFileAsItIs(inputAudioData, inPutWavFornat);
                }
                else if (mp3Fromat)
                {
                    await SaveWebmToMp3Format(inputAudioData);
                }
                return await File.ReadAllTextAsync("D:\\ArmanFanavaranParsRayaneh\\Projects\\bigProject\\MusicAi\\AIOutPut\\a.musicxml");
            }
            else
            {
                string inputFilePath = "";
                if (wavFormat)
                {
                    inputFilePath = await SaveFileAsItIs(inputAudioData, inPutWavFornat);
                }
                else if (mp3Fromat)
                {
                    inputFilePath = await SaveWebmToMp3Format(inputAudioData);
                }
                return await ProcessAudio(inputFilePath);
            }

        }

        private async Task RunAnthemScore(string anthemScorePath, string inputFile, string outputFile)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = anthemScorePath,
                Arguments = $"\"{inputFile}\" -a -x \"{outputFile}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                await process.WaitForExitAsync();
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

        private void MonitorOutputFile(string outputFile)
        {
            int timeout = 30000; // 1 minute timeout
            int elapsed = 0;
            int interval = 100; // Check every second

            while (elapsed < timeout)
            {
                if (File.Exists(outputFile))
                {
                    logger.Debug("Output file created successfully.");
                    return;
                }

                Thread.Sleep(interval);
                elapsed += interval;
            }

            throw new Exception("Timeout: Output file was not created within the expected time.");
        }

        private async Task<string> ProcessOutputFile(string outputFile)
        {
            if (File.Exists(outputFile))
            {
                string content = await File.ReadAllTextAsync(outputFile);
                logger.Debug("Processing output file...");
                return content;
            }
            else
            {
                logger.Debug("Error: Output file not found.");
                throw new Exception("Error: Output file not found.");
            }
        }
    }
}
