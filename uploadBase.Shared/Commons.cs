﻿using MediatR;
using uploadBase.Shared.ErrorOr;
using uploadBase.Shared.Models;

namespace uploadBase.Shared
{

    public class Interfaces
    {
        //this interface is for mediatr , which require connectionid provided during subscription of signalr event

        //the detail can be found in vtecore, WeatherForcastHandler
        //1. handler is inited by mediatr for handling the request
        //2. handler will be injected with gateway (channel) to deliver the message
        //3. gateway has list of observer, the signalr client send signal to hub and create subscriber with (id,method and client proxy) and using the gateway for subscription

        //not using here for HyD project
        //the type argument in the interface is data type to be requested
        //a concrete interface (non-generic) is also required for mediatr to work
        public interface IRqBase<T> : IRequest<ErrorOr<T>>
        {
            string ConnectionId { get; set; }
        }
        
        public interface IResultGateway<T> : IObservable<KeyValuePair<string, T>>
        {
            Task OnDeliverResultAsync(KeyValuePair<string, T> result);
        }

        public interface ILanguageService
        {
            public string LanguageId { get; }
        }
        public interface IBelongtoTable
        {
            string tablename { get; set; }
        }

        public interface IFileService
        {
            string CreatePathFor(string type, string filename, bool inupload = false);
            string GenerateWordWithData(string xmldata, string templatename, string type = null);
            Task<string> DownloadFilesAsync(Stream fileStream, string type, string filename, bool inupload = false);
            Task<FileUploadSummary> UploadFileAsync(Stream fileStream, string contentType, string type);
        }

        public interface IAuthResult
        {
            string UserName { get; }
            string Email { get; }
            string UserId { get; }
        }
    }

    public class Constants
    {
        public enum PathType
        {
            Share,
            Upload,
            Stream,
            Template
        }

        //describe the field using 
        public enum FieldType
        {

        }

        public static class Op
        {

            public const string equal = "eq";
            public const string greaterThanOrEqual = "gte";
            public const string lessThanOrEqual = "lte";
            public const string lessThan = "lt";
            public const string itLikes = "ct";
            public const string Between = "bw";
            public const string BeginWith = "bt";
            public const string EndWith = "et";
            public const string Within = "in";
            public const string CheckListIn = "at";
        }
        public enum QueryOpType
        {
            Equal,
            StartsWith,
            EndsWith,
            ContainsWith,
            LikesWith,
            NotEq,
            GreaterOrEq,
            LessOrEq,
            Less,
            InListOp,
            OrderBy,
            ThenBy,
        }

        public static class Setting
        {
            public const string PathSetting = nameof(PathSetting);
            public const string AuthSetting = nameof(AuthSetting);
            public const string TemplateSetting = nameof(TemplateSetting);
            public const string CorsPolicySetting = nameof(CorsPolicySetting);
            public const int JWTExpirationInMins = 12 * 30 * 2;

        }

    }
}
