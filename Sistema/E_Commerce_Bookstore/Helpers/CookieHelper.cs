using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce_Bookstore.Helpers
{
    public static class CookieHelper
    {
        public static string ObtenerCookieId(HttpRequest request, HttpResponse response)
        {
            HttpCookie cookie = request.Cookies["CarritoId"];
            if (cookie == null)
            {
                string nuevoId = Guid.NewGuid().ToString();
                HttpCookie nuevaCookie = new HttpCookie("CarritoId", nuevoId)
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true
                };
                response.Cookies.Add(nuevaCookie);
                return nuevoId;
            }
            return cookie.Value;
        }
    }
}