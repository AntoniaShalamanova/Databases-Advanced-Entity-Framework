#pragma checksum "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Items\All.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0c29ae294ee7ac5105e3b9fc4f22f7b4525f469b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Items_All), @"mvc.1.0.view", @"/Views/Items/All.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Items/All.cshtml", typeof(AspNetCore.Views_Items_All))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\_ViewImports.cshtml"
using FastFood.Web;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0c29ae294ee7ac5105e3b9fc4f22f7b4525f469b", @"/Views/Items/All.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6e2355b4d2dd102d586b09f0f668ac669855f614", @"/Views/_ViewImports.cshtml")]
    public class Views_Items_All : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IList<FastFood.Web.ViewModels.Items.ItemsAllViewModels>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(64, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Items\All.cshtml"
  
    ViewData["Title"] = "All Items";

#line default
#line hidden
            BeginContext(111, 351, true);
            WriteLiteral(@"<h1 class=""text-center"">All Items</h1>
<hr class=""hr-2"" />
<table class=""table mx-auto"">
    <thead>
        <tr class=""row"">
            <th class=""col-md-1"">#</th>
            <th class=""col-md-2"">Name</th>
            <th class=""col-md-2"">Price</th>
            <th class=""col-md-2"">Category</th>
        </tr>
    </thead>
    <tbody>
");
            EndContext();
#line 18 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Items\All.cshtml"
         for (var i = 0; i < Model.Count(); i++)
        {

#line default
#line hidden
            BeginContext(523, 67, true);
            WriteLiteral("            <tr class=\"row\">\r\n                <th class=\"col-md-1\">");
            EndContext();
            BeginContext(592, 5, false);
#line 21 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Items\All.cshtml"
                                 Write(i + 1);

#line default
#line hidden
            EndContext();
            BeginContext(598, 44, true);
            WriteLiteral("</th>\r\n                <td class=\"col-md-2\">");
            EndContext();
            BeginContext(643, 13, false);
#line 22 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Items\All.cshtml"
                                Write(Model[i].Name);

#line default
#line hidden
            EndContext();
            BeginContext(656, 44, true);
            WriteLiteral("</td>\r\n                <td class=\"col-md-2\">");
            EndContext();
            BeginContext(701, 14, false);
#line 23 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Items\All.cshtml"
                                Write(Model[i].Price);

#line default
#line hidden
            EndContext();
            BeginContext(715, 44, true);
            WriteLiteral("</td>\r\n                <td class=\"col-md-2\">");
            EndContext();
            BeginContext(760, 17, false);
#line 24 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Items\All.cshtml"
                                Write(Model[i].Category);

#line default
#line hidden
            EndContext();
            BeginContext(777, 26, true);
            WriteLiteral("</td>\r\n            </tr>\r\n");
            EndContext();
#line 26 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Items\All.cshtml"
        }

#line default
#line hidden
            BeginContext(814, 22, true);
            WriteLiteral("    </tbody>\r\n</table>");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IList<FastFood.Web.ViewModels.Items.ItemsAllViewModels>> Html { get; private set; }
    }
}
#pragma warning restore 1591
