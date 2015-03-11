// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
            var tsourceTypeInfo = tsourceType.GetTypeInfo();
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

            const string fetcher = "Fetcher";
            if (tsourceTypeInfo.CustomAttributes.Any(i => i.AttributeType == typeof(LowerCasePropertyAttribute)))
            {
                string typeName = tsourceTypeInfo.Namespace + "." + tsourceTypeInfo.Name.Substring(1);
                if (typeName.EndsWith(fetcher))
                {
                    typeName = typeName.Substring(typeName.Length - fetcher.Length);
                }

                return tsourceTypeInfo.Assembly.GetType(typeName);
            }
            
            return null;
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

        protected Task<IPagedCollection<TSource>> ExecuteAsyncInternal()
        {
            if (_concreteType.Value != null)
            {
                var contextTypeInfo = typeof(DataServiceContextWrapper).GetTypeInfo();

                var executeAsyncMethodInfo =
                    (from i in contextTypeInfo.GetDeclaredMethods("ExecuteAsync")
                        let parameters = i.GetParameters()
                        where parameters.Length == 1 && parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(DataServiceQuery<>)
                        select i).First();

                return (Task<IPagedCollection<TSource>>)executeAsyncMethodInfo.MakeGenericMethod(_concreteType.Value, typeof(TSource)).Invoke(Context, new object[] { Query });
            }
            
            return Context.ExecuteAsync<TSource, TSource>((DataServiceQuery<TSource>)Query);
        }

        protected Task<TSource> ExecuteSingleAsyncInternal()
        {
            if (_concreteType.Value != null)
            {
                var contextTypeInfo = typeof(DataServiceContextWrapper).GetTypeInfo();

                var executeAsyncMethodInfo =
                    (from i in contextTypeInfo.GetDeclaredMethods("ExecuteSingleAsync")
                        let parameters = i.GetParameters()
                        where parameters.Length == 1 && parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(DataServiceQuery<>)
                        select i).First();

                return (Task<TSource>)executeAsyncMethodInfo.MakeGenericMethod(_concreteType.Value, typeof(TSource)).Invoke(Context, new object[] { Query });
            }
            
            return Context.ExecuteSingleAsync<TSource, TSource>((DataServiceQuery<TSource>)Query);
        }

        #region LINQ


        private class PascalCaseExpressionVisitor : ExpressionVisitor
        {
            private readonly Dictionary<ParameterExpression, ParameterExpression>
                _parameterDictionary = new Dictionary<ParameterExpression, ParameterExpression>();

            protected override Expression VisitExtension(Expression node)
            {
                return node;
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                var originalDelegateType = typeof(T);

                if (originalDelegateType.GetGenericTypeDefinition() == typeof(Func<,>))
                {
                    var newParameterArray = originalDelegateType.GetTypeInfo().GenericTypeArguments;
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
                                _parameterDictionary[invocationParameters[i]] = Expression.Parameter(
                                    concreteType, invocationParameters[i].Name);
                            }

                            invocationParameters[i] = _parameterDictionary[invocationParameters[i]];
                        }
                    }

                    var body = Visit(node.Body);

                    var newLambda = Expression.Lambda(
                        newdDelegateType,
                        body,
                        node.TailCall,
                        invocationParameters);

                    return newLambda;
                }

                return base.VisitLambda(node);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                var concreteType = CreateConcreteType(node.Type);

                if (concreteType == null)
                {
                    return base.VisitParameter(node);
                }

                if (!_parameterDictionary.ContainsKey(node))
                {
                    _parameterDictionary[node] = Expression.Parameter(
                        concreteType,
                        node.Name);
                }

                return base.VisitParameter(_parameterDictionary[node]);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Member is PropertyInfo)
                {
                    var interfaceType = CreateConcreteType(node.Type) != null;

                    var toLower = node.Member.CustomAttributes.Any(i => i.AttributeType == typeof(LowerCasePropertyAttribute));

                    if (interfaceType || toLower)
                    {
                        var newExpression = Visit(node.Expression);

                        return base.VisitMember(Expression.Property(
                            newExpression,
                            newExpression.Type.GetRuntimeProperty(toLower
                                ? char.ToLower(node.Member.Name[0]) + node.Member.Name.Substring(1)
                                : node.Member.Name)
                            ));
                    }
                }
                    /*
                    Example - ""me"" is a field:

                    var me = await client.Me.ExecuteAsync();
            
                    var filesQuery = await client.Users.Where(i => i.UserPrincipalName != me.UserPrincipalName).ExecuteAsync();
                */
                else
                {
                    var fieldInfo = node.Member as FieldInfo;
                    if (fieldInfo != null) // for local variables
                    {
                        var fieldTypeInfo = fieldInfo.FieldType.GetTypeInfo();
                        if (fieldTypeInfo.CustomAttributes.Any(i => i.AttributeType == typeof(LowerCasePropertyAttribute)))
                        {
                            var expression = Expression.TypeAs(node, CreateConcreteType(fieldTypeInfo.AsType()));
                            return expression;
                        }
                    }
                }

                return base.VisitMember(node);
            }
        }

        private readonly ExpressionVisitor _pascalCaseExpressionVisitor = new PascalCaseExpressionVisitor();

        public IReadOnlyQueryableSet<TResult> Select<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            var newSelector = _pascalCaseExpressionVisitor.Visit(selector);

            DataServiceQuery query = CallLinqMethod(newSelector);

            return new ReadOnlyQueryableSet<TResult>(
                query,
                Context);
        }

        public IReadOnlyQueryableSet<TSource> Where(Expression<Func<TSource, bool>> predicate)
        {
            // Fix for DevDiv 941323:
            if (predicate.Body.NodeType == ExpressionType.Coalesce)
            {
                var binary = (BinaryExpression)predicate.Body;

                var constantRight = binary.Right as ConstantExpression;

                // If we are coalescing bool to false, it is a no-op
                if (constantRight != null &&
                    constantRight.Value is bool &&
                    !(bool)constantRight.Value &&
                    binary.Left.Type == typeof(bool?) &&
                    binary.Left is BinaryExpression)
                {
                    var oldLeft = (BinaryExpression)binary.Left;

                    var newLeft = Expression.MakeBinary(
                        oldLeft.NodeType,
                        oldLeft.Left,
                        oldLeft.Right);

                    predicate = (Expression<Func<TSource, bool>>)Expression.Lambda(
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
                                     (DataServiceQuery)((IQueryable<TSource>)Query).OfType<TResult>();

            return new ReadOnlyQueryableSet<TResult>(
                query,
                Context);
        }

        public IReadOnlyQueryableSet<TSource> Skip(int count)
        {
            DataServiceQuery query = ApplyLinq(new[] { typeof(TSource) }, new object[] { Query, count }) ??
                                     (DataServiceQuery)((IQueryable<TSource>)Query).Skip(count);

            return new ReadOnlyQueryableSet<TSource>(
                query,
                Context);
        }

        public IReadOnlyQueryableSet<TSource> Take(int count)
        {
            DataServiceQuery query = ApplyLinq(new[] { typeof(TSource) }, new object[] { Query, count }) ??
                                     (DataServiceQuery)((IQueryable<TSource>)Query).Take(count);

            return new ReadOnlyQueryableSet<TSource>(
                query,
                Context);
        }

        public IReadOnlyQueryableSet<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            var newSelector = _pascalCaseExpressionVisitor.Visit(keySelector);

            DataServiceQuery query = CallLinqMethod(newSelector);

            return new ReadOnlyQueryableSet<TSource>(
                query,
                Context);
        }

        public IReadOnlyQueryableSet<TSource> OrderByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            var newSelector = _pascalCaseExpressionVisitor.Visit(keySelector);

            DataServiceQuery query = CallLinqMethod(newSelector);

            return new ReadOnlyQueryableSet<TSource>(
                query,
                Context);
        }

        public IReadOnlyQueryableSet<TSource> Expand<TTarget>(Expression<Func<TSource, TTarget>> navigationPropertyAccessor)
        {
            var newSelector = _pascalCaseExpressionVisitor.Visit(navigationPropertyAccessor);

            var concreteType = _concreteType.Value ?? typeof(TSource);
            var concreteDsq = typeof(DataServiceQuery<>).MakeGenericType(concreteType);

            DataServiceQuery query = CallOnConcreteType(concreteDsq, Query, new[] { typeof(TTarget) }, new object[] { newSelector });

            return new ReadOnlyQueryableSet<TSource>(
                query,
                Context);
        }

        public IReadOnlyQueryableSet<TSource> Expand(string expansion)
        {
            var concreteType = _concreteType.Value ?? typeof(TSource);
            var concreteDsq = typeof(DataServiceQuery<>).MakeGenericType(concreteType);

            DataServiceQuery query = CallOnConcreteType(concreteDsq, Query, new object[] { expansion });

            return new ReadOnlyQueryableSet<TSource>(
                query,
                Context);
        }

        private DataServiceQuery ApplyLinq(Type[] typeParams, object[] callParams, [CallerMemberName] string methodName = null)
        {
            return CallOnConcreteType(typeof (Queryable), null, typeParams, callParams, methodName);
        }

        private DataServiceQuery CallOnConcreteType(Type targetType, object instance, Type[] typeParams, object[] callParams, [CallerMemberName] string methodName = null)
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

            var typeInfo = targetType.GetTypeInfo();
            var methodInfo =
                (from i in typeInfo.GetDeclaredMethods(methodName)
                    let parameters = i.GetParameters()
                    where i.GetGenericArguments().Length == typeParams.Length
                    let constructedMethod = i.MakeGenericMethod(typeParams)
                    where AllParametersAreAssignable(constructedMethod.GetParameters(), callParams)
                    select constructedMethod).First();

            return (DataServiceQuery)methodInfo.Invoke(instance, callParams);
        }

        private DataServiceQuery CallOnConcreteType(Type targetType, object instance, object[] callParams, [CallerMemberName] string methodName = null)
        {
            var typeInfo = targetType.GetTypeInfo();
            var methodInfo =
                (from i in typeInfo.GetDeclaredMethods(methodName)
                 where AllParametersAreAssignable(i.GetParameters(), callParams)
                 select i).First();

            return (DataServiceQuery)methodInfo.Invoke(instance, callParams);
        }

        private bool AllParametersAreAssignable(IEnumerable<ParameterInfo> parameterInfo, object[] callParams)
        {
            return !parameterInfo.Where((t, i) => callParams[i] != null && !t.ParameterType.GetTypeInfo().IsAssignableFrom(callParams[i].GetType().GetTypeInfo())).Any();
        }

        private DataServiceQuery CallLinqMethod(Expression predicate, bool singleGenericParameter = false, [CallerMemberName] string methodName = null)
        {
            var typeParams = singleGenericParameter ?
                new[] { predicate.Type.GenericTypeArguments[0] } :
                predicate.Type.GenericTypeArguments;

            var callParams = new object[] { Query, predicate };

            var typeInfo = typeof(Queryable).GetTypeInfo();
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