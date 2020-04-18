// Gotenberg.Sharp.Api.Client - Copyright (c) 2019 CaptiveAire

using Gotenberg.Sharp.API.Client.Domain.Requests;
using Gotenberg.Sharp.API.Client.Domain.Requests.Content;

using JetBrains.Annotations;

namespace Gotenberg.Sharp.API.Client.Domain.Builders
{
    public class DimensionBuilder : PdfRequestBuilder
    {
        public DimensionBuilder(ContentRequest request)
        {
            this.Request = request;
            this.Request.Dimensions ??= new Dimensions();
        }
        
        [PublicAPI]
        public DimensionBuilder SetScale(double scale)
        {
            this.Request.Dimensions.Scale = scale;
            return this;
        }

        
        [PublicAPI]
        public DimensionBuilder PaperWidth(double width)
        {
            this.Request.Dimensions.PaperWidth = width;
            return this;
        }

        [PublicAPI]
        public DimensionBuilder PaperHeight(double height)
        {
            this.Request.Dimensions.PaperHeight = height;
            return this;
        }

        [PublicAPI]
        public DimensionBuilder MarginTop(double marginTop)
        {
            this.Request.Dimensions.MarginTop = marginTop;
            return this;
        }

        [PublicAPI]
        public DimensionBuilder MarginBottom(double marginBottom)
        {
            this.Request.Dimensions.MarginBottom = marginBottom;
            return this;
        }
        
        [PublicAPI]
        public DimensionBuilder MarginLeft(double marginLeft)
        {
            this.Request.Dimensions.MarginLeft = marginLeft;
            return this;
        }
        
        [PublicAPI]
        public DimensionBuilder MarginRight(double marginRight)
        {
            this.Request.Dimensions.MarginRight = marginRight;
            return this;
        }

        [PublicAPI]
        public DimensionBuilder LandScape(bool landscape)
        {
            this.Request.Dimensions.Landscape = landscape;
            return this;
        }
    }
}