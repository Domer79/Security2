using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Itis.ProxyDataPlatform.Converting;

namespace Itis.ProxyDataPlatform.Linq
{
    internal class MemberTypeReplacement : ExpressionVisitor
    {
        private const string NotImplemented = "Не реализовано. Запланируйте реализацию: https://pssterms.visualstudio.com/DefaultCollection/%D0%A1%D0%A3%D0%9D%D0%9E/%D0%A1%D0%A3%D0%9D%D0%9E%20Team/_workitems";
        private readonly Type _replaceType;
        private readonly List<ParameterExpression> _parameters = new List<ParameterExpression>();

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Linq.Expressions.ExpressionVisitor"/>.
        /// </summary>
        public MemberTypeReplacement(Type replaceType)
        {
            _replaceType = replaceType;
        }

        public override Expression Visit(Expression node)
        {
            if (node == null)
                return null;

            switch (node.NodeType)
            {
                case ExpressionType.Negate:
                    throw new NotImplementedException(NotImplemented);
                case ExpressionType.NegateChecked:
                    throw new NotImplementedException(NotImplemented);
                case ExpressionType.Not:
                case ExpressionType.Convert:
                    return VisitUnary((UnaryExpression)node);
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
//                    return VisitTypeAs((UnaryExpression)node);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.OrElse:
//                    return VisitOrElse((BinaryExpression)node);
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
//                    return VisitLessCreater((BinaryExpression)node);
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:

                    #region Тоже BinaryExpression

                case ExpressionType.ExclusiveOr:
                    return VisitBinary((BinaryExpression) node);
                case ExpressionType.Equal:
                    return VisitEqual((BinaryExpression) node);
                case ExpressionType.And:
                    return VisitAnd((BinaryExpression) node);
                case ExpressionType.AndAlso:
                    return VisitAndAlso((BinaryExpression) node);
                case ExpressionType.Or:
                    return VisitOr((BinaryExpression) node);

                    #endregion

                case ExpressionType.TypeIs:
                    return VisitTypeIs((TypeBinaryExpression)node);
                case ExpressionType.Conditional:
                    return VisitConditional((ConditionalExpression)node);
                case ExpressionType.Constant:
                    return VisitConstant((ConstantExpression)node);
                case ExpressionType.Parameter:
                    return VisitParameter((ParameterExpression)node);
                case ExpressionType.MemberAccess:
                    return VisitMemberAccess((MemberExpression)node);
                case ExpressionType.Call:
                    return VisitMethodCall((MethodCallExpression)node);
                case ExpressionType.Lambda:
                    return VisitLambda((LambdaExpression)node);
                case ExpressionType.New:
                    return VisitNew((NewExpression)node);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return VisitNewArray((NewArrayExpression)node);
                case ExpressionType.Invoke:
                    return VisitInvocation((InvocationExpression)node);
                case ExpressionType.MemberInit:
                    return VisitMemberInit((MemberInitExpression)node);
                case ExpressionType.ListInit:
                    return VisitListInit((ListInitExpression)node);
                default:
                    throw new Exception(String.Format("Unhandled expression type: '{0}'", node.NodeType));
            }
        }

        private Expression VisitAndAlso(BinaryExpression node)
        {
            Expression left;
            Expression right;
            ConvertToNullable(node, out left, out right);

            return Expression.AndAlso(left, right);
        }

        private Expression VisitOr(BinaryExpression node)
        {
            Expression left;
            Expression right;
            ConvertToNullable(node, out left, out right);

            return Expression.Or(left, right);
        }

        private void ConvertToNullable(BinaryExpression node, out Expression left, out Expression right)
        {
            left = Visit(node.Left);
            right = Visit(node.Right);

            if (IsNullableType(left.Type) && !IsNullableType(right.Type))
                right = Expression.Convert(right, left.Type);
            else if (!IsNullableType(left.Type) && IsNullableType(right.Type))
                left = Expression.Convert(left, right.Type);
        }

