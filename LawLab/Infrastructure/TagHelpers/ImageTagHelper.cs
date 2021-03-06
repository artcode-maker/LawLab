using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace LawLab.Infrastructure.TagHelpers
{
    [HtmlTargetElement(tag: "img", Attributes = "img-action, img-controller, img-id")]
    public class ImageTagHelper : TagHelper
    {
        public string ImgAction { get; set; }
        public string ImgController { get; set; }
        public long ImgId { get; set; }

        LinkGenerator _linkGenerator;

        public ImageTagHelper(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var url = _linkGenerator.GetPathByAction(ImgAction, ImgController) + $"/{ImgId}";
            output.Attributes.Add("src", url);
        }
    }
}