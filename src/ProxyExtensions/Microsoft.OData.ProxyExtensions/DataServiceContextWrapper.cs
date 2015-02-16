using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.OData.Client;
using Microsoft.OData.Core;

namespace Microsoft.OData.ProxyExtensions
{
    public partial class DataServiceContextWrapper : DataServiceContext
    {
        private object _syncLock = new object();
        private string _accessToken;
        private global::System.Func<global::System.Threading.Tasks.Task<string>> _accessTokenGetter;
        private System.Func<System.Threading.Tasks.Task> _accessTokenSetter;
        private HashSet<EntityBase> _modifiedEntities = new HashSet<EntityBase>();

        public void UpdateObject(EntityBase entity)
        {
            if (GetEntityDescriptor(entity) != null)
            {
                _modifiedEntities.Add(entity);
                base.UpdateObject(entity);
            }
        }

        private async global::System.Threading.Tasks.Task SetToken()
        {
            var token = await _accessTokenGetter();
            lock (_syncLock)
            {
                _accessToken = token;
            }
        }

        public DataServiceContextWrapper(Uri serviceRoot, global::Microsoft.OData.Client.ODataProtocolVersion maxProtocolVersion, global::System.Func<global::System.Threading.Tasks.Task<string>> accessTokenGetter)
            : base(serviceRoot, maxProtocolVersion)
        {
            _accessTokenGetter = accessTokenGetter;
            _accessTokenSetter = SetToken;

            IgnoreMissingProperties = true;

            BuildingRequest += (sender, args) =>
            {
                args.Headers.Add("Authorization", "Bearer " + _accessToken);
            };

            Configurations.RequestPipeline.OnEntryStarting((args) =>
            {
                var entity = (EntityBase)args.Entity;

                if ((!entity.ChangedProperties.IsValueCreated || entity.ChangedProperties.Value.Count == 0))
                {
                    args.Entry.Properties = new ODataProperty[0];
                    return;
                }

                if (!_modifiedEntities.Contains(entity))
                {
                    _modifiedEntities.Add(entity);
                }

                IEnumerable<ODataProperty> properties = new ODataProperty[0];

                if (entity.ChangedProperties.IsValueCreated)
                {
                    properties = properties.Concat(args.Entry.Properties.Where(i => entity.ChangedProperties.Value.Contains(i.Name)));
                }

                args.Entry.Properties = properties;
            });

            Configurations.ResponsePipeline.OnEntityMaterialized((args) =>
            {
                var entity = (EntityBase)args.Entity;

                entity.ResetChanges();
            });

            OnCreated();
        }

        partial void OnCreated();

        public System.Type DefaultResolveTypeInternal(string typeName, string fullNamespace, string languageDependentNamespace)
        {
            return DefaultResolveType(typeName, fullNamespace, languageDependentNamespace);
        }

        public string DefaultResolveNameInternal(global::System.Type clientType, string fullNamespace, string languageDependentNamespace)
        {
            if (clientType.Namespace.Equals(languageDependentNamespace, global::System.StringComparison.Ordinal))
            {
                return string.Concat(fullNamespace, ".", clientType.Name);
            }

            return string.Empty;
        }

