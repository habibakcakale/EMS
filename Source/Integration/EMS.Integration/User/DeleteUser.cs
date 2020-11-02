namespace EMS.Integration.User {
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Common.User;
    using MediatR;
    using RestSharp;

    public class DeleteUser {
        public class Request : IRequest {
            public int Id { get; set; }
        }

        public class Query : IRequestHandler<Request> {
            private readonly IRestClient restClient;
            private const string DeleteUserUrlFormat = "users/{0}";

            public Query(IRestClient restClient) {
                this.restClient = restClient;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken) {
                if (request == null) throw new ArgumentNullException(nameof(request));
                var restRequest = new RestRequest(new Uri(string.Format(DeleteUserUrlFormat, request.Id), UriKind.Relative));
                var response = await restClient.DeleteAsync<ApiResult<User>>(restRequest, cancellationToken).ConfigureAwait(false);
                response.GetResult(HttpStatusCode.NoContent);
                return Unit.Value;
            }
        }
    }
}