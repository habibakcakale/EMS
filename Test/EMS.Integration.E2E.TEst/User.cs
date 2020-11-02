namespace EMS.Integration.E2E.Test {
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.User;
    using Integration.User;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Xunit;

    public class User : IClassFixture<TestHost> {
        private readonly IHost host;

        public static IEnumerable<object[]> UserRequest {
            get {
                return new List<object[]>() {
                    new object[] {
                        new CreateUser.Request() {
                            Gender = Gender.Male,
                            Name = "habib akcakale",
                            Status = UserStatus.Active,
                            //TODO: not best but this will try to create a unique mail address, find a better way.
                            Email = $"{Guid.NewGuid()}@{Guid.NewGuid()}.com"
                        }
                    }
                };
            }
        }

        public User(TestHost host) {
            this.host = host.Host;
        }

        [Theory, MemberData(nameof(UserRequest))]
        public async void SearchUser_ShouldReturn_Response(CreateUser.Request request) {
            var mediator = host.Services.GetService<IMediator>();
            var response = await mediator.Send(new GetUserList.Request {
                Name = request.Email
            });
            Assert.NotNull(response);
        }

        [Theory, MemberData(nameof(UserRequest))]
        public async void CreateUser_ShouldCreate_User(CreateUser.Request request) {
            var mediator = host.Services.GetService<IMediator>();

            var response = await mediator.Send(request);
            Assert.NotNull(response);
            Assert.True(response.Id > 0);
            Assert.Equal(request.Email, response.Email);
            Assert.Equal(request.Gender, response.Gender);
            Assert.Equal(request.Status, response.Status);
            Assert.Equal(request.Name, response.Name);
        }

        [Theory, MemberData(nameof(UserRequest))]
        public async void UpdateUser_ShouldUpdate_UserName(CreateUser.Request request) {
            var mediator = host.Services.GetService<IMediator>();
            // Create a user
            var created = await mediator.Send(request);
            Assert.NotNull(created);
            Assert.True(created.Id > 0);
            const string name = "George Who Knows";
            var response = await mediator.Send(new UpdateUser.Request {
                Id = created.Id,
                Name = name
            });
            Assert.Equal(created.Id, response.Id);
            Assert.Equal(name, response.Name);
            Assert.Equal(request.Email, response.Email);
        }

        [Theory, MemberData(nameof(UserRequest))]
        public async void DeleteUser_ShouldDelete_User(CreateUser.Request request) {
            var mediator = host.Services.GetService<IMediator>();
            // Create a user
            var created = await mediator.Send(request);
            Assert.NotNull(created);
            Assert.True(created.Id > 0);
            // It should delete created user
            await mediator.Send(new DeleteUser.Request {
                Id = created.Id
            });
            await Assert.ThrowsAsync<GoRestApiException>((() => mediator.Send(new DeleteUser.Request() {
                Id = created.Id
            })));
        }

        [Theory, MemberData(nameof(UserRequest))]
        public async void GetUser_ShouldReturn_User(CreateUser.Request request) {
            var mediator = host.Services.GetService<IMediator>();
            // Create a user
            var created = await mediator.Send(request);
            Assert.NotNull(created);
            Assert.True(created.Id > 0);
            var user = await mediator.Send(new GetUser.Request {Id = created.Id});
            Assert.NotNull(user);
            Assert.NotEmpty(user.Email);
            Assert.Equal(request.Email, user.Email);
        }
    }
}