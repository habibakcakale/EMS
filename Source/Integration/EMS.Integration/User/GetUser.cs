namespace EMS.Integration.User {
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Common.User;
    using MediatR;
    using RestSharp;

    public class GetUser {
        public class Request : IRequest<User> {
            public int Id { get; set; }
        }

        public class Query : IRequestHandler<Request, User> {
            private readonly IRestClient restClient;
            private const string GetUserUrlFormat = "users/{0}";

            public Query(IRestClient restClient) {
                this.restClient = restClient;
            }

            public async Task<User> Handle(Request request, CancellationToken cancellationToken) {
                if (request == null) throw new ArgumentNullException(nameof(request));
                var getUri = new Uri(string.Format(GetUserUrlFormat, request.Id), UriKind.Relative);
                var userResponse = await restClient.GetAsync<ApiResult<User>>(new RestRequest(getUri), cancellationToken).ConfigureAwait(false);
                return userResponse.GetResult(HttpStatusCode.OK);
            }
        }
    }
}