namespace EMS.Integration.E2E.Test {
    using System;
    using System.Collections.Generic;
    using Common;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Xunit;

    public class User : IClassFixture<TestHost> {
        private readonly IHost host;

        public static readonly IEnumerable<object[]> UserRequest = new List<object[]>() {
            new object[] {
                new CreateUser.Request() {
                    Email = "habibakcakale@gmail.com",
                    Gender = Gender.Male,
                    Name = "habib akcakale",
                    Status = UserStatus.Active
                }
            }
        };

        public User(TestHost host) {
            this.host = host.Host;
        }

        [Fact]
        public async void SearchUserShouldHaveResponse() {
            var mediator = host.Services.GetService<IMediator>();
            var response = await mediator.Send(new UserList.Request {
                Name = "habib"
            });
            Assert.NotNull(response);
        }

        [Theory, MemberData(nameof(UserRequest))]
        public async void CreateUserShouldCreateUser(CreateUser.Request request) {
            var mediator = host.Services.GetService<IMediator>();
            //TODO: not best but this will try to create a unique mail address, find a better way.
            request.Email = $"{Guid.NewGuid()}@mail.com";
            var response = await mediator.Send(request);
            Assert.NotNull(response);
            Assert.True(response.Id > 0);
            Assert.Equal(request.Email, response.Email);
            Assert.Equal(request.Gender, response.Gender);
            Assert.Equal(request.Status, response.Status);
            Assert.Equal(request.Name, response.Name);
        }
    }
}