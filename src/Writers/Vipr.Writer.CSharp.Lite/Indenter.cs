// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Vipr.Writer.CSharp.Lite
{
    public class Indenter
    {
        private readonly String _indentStep;
        private readonly IList<String> _indents = new List<string> { string.Empty };
        private int _indentLevel = 0;

        public Indenter(string indentStep = "    ")
        {
            _indentStep = indentStep;
        }

        public IDisposable Indent { get { return new IndentScope(this); } }

        public string Indentation { get { return _indents[_indentLevel]; } }

        private void PushIndent()
        {
            var maxIndent = _indents.Count;

            if (maxIndent == _indentLevel + 1)
            {
                _indents.Add(_indents[maxIndent - 1] + _indentStep);
            }

            _indentLevel++;
        }

        private void PopIndent()
        {
            _indentLevel--;
        }

        private class IndentScope : IDisposable
        {
            private readonly Indenter _indenter;

            internal IndentScope(Indenter indenter)
            {
                _indenter = indenter;

                _indenter.PushIndent();
            }

            public void Dispose()
            {
                _indenter.PopIndent();
            }
        }
    }
}