#pragma checksum "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Orders\All.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e2ec568e2b3f6279ac0ff8cbd4bc578a4fbeced3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Orders_All), @"mvc.1.0.view", @"/Views/Orders/All.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Orders/All.cshtml", typeof(AspNetCore.Views_Orders_All))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e2ec568e2b3f6279ac0ff8cbd4bc578a4fbeced3", @"/Views/Orders/All.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6e2355b4d2dd102d586b09f0f668ac669855f614", @"/Views/_ViewImports.cshtml")]
    public class Views_Orders_All : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IList<FastFood.Web.ViewModels.Orders.OrderAllViewModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(64, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Orders\All.cshtml"
  
    ViewData["Title"] = "All Orders";

#line default
#line hidden
            BeginContext(112, 406, true);
            WriteLiteral(@"<h1 class=""text-center"">All Orders</h1>
<hr class=""hr-2"" />
<table class=""table mx-auto"">
    <thead>
        <tr class=""row"">
            <th class=""col-md-1"">#</th>
            <th class=""col-md-2"">OrderId</th>
            <th class=""col-md-2"">Customer</th>
            <th class=""col-md-2"">Employee</th>
            <th class=""col-md-2"">DateTime</th>
        </tr>
    </thead>
    <tbody>
");
            EndContext();
#line 19 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Orders\All.cshtml"
         for (var i = 0; i < Model.Count(); i++)
        {

#line default
#line hidden
            BeginContext(579, 67, true);
            WriteLiteral("            <tr class=\"row\">\r\n                <th class=\"col-md-1\">");
            EndContext();
            BeginContext(648, 5, false);
#line 22 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Orders\All.cshtml"
                                 Write(i + 1);

#line default
#line hidden
            EndContext();
            BeginContext(654, 44, true);
            WriteLiteral("</th>\r\n                <td class=\"col-md-2\">");
            EndContext();
            BeginContext(699, 16, false);
#line 23 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Orders\All.cshtml"
                                Write(Model[i].OrderId);

#line default
#line hidden
            EndContext();
            BeginContext(715, 44, true);
            WriteLiteral("</td>\r\n                <td class=\"col-md-2\">");
            EndContext();
            BeginContext(760, 17, false);
#line 24 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Orders\All.cshtml"
                                Write(Model[i].Customer);

#line default
#line hidden
            EndContext();
            BeginContext(777, 44, true);
            WriteLiteral("</td>\r\n                <td class=\"col-md-2\">");
            EndContext();
            BeginContext(822, 17, false);
#line 25 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Orders\All.cshtml"
                                Write(Model[i].Employee);

#line default
#line hidden
            EndContext();
            BeginContext(839, 44, true);
            WriteLiteral("</td>\r\n                <td class=\"col-md-2\">");
            EndContext();
            BeginContext(884, 17, false);
#line 26 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Orders\All.cshtml"
                                Write(Model[i].DateTime);

#line default
#line hidden
            EndContext();
            BeginContext(901, 26, true);
            WriteLiteral("</td>\r\n            </tr>\r\n");
            EndContext();
#line 28 "D:\DatabasesAdvancedEntityFramework\9AutoMappingObjects\FastFood.Web\Views\Orders\All.cshtml"
        }

#line default
#line hidden
            BeginContext(938, 22, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IList<FastFood.Web.ViewModels.Orders.OrderAllViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