        private Expression VisitAnd(BinaryExpression node)
        {
            Expression left;
            Expression right;
            ConvertToNullable(node, out left, out right);

            return Expression.And(left, right);
        }

        private Expression VisitLessGreater(BinaryExpression node)
        {
            Expression left;
            Expression right;
            ConvertToNullable(node, out left, out right);

            switch (node.NodeType)
            {
                case ExpressionType.OrElse:
                    return Expression.OrElse(left, right);
                case ExpressionType.LessThan:
                    return Expression.LessThan(left, right);
                case ExpressionType.LessThanOrEqual:
                    return Expression.LessThanOrEqual(left, right);
                case ExpressionType.GreaterThan:
                    return Expression.GreaterThan(left, right);
                case ExpressionType.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(left, right);
                default:
                    throw new NotImplementedException(NotImplemented);
            }
        }

        private Expression VisitEqual(BinaryExpression node)
        {
            Expression left;
            Expression right;
            ConvertToNullable(node, out left, out right);

            return Expression.Equal(left, right);
        }

        static bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private Expression VisitConvert(UnaryExpression node)
        {
            return Visit(node.Operand);
        }

//        private Expression VisitTypeAs(UnaryExpression node)
//        {
//            return node.Update(node.Operand);
//        }

        private Expression VisitLambda(LambdaExpression node)
        {
            var parameters = node.Parameters.Select(Visit).Cast<ParameterExpression>().ToArray();
            var body = Visit(node.Body);
            return Expression.Lambda(body, parameters);
        }

        private Expression VisitMemberAccess(MemberExpression node)
        {
            if (node.Expression == null)
                return node;
             
            var expression = Visit(node.Expression);

            if (node.Expression is ParameterExpression)
            {
//                if (((ParameterExpression) parameterExpression).Type != _replaceType)
                if (!Converter.EntityTypeMaps.Values.Contains(((ParameterExpression) expression).Type))
                    return node;
            }

            if (expression is ConstantExpression)
            {
                object container = ((ConstantExpression)expression).Value;
                var member = node.Member;
                if (member is FieldInfo)
                {
                    object value = ((FieldInfo)member).GetValue(container);
                    return Expression.Constant(value);
                }
                if (member is PropertyInfo)
                {
                    object value = ((PropertyInfo)member).GetValue(container, null);
                    return Expression.Constant(value);
                }
            }

//            var memberInfo = _replaceType.GetMember(node.Member.Name).FirstOrDefault();
            var parameterExpression = expression as ParameterExpression;
            var propertyExpression = expression as MemberExpression;
//            var propertyExpression = expression as PropertyExpression;
            MemberInfo memberInfo = null;
            if (parameterExpression != null)
                memberInfo = parameterExpression.Type.GetMember(node.Member.Name).FirstOrDefault();

            if (propertyExpression != null)
            {
                var fieldInfo = propertyExpression.Member as FieldInfo;
                var propertyInfo = propertyExpression.Member as PropertyInfo;

                Type type = null;

                if (fieldInfo != null)
                    type = fieldInfo.FieldType;

                if (propertyInfo != null)
                    type = propertyInfo.PropertyType;

                if (type == null)
                    throw new InvalidOperationException("Ошибка преобразования типов в дереве выражений.");

                memberInfo = type.GetMember(node.Member.Name).FirstOrDefault();
            }
#if DEBUG
            if (memberInfo == null)
                throw new InvalidOperationException("Ошибка преобразования типов в дереве выражений.");
#endif
            
            return Expression.MakeMemberAccess(expression, memberInfo ?? node.Member);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            var parameterExpression = _parameters.FirstOrDefault(p => p.Name == node.Name);
            if (parameterExpression == null)
            {
//                if (Converter.BllTypeMap(node.Type) != _replaceType)
//                    return node;

                if (!Converter.EntityTypeMaps.ContainsKey(node.Type))
                    return node;

                var entityType = Converter.EntityTypeMaps[node.Type];

//                parameterExpression = Expression.Parameter(_replaceType, node.Name);
                parameterExpression = Expression.Parameter(entityType, node.Name);
                _parameters.Add(parameterExpression);
            }

            return parameterExpression;
        }

