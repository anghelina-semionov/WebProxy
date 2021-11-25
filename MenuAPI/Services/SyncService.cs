using Common.Models;
using Common.Utilities;
using MenuAPI.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MenuAPI.Services
{
    public class SyncService<T> : ISyncService<T> where T : MongoDocument
    {
        private readonly ISyncServiceSettings _settings;
        private readonly string _origin;
        public SyncService(ISyncServiceSettings settings,IHttpContextAccessor httpContext)
        {
            _settings = settings;
            _origin = httpContext.HttpContext.Request.Host.ToString();


        }
        public HttpResponseMessage Delete(T item)
        {
            var syncType = _settings.DeleteHttpMethod;
            var json = ToSyncEntityJson(item, syncType);

           var response = HttpClientUtility.SendJson(json, _settings.Host, "POST");

           return response;

        }

        public HttpResponseMessage Upsert(T item)
        {
            var syncType = _settings.UpsertHttpMethod;
            var json = ToSyncEntityJson(item, syncType);

            var response = HttpClientUtility.SendJson(json, _settings.Host, "POST");

            return response;
        }


        private string ToSyncEntityJson(T item,string syncType)
        {

            var objectType = typeof(T);

            var syncEntity = new SyncEntity()
            {
                JsonData = JsonSerializer.Serialize(item),
                SyncType = syncType,
                ObjectType = objectType.Name,
                Id = item.Id,
                LastChangedAt = item.LastChangedAt,
                Origin = _origin
            };
            var json = JsonSerializer.Serialize(syncEntity);
            return json;

        }

    }
}
