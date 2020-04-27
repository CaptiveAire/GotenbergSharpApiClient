﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

using Gotenberg.Sharp.API.Client.Infrastructure;

using JetBrains.Annotations;

namespace Gotenberg.Sharp.API.Client.Domain.Requests.Content
{
    /// <summary>
    ///  Represents the dimensions of the pdf document
    /// </summary>
    /// <remarks>
    ///     Paper size and margins have to be provided in inches. Same for margins.
    ///     See unit info here: https://thecodingmachine.github.io/gotenberg/#html.paper_size_margins_orientation
    /// </remarks>
    public sealed class Dimensions : IConvertToHttpContent
    {
        // ReSharper disable once InconsistentNaming
        static readonly Type _attributeType = typeof(MultiFormHeaderAttribute);

        #region Properties

        /// <summary>
        /// Gets or sets the scale. Defaults to 1.0
        /// </summary>
        [MultiFormHeader(Constants.Gotenberg.FormFieldNames.Dims.Scale)]
        public double Scale { [UsedImplicitly] get; set; } = 1.0;
        
        /// <summary>
        /// Gets or sets the width of the paper.
        /// </summary>
        /// <value>
        /// The width of the paper.
        /// </value>
        [MultiFormHeader(Constants.Gotenberg.FormFieldNames.Dims.PaperWidth)]
        public double PaperWidth { [UsedImplicitly] get; set; }

        /// <summary>
        /// Gets or sets the height of the paper.
        /// </summary>
        /// <value>
        /// The height of the paper.
        /// </value>
        [MultiFormHeader(Constants.Gotenberg.FormFieldNames.Dims.PaperHeight)]
        public double PaperHeight { [UsedImplicitly] get; set; }

        /// <summary>
        /// Gets or sets the margin top.
        /// </summary>
        /// <value>
        /// The margin top.
        /// </value>
        [MultiFormHeader(Constants.Gotenberg.FormFieldNames.Dims.MarginTop)]
        public double MarginTop { [UsedImplicitly] get; set; }


        /// <summary>
        /// Gets or sets the margin bottom.
        /// </summary>
        /// <value>
        /// The margin bottom.
        /// </value>
        [MultiFormHeader(Constants.Gotenberg.FormFieldNames.Dims.MarginBottom)]
        public double MarginBottom { [UsedImplicitly] get; set; }


        /// <summary>
        /// Gets or sets the margin left.
        /// </summary>
        /// <value>
        /// The margin left.
        /// </value>
        [MultiFormHeader(Constants.Gotenberg.FormFieldNames.Dims.MarginLeft)]
        public double MarginLeft  { [UsedImplicitly] get; set; }


        /// <summary>
        /// Gets or sets the margin right.
        /// </summary>
        /// <value>
        /// The margin right.
        /// </value>
        [MultiFormHeader(Constants.Gotenberg.FormFieldNames.Dims.MarginRight)]
        public double MarginRight { [UsedImplicitly] get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Dimensions"/> is landscape.
        /// </summary>
        /// <value>
        ///   <c>true</c> if landscape; otherwise, <c>false</c>.
        /// </value>
        [MultiFormHeader(Constants.Gotenberg.FormFieldNames.Dims.Landscape)]
        public bool Landscape { [UsedImplicitly] get; set; }

        #endregion

        #region public methods

       // https://www.prepressure.com/library/paper-size

        public static Dimensions ToA4WithNoMargins()
        {
            return new Dimensions
            {
                PaperWidth = 8.27,
                PaperHeight = 11.7,
                MarginTop = 0,
                MarginBottom = 0,
                MarginLeft = 0,
                MarginRight = 0
            };
        }

        /// <summary>
        ///     Default Google Chrome printer options
        /// </summary>
        /// <remarks>
        ///     Source: https://github.com/thecodingmachine/gotenberg/blob/7e69ec4367069df52bb61c9ee0dce241b043a257/internal/pkg/printer/chrome.go#L47
        /// </remarks>
        /// <returns></returns>
        public static Dimensions ToChromeDefaults()
        {
            return new Dimensions { 
                PaperWidth = 8.27, 
                PaperHeight = 11.7,
                MarginTop = 1,
                MarginBottom = 1,
                MarginLeft = 1,
                MarginRight = 1
            };
        }
        
        /// <summary>
        /// Defaults used for deliverables
        /// </summary>
        /// <returns></returns>
        public static Dimensions ToDeliverableDefault()
        {
            return new Dimensions { 
                PaperWidth = 8.26, 
                PaperHeight = 11.69,
                Landscape = false,
                MarginTop = 0,
                MarginBottom = .5,  
                MarginLeft = 0,
                MarginRight = 0
            };
        }

        /// <summary>
        /// Transforms the instance to a list of StringContent items
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HttpContent> ToHttpContent()
        {   
            return this.GetType().GetProperties()
                .Where(prop => Attribute.IsDefined(prop, _attributeType))
                .Select(p=> new { Prop = p, Attrib = (MultiFormHeaderAttribute)Attribute.GetCustomAttribute(p, _attributeType) })
                .Select(item =>
                {
                    var value =  item.Prop.GetValue(this);

                    if (value == null) return null;

                    var contentItem =new StringContent(value.ToString());
                    contentItem.Headers.ContentDisposition = new ContentDispositionHeaderValue(item.Attrib.ContentDisposition) { Name = item.Attrib.Name  };

                    return contentItem;
                }).Where(item=> item != null);
        }
        
        #endregion
        
    }
}