        private Expression VisitTypeIs(TypeBinaryExpression node)
        {
            return Expression.TypeIs(Visit(node.Expression), node.Type);
        }

        /// <summary>
        /// Просматривает дочерний элемент выражения <see cref="T:System.Linq.Expressions.UnaryExpression"/>.
        /// </summary>
        /// <returns>
        /// Измененное выражение в случае изменения самого выражения или любого его подвыражения; в противном случае возвращается исходное выражение.
        /// </returns>
        /// <param name="node">Выражение, которое необходимо просмотреть.</param>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            return Expression.MakeUnary(node.NodeType, Visit(node.Operand), node.Type, node.Method);
        }

        /// <summary>
        /// Просматривает дочерний элемент выражения <see cref="T:System.Linq.Expressions.BinaryExpression"/>.
        /// </summary>
        /// <returns>
        /// Измененное выражение в случае изменения самого выражения или любого его подвыражения; в противном случае возвращается исходное выражение.
        /// </returns>
        /// <param name="node">Выражение, которое необходимо просмотреть.</param>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            Expression left;
            Expression right;
            ConvertToNullable(node, out left, out right);

            var expression = Expression.MakeBinary(node.NodeType, Visit(node.Left), Visit(node.Right), node.IsLiftedToNull, node.Method, (LambdaExpression)Visit(node.Conversion));
            return expression;
        }

        internal static Expression ReplaceParameterType(Type replaceType, Expression expression)
        {
            return new MemberTypeReplacement(replaceType).Visit(expression);
        }
    }
}

#region doc help

// Взято с http://huagati.blogspot.ru/2009/10/code-sample-search-and-replace-in-linq.html

