﻿// <copyright file="CieLab.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Colors.Spaces
{
    using System;
    using System.ComponentModel;
    using System.Numerics;

    /// <summary>
    /// Represents an CIE LAB 1976 color.
    /// <see href="https://en.wikipedia.org/wiki/Lab_color_space"/>
    /// </summary>
    public struct CieLab : IColorVector, IEquatable<CieLab>, IAlmostEquatable<CieLab, float>
    {
        /// <summary>
        /// D50 standard illuminant.
        /// Used when reference white is not specified explicitly.
        /// </summary>
        public static readonly CieXyz DefaultWhitePoint = Illuminants.D50;

        /// <summary>
        /// Represents a <see cref="CieLab"/> that has L, A, B values set to zero.
        /// </summary>
        public static readonly CieLab Empty = default(CieLab);

        /// <summary>
        /// The backing vector for SIMD support.
        /// </summary>
        private readonly Vector3 backingVector;

        /// <summary>
        /// Initializes a new instance of the <see cref="CieLab"/> struct.
        /// </summary>
        /// <param name="l">The lightness dimension.</param>
        /// <param name="a">The a (green - magenta) component.</param>
        /// <param name="b">The b (blue - yellow) component.</param>
        /// <remarks>Uses <see cref="DefaultWhitePoint"/> as white point.</remarks>
        public CieLab(float l, float a, float b)
            : this(new Vector3(l, a, b), DefaultWhitePoint)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CieLab"/> struct.
        /// </summary>
        /// <param name="l">The lightness dimension.</param>
        /// <param name="a">The a (green - magenta) component.</param>
        /// <param name="b">The b (blue - yellow) component.</param>
        /// <param name="whitePoint">The reference white point. <see cref="Illuminants"/></param>
        public CieLab(float l, float a, float b, CieXyz whitePoint)
            : this(new Vector3(l, a, b), whitePoint)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CieLab"/> struct.
        /// </summary>
        /// <param name="vector">The vector representing the l, a, b components.</param>
        /// <remarks>Uses <see cref="DefaultWhitePoint"/> as white point.</remarks>
        public CieLab(Vector3 vector)
            : this(vector, DefaultWhitePoint)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CieLab"/> struct.
        /// </summary>
        /// <param name="vector">The vector representing the l a b components.</param>
        /// <param name="whitePoint">The reference white point. <see cref="Illuminants"/></param>
        public CieLab(Vector3 vector, CieXyz whitePoint)
            : this()
        {
            this.backingVector = vector;
            this.WhitePoint = whitePoint;
        }

        public CieXyz WhitePoint { get; }

        /// <summary>
        /// Gets the lightness dimension.
        /// <remarks>A value ranging between 0 (black), 100 (diffuse white) or higher (specular white).</remarks>
        /// </summary>
        public float L => this.backingVector.X;

        /// <summary>
        /// Gets the a color component.
        /// <remarks>A value ranging from -100 to 100. Negative is green, positive magenta.</remarks>
        /// </summary>
        public float A => this.backingVector.Y;

        /// <summary>
        /// Gets the b color component.
        /// <remarks>A value ranging from -100 to 100. Negative is blue, positive is yellow</remarks>
        /// </summary>
        public float B => this.backingVector.Z;

        /// <summary>
        /// Gets a value indicating whether this <see cref="CieLab"/> is empty.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsEmpty => this.Equals(Empty);

        /// <inheritdoc />
        public Vector3 Vector => this.backingVector;

        /// <summary>
        /// Compares two <see cref="CieLab"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="CieLab"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="CieLab"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the current left is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator ==(CieLab left, CieLab right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="CieLab"/> objects for inequality
        /// </summary>
        /// <param name="left">
        /// The <see cref="CieLab"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="CieLab"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the current left is unequal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator !=(CieLab left, CieLab right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.backingVector.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (this.IsEmpty)
            {
                return "CieLab [Empty]";
            }

            return $"CieLab [ L={this.L:#0.##}, A={this.A:#0.##}, B={this.B:#0.##}]";
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is CieLab)
            {
                return this.Equals((CieLab)obj);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Equals(CieLab other)
        {
            return this.backingVector.Equals(other.backingVector);
        }

        /// <inheritdoc/>
        public bool AlmostEquals(CieLab other, float precision)
        {
            Vector3 result = Vector3.Abs(this.backingVector - other.backingVector);

            return result.X <= precision
                && result.Y <= precision
                && result.Z <= precision;
        }
    }
}