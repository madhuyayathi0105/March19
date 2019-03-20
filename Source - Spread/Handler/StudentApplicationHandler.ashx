<%@ WebHandler Language="C#" Class="StudentApplicationHandler" %>

using System;
using System.Web;

public class StudentApplicationHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "image/jpg";
        var filepath = context.Request.QueryString["filename"].ToString();
        context.Response.WriteFile(filepath);

    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}