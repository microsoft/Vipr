// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.OData.Client;
using Microsoft.OData.Core;

namespace Microsoft.OData.ProxyExtensions
{
    public partial class DataServiceContextWrapper : DataServiceContext
    {
        private readonly Func<Task<string>> _accessTokenGetter;
        private readonly HashSet<EntityBase> _modifiedEntities = new HashSet<EntityBase>();

        public void UpdateObject(EntityBase entity)
        {
            if (GetEntityDescriptor(entity) != null)
            {
                _modifiedEntities.Add(entity);
                base.UpdateObject(entity);
            }
        }

        public DataServiceContextWrapper(Uri serviceRoot, ODataProtocolVersion maxProtocolVersion, Func<Task<string>> accessTokenGetter)
            : base(serviceRoot, maxProtocolVersion)
        {
            _accessTokenGetter = accessTokenGetter;

            IgnoreMissingProperties = true;

            BuildingRequest += (sender, args) => args.Headers.Add("Authorization", "Bearer " + _accessTokenGetter().Result);

            Configurations.RequestPipeline.OnEntryStarting(args =>
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

            Configurations.ResponsePipeline.OnEntityMaterialized(args =>
            {
                var entity = (EntityBase)args.Entity;

                entity.ResetChanges();
            });

            OnCreated();
        }

        partial void OnCreated();

        public Type DefaultResolveTypeInternal(string typeName, string fullNamespace, string languageDependentNamespace)
        {
            return DefaultResolveType(typeName, fullNamespace, languageDependentNamespace);
        }

        public string DefaultResolveNameInternal(Type clientType, string fullNamespace, string languageDependentNamespace)
        {
            if (clientType.Namespace.Equals(languageDependentNamespace, StringComparison.Ordinal))
            {
                return string.Concat(fullNamespace, ".", clientType.Name);
            }

            return string.Empty;
        }

        public async Task<TInterface> ExecuteSingleAsync<TSource, TInterface>(DataServiceQuery<TSource> inner)
        {
            try
            {
                return await Task.Factory.FromAsync(
                    inner.BeginExecute,
                    i => inner.EndExecute(i).Cast<TInterface>().SingleOrDefault(),
                    TaskCreationOptions.None);
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

        public async Task<IBatchElementResult[]> ExecuteBatchAsync(params IReadOnlyQueryableSetBase[] queries)
        {
            try
            {
                var requests = (from i in queries select (DataServiceRequest)i.Query).ToArray();

                var responses = await Task.Factory.FromAsync<DataServiceRequest[], DataServiceResponse>(
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
                    var pcCreator = pcTypeInfo.GetDeclaredMethod("Create");

                    // Handle an error response. 
                    // from http://msdn.microsoft.com/en-us/library/dd744838(v=vs.110).aspx
                    if (response.StatusCode > 299 || response.StatusCode < 200)
                    {
                        retVal[index] = new BatchElementResult(ProcessException(response.Error) ?? response.Error);
                    }
                    else
                    {
                        retVal[index] = new BatchElementResult((IPagedCollection)pcCreator.Invoke(null, new object[] { this, response }));
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

        public async Task<Stream> GetStreamAsync(Uri requestUriTmp, IDictionary<string, string> headers)
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

        public async Task<DataServiceStreamResponse> GetReadStreamAsync(EntityBase entity, string streamName, string contentType)
        {
            try
            {
                if (!string.IsNullOrEmpty(streamName))
                {
                    var resp = await Task.Factory.FromAsync<object, string, DataServiceRequestArgs, DataServiceStreamResponse>(
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
                    var resp = await Task.Factory.FromAsync<object, DataServiceRequestArgs, DataServiceStreamResponse>(
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

        public async Task<IPagedCollection<TInterface>> ExecuteAsync<TSource, TInterface>(DataServiceQuery<TSource> inner) where TSource : TInterface
        {
            try
            {
                return await Task.Factory.FromAsync(
                    inner.BeginExecute,
                    new Func<IAsyncResult, IPagedCollection<TInterface>>(
                        r =>
                            new PagedCollection<TInterface, TSource>(
                                this,
                                (QueryOperationResponse<TSource>) inner.EndExecute(r))),
                    TaskCreationOptions.None);
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

        public new Task<IEnumerable<T>> ExecuteAsync<T>(Uri uri, string httpMethod, bool singleResult, params OperationParameter[] operationParameters)
        {
            return ExecuteAsyncInternal<T>(uri, httpMethod, singleResult, null, operationParameters);
        }

        public Task<IEnumerable<T>> ExecuteAsync<T>(Uri uri, string httpMethod, bool singleResult, Stream stream, params OperationParameter[] operationParameters)
        {
            return ExecuteAsyncInternal<T>(uri, httpMethod, singleResult, stream ?? new MemoryStream(), operationParameters);
        }

        public async Task<IEnumerable<T>> ExecuteAsyncInternal<T>(Uri uri, string httpMethod, bool singleResult, Stream stream, params OperationParameter[] operationParameters)
        {
            try
            {
                if (stream != null)
                {
                    Configurations.RequestPipeline.OnMessageCreating = args =>
                    {
                        args.Headers.Remove("Content-Length");

                        var msg = new HttpWebRequestMessage(args);

                        Task.Factory.FromAsync<Stream>(msg.BeginGetRequestStream, msg.EndGetRequestStream, null)
                            .ContinueWith(s => stream.CopyTo(s.Result))
                            .Wait();

                        return msg;
                    };
                }

                return await Task.Factory.FromAsync<IEnumerable<T>>(
                    (callback, state) =>
                        BeginExecute<T>(uri, callback, state, httpMethod, singleResult, operationParameters),
                    EndExecute<T>,
                    TaskCreationOptions.None);
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

        public new Task ExecuteAsync(Uri uri, string httpMethod, params OperationParameter[] operationParameters)
        {
            return ExecuteAsync(uri, httpMethod, null, operationParameters);
        }

        public async Task ExecuteAsync(Uri uri, string httpMethod, Stream stream, params OperationParameter[] operationParameters)
        {
            try
            {
                if (stream != null)
                {
                    Configurations.RequestPipeline.OnMessageCreating = args =>
                    {
                        args.Headers.Remove("Content-Length");

                        var msg = new HttpWebRequestMessage(args);

                        Task.Factory.FromAsync<Stream>(msg.BeginGetRequestStream, msg.EndGetRequestStream, null)
                            .ContinueWith(s => stream.CopyTo(s.Result))
                            .Wait();

                        return msg;
                    };
                }

                await Task.Factory.FromAsync(
                    (callback, state) => BeginExecute(uri, callback, state, httpMethod, operationParameters),
                    new Action<IAsyncResult>(i => EndExecute(i)),
                    TaskCreationOptions.None);
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

        public async Task<QueryOperationResponse<TSource>> ExecuteAsync<TSource, TInterface>(DataServiceQueryContinuation<TSource> token)
        {
            try
            {
                return await Task.Factory.FromAsync(
                    (callback, state) => BeginExecute(token, callback, state),
                    i => (QueryOperationResponse<TSource>)EndExecute<TSource>(i),
                    TaskCreationOptions.None);
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

        public async new Task<DataServiceResponse> SaveChangesAsync(SaveChangesOptions options)
        {
            try
            {
                var result = await Task.Factory.FromAsync(
                    BeginSaveChanges,
                    new Func<IAsyncResult, DataServiceResponse>(EndSaveChanges),
                    options,
                    null,
                    TaskCreationOptions.None);

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

        public new Task<DataServiceResponse> SaveChangesAsync()
        {
            return SaveChangesAsync(SaveChangesOptions.None);
        }

#if NOTYET
        public async Task<IPagedCollection<TInterface>> LoadPropertyAsync<TSource, TInterface>(string path, object entity) where TSource : TInterface
        {
            try
            {
                return await Task.Factory.FromAsync(
                    (callback, state) => BeginLoadProperty(entity, path, callback, state),
                    new Func<IAsyncResult, IPagedCollection<TInterface>>(
                        r => new PagedCollection<TInterface, TSource>(this, (QueryOperationResponse<TSource>) EndLoadProperty(r))),
                    TaskCreationOptions.None);
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
            var dataServiceRequestException = ex as DataServiceRequestException;
            if (dataServiceRequestException != null)
            {
                var response = dataServiceRequestException.Response.FirstOrDefault();

                if (response != null)
                {
                    return ProcessError(dataServiceRequestException, dataServiceRequestException.InnerException as DataServiceClientException, response.Headers);
                }
            }

            var dataServiceQueryException = ex as DataServiceQueryException;
            if (dataServiceQueryException != null)
            {
                return ProcessError(dataServiceQueryException, dataServiceQueryException.InnerException as DataServiceClientException,
                    dataServiceQueryException.Response.Headers);
            }

            var dataServiceClientException = ex as DataServiceClientException;
            if (dataServiceClientException != null)
            {
                return ProcessError(dataServiceClientException, dataServiceClientException,
                    new Dictionary<string, string>
                    {
                        {"Content-Type", dataServiceClientException.Message.StartsWith("<") ? "application/xml" : "application/json"}
                    });
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

        private static async Task<Exception> ProcessErrorAsync(System.Net.Http.HttpResponseMessage response)
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
                var headers = response.Content.Headers.ToDictionary(i => i.Key, i => i.Value.FirstOrDefault());

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

        private static StreamWriter WriteToStream(string contents)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(contents);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return writer;
        }
    }
}
