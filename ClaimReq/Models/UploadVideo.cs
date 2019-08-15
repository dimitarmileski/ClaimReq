using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Configuration;

namespace Google.Apis.YouTube.Samples
{
    /// <summary>
    /// YouTube Data API v3 sample: upload a video.
    /// Relies on the Google APIs Client Library for .NET, v1.7.0 or higher.
    /// See https://developers.google.com/api-client-library/dotnet/get_started
    /// </summary>

    internal class UploadVideo
    {
        public IConfiguration _config { get;}

        public UploadVideo(IConfiguration config)
        {
            _config = config;
        }

        public string VideoId;

        //[STAThread]
        //static void Main(string[] args)
        //{
        //    System.Diagnostics.Debug.WriteLine("YouTube Data API: Upload Video");
        //    System.Diagnostics.Debug.WriteLine("==============================");

        //    try
        //    {
        //        new UploadVideo().Run().Wait();
        //    }
        //    catch (AggregateException ex)
        //    {
        //        foreach (var e in ex.InnerExceptions)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Error: " + e.Message);
        //        }
        //    }

        //    System.Diagnostics.Debug.WriteLine("Press any key to continue...");
        //    Console.ReadKey();
        //}

        public async Task Run(string videoPath, string videoTitle, string videoTags, string videoCategorySelected, string videoDesc)
        {
            UserCredential credential;
            
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets() {
                    ClientId = _config["ClientSecrets:ClientId"],
                    ClientSecret = _config["ClientSecrets:ClientSecret"]
                },
                // This OAuth 2.0 access scope allows an application to upload files to the
                // authenticated user's YouTube channel, but doesn't allow other types of access.
                new[] { YouTubeService.Scope.YoutubeUpload },
                "user",
                CancellationToken.None
            );
            

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });

            var video = new Video();
            video.Snippet = new VideoSnippet();
            video.Snippet.Title = videoTitle;
            video.Snippet.Description = videoDesc;

            string[] tagsArray = videoTags.Split(',');
            video.Snippet.Tags = tagsArray;


            video.Snippet.CategoryId = videoCategorySelected; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
            video.Status = new VideoStatus();
            video.Status.PrivacyStatus = "public"; // or "private" or "public"
            var filePath = videoPath; // Replace with path to actual movie file.

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
                videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
                videosInsertRequest.ResponseReceived += videosInsertRequest_ResponseReceived;

                await videosInsertRequest.UploadAsync();
            }
        }

        void videosInsertRequest_ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
        {
            switch (progress.Status)
            {
                case UploadStatus.Uploading:
                    System.Diagnostics.Debug.WriteLine("{0} bytes sent.", progress.BytesSent);
                    break;

                case UploadStatus.Failed:
                    System.Diagnostics.Debug.WriteLine("An error prevented the upload from completing.\n{0}", progress.Exception);
                    break;
            }
        }

        void videosInsertRequest_ResponseReceived(Video video)
        {
            System.Diagnostics.Debug.WriteLine("Video id '{0}' was successfully uploaded.", video.Id);

            this.VideoId = video.Id;
        }
    }
}