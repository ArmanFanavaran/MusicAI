using MusicAI.Contracts.Enums;
using MusicAI.Infrastructure.Data.Configurations;
using MusicAI.Services.Logs;
using System.Diagnostics;
using System.Xml.Linq;

namespace MusicAI.Services.MusicService
{
    public class AnthemScoreCMDService
    {
        NLog.ILogger logger = MyNLog.GetLogger(LoggerType.InfoLogger);

        #region main
        public async Task<string> ProcessAudio(byte[] inputAudioData, string roomID, long timeSlotId, byte instrumentType)
        {
            var inputFilePath = await SaveWavFile(inputAudioData, roomID, timeSlotId);
            // fileName
            string inputFileNameWithoutFormat = Path.GetFileNameWithoutExtension(inputFilePath);
            string outputFile = Path.Combine(AnthemConfig.outPutDir + "r_" + roomID + "//" + timeSlotId, inputFileNameWithoutFormat + AnthemConfig.outPutFormat);
            var xmlDoc = await ProcessAudio(AnthemConfig.anthemScorePath, inputFilePath, outputFile);
            // we don't remove file here since we may need generated xml files to save a notebook file for student
            //RemoveUnCrutialTag(xmlDoc);
            ChangeNoteType(xmlDoc, instrumentType);
            // comnit chnages
            xmlDoc.Save(inputFilePath);
            var result = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + xmlDoc.ToString();
            return result;
        }

        private async Task<string> SaveWavFile(byte[] audioData, string roomID, long timeSlotId)
        {
            string dir = Path.Combine(AnthemConfig.inPutDir, "r_" + roomID + "//" + timeSlotId);
            var newName = dir + "//" + DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss");
            string filePath = Path.Combine(AnthemConfig.inPutDir, newName + AnthemConfig.inPutFormat);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
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

        private void ChangeNoteType(XDocument musicXml, byte instrumentType)
        {
            string newInsgrumentName = InstrumentType.typesWithMidiProgram.GetValueOrDefault(instrumentType).Instrument;
            byte newInsgrumentMIDI = InstrumentType.typesWithMidiProgram.GetValueOrDefault(instrumentType).MidiProgram;
            var scorePart = musicXml.Descendants("score-part").FirstOrDefault();
            // Change the instrument name in <instrument-name> inside <score-instrument>
            var instrumentElement = scorePart.Element("score-instrument");
            if (instrumentElement != null)
            {
                instrumentElement.Element("instrument-name")?.SetValue(newInsgrumentName);
            }
            // Change the MIDI program and instrument sound in <midi-instrument>
            var midiInstrument = scorePart.Element("midi-instrument");
            if (midiInstrument != null)
            {
                midiInstrument.Element("midi-program")?.SetValue(newInsgrumentMIDI.ToString());
            }
        }

        public async Task<XDocument> ProcessAudio(string anthemScorePath, string inputFilePath, string outputFilePath)
        {
            // Run AnthemScore with the CLI options to process the audio chunks and store a .musicxml file
            await RunAnthemScore(anthemScorePath, inputFilePath, outputFilePath);

            // Monitor output directory for the creation of .musicxml file
            MonitorOutputFile(outputFilePath);

            // read the output file and fetch .musicxml file content
            var outPutDocXml = await ReadOutputFileInXmlDoc(outputFilePath);
            return outPutDocXml;
        }

        public async Task<string> ProcessAudioWithStringOutput(string anthemScorePath, string inputFilePath, string outputFilePath)
        {
            // Run AnthemScore with the CLI options to process the audio chunks and store a .musicxml file
            await RunAnthemScore(anthemScorePath, inputFilePath, outputFilePath);

            // Monitor output directory for the creation of .musicxml file
            MonitorOutputFile(outputFilePath);

            return File.ReadAllText(outputFilePath);
        }

        private async Task RunAnthemScore(string anthemScorePath, string inputFile, string outputFile)
        {

            var startInfo = PrepareStartInfo(anthemScorePath, inputFile, outputFile);
            await StartAProcess(startInfo);
            //using (Process process = Process.Start(startInfo))
            //{
            //    await process.WaitForExitAsync();
            //    Console.WriteLine("finished processing : " + inputFile + " at :" + DateTime.Now.ToString());
            //    string output = process.StandardOutput.ReadToEnd();
            //    string error = process.StandardError.ReadToEnd();

            //    if (!string.IsNullOrEmpty(error) || output.Contains("Error:"))
            //    {
            //        logger.Debug($"Error: {output + error}");
            //        throw new Exception(output + error);
            //    }
            //    else
            //    {
            //        logger.Debug($"Output: {output}");
            //    }
            //}
        }

        private ProcessStartInfo PrepareStartInfo(string anthemScorePath, string inputFile, string outputFile)
        {

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = anthemScorePath,
                Arguments = $"\"{inputFile}\" -a -x \"{outputFile}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            // Remove existing APPDATA if it exists, then add your own
            if (startInfo.EnvironmentVariables.ContainsKey("APPDATA"))
            {
                startInfo.EnvironmentVariables.Remove("APPDATA");
            }
            logger.Debug(AnthemConfig.appDataLocal);
            startInfo.EnvironmentVariables.Add("APPDATA", AnthemConfig.appDataLocal);
            return startInfo;
        }

        private async Task StartAProcess(ProcessStartInfo startInfo)
        {
            var process = await ProcessPoolManager.GetProcessAsync();
            try
            {
                process.StartInfo = startInfo;
                process.Start();
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
                    logger.Debug($"Output produced: ");
                }
            }
            finally
            {
                ProcessPoolManager.ReleaseProcess(process);
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
                    //Console.WriteLine(outputFile + " created at :" + DateTime.Now.ToString());
                    //logger.Debug("Output file created successfully.");
                    return;
                }

                Thread.Sleep(interval);
                elapsed += interval;
            }

            throw new Exception("Timeout: Output file was not created within the expected time.");
        }

        private async Task<string> ReadOutputFile(string outputFilePath)
        {
            if (File.Exists(outputFilePath))
            {
                string content = await File.ReadAllTextAsync(outputFilePath);
                //logger.Debug("Processing output file...");
                return content;
            }
            else
            {
                logger.Debug("Error: Output file not found.");
                throw new Exception("Error: Output file not found.");
            }
        }

        private void RemoveUnCrutialTag(XDocument musicXml)
        {
            // Optionally remove certain sections (like <work> or <part-name>)
            // Remove the <work> section
            XElement workElement = musicXml.Descendants("work").FirstOrDefault();
            if (workElement != null)
            {
                workElement.Remove();
            }
            // Remove the <part-name>Piano</part-name> element
            var partNameElement = musicXml.Descendants("part-name")
                .Where(x => x.Value == "Piano")
                .FirstOrDefault();
            if (partNameElement != null)
            {
                partNameElement.Remove();
            }
        }

        private async Task<XDocument> ReadOutputFileInXmlDoc(string outputFilePath)
        {
            if (File.Exists(outputFilePath))
            {
                XDocument musicXml = XDocument.Load(outputFilePath);
                return musicXml;
            }
            else
            {
                logger.Debug("Error: Output file not found.");
                throw new Exception("Error: Output file not found.");
            }
        }

        private void RemoveFile(string outputFilePath)
        {
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
        }

        #endregion




    }
}
