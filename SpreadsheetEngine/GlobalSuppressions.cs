// <copyright file="GlobalSuppressions.cs" company="Linh Stitsel">
// Copyright (c) Linh Stitsel. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Stated in doc that it is in lowercase.", Scope = "member", Target = "~P:CptS321.ExpressionTree.expression")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Needs to be public so MakeTree could use it.", Scope = "member", Target = "~F:CptS321.OpNode.Rhs")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Needs to be public so MakeTree could use it.", Scope = "member", Target = "~F:CptS321.OpNode.Lhs")]
