﻿// <copyright file="ColorConverter.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Colors.Conversion
{
    using System;
    using Spaces;

    /// <summary>
    /// Converts between color spaces ensuring that the color is adapted using chromatic adaptation.
    /// </summary>
    public partial class ColorConverter
    {
        /// <summary>
        /// Performs chromatic adaptation of given XYZ color.
        /// Target white point is <see cref="WhitePoint"/>.
        /// </summary>
        public CieXyz Adapt(CieXyz color, CieXyz sourceWhitePoint)
        {
            Guard.NotNull(color, nameof(color));
            Guard.NotNull(sourceWhitePoint, nameof(sourceWhitePoint));

            if (!this.IsChromaticAdaptationPerformed)
            {
                throw new InvalidOperationException("Cannot perform chromatic adaptation, provide a chromatic adaptation method and white point.");
            }

            return this.ChromaticAdaptation.Transform(color, sourceWhitePoint, this.WhitePoint);
        }
    }
}