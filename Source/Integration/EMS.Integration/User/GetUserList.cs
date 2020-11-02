namespace EMS.Integration.User {
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Common.User;
    using MediatR;
    using RestSharp;

    public class GetUserList {
        public class Request : IRequest<PagedResult<User>> {
            public string Name { get; set; }
            public string Email { get; set; }
        }

        public class Query : IRequestHandler<Request, PagedResult<User>> {
            private const string UserList = "users?name={0}&email={1}";
            private readonly IRestClient restClient;

            public Query(IRestClient restClient) {
                this.restClient = restClient;
            }

            public async Task<PagedResult<User>> Handle(Request request, CancellationToken cancellationToken) {
                if (request == null) throw new ArgumentNullException(nameof(request));
                var requestUri = new Uri(string.Format(UserList, request.Name, request.Email), UriKind.Relative);
                var userListResponse = await restClient.GetAsync<ApiResult<IEnumerable<User>>>(new RestRequest(requestUri), cancellationToken).ConfigureAwait(false);
                var data = userListResponse.GetResult(HttpStatusCode.OK);
                return new PagedResult<User> {Pagination = userListResponse.Meta?.Pagination, Items = data};
            }
        }
    }
}