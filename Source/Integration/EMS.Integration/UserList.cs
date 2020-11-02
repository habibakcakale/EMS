namespace EMS.Integration {
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using MediatR;
    using RestSharp;

    public class UserList {
        public class Request : IRequest<ApiResult<User>> {
            public string Name { get; set; }
        }

        public class Query : IRequestHandler<Request, ApiResult<User>> {
            private static readonly string UserList = "users?name={0}";
            private readonly IRestClient restClient;

            public Query(IRestClient restClient) {
                this.restClient = restClient;
            }

            public Task<ApiResult<User>> Handle(Request request, CancellationToken cancellationToken) {
                if (request == null) throw new ArgumentNullException(nameof(request));
                var requestUri = new Uri(string.Format(UserList, request.Name), UriKind.Relative);
                var users = restClient.GetAsync<ApiResult<User>>(new RestRequest(requestUri), cancellationToken);
                return users;
            }
        }
    }
}