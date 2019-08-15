/*
 * Copyright 2015 Google Inc. All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 */

using System;
using System.Collections.Generic;
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
using ClaimReq.Models;

namespace Google.Apis.YouTube.Samples
{
    /// <summary>
    /// YouTube Data API v3 sample: search by keyword.
    /// Relies on the Google APIs Client Library for .NET, v1.7.0 or higher.
    /// See https://code.google.com/p/google-api-dotnet-client/wiki/GettingStarted
    ///
    /// Set ApiKey to the API key value from the APIs & auth > Registered apps tab of
    ///   https://cloud.google.com/console
    /// Please ensure that you have enabled the YouTube Data API for your project.
    /// </summary>
    public class Search
    {

        public IConfiguration _config { get; }

        public Search(IConfiguration config)
        {
            _config = config;
        }

        //[STAThread]
        //static void Main(string[] args)
        //{
        //    System.Diagnostics.Debug.WriteLine("YouTube Data API: Search");
        //    System.Diagnostics.Debug.WriteLine("========================");

        //    try
        //    {
        //        new Search().Run().Wait();
        //    }
        //    catch (AggregateException ex)
        //    {
        //        foreach (var e in ex.InnerExceptions)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Error: " + e.Message);
        //        }
        //    }

        //    System.Diagnostics.Debug.WriteLine("Press any key to continue...");

        //}

        public async Task<List<VideoSearchInfo>> Run(string searchKeyword)
        {
            VideoSearchInfo videoSearchInfo = new VideoSearchInfo();

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _config["ClientSecrets:ApiKeyYoutubeSearch"],
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = searchKeyword; // Replace with your search term.
            searchListRequest.MaxResults = 50;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<VideoSearchInfo> videoSearchInfos = new List<VideoSearchInfo>();
            List<string> channels = new List<string>();
            List<string> playlists = new List<string>();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videoSearchInfos.Add(new VideoSearchInfo{
                            Title = searchResult.Snippet.Title,
                            VideoId = searchResult.Id.VideoId,
                            Description = searchResult.Snippet.Description,
                            PublishedAt = searchResult.Snippet.PublishedAt.ToString(),
                            ThumbnailUrl = searchResult.Snippet.Thumbnails.High.Url,
                            ChannelTitle = searchResult.Snippet.ChannelTitle,
                            ChannelId = searchResult.Snippet.ChannelId,
                            isChannel = false,
                            isPlaylist = false
                           
                        });
                        //videoSearchInfos.Add(String.Format("VideoTitle: {0} VideoId: ({1}) Videodescription: ({2}) Video published: ({3}) Video ({4})", searchResult.Snippet.Title, searchResult.Id.VideoId, searchResult.Snippet.Description, searchResult.Snippet.PublishedAt));
                        break;

                    case "youtube#channel":

                        videoSearchInfos.Add(new VideoSearchInfo
                        {
                            Title = searchResult.Snippet.Title,
                            ChannelId = searchResult.Id.ChannelId,
                            ThumbnailUrl = searchResult.Snippet.Thumbnails.High.Url,
                            Description = searchResult.Snippet.Description,
                            isChannel = true,
                            isPlaylist = false
                        });
                        //channels.Add(String.Format("ChannelTitle: {0} ChannelId: ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
                        break;

                    case "youtube#playlist":
                        videoSearchInfos.Add(new VideoSearchInfo
                        {
                            Title = searchResult.Snippet.Title,
                            PlaylistId = searchResult.Id.PlaylistId,
                            ThumbnailUrl = searchResult.Snippet.Thumbnails.High.Url,
                            Description = searchResult.Snippet.Description,
                            isChannel = false,
                            isPlaylist = true
                        });
                        //playlists.Add(String.Format("PlaylistTitle: {0}  PlaylistId: ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
                        break;
                }
            }

            //System.Diagnostics.Debug.WriteLine(String.Format("Videos:\n{0}\n", string.Join("\n", videos)));
            //System.Diagnostics.Debug.WriteLine(String.Format("Channels:\n{0}\n", string.Join("\n", channels)));
            //System.Diagnostics.Debug.WriteLine(String.Format("Playlists:\n{0}\n", string.Join("\n", playlists)));


            return videoSearchInfos;
        }
    }
}
