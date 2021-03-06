﻿using FluentValidation;
using Identity.Application.Errors;
using Identity.Application.Interfaces;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Application.User
{
    public class Login
    {
        public class Query : IRequest<User>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }

        }

        public class Handler : IRequestHandler<Query,User>
        {

            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            
            private readonly IJwtGenerator _jwtGenerator;
            public Handler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtGenerator jwtGenerator)
            {
                 _signInManager = signInManager;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;

            }
            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (result.Succeeded)
                {
                    return new User
                    {
                        DisplayName=user.DisplayName,
                        Token = _jwtGenerator.CreateToken(user),
                        Username =user.UserName,
                        Image=null
                    };
                    //todo: generate token
                    //return new AppUser
                    //{
                    //    DisplayName = user.DisplayName,
                    //    Token = _jwtGenerator.CreateToken(user),
                    //    Username = user.UserName,
                    //    Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                    //};
                }

                throw new RestException(HttpStatusCode.Unauthorized);

            }
        }

    }
}