/*
internal static class ExpressionExtensions
{
    /// <summary>
    /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree
    /// </summary>
    /// <param name="expression">Tree to replace parameters in</param>
    /// <param name="oldParameter">Parameter to replace</param>
    /// <param name="newParameter">Parameter to use as replacement</param>
    /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>
    public static Expression ReplaceParameter(this Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        Expression exp = null;
        Type expressionType = expression.GetType();
        if (expressionType == typeof(ParameterExpression))
        {
            exp = ((ParameterExpression)expression).ReplaceParameter(oldParameter, newParameter);
        }
        else if (expressionType == typeof(MemberExpression))
        {
            exp = ((MemberExpression)expression).ReplaceParameter(oldParameter, newParameter);
        }
        else if (expressionType == typeof(MethodCallExpression))
        {
            exp = ((MethodCallExpression)expression).ReplaceParameter(oldParameter, newParameter);
        }
        else if (expressionType == typeof(NewExpression))
        {
            exp = ((NewExpression)expression).ReplaceParameter(oldParameter, newParameter);
        }
        else if (expressionType == typeof(UnaryExpression))
        {
            exp = ((UnaryExpression)expression).ReplaceParameter(oldParameter, newParameter);
        }
        else if (expressionType == typeof(ConstantExpression))
        {
            exp = ((ConstantExpression)expression).ReplaceParameter(oldParameter, newParameter);
        }
        else if (expressionType == typeof(ConditionalExpression))
        {
            exp = ((ConditionalExpression)expression).ReplaceParameter(oldParameter, newParameter);
        }
        else if (expressionType == typeof(LambdaExpression))
        {
            exp = ((LambdaExpression)expression).ReplaceParameter(oldParameter, newParameter);
        }
        else if (expressionType == typeof(MemberInitExpression))
        {
            exp = ((MemberInitExpression)expression).ReplaceParameter(oldParameter, newParameter);
        }
        else if (expressionType == typeof(BinaryExpression))
        {
            exp = ((BinaryExpression)expression).ReplaceParameter(oldParameter, newParameter);
        }
        else
        {
            //did I forget some expression type? probably. this will take care of that... :)
            throw new NotImplementedException("Expression type " + expression.GetType().FullName + " not supported by this expression tree parser.");
        }
        return exp;
    }

    /// <summary>
    /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree
    /// </summary>
    /// <param name="expression">LambdaExpression to replace parameters in</param>
    /// <param name="oldParameter">Parameter to replace</param>
    /// <param name="newParameter">Parameter to use as replacement</param>
    /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>
    public static LambdaExpression ReplaceParameter(this LambdaExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        LambdaExpression lambdaExpression = null;
        lambdaExpression = Expression.Lambda(
            expression.Type,
            expression.Body.ReplaceParameter(oldParameter, newParameter),
            (expression.Parameters != null) ? expression.Parameters.ReplaceParameter(oldParameter, newParameter) : null
            );
        return lambdaExpression;
    }

    /// <summary>
    /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree
    /// </summary>
    /// <param name="expression">BinaryExpression to replace parameters in</param>
    /// <param name="oldParameter">Parameter to replace</param>
    /// <param name="newParameter">Parameter to use as replacement</param>
    /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>
    public static BinaryExpression ReplaceParameter(this BinaryExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        BinaryExpression binaryExp = null;
        binaryExp = Expression.MakeBinary(
            expression.NodeType,
            (expression.Left != null) ? expression.Left.ReplaceParameter(oldParameter, newParameter) : null,
            (expression.Right != null) ? expression.Right.ReplaceParameter(oldParameter, newParameter) : null,
            expression.IsLiftedToNull,
            expression.Method,
            (expression.Conversion != null) ? expression.Conversion.ReplaceParameter(oldParameter, newParameter) : null
            );
        return binaryExp;
    }

    /// <summary>
    /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree
    /// </summary>
    /// <param name="expression">ParameterExpression to replace parameters in</param>
    /// <param name="oldParameter">Parameter to replace</param>
    /// <param name="newParameter">Parameter to use as replacement</param>
    /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>
    public static ParameterExpression ReplaceParameter(this ParameterExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        ParameterExpression paramExpression = null;
        if (expression.Equals(oldParameter))
        {
            paramExpression = newParameter;
        }
        else
        {
            paramExpression = expression;
        }
        return paramExpression;
    }

    /// <summary>
    /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree
    /// </summary>
    /// <param name="expression">MemberExpression to replace parameters in</param>
    /// <param name="oldParameter">Parameter to replace</param>
    /// <param name="newParameter">Parameter to use as replacement</param>
    /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>
    public static MemberExpression ReplaceParameter(this MemberExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        return Expression.MakeMemberAccess(
            (expression.Expression != null) ? expression.Expression.ReplaceParameter(oldParameter, newParameter) : null,
            expression.Member);
    }

    /// <summary>
    /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree
    /// </summary>
    /// <param name="expression">MemberInitExpression to replace parameters in</param>
    /// <param name="oldParameter">Parameter to replace</param>
    /// <param name="newParameter">Parameter to use as replacement</param>
    /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>
    public static MemberInitExpression ReplaceParameter(this MemberInitExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        return Expression.MemberInit(
            (expression.NewExpression != null) ? expression.NewExpression.ReplaceParameter(oldParameter, newParameter) : null,
            (expression.Bindings != null) ? expression.Bindings.ReplaceParameter(oldParameter, newParameter) : null
            );
    }

    /// <summary>
    /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree
    /// </summary>
    /// <param name="expression">MethodCallExpression to replace parameters in</param>
    /// <param name="oldParameter">Parameter to replace</param>
    /// <param name="newParameter">Parameter to use as replacement</param>
    /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>
    public static MethodCallExpression ReplaceParameter(this MethodCallExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        MethodCallExpression callExpression = null;
        callExpression = Expression.Call(
            (expression.Object != null) ? expression.Object.ReplaceParameter(oldParameter, newParameter) : null,
            expression.Method,
            (expression.Arguments != null) ? expression.Arguments.ReplaceParameter(oldParameter, newParameter) : null
            );
        return callExpression;
    }

    /// <summary>
    /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree
    /// </summary>
    /// <param name="expression">NewExpression to replace parameters in</param>
    /// <param name="oldParameter">Parameter to replace</param>
    /// <param name="newParameter">Parameter to use as replacement</param>
    /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>
    public static NewExpression ReplaceParameter(this NewExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        return Expression.New(
            expression.Constructor,
            (expression.Arguments != null) ? expression.Arguments.ReplaceParameter(oldParameter, newParameter) : null,
            expression.Members);
    }

    /// <summary>
    /// Replace all occurences of a ParameterExpression within a ReadonlyCollection of ParameterExpressions with another ParameterExpression, and return as an IEnumerable
    /// </summary>
    /// <param name="expression">ReadOnlyCollection&lt;ParameterExpression&gt; to replace parameters in</param>
    /// <param name="oldParameter">Parameter to replace</param>
    /// <param name="newParameter">Parameter to use as replacement</param>
    /// <returns>A IEnumerable returning the passed in set of ParameterExpressions, with occurences of oldParameter replaced with newParameter</returns>
    public static IEnumerable<ParameterExpression> ReplaceParameter(this System.Collections.ObjectModel.ReadOnlyCollection<ParameterExpression> expressionArguments, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        if (expressionArguments != null)
        {
            foreach (ParameterExpression argument in expressionArguments)
            {
                if (argument != null)
                {
                    yield return argument.ReplaceParameter(oldParameter, newParameter);
                }
                else
                {
                    yield return null;
                }
            }
        }
    }

    /// <summary>
    /// Replace all occurences of a ParameterExpression within a ReadonlyCollection of Expressions with another ParameterExpression, and return as an IEnumerable
    /// </summary>
    /// <param name="expression">ReadOnlyCollection&lt;Expression&gt; to replace parameters in</param>
    /// <param name="oldParameter">Parameter to replace</param>
    /// <param name="newParameter">Parameter to use as replacement</param>
    /// <returns>A IEnumerable returning the passed in set of Expressions, with occurences of oldParameter replaced with newParameter</returns>
    public static IEnumerable<Expression> ReplaceParameter(this System.Collections.ObjectModel.ReadOnlyCollection<Expression> expressionArguments, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        if (expressionArguments != null)
        {
            foreach (Expression argument in expressionArguments)
            {
                if (argument != null)
                {
                    yield return argument.ReplaceParameter(oldParameter, newParameter);
                }
                else
                {
                    yield return null;
                }
            }
        }
    }

    /// <summary>
    /// Replace all occurences of a ParameterExpression within a ReadonlyCollection of ElementInits with another ParameterExpression, and return as an IEnumerable
    /// </summary>
    /// <param name="expression">ReadOnlyCollection&lt;ElementInit&gt; to replace parameters in</param>
    /// <param name="oldParameter">Parameter to replace</param>
    /// <param name="newParameter">Parameter to use as replacement</param>
    /// <returns>A IEnumerable returning the passed in set of ParameterExpressions, with occurences of oldParameter replaced with newParameter</returns>
    public static IEnumerable<ElementInit> ReplaceParameter(this System.Collections.ObjectModel.ReadOnlyCollection<ElementInit> elementInits, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        if (elementInits != null)
        {
            foreach (ElementInit elementInit in elementInits)
            {
                if (elementInit != null)
                {
                    yield return Expression.ElementInit(elementInit.AddMethod, elementInit.Arguments.ReplaceParameter(oldParameter, newParameter));
                }
                else
                {
                    yield return null;
                }
            }
        }
    }

    /// <summary>
    /// Replace all occurences of a ParameterExpression within a ReadonlyCollection of MemberBindings with another ParameterExpression, and return as an IEnumerable
    /// </summary>
    /// <param name="expression">ReadOnlyCollection&lt;MemberBinding&gt; to replace parameters in</param>
    /// <param name="oldParameter">Parameter to replace</param>
    /// <param name="newParameter">Parameter to use as replacement</param>
    /// <returns>A IEnumerable returning the passed in set of ParameterExpressions, with occurences of oldParameter replaced with newParameter</returns>
    public static IEnumerable<MemberBinding> ReplaceParameter(this System.Collections.ObjectModel.ReadOnlyCollection<MemberBinding> memberBindings, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        if (memberBindings != null)
        {
            foreach (MemberBinding binding in memberBindings)
            {
                if (binding != null)
                {
                    switch (binding.BindingType)
                    {
                        case MemberBindingType.Assignment:
                            MemberAssignment memberAssignment = (MemberAssignment)binding;
                            yield return Expression.Bind(binding.Member, memberAssignment.Expression.ReplaceParameter(oldParameter, newParameter));
                            break;
                        case MemberBindingType.ListBinding:
                            MemberListBinding listBinding = (MemberListBinding)binding;
                            yield return Expression.ListBind(binding.Member, listBinding.Initializers.ReplaceParameter(oldParameter, newParameter));
                            break;
                        case MemberBindingType.MemberBinding:
                            MemberMemberBinding memberMemberBinding = (MemberMemberBinding)binding;
                            yield return Expression.MemberBind(binding.Member, memberMemberBinding.Bindings.ReplaceParameter(oldParameter, newParameter));
                            break;
                    }
                }
                else
                {
                    yield return null;
                }
            }
        }
    }

    /// <summary>
    /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree
    /// </summary>
    /// <param name="expression">UnaryExpression to replace parameters in</param>
    /// <param name="oldParameter">Parameter to replace</param>
    /// <param name="newParameter">Parameter to use as replacement</param>
    /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>
    public static UnaryExpression ReplaceParameter(this UnaryExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        return Expression.MakeUnary(
            expression.NodeType,
            (expression.Operand != null) ? expression.Operand.ReplaceParameter(oldParameter, newParameter) : null,
            expression.Type,
            expression.Method);
    }

    /// <summary>
    /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree. Note: this version of ReplaceParameter exists just for conformity - there can't be a parameter expression hiding under a constant expression so this could really be skipped.
    /// </summary>
    /// <param name="expression">ConstantExpression to replace parameters in</param>
    /// <param name="oldParameter">Parameter to replace</param>
    /// <param name="newParameter">Parameter to use as replacement</param>
    /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>
    public static ConstantExpression ReplaceParameter(this ConstantExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        //return Expression.Constant(expression.Value, expression.Type);
        return expression;
    }

    /// <summary>
    /// Replace all occurences of a ParameterExpression within an expression tree with another ParameterExpression, and return a cloned tree
    /// </summary>
    /// <param name="expression">ConditionalExpression to replace parameters in</param>
    /// <param name="oldParameter">Parameter to replace</param>
    /// <param name="newParameter">Parameter to use as replacement</param>
    /// <returns>A cloned expression tree with all occurences of oldParameter replaced with newParameter</returns>
    public static ConditionalExpression ReplaceParameter(this ConditionalExpression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        return Expression.Condition(
            (expression.Test != null) ? expression.Test.ReplaceParameter(oldParameter, newParameter) : null,
            (expression.IfTrue != null) ? expression.IfTrue.ReplaceParameter(oldParameter, newParameter) : null,
            (expression.IfFalse != null) ? expression.IfFalse.ReplaceParameter(oldParameter, newParameter) : null
            );
    }
}
*/

#endregion