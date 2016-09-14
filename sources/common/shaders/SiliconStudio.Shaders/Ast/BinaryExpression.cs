﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using System;
using System.Collections;
using System.Collections.Generic;

namespace SiliconStudio.Shaders.Ast
{
    /// <summary>
    /// Describes a binary expression.
    /// </summary>
    public partial class BinaryExpression : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryExpression"/> class.
        /// </summary>
        public BinaryExpression()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryExpression"/> class.
        /// </summary>
        /// <param name="operator">The @operator.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        public BinaryExpression(BinaryOperator @operator, Expression left, Expression right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        #region Public Properties

        /// <summary>
        ///   Gets or sets the left expression.
        /// </summary>
        /// <value>
        ///   The left expression.
        /// </value>
        public Expression Left { get; set; }

        /// <summary>
        ///   Gets or sets the binary operator.
        /// </summary>
        /// <value>
        ///   The binary operator.
        /// </value>
        public BinaryOperator Operator { get; set; }

        /// <summary>
        ///   Gets or sets the right expression.
        /// </summary>
        /// <value>
        ///   The right expression.
        /// </value>
        public Expression Right { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override IEnumerable<Node> Childrens()
        {
            ChildrenList.Clear();
            ChildrenList.Add(Left);
            ChildrenList.Add(Right);
            return ChildrenList;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Left, Operator.ConvertToString(), Right);
        }

        #endregion
    }
}