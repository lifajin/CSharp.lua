using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLua.LuaAst {
    public sealed class LuaForInStatementSyntax : LuaStatementSyntax {
        public LuaExpressionSyntax Expression { get; }
        public string ForKeyword => Tokens.For;
        public LuaIdentifierNameSyntax Identifier { get; }
        public string InKeyword => Tokens.In;

        public LuaBlockSyntax Body { get; } = new LuaBlockSyntax() {
            OpenBraceToken = Tokens.Do,
            CloseBraceToken = Tokens.End,
        };

        public LuaForInStatementSyntax(LuaIdentifierNameSyntax identifier, LuaExpressionSyntax expression) {
            if(identifier == null) {
                throw new ArgumentNullException(nameof(identifier));
            }
            if(expression == null) {
                throw new ArgumentNullException(nameof(expression));
            }
            LuaInvocationExpressionSyntax invocationExpression = new LuaInvocationExpressionSyntax(LuaIdentifierNameSyntax.Each);
            invocationExpression.ArgumentList.Arguments.Add(new LuaArgumentSyntax(expression));

            Identifier = identifier;
            Expression = invocationExpression;
        }

        internal override void Render(LuaRenderer renderer) {
            renderer.Render(this);
        }
    }

    public sealed class LuaWhileStatementSyntax : LuaStatementSyntax {
        public LuaExpressionSyntax Condition { get; }
        public string WhileKeyword => LuaSyntaxNode.Tokens.While;

        public LuaBlockSyntax Body { get; } = new LuaBlockSyntax() {
            OpenBraceToken = Tokens.Do,
            CloseBraceToken = Tokens.End,
        };

        public LuaWhileStatementSyntax(LuaExpressionSyntax condition) {
            if(condition == null) {
                throw new ArgumentNullException(nameof(condition));
            }
            Condition = condition;
        }

        internal override void Render(LuaRenderer renderer) {
            renderer.Render(this);
        }
    }

    public sealed class LuaForAdapterStatementSyntax : LuaStatementSyntax {
        public LuaForAdapterStatementSyntax(
            LuaVariableDeclarationSyntax declaration, 
            IEnumerable<LuaExpressionStatementSyntax> initializers, 
            LuaExpressionSyntax condition, 
            LuaBlockSyntax statementBlock, 
            IEnumerable<LuaExpressionStatementSyntax> incrementors) {

            if(declaration != null) {
                Body.Statements.Add(declaration);
            }
            Body.Statements.AddRange(initializers);
            LuaWhileStatementSyntax whileStatement = new LuaWhileStatementSyntax(condition ?? LuaIdentifierNameSyntax.True);
            whileStatement.Body.Statements.AddRange(statementBlock.Statements);
            whileStatement.Body.Statements.AddRange(incrementors);
            Body.Statements.Add(whileStatement);
        }

        public LuaBlockSyntax Body { get; } = new LuaBlockSyntax() {
            OpenBraceToken = Tokens.Do,
            CloseBraceToken = Tokens.End,
        };

        internal override void Render(LuaRenderer renderer) {
            renderer.Render(this);
        }
    }

    public sealed class LuaRepeatStatementSyntax : LuaStatementSyntax {
        public LuaExpressionSyntax Condition { get; }
        public string RepeatKeyword => Tokens.Repeat;
        public string UntilKeyword => Tokens.Until;
        public LuaBlockSyntax Body { get; } = new LuaBlockSyntax();

        public LuaRepeatStatementSyntax(LuaExpressionSyntax condition) {
            if(condition == null) {
                throw new ArgumentNullException(nameof(condition));
            }
            Condition = condition;
        }

        internal override void Render(LuaRenderer renderer) {
            renderer.Render(this);
        }
    }
}
