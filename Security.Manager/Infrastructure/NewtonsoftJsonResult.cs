using System;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Security.Manager.Infrastructure
{
    public class NewtonsoftJsonResult: ActionResult
    {
        private readonly object _data;
        private readonly JsonRequestBehavior _behavior;
        private readonly TypeNameHandling _secTypeNameHandling;
        private readonly PreserveReferencesHandling _secPreserveReferencesHandling;

        #region .ctor

        public NewtonsoftJsonResult(object data, JsonRequestBehavior behavior) 
            : this(data, behavior, TypeNameHandling.None)
        {
        }

        public NewtonsoftJsonResult(object data, JsonRequestBehavior behavior, TypeNameHandling secTypeNameHandling) 
            : this(data, behavior, secTypeNameHandling, PreserveReferencesHandling.None)
        {
        }

        public NewtonsoftJsonResult(object data, JsonRequestBehavior behavior, PreserveReferencesHandling secPreserveReferencesHandling) 
            : this(data, behavior, TypeNameHandling.None, secPreserveReferencesHandling)
        {
        }

        public NewtonsoftJsonResult(object data, JsonRequestBehavior behavior, 
            TypeNameHandling secTypeNameHandling, PreserveReferencesHandling secPreserveReferencesHandling)
        {
            _data = data;
            _behavior = behavior;
            _secTypeNameHandling = secTypeNameHandling;
            _secPreserveReferencesHandling = secPreserveReferencesHandling;
        }

        #endregion

        public override void ExecuteResult(ControllerContext context)
        {
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = _secTypeNameHandling;
            settings.PreserveReferencesHandling = _secPreserveReferencesHandling;

            if (_behavior == JsonRequestBehavior.DenyGet && context.HttpContext.Request.HttpMethod.ToLower() == "get")
                throw new InvalidOperationException("Запрос GET не разрешен");

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.Write(JsonConvert.SerializeObject(_data, settings));
        }
    }
}