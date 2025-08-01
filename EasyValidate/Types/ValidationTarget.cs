using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Types;

internal class ValidationTarget(ISymbol symbol, TargetType targetType, List<MemberInfo>? members = null)
{

    public ISymbol Symbol { get; } = symbol;
    public TargetType TargetType { get; } = targetType;
    public List<MemberInfo> Members { get; } = members ?? [];
}

internal enum TargetType
{
    CurretClass,
    Method
}