        public async System.Threading.Tasks.Task<TInterface> ExecuteSingleAsync<TSource, TInterface>(DataServiceQuery<TSource> inner)
        {
            try
            {
                await SetToken();
                return await global::System.Threading.Tasks.Task.Factory.FromAsync<TInterface>(
                    inner.BeginExecute,
                    new global::System.Func<global::System.IAsyncResult, TInterface>(i =>
                        global::System.Linq.Enumerable.SingleOrDefault(
                            global::System.Linq.Enumerable.Cast<TInterface>(inner.EndExecute(i)))),
                    global::System.Threading.Tasks.TaskCreationOptions.None);
            }
            catch (Exception ex)
            {
                var newException = ProcessException(ex);

                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
        }

        public async System.Threading.Tasks.Task<IBatchElementResult[]> ExecuteBatchAsync(params IReadOnlyQueryableSetBase[] queries)
        {
            try
            {
                var requests = (from i in queries select (DataServiceRequest)i.Query).ToArray();

                await SetToken();

                var responses = await global::System.Threading.Tasks.Task.Factory.FromAsync<DataServiceRequest[], DataServiceResponse>(
                    (q, callback, state) => BeginExecuteBatch(callback, state, q), // need to reorder parameters
                    EndExecuteBatch,
                    requests,
                    null);

                var retVal = new IBatchElementResult[queries.Length];

                var index = 0;
                foreach (var response in responses)
                {
                    Type tConcrete = ((IConcreteTypeAccessor)queries[index]).ConcreteType;
                    Type tInterface = ((IConcreteTypeAccessor)queries[index]).ElementType;

                    var pcType = typeof(PagedCollection<,>).MakeGenericType(tInterface, tConcrete);
                    var pcTypeInfo = pcType.GetTypeInfo();
                    var PCCreator = pcTypeInfo.GetDeclaredMethod("Create");

                    // Handle an error response. 
                    // from http://msdn.microsoft.com/en-us/library/dd744838(v=vs.110).aspx
                    if (response.StatusCode > 299 || response.StatusCode < 200)
                    {
                        retVal[index] = new BatchElementResult(ProcessException(response.Error) ?? response.Error);
                    }
                    else
                    {
                        retVal[index] = new BatchElementResult((IPagedCollection)PCCreator.Invoke(null, new object[] { this, response }));
                    }

                    index++;
                }

                return retVal;
            }
            catch (Exception ex)
            {
                var newException = ProcessException(ex);

                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
        }

        public async System.Threading.Tasks.Task<System.IO.Stream> GetStreamAsync(Uri requestUriTmp, IDictionary<string, string> headers)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                using (var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, requestUriTmp))
                {
                    request.Headers.Add("Authorization", "Bearer " + await _accessTokenGetter());
                    request.Headers.Add("Accept", "*/*");
                    request.Headers.Add("Accept-Charset", "UTF-8");
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            request.Headers.Add(header.Key, header.Value);
                        }
                    }


                    // Do not dispose the response. If disposed, it will also dispose the
                    // stream we are returning
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStreamAsync();
                    }

                    var newException = await ProcessErrorAsync(response);

                    if (newException != null)
                    {
                        throw newException;
                    }

                    response.EnsureSuccessStatusCode();

