﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Core.Serialization;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Auth.Jwt;
using TorneioLeft4Dead2.Jogadores.Commands;
using TorneioLeft4Dead2FunctionApp.Exceptions;

namespace TorneioLeft4Dead2FunctionApp.Extensions;

public static class HttpRequestDataExtensions
{
    private static readonly JsonObjectSerializer Serializer = new(new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });

    public static HttpResponseData Ok(this HttpRequestData httpRequest)
    {
        return httpRequest.CreateResponse(HttpStatusCode.OK);
    }

    public static HttpResponseData NotFound(this HttpRequestData httpRequest)
    {
        return httpRequest.CreateResponse(HttpStatusCode.NotFound);
    }

    public static HttpResponseData Unauthorized(this HttpRequestData httpRequest)
    {
        return httpRequest.CreateResponse(HttpStatusCode.Unauthorized);
    }

    public static async Task<HttpResponseData> OkAsync<T>(this HttpRequestData httpRequest, T data)
    {
        var response = httpRequest.CreateResponse(HttpStatusCode.OK);

        await response.WriteAsJsonAsync(data, Serializer);

        return response;
    }

    public static async Task<HttpResponseData> BadRequestAsync(this HttpRequestData httpRequest, Exception exception)
    {
        var friendlyResult = FriendlyResult.New(exception);

        return await httpRequest.BadRequestAsync(friendlyResult);
    }

    private static async Task<HttpResponseData> BadRequestAsync<T>(this HttpRequestData httpRequest, T data)
    {
        var response = httpRequest.CreateResponse(HttpStatusCode.BadRequest);

        await response.WriteAsJsonAsync(data, Serializer);

        response.StatusCode = HttpStatusCode.BadRequest;

        return response;
    }

    public static async Task<T> DeserializeBodyAsync<T>(this HttpRequestData httpRequest)
    {
        using var streamReader = new StreamReader(httpRequest.Body);
        var json = await streamReader.ReadToEndAsync();
        var t = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return t;
    }

    public static ClaimsPrincipal CurrentUser(this HttpRequestData httpRequest)
    {
        var values = httpRequest.Headers.GetValues("Authorization").ToList();
        if (values.Count == 0)
            return null;

        var authorization = values.First();
        var accessToken = authorization.Split(" ").Last();

        return UsuarioJwtService.IsValidToken(accessToken) ? UsuarioJwtService.ClaimsPrincipal(accessToken) : null;
    }

    public static AutenticarJogadorCommand BuildAutenticarJogadorCommand(this HttpRequestData httpRequest)
    {
        var values = httpRequest.Headers.GetValues("Capitao").ToList();
        if (values.Count == 0)
            return null;

        var authorization = values.First();

        return AutenticarJogadorCommand.Parse(authorization);
    }
}