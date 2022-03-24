using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chapter.WebApi.Controllers;
using Chapter.WebApi.Interfaces;
using Chapter.WebApi.Models;
using Chapter.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TestXunit.Controllers
{
    public class LoginControllerTeste
    {
        [Fact]
        public void LoginController_Retornar_UsuarioInfalido()
        {//Arrange
            var repositorioFalso = new Mock<IUsuarioRepository>();
            repositorioFalso.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns((Usuario)null);

            LoginViewModel dados = new LoginViewModel();
            dados.Email = "email@email.com";
            dados.Senha = "1234";

            var controller = new LoginController(repositorioFalso.Object);

            //Act
            var resultado = controller.Login(dados);

            //Assert
            Assert.IsType<UnauthorizedObjectResult>(resultado);
        }

        [Fact]
        public void LoginController_Retornar_Usuario()
        {
            //Arrange
            string issuerValidacao = "chapter.webApi";

            Usuario usuarioFake = new Usuario();
            usuarioFake.Email = "email@email.com";
            usuarioFake.Senha = "1234";

            var repositorioFalso = new Mock<IUsuarioRepository>();
            repositorioFalso.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(usuarioFake);

            var controller = new LoginController(repositorioFalso.Object);

            LoginViewModel dados = new LoginViewModel();
            dados.Email = "email@email.com";
            dados.Senha = "1234";

            //Act
            OkObjectResult resultado = (OkObjectResult)controller.Login(dados);

            var token = resultado.Value.ToString().Split(' ')[3];

            var jstHandler = new JwtSecurityTokenHandler();
            var jwtToken = jstHandler.ReadJwtToken(token);

            //Assert
            Assert.Equal(issuerValidacao, jwtToken.Issuer);

        }
    }
}
