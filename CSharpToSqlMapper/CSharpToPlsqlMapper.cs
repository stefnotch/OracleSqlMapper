using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Linq;

namespace CSharpToSqlMapper
{
    /*public class CSharpToPlsqlMapper
    {
        // TODO: Map an entire .cs file to PLSQL >:D

        public List<Function> Functions { get; } = new List<Function>();

        public void AddExpression<TDelegate>(string functionName, Expression<TDelegate> expression)
        {
            // TODO: Datatypes without a length (the brackets).

            var parameters = expression.Parameters;
            var returnType = expression.ReturnType;

            var plsqlFunction = new Function(functionName);
            plsqlFunction.Parameters.AddRange(parameters.Select(p => new Parameter(plsqlFunction, p.Name, Datatype.FromType(p.Type))));
            plsqlFunction.ReturnType = Datatype.FromType(returnType);

            plsqlFunction.Body = $"RETURN {ToPlsql(expression.Body, plsqlFunction)};\n";
            Functions.Add(plsqlFunction);
            Console.WriteLine(plsqlFunction.ToString());
        }

        private string ToPlsql(Expression expression, Function plsqlFunction)
        {
            return expression switch
            {
                ConstantExpression constant =>
                    // TODO: Convert constant values to plsql
                    constant.Value + "",
                ParameterExpression param =>
                    param.Name,
                BinaryExpression binary when binary.NodeType == ExpressionType.Modulo =>
                    $"MOD({ToPlsql(binary.Left, plsqlFunction)}, {ToPlsql(binary.Right, plsqlFunction)})",
                BinaryExpression binary when binary.NodeType == ExpressionType.Modulo =>
                    $"POWER({ToPlsql(binary.Left, plsqlFunction)}, {ToPlsql(binary.Right, plsqlFunction)})",
                BinaryExpression binary =>
                    $"({ToPlsql(binary.Left, plsqlFunction)} {ToPlsql(binary.NodeType)} {ToPlsql(binary.Right, plsqlFunction)})",
                //MethodCallExpression methodCallExpression => 
                _ => throw new ArgumentException("Unknown expression type ", nameof(expression))
            };
        }

        private string ToPlsql(ExpressionType nodeType)
        {
            // TODO: Implement all of 
            //https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.binaryexpression?view=netframework-4.8
            //https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.expressiontype?view=netframework-4.8
            return nodeType switch
            {
                ExpressionType.Add => "+",
                ExpressionType.AddChecked => "+",
                ExpressionType.Divide => "/",
                ExpressionType.Modulo => throw new NotSupportedException("Use MOD instead"),
                ExpressionType.Multiply => "*",
                ExpressionType.MultiplyChecked => "*",
                ExpressionType.Power => throw new NotSupportedException("Use POWER instead"),
                ExpressionType.Subtract => "-",
                ExpressionType.SubtractChecked => "-",
                ExpressionType.AndAlso => "AND",
                ExpressionType.OrElse => "OR",
                ExpressionType.Equal => "=",
                ExpressionType.NotEqual => "!=",
                ExpressionType.GreaterThanOrEqual => ">=",
                ExpressionType.GreaterThan => ">",
                ExpressionType.LessThanOrEqual => "<=",
                ExpressionType.LessThan => "<",
                _ => throw new NotSupportedException("Not implemented"),
            };
        }
    }*/
}
