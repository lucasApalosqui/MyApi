﻿using BlogAspNet.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BlogAspNet.Services
{
    public class TokenService
    {
        public string GenerateToken(User user)
        {
            // criar instancia do token handler que gera o token
            var tokenHandler = new JwtSecurityTokenHandler();

            // Pega a chave 
            var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

            // Cria uma especificação do token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials( 
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            };

            // Cria o token com as especificações selecionadas
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // converte tudo em uma string
            return tokenHandler.WriteToken(token);

        }
    }
}
