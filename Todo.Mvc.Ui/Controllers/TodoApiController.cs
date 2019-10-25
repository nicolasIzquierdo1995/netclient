//Copyright 2017 (c) SmartIT. All rights reserved.
//By John Kocer
// This file is for Swagger test, this application does not use this file
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using SmartIT.Employee.MockDB;
using Todo.Mvc.Ui;

namespace TodoAngular.Ui.Controllers
{
  [Produces("application/json")]
  [Route("api/Todo")]
  public class TodoApiController : Controller
  {
    TodoRepository _todoRepository = new TodoRepository();
    private const string URL = "http://localhost:8080/webir/service/sendToken";

    [Route("~/api/LoginUser")]
    [HttpPost]
    public HttpResponseMessage LoginUser([FromBody]Credential credentials)
    {
        // si mis credenciales estan en la clase estática
        if (Users.Login(credentials.User, credentials.Password))
        {
            // obtengo un token
            var token = Users.GetToken();

            try
            {
                // se lo envio a java
                SendTokenToServer(token);
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(token);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return response;
            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(ex.Message);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                return response;
            }
        }
        else
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = new StringContent("Todo roto");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            return response;
        }
    }

        [Route("~/api/RegisterUser")]
        [HttpPost]
        public HttpResponseMessage Register([FromBody]Credential credentials)
        {
            // agrego el usuario a la clase estática
            Users.AddUser(credentials.User, credentials.Password);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent("Todo joya");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            return response;
        }

        private void SendTokenToServer(string token)
        {
            // creo el cliente
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8080");
            string urlParameters = "?token=" + token;

            // no estoy seguro si se hace así el post
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("token", token)
            });

            // List data response.
            HttpResponseMessage response = client.PostAsync("/webir/service/sendToken", content).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("se rompió todo papu");
            }

            client.Dispose();

        }

    }

    public class Credential
    {
        public string User { get; set; }
        public string Password { get; set; }
    }
}
