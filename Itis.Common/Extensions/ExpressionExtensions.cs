using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Itis.Common.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression RemoveConvert(this Expression expression)
        {
            while (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)
                expression = ((UnaryExpression)expression).Operand;
            return expression;
        }

        public static bool TryParsePath(Expression expression, out string path)
        {
            path = (string)null;
            Expression expression1 = RemoveConvert(expression);
            MemberExpression memberExpression = expression1 as MemberExpression;
            MethodCallExpression methodCallExpression = expression1 as MethodCallExpression;
            if (memberExpression != null)
            {
                string name = memberExpression.Member.Name;
                string path1;
                if (!TryParsePath(memberExpression.Expression, out path1))
                    return false;
                path = path1 == null ? name : path1 + "." + name;
            }
            else if (methodCallExpression != null)
            {
                string path1;
                if (methodCallExpression.Method.Name == "Select" && methodCallExpression.Arguments.Count == 2 && (TryParsePath(methodCallExpression.Arguments[0], out path1) && path1 != null))
                {
                    LambdaExpression lambdaExpression = methodCallExpression.Arguments[1] as LambdaExpression;
                    string path2;
                    if (lambdaExpression != null && TryParsePath(lambdaExpression.Body, out path2) && path2 != null)
                    {
                        path = path1 + "." + path2;
                        return true;
                    }
                }
                return false;
            }
            return true;
        }
    }
}
