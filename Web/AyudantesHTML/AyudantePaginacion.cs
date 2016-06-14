using System;
using System.Text;
using System.Web.Mvc;
using Web.Models;

namespace Web.AyudantesHTML
{
    public static class AyudantePaginacion
    {
        public static MvcHtmlString LinkPaginacion(this HtmlHelper html, InformacionPagina informacionPagina, Func<int, string> URL) {
            
            StringBuilder result = new StringBuilder();

            for (int i = 1; i <= informacionPagina.TotalPaginas; i++) {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", URL(i));
                tag.InnerHtml = i.ToString();

                if (i == informacionPagina.PaginaActual) {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-success");
                }

                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }

    }
}