                    // unreachable
                    return null;
                }
            }
        }

        public async System.Threading.Tasks.Task<DataServiceStreamResponse> GetReadStreamAsync(EntityBase entity, string streamName, string contentType)
        {
            try
            {
                await SetToken();

                if (!string.IsNullOrEmpty(streamName))
                {
                    var resp = await global::System.Threading.Tasks.Task.Factory.FromAsync<object, string, DataServiceRequestArgs, DataServiceStreamResponse>(
                        BeginGetReadStream,
                        EndGetReadStream,
                        entity,
                        streamName,
                        new DataServiceRequestArgs { ContentType = contentType /*, Headers = {todo}*/ },
                        null);
                    return resp;
                }
                else
                {
                    var resp = await global::System.Threading.Tasks.Task.Factory.FromAsync<object, DataServiceRequestArgs, DataServiceStreamResponse>(
                        BeginGetReadStream,
                        EndGetReadStream,
                        entity,
                        new DataServiceRequestArgs { ContentType = contentType /*, Headers = {todo}*/ },
                        null);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var newException = ProcessException(ex);

                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
        }

        public async System.Threading.Tasks.Task<IPagedCollection<TInterface>> ExecuteAsync<TSource, TInterface>(DataServiceQuery<TSource> inner) where TSource : TInterface
        {
            try
            {
                await SetToken();
                return await global::System.Threading.Tasks.Task.Factory.FromAsync<
                    IPagedCollection<TInterface>>(inner.BeginExecute,
                        new global::System.Func<global::System.IAsyncResult, IPagedCollection<TInterface>>(
                            r =>
                            {
                                var innerResult = (QueryOperationResponse<TSource>)inner.EndExecute(r);


                                return new PagedCollection<TInterface, TSource>(this, innerResult);
                            }
                            ), global::System.Threading.Tasks.TaskCreationOptions.None);
            }
            catch (Exception ex)
            {
                var newException = ProcessException(ex);

                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
        }

        public new global::System.Threading.Tasks.Task<global::System.Collections.Generic.IEnumerable<T>> ExecuteAsync<T>(
            global::System.Uri uri,
            string httpMethod,
            bool singleResult,
            params OperationParameter[] operationParameters)
        {
            return ExecuteAsyncInternal<T>(uri, httpMethod, singleResult, (System.IO.Stream)null, operationParameters);
        }

        public global::System.Threading.Tasks.Task<global::System.Collections.Generic.IEnumerable<T>> ExecuteAsync<T>(
            global::System.Uri uri,
            string httpMethod,
            bool singleResult,
            System.IO.Stream stream,
            params OperationParameter[] operationParameters)
        {
            return ExecuteAsyncInternal<T>(uri, httpMethod, singleResult, stream ?? new System.IO.MemoryStream(), operationParameters);
        }

        public async global::System.Threading.Tasks.Task<global::System.Collections.Generic.IEnumerable<T>> ExecuteAsyncInternal<T>(
            global::System.Uri uri,
            string httpMethod,
            bool singleResult,
            System.IO.Stream stream,
            params OperationParameter[] operationParameters)
        {
            try
            {
                await SetToken();

                if (stream != null)
                {
                    Configurations.RequestPipeline.OnMessageCreating = (global::Microsoft.OData.Client.DataServiceClientRequestMessageArgs args) =>
                    {
                        args.Headers.Remove("Content-Length");

                        var msg = new global::Microsoft.OData.Client.HttpWebRequestMessage(args);

                        global::System.Threading.Tasks.Task.Factory.FromAsync<System.IO.Stream>(msg.BeginGetRequestStream, msg.EndGetRequestStream, null).ContinueWith
                            (s => stream.CopyTo(s.Result))
                            .Wait();

                        return msg;
                    };
                }

                return await global::System.Threading.Tasks.Task.Factory.FromAsync<global::System.Collections.Generic.IEnumerable<T>>
                    (
                        (callback, state) => BeginExecute<T>(uri, callback, state, httpMethod, singleResult, operationParameters),
                        EndExecute<T>, global::System.Threading.Tasks.TaskCreationOptions.None);
            }
            catch (Exception ex)
            {
                var newException = ProcessException(ex);

                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
            finally
            {
                if (stream != null)
                {
                    Configurations.RequestPipeline.OnMessageCreating = null;
                }
            }
        }

        public new global::System.Threading.Tasks.Task ExecuteAsync(
            global::System.Uri uri,
            string httpMethod,
            params OperationParameter[] operationParameters)
        {
            return ExecuteAsync(uri, httpMethod, (System.IO.Stream)null, operationParameters);
        }

        public async global::System.Threading.Tasks.Task ExecuteAsync(
            global::System.Uri uri,
            string httpMethod,
            System.IO.Stream stream,
            params OperationParameter[] operationParameters
            )
        {
            try
            {
                await SetToken();
                if (stream != null)
                {
                    Configurations.RequestPipeline.OnMessageCreating = (global::Microsoft.OData.Client.DataServiceClientRequestMessageArgs args) =>
                    {
                        args.Headers.Remove("Content-Length");

                        var msg = new global::Microsoft.OData.Client.HttpWebRequestMessage(args);

                        global::System.Threading.Tasks.Task.Factory.FromAsync<System.IO.Stream>(msg.BeginGetRequestStream, msg.EndGetRequestStream, null).ContinueWith
                            (s => stream.CopyTo(s.Result))
                            .Wait();

                        return msg;
                    };
                }

                await global::System.Threading.Tasks.Task.Factory.FromAsync
                    (
                        new global::System.Func<global::System.AsyncCallback, object, global::System.IAsyncResult>(
                            (callback, state) => BeginExecute(uri, callback, state, httpMethod, operationParameters)),
                        new global::System.Action<global::System.IAsyncResult>((i) => EndExecute(i)),
                        global::System.Threading.Tasks.TaskCreationOptions.None);
            }
            catch (Exception ex)
            {
                var newException = ProcessException(ex);

                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
            finally
            {
                if (stream != null)
                {
                    Configurations.RequestPipeline.OnMessageCreating = null;
                }
            }
        }

        public async System.Threading.Tasks.Task<QueryOperationResponse<TSource>> ExecuteAsync<TSource, TInterface>(DataServiceQueryContinuation<TSource> token)
        {
            try
            {
                await SetToken();

                return await global::System.Threading.Tasks.Task.Factory.FromAsync<QueryOperationResponse<TSource>>(
                    (callback, state) => BeginExecute(token, callback, state),
                    (i) => (QueryOperationResponse<TSource>)EndExecute<TSource>(i),
                    global::System.Threading.Tasks.TaskCreationOptions.None);
            }
            catch (Exception ex)
            {
                var newException = ProcessException(ex);

                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
        }

        public async new System.Threading.Tasks.Task<DataServiceResponse> SaveChangesAsync(SaveChangesOptions options)
        {
            try
            {
                await SetToken();
                var result = await global::System.Threading.Tasks.Task.Factory.FromAsync<SaveChangesOptions, DataServiceResponse>(
                    BeginSaveChanges,
                    new global::System.Func<global::System.IAsyncResult, DataServiceResponse>(EndSaveChanges),
                    options,
                    null,
                    global::System.Threading.Tasks.TaskCreationOptions.None);

                foreach (var i in _modifiedEntities)
                {
                    i.ResetChanges();
                }

                _modifiedEntities.Clear();
                return result;
            }
            catch (Exception ex)
            {
                var newException = ProcessException(ex);

                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
        }

        public new System.Threading.Tasks.Task<DataServiceResponse> SaveChangesAsync()
        {
            return SaveChangesAsync(SaveChangesOptions.None);
        }

#if NOTYET
        public async System.Threading.Tasks.Task<IPagedCollection<TSource>> LoadPropertyAsync<TSource>(string path, object entity)
        {
            try
            {
                await SetToken();
                return await global::System.Threading.Tasks.Task.Factory.FromAsync<
                    IPagedCollection<TSource>>(
                    (AsyncCallback callback, object state) =>
                    {
                        return BeginLoadProperty(entity, path, callback, state);
                    },
                    new global::System.Func<global::System.IAsyncResult, IPagedCollection<TSource>>(
                        r =>
                        {
                            var innerResult = (QueryOperationResponse<TSource>)EndLoadProperty(r);

                            return new PagedCollection<TSource>(this, innerResult);
                        }
                        ), global::System.Threading.Tasks.TaskCreationOptions.None);
            }
            catch (Exception ex)
            {
                var newException = ProcessException(ex);

                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
        }
#endif

        public static Exception ProcessException(Exception ex)
        {
            if (ex is DataServiceRequestException)
            {
                var response = ((DataServiceRequestException)ex).Response.FirstOrDefault();

                if (response != null)
                {
                    return ProcessError((DataServiceRequestException)ex, ex.InnerException as DataServiceClientException, response.Headers);
                }
            }

            if (ex is DataServiceQueryException)
            {
                return ProcessError((DataServiceQueryException)ex, ex.InnerException as DataServiceClientException, ((DataServiceQueryException)ex).Response.Headers);
            }

            if (ex is DataServiceClientException)
            {
                return ProcessError(ex, (DataServiceClientException)ex, new Dictionary<string, string> { { "Content-Type", ex.Message.StartsWith("<") ? "application/xml" : "application/json" } });
            }

            return null;
        }


        private static Exception ProcessError(Exception outer, DataServiceClientException inner, IDictionary<string, string> headers)
        {
            if (inner == null)
            {
                return null;
            }

            using (var writer = WriteToStream(inner.Message))
            {
                var httpMessage = new HttpWebResponseMessage(
                    headers,
                    inner.StatusCode,
                    () => writer.BaseStream);

                var reader = new ODataMessageReader(httpMessage);

                try
                {
                    var error = reader.ReadError();
                    return new ODataErrorException(error.Message, outer, error);
                }
                catch
                {
                }
            }

            return null;
        }

        private static async System.Threading.Tasks.Task<Exception> ProcessErrorAsync(System.Net.Http.HttpResponseMessage response)
        {
            if (response.Content == null)
            {
                return null;
            }

            if (response.Content.Headers.ContentType == null)
            {
                return new System.Net.Http.HttpRequestException(await response.Content.ReadAsStringAsync());
            }

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var headers = Enumerable.ToDictionary(response.Content.Headers, i => i.Key, i => i.Value.FirstOrDefault());

                var httpMessage = new HttpWebResponseMessage(
                    headers,
                    (int)response.StatusCode,
                    () => stream);

                var reader = new ODataMessageReader(httpMessage);

                try
                {
                    var error = reader.ReadError();
                    return new ODataErrorException(error.Message, null, error);
                }
                catch
                {
                }
            }

            return null;
        }

        private static System.IO.StreamWriter WriteToStream(string contents)
        {
            var stream = new System.IO.MemoryStream();
            var writer = new System.IO.StreamWriter(stream);
            writer.Write(contents);
            writer.Flush();
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            return writer;
        }
    }
}