using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoLibrary;
namespace ClaimReq.Models
{

    public class Video
    {

        private static string homePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                 Environment.OSVersion.Platform == PlatformID.MacOSX)
               ? Environment.GetEnvironmentVariable("HOME")
               : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

        public static void SaveVideoToDisk(string link, string format)
        {

            //Full Url or only Youtube ID
            if (!link.Contains("https://www.youtube.com/watch?v="))
            {
                link = "https://www.youtube.com/watch?v=" + link;
            }
           
            var youtube = YouTube.Default;
            var video = youtube.GetVideo(link);
            System.IO.File.WriteAllBytes(homePath + @"\Downloads\" + video.FullName, video.GetBytes());

            if (format.Equals("mp4"))
            {
                return;
            }

            if (Formats.isImageFormat(format))
            {
                DownloadVideoThumb(video.FullName, video.Title, format);
            }
            else
            {

                var inputFile = new MediaFile { Filename = homePath + @"\Downloads\" + video.FullName };
                var outputFile = new MediaFile { Filename = $"{ homePath + @"\Downloads\" + video.Title}." + format };

                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputFile);

                    engine.Convert(inputFile, outputFile);
                }

            }
            //Deleting video downloaded in mp4 format, for converting to others format.
            File.Delete(homePath + @"\Downloads\" + video.FullName);

        }

        //Saves the frame located on the 15th second of the video.
        public static void DownloadVideoThumb(string inputName, string outputName, string outputFormat)
        {

            var inputFile = new MediaFile { Filename = homePath + @"\Downloads\" + inputName };
            var outputFile = new MediaFile { Filename = homePath + @"\Downloads\" + outputName + "." + outputFormat };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);

                //Saves the frame located on the 15th second of the video.
                var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(15) };
                engine.GetThumbnail(inputFile, outputFile, options);
            }

        }

        public static void Convert(string inputName, string inputFormat, string outputName, string outputFormat)
        {

            //DVD format 
            if (outputFormat.Equals("vob")) {
                convertToDVD(inputName,inputFormat, outputName);
            }

            var inputFile = new MediaFile { Filename = homePath + @"\Downloads\" + inputName + "." + inputFormat };
            var outputFile = new MediaFile { Filename = homePath + @"\Downloads\" + outputName + "." + outputFormat };

            using (var engine = new Engine())
            {
                engine.Convert(inputFile, outputFile);
            }
        }

        public static void convertToDVD(string inputName,string inputFormat, string outputName)
        {

            var inputFile = new MediaFile { Filename = homePath + @"\Downloads\" + inputName + "." + inputFormat };
            var outputFile = new MediaFile { Filename = homePath + @"\Downloads\" + outputName + ".vob" };

            var conversionOptions = new ConversionOptions
            {
                Target = Target.DVD,
                TargetStandard = TargetStandard.PAL
            };

            using (var engine = new Engine())
            {
                engine.Convert(inputFile, outputFile, conversionOptions);
            }

        }

        public static void ConvertOptions(string inputName, string outputName)
        {
            var inputFile = new MediaFile { Filename = homePath + @"\Downloads\" + inputName + ".mp4" };
            var outputFile = new MediaFile { Filename = homePath + @"\Downloads\" + outputName + ".mp4" };

            var conversionOptions = new ConversionOptions
            {
                MaxVideoDuration = TimeSpan.FromSeconds(30),
                VideoAspectRatio = VideoAspectRatio.R16_9,
                VideoSize = VideoSize.Hd480,
                AudioSampleRate = AudioSampleRate.Hz44100
            };

            using (var engine = new Engine())
            {
                engine.Convert(inputFile, outputFile, conversionOptions);
            }

        }

        public static void getVideoMetadata(string inputName)
        {
            var inputFile = new MediaFile { Filename = homePath + @"\Downloads\" + inputName };
            //METADATA
            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
            }

            System.Diagnostics.Debug.WriteLine(inputFile.Metadata.Duration);

        }

        public static void cutVideo(string inputName, string outputName, int startingSec, int duration)
        {

            var inputFile = new MediaFile { Filename = homePath + @"\Downloads\" + inputName };
            var outputFile = new MediaFile { Filename = homePath + @"\Downloads\" + outputName };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);

                var options = new ConversionOptions();

                // This example will create a 25 second video, starting from the 
                // 30th second of the original video.
                //// First parameter requests the starting frame to cut the media from.
                //// Second parameter requests how long to cut the video.
                options.CutMedia(TimeSpan.FromSeconds(startingSec), TimeSpan.FromSeconds(duration));

                engine.Convert(inputFile, outputFile, options);

            }


        }
    }

}
