namespace EMS.Integration.User {
    using System;
    using System.Net;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Common.User;
    using MediatR;
    using RestSharp;

    public static class UpdateUser {
        public class Request : IRequest<User> {
            [JsonPropertyName("id")] public int Id { get; set; }
            [JsonPropertyName("name")] public string Name { get; set; }
            [JsonPropertyName("email")] public string Email { get; set; }
            [JsonPropertyName("gender")] public Gender Gender { get; set; }
            [JsonPropertyName("status")] public UserStatus Status { get; set; }
        }

        public class Query : IRequestHandler<Request, User> {
            private readonly IRestClient restClient;
            private const string UpdateUserUrl = "users/{0}";

            public Query(IRestClient restClient) {
                this.restClient = restClient;
            }

            public async Task<User> Handle(Request request, CancellationToken cancellationToken) {
                var uri = new Uri(string.Format(UpdateUserUrl, request.Id), UriKind.Relative);
                var updateUserRequest = new RestRequest(uri, Method.PUT);
                updateUserRequest.AddJsonBody(request);
                var response = await restClient.PutAsync<ApiResult<User>>(updateUserRequest, cancellationToken).ConfigureAwait(false);
                // Put Returns 200 Code after success.
                return response.GetResult(HttpStatusCode.OK);
            }
        }
    }
}