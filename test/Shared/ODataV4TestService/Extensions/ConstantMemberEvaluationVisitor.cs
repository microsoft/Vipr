using System.Linq.Expressions;
using System.Reflection;

namespace ODataV4TestService.SelfHost
{
    public class ConstantMemberEvaluationVisitor : ExpressionVisitor
    {
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression.NodeType == ExpressionType.Constant)
            {
                var constantExpression = ((ConstantExpression)node.Expression).Value;
                object constantValue = null;

                switch (node.Member.MemberType)
                {
                    case MemberTypes.Property:
                        constantValue = ((PropertyInfo)node.Member).GetValue(constantExpression, null);
                        break;
                    case MemberTypes.Field:
                        constantValue = ((FieldInfo)node.Member).GetValue(constantExpression);
                        break;
                }

                if (constantValue != null) return Expression.Constant(constantValue, node.Type);
            }

            return base.VisitMember(node);
        }
    }
}