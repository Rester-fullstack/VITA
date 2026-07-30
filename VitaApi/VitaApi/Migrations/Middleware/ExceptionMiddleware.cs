using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VitaApi.Responses;

namespace VitaApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _environment;

    public ExceptionMiddleware(
        RequestDelegate next,
        IWebHostEnvironment environment
    )
    {
        _next = next;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            Console.WriteLine("========== ERRO DA API ==========");
            Console.WriteLine(ex.ToString());
            Console.WriteLine("=================================");

            var mensagem = ex.Message;

            if (_environment.IsDevelopment())
            {
                mensagem = ObterMensagemCompleta(ex);
            }

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = mensagem
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }

    private static string ObterMensagemCompleta(Exception exception)
    {
        var mensagens = new List<string>();

        Exception? atual = exception;

        while (atual != null)
        {
            if (!string.IsNullOrWhiteSpace(atual.Message))
                mensagens.Add(atual.Message);

            atual = atual.InnerException;
        }

        return string.Join(" | INNER: ", mensagens);
    }
}