using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RecipeManager.Generic.DataAccess {

    public class QueryTranslator : ExpressionVisitor {
        private StringBuilder sb;

        public QueryTranslator() {
        }

        public string Translate(Expression expression) {
            this.sb = new StringBuilder();
            this.Visit(expression);
            return this.sb.ToString();
        }

        private static Expression StripQuotes(Expression e) {
            while (e.NodeType == ExpressionType.Quote) {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m) {
            if (m.Method.DeclaringType == typeof(Enumerable) && m.Method.Name == "Contains") {
                sb.Append("(");
                this.Visit(m.Arguments[1]);
                sb.Append(" IN(");
                this.Visit(m.Arguments[0]);
                sb.Append("))");
                return m;
            }
            if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where") {
                this.Visit(m.Arguments[0]);
                sb.Append(" WHERE ");
                LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                this.Visit(lambda.Body);
                return m;
            }
            if (m.Method.Name == "FirstOrDefault") {
                this.Visit(m.Arguments[0]);
                return m;
            }

            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u) {
            switch (u.NodeType) {
                case ExpressionType.Not:
                    sb.Append(" NOT ");
                    this.Visit(u.Operand);
                    break;
                case ExpressionType.Convert:
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }

        protected override Expression VisitBinary(BinaryExpression b) {
            sb.Append("(");
            this.Visit(b.Left);
            switch (b.NodeType) {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    sb.Append(" AND ");
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    sb.Append(" OR ");
                    break;
                case ExpressionType.Equal:
                    sb.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    sb.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }
            this.Visit(b.Right);
            sb.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c) {
            switch (Type.GetTypeCode(c.Value.GetType())) {
                case TypeCode.Boolean:
                    sb.Append(((bool)c.Value) ? 1 : 0);
                    break;
                case TypeCode.String:
                    sb.Append("'");
                    sb.Append(c.Value);
                    sb.Append("'");
                    break;
                case TypeCode.Object:
                    break;
                //throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
                default:
                    sb.Append(c.Value);
                    break;
            }

            return c;
        }

        protected override Expression VisitMember(MemberExpression m) {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter) {
                sb.Append(m.Member.Name);
                return m;
            }
            if (m.Type.IsGenericType) {
                foreach (var arg in m.Type.GetGenericArguments()) {
                    Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(m.Type)) {
                        dynamic items = Expression.Lambda(m).Compile().DynamicInvoke();
                        foreach (var item in items) {
                            sb.Append(string.Format("{0}, ", item));
                        }
                        sb.Remove(sb.Length - 2, 2);

                        return m;
                    }
                }
                return m;
            }
            throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
        }
    }
}