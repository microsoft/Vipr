// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vipr.Writer.CSharp.Lite
{
    internal class SourceCodeBuilder
    {
        private readonly StringBuilder _builder = new StringBuilder();

        private readonly Indenter _indenter = new Indenter();

        public IDisposable Indent { get { return _indenter.Indent; } }
        public IDisposable IndentBraced { get { return new BracedIndentScope(this); } }

        public void Write(string text)
        {
            var lines = GetLines(text);

            AppendLines(lines);
        }

        public void WriteLine(string text)
        {
            Write(text);

            _builder.AppendLine();
        }

        public override string ToString()
        {
            return _builder.ToString();
        }

        private void AppendLines(IList<string> lines)
        {
            for (var x = 0; x < lines.Count - 1; x++)
            {
                AppendIndented(lines[x]);
            }

            AppendIndented(lines[lines.Count - 1]);
        }

        private static string[] GetLines(string text)
        {
            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
            return lines;
        }

        private void AppendIndented(string text)
        {
            _builder.Append(_indenter.Indentation);

            _builder.AppendLine(text);
        }

        public void WriteLine(string text, params object[] args)
        {
            WriteLine(String.Format(text, args));
        }

        public void WriteLine()
        {
            _builder.AppendLine();
        }

        public void Write(string text, params object[] args)
        {
            Write(String.Format(text, args));
        }

        private class BracedIndentScope : IDisposable
        {
            private readonly SourceCodeBuilder _builder;

            private readonly IDisposable _indenter;

            internal BracedIndentScope(SourceCodeBuilder builder)
            {
                _builder = builder;

                _builder.WriteLine("{");

                _indenter = builder.Indent;
            }

            public void Dispose()
            {
                _indenter.Dispose();

                _builder.WriteLine("}");
            }
        }
    }
}
