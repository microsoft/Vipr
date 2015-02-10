using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions
{
    public abstract class ReadOnlyQueryableSetBase
    {
        private DataServiceQuery _inner;
        private DataServiceContextWrapper _context;
        
        // Will return null if not an interface
        protected static Type CreateConcreteType(Type tsourceType)
        {
            var tsourceTypeInfo = System.Reflection.IntrospectionExtensions.GetTypeInfo(tsourceType);
            if (tsourceTypeInfo.IsGenericType)
            {
                var arguments = tsourceTypeInfo.GenericTypeArguments;
                bool modified = false;

                for (int i = 0; i < arguments.Length; i++)
                {
                    var converted = CreateConcreteType(arguments[i]);
                    if (converted != null)
                    {
                        arguments[i] = converted;
                        modified = true;
                    }
                }

                if (!modified)
                {
                    return null;
                }

                // Properties declared as IPagedCollection on the interface are declared as IList on the concrete type
                if (tsourceTypeInfo.GetGenericTypeDefinition() == typeof(IPagedCollection<>))
                {
                    return typeof(IList<>).MakeGenericType(arguments);
                }

                return tsourceTypeInfo.GetGenericTypeDefinition().MakeGenericType(arguments);
            }

            const string Fetcher = "Fetcher";
            if (System.Linq.Enumerable.Any<System.Reflection.CustomAttributeData>(
                tsourceTypeInfo.CustomAttributes,
                i => i.AttributeType == typeof(LowerCasePropertyAttribute)))
            {
                string typeName = tsourceTypeInfo.Namespace + "." + tsourceTypeInfo.Name.Substring(1);
                if (typeName.EndsWith(Fetcher))
                {
                    typeName = typeName.Substring(typeName.Length - Fetcher.Length);
                }
                return tsourceTypeInfo.Assembly.GetType(typeName);
            }
            else
            {
                return null;
            }
        }

        public DataServiceContextWrapper Context
        {
            get { return _context; }
        }

        public DataServiceQuery Query
        {
            get { return _inner; }
        }

        protected void Initialize(DataServiceQuery inner,
            DataServiceContextWrapper context)
        {
            _inner = inner;
            _context = context;
        }
    }

    public abstract class ReadOnlyQueryableSetBase<TSource> : ReadOnlyQueryableSetBase, IReadOnlyQueryableSetBase<TSource>, IConcreteTypeAccessor
    {
        private readonly Lazy<Type> _concreteType = new Lazy<Type>(() => CreateConcreteType(typeof(TSource)), true);

        protected ReadOnlyQueryableSetBase(
            DataServiceQuery inner,
            DataServiceContextWrapper context)
        {
            Initialize(inner, context);
        }

        #region IConcreteTypeAccessor implementation

        Type IConcreteTypeAccessor.ConcreteType
        {
            get
            {
                return _concreteType.Value ?? typeof(TSource);
            }
        }

        Type IConcreteTypeAccessor.ElementType
        {
            get
            {
                return typeof(TSource);
            }
        }

        #endregion

        protected global::System.Threading.Tasks.Task<IPagedCollection<TSource>> ExecuteAsyncInternal()
        {
            if (_concreteType.Value != null)
            {
                var contextTypeInfo = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(DataServiceContextWrapper));

                var executeAsyncMethodInfo =
                    (from i in contextTypeInfo.GetDeclaredMethods("ExecuteAsync")
                        let parameters = i.GetParameters()
                        where parameters.Length == 1 && parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(DataServiceQuery<>)
                        select i).First();

                return (global::System.Threading.Tasks.Task<IPagedCollection<TSource>>)
                    executeAsyncMethodInfo.MakeGenericMethod(_concreteType.Value, typeof(TSource)).Invoke(Context, new[] { Query });
            }
            else
            {
                return Context.ExecuteAsync<TSource, TSource>((DataServiceQuery<TSource>)Query);
            }
        }

        protected global::System.Threading.Tasks.Task<TSource> ExecuteSingleAsyncInternal()
        {
            if (_concreteType.Value != null)
            {
                var contextTypeInfo = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(DataServiceContextWrapper));

                var executeAsyncMethodInfo =
                    (from i in contextTypeInfo.GetDeclaredMethods("ExecuteSingleAsync")
                        let parameters = i.GetParameters()
                        where parameters.Length == 1 && parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(DataServiceQuery<>)
                        select i).First();

                return (global::System.Threading.Tasks.Task<TSource>)
                    executeAsyncMethodInfo.MakeGenericMethod(_concreteType.Value, typeof(TSource)).Invoke(Context, new[] { Query });
            }
            else
            {
                return Context.ExecuteSingleAsync<TSource, TSource>((DataServiceQuery<TSource>)Query);
            }
        }

        #region LINQ


        private class PascalCaseExpressionVisitor : System.Linq.Expressions.ExpressionVisitor
        {
            private Dictionary<System.Linq.Expressions.ParameterExpression, System.Linq.Expressions.ParameterExpression>
                _parameterDictionary = new Dictionary<System.Linq.Expressions.ParameterExpression, System.Linq.Expressions.ParameterExpression>();

            protected override System.Linq.Expressions.Expression VisitExtension(System.Linq.Expressions.Expression node)
            {
                return node;
            }

            protected override System.Linq.Expressions.Expression VisitLambda<T>(System.Linq.Expressions.Expression<T> node)
            {
                var originalDelegateType = typeof(T);

                if (originalDelegateType.GetGenericTypeDefinition() == typeof(Func<,>))
                {
                    var newParameterArray = System.Reflection.IntrospectionExtensions.GetTypeInfo(originalDelegateType).GenericTypeArguments;
                    bool hasInterfaces = false;

                    var ct = CreateConcreteType(newParameterArray[0]);
                    if (ct != null)
                    {
                        hasInterfaces = true;
                        newParameterArray[0] = ct;
                    }

                    ct = CreateConcreteType(newParameterArray[1]);
                    if (ct != null)
                    {
                        hasInterfaces = true;
                        newParameterArray[1] = ct;
                    }

                    if (!hasInterfaces)
                    {
                        return base.VisitLambda(node);
                    }

                    var newdDelegateType = typeof(Func<,>).MakeGenericType(newParameterArray);

                    var invocationParameters = node.Parameters.ToArray();

                    for (int i = 0; i < invocationParameters.Length; i++)
                    {
                        var concreteType = CreateConcreteType(invocationParameters[i].Type);

                        if (concreteType != null)
                        {
                            if (!_parameterDictionary.ContainsKey(invocationParameters[i]))
                            {
                                _parameterDictionary[invocationParameters[i]] = System.Linq.Expressions.Expression.Parameter(
                                    concreteType, invocationParameters[i].Name);
                            }

                            invocationParameters[i] = _parameterDictionary[invocationParameters[i]];
                        }
                    }

                    var body = Visit(node.Body);

                    var newLambda = System.Linq.Expressions.Expression.Lambda(
                        newdDelegateType,
                        body,
                        node.TailCall,
                        invocationParameters);

                    return newLambda;
                }

                return base.VisitLambda<T>(node);
            }

            protected override System.Linq.Expressions.Expression VisitParameter(System.Linq.Expressions.ParameterExpression node)
            {
                var concreteType = CreateConcreteType(node.Type);

                if (concreteType == null)
                {
                    return base.VisitParameter(node);
                }

                if (!_parameterDictionary.ContainsKey(node))
                {
                    _parameterDictionary[node] = System.Linq.Expressions.Expression.Parameter(
                        concreteType,
                        node.Name);
                }

                return base.VisitParameter(_parameterDictionary[node]);
            }

            protected override System.Linq.Expressions.Expression VisitMember(System.Linq.Expressions.MemberExpression node)
            {
                if (node.Member is System.Reflection.PropertyInfo)
                {
                    var interfaceType = CreateConcreteType(node.Type) != null;

                    var toLower = System.Linq.Enumerable.Any(
                        node.Member.CustomAttributes, i => i.AttributeType == typeof(LowerCasePropertyAttribute));

                    if (interfaceType || toLower)
                    {
                        var newExpression = Visit(node.Expression);

                        return base.VisitMember(
                            System.Linq.Expressions.Expression.Property(
                                newExpression,
                                System.Reflection.RuntimeReflectionExtensions.GetRuntimeProperty(
                                    newExpression.Type,
                                    toLower ? char.ToLower(node.Member.Name[0]) + node.Member.Name.Substring(1) : node.Member.Name
                                    )
                                )
                            );
                    }
                }
                    /*
                    Example - ""me"" is a field:

                    var me = await client.Me.ExecuteAsync();
            
                    var filesQuery = await client.Users.Where(i => i.UserPrincipalName != me.UserPrincipalName).ExecuteAsync();
                */
                else if (node.Member is System.Reflection.FieldInfo) // for local variables
                {
                    var fieldTypeInfo = System.Reflection.IntrospectionExtensions.GetTypeInfo(((System.Reflection.FieldInfo)node.Member).FieldType);
                    if (System.Linq.Enumerable.Any<System.Reflection.CustomAttributeData>(fieldTypeInfo.CustomAttributes, i => i.AttributeType == typeof(LowerCasePropertyAttribute)))
                    {
                        var expression = System.Linq.Expressions.Expression.TypeAs(node, CreateConcreteType(fieldTypeInfo.AsType()));
                        return expression;
                    }
                }

                return base.VisitMember(node);
            }
        }

        private System.Linq.Expressions.ExpressionVisitor _pascalCaseExpressionVisitor = new PascalCaseExpressionVisitor();

        public IReadOnlyQueryableSet<TResult> Select<TResult>(System.Linq.Expressions.Expression<System.Func<TSource, TResult>> selector)
        {
            var newSelector = _pascalCaseExpressionVisitor.Visit(selector);

            DataServiceQuery query = CallLinqMethod(newSelector);

            return new ReadOnlyQueryableSet<TResult>(
                query,
                Context);
        }

        public IReadOnlyQueryableSet<TSource> Where(System.Linq.Expressions.Expression<System.Func<TSource, bool>> predicate)
        {
            // Fix for DevDiv 941323:
            if (predicate.Body.NodeType == System.Linq.Expressions.ExpressionType.Coalesce)
            {
                var binary = (System.Linq.Expressions.BinaryExpression)predicate.Body;

                var constantRight = binary.Right as System.Linq.Expressions.ConstantExpression;

                // If we are coalescing bool to false, it is a no-op
                if (constantRight != null &&
                    constantRight.Value is bool &&
                    !(bool)constantRight.Value &&
                    binary.Left.Type == typeof(bool?) &&
                    binary.Left is System.Linq.Expressions.BinaryExpression)
                {
                    var oldLeft = (System.Linq.Expressions.BinaryExpression)binary.Left;

                    var newLeft = System.Linq.Expressions.Expression.MakeBinary(
                        oldLeft.NodeType,
                        oldLeft.Left,
                        oldLeft.Right);

                    predicate = (System.Linq.Expressions.Expression<System.Func<TSource, bool>>)System.Linq.Expressions.Expression.Lambda(
                        predicate.Type,
                        newLeft,
                        predicate.TailCall,
                        predicate.Parameters);
                }
            }

            var newSelector = _pascalCaseExpressionVisitor.Visit(predicate);

            DataServiceQuery query = CallLinqMethod(newSelector, true);

            return new ReadOnlyQueryableSet<TSource>(
                query,
                Context);
        }

        public IReadOnlyQueryableSet<TResult> OfType<TResult>()
        {
            DataServiceQuery query = ApplyLinq(new[] { typeof(TResult) }, new object[] { Query }) ??
                                     (DataServiceQuery)System.Linq.Queryable.OfType<TResult>((System.Linq.IQueryable<TSource>)Query);

            return new ReadOnlyQueryableSet<TResult>(
                query,
                Context);
        }

        public IReadOnlyQueryableSet<TSource> Skip(int count)
        {
            DataServiceQuery query = ApplyLinq(new[] { typeof(TSource) }, new object[] { Query, count }) ??
                                     (DataServiceQuery)System.Linq.Queryable.Skip<TSource>((System.Linq.IQueryable<TSource>)Query, count);

            return new ReadOnlyQueryableSet<TSource>(
                query,
                Context);
        }

        public IReadOnlyQueryableSet<TSource> Take(int count)
        {
            DataServiceQuery query = ApplyLinq(new[] { typeof(TSource) }, new object[] { Query, count }) ??
                                     (DataServiceQuery)System.Linq.Queryable.Take<TSource>((System.Linq.IQueryable<TSource>)Query, count);

            return new ReadOnlyQueryableSet<TSource>(
                query,
                Context);
        }

        public IReadOnlyQueryableSet<TSource> OrderBy<TKey>(System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector)
        {
            var newSelector = _pascalCaseExpressionVisitor.Visit(keySelector);

            DataServiceQuery query = CallLinqMethod(newSelector);

            return new ReadOnlyQueryableSet<TSource>(
                query,
                Context);
        }

        public IReadOnlyQueryableSet<TSource> OrderByDescending<TKey>(System.Linq.Expressions.Expression<System.Func<TSource, TKey>> keySelector)
        {
            var newSelector = _pascalCaseExpressionVisitor.Visit(keySelector);

            DataServiceQuery query = CallLinqMethod(newSelector);

            return new ReadOnlyQueryableSet<TSource>(
                query,
                Context);
        }

        public IReadOnlyQueryableSet<TSource> Expand<TTarget>(System.Linq.Expressions.Expression<Func<TSource, TTarget>> navigationPropertyAccessor)
        {
            var newSelector = _pascalCaseExpressionVisitor.Visit(navigationPropertyAccessor);

            var concreteType = _concreteType.Value ?? typeof(TSource);
            var concreteDsq = typeof(DataServiceQuery<>).MakeGenericType(concreteType);

            DataServiceQuery query = CallOnConcreteType(concreteDsq, Query, new[] { typeof(TTarget) }, new object[] { newSelector });

            return new ReadOnlyQueryableSet<TSource>(
                query,
                Context);
        }

        private DataServiceQuery ApplyLinq(Type[] typeParams, object[] callParams, [System.Runtime.CompilerServices.CallerMemberName] string methodName = null)
        {
            return CallOnConcreteType(typeof(System.Linq.Queryable), null, typeParams, callParams, methodName);
        }

        private DataServiceQuery CallOnConcreteType(Type targetType, object instance, Type[] typeParams, object[] callParams, [System.Runtime.CompilerServices.CallerMemberName] string methodName = null)
        {
            for (int i = 0; i < typeParams.Length; i++)
            {
                if (typeParams[i] == typeof(TSource))
                {
                    typeParams[i] = _concreteType.Value;
                }
                else
                {
                    var concreteType = CreateConcreteType(typeParams[i]);

                    if (concreteType != null)
                    {
                        typeParams[i] = concreteType;
                    }
                }
            }

            var typeInfo = System.Reflection.IntrospectionExtensions.GetTypeInfo(targetType);
            var methodInfo =
                (from i in typeInfo.GetDeclaredMethods(methodName)
                    let parameters = i.GetParameters()
                    where i.GetGenericArguments().Length == typeParams.Length
                    let constructedMethod = i.MakeGenericMethod(typeParams)
                    where AllParametersAreAssignable(constructedMethod.GetParameters(), callParams)
                    select constructedMethod).First();

            return (DataServiceQuery)methodInfo.Invoke(instance, callParams);
        }

        private bool AllParametersAreAssignable(System.Reflection.ParameterInfo[] parameterInfo, object[] callParams)
        {
            for (int i = 0; i < parameterInfo.Length; i++)
            {
                if (callParams[i] != null &&
                    !System.Reflection.IntrospectionExtensions.GetTypeInfo(parameterInfo[i].ParameterType).IsAssignableFrom(
                        System.Reflection.IntrospectionExtensions.GetTypeInfo(callParams[i].GetType())))
                {
                    return false;
                }
            }

            return true;
        }

        private DataServiceQuery CallLinqMethod(
            System.Linq.Expressions.Expression predicate,
            bool singleGenericParameter = false,
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = null)
        {
            System.Type[] typeParams = singleGenericParameter ?
                new Type[] { predicate.Type.GenericTypeArguments[0] } :
                predicate.Type.GenericTypeArguments;

            var callParams = new object[] { Query, predicate };

            var typeInfo = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(System.Linq.Queryable));
            var methodInfo =
                (from i in typeInfo.GetDeclaredMethods(methodName)
                    let parameters = i.GetParameters()
                    where i.GetGenericArguments().Length == typeParams.Length
                    let constructedMethod = i.MakeGenericMethod(typeParams)
                    where AllParametersAreAssignable(constructedMethod.GetParameters(), callParams)
                    select constructedMethod).First();

            return (DataServiceQuery)methodInfo.Invoke(null, callParams);
        }
        #endregion
    }
}