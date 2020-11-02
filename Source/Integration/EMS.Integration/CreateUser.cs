namespace EMS.Integration {
    using System;
    using System.Net;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using MediatR;
    using RestSharp;

    public class CreateUser {
        public class Request : IRequest<User> {
            [JsonPropertyName("name")] public string Name { get; set; }
            [JsonPropertyName("email")] public string Email { get; set; }
            [JsonPropertyName("gender")] public Gender Gender { get; set; }
            [JsonPropertyName("status")] public UserStatus Status { get; set; }
        }

        public class Query : IRequestHandler<Request, User> {
            private readonly IRestClient restClient;

            public Query(IRestClient restClient) {
                this.restClient = restClient;
            }

            public async Task<User> Handle(Request request, CancellationToken cancellationToken) {
                if (request == null) throw new ArgumentNullException(nameof(request));
                var userRequest = new RestRequest(new Uri("users", UriKind.Relative));
                userRequest.AddJsonBody(request);

                var response = await restClient.ExecutePostAsync(userRequest, cancellationToken);
                var document = JsonDocument.Parse(response.Content);
                var code = (HttpStatusCode)document.RootElement.GetProperty("code").GetInt32();
                if (code != HttpStatusCode.Created) {
                    throw new InvalidOperationException("Error while creating user.") {
                        //Data = {{"Response", response.Content}}
                    };
                }

                var user = JsonSerializer.Deserialize<User>(document.RootElement.GetProperty("data").GetRawText());
                return user;
            }
        }

    }
}