namespace EMS.Integration {
    using System;
    using System.Linq;
    using System.Net;
    using Common;

    public static class ApiResultExtensions {
        /// <summary>
        /// Get Result of inner api result if expected status code otherwise throw an exception with API Errors.
        /// </summary>
        /// <param name="result">Api Result</param>
        /// <param name="expectedStatusCode">Expected Http Status Code</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="GoRestApiException"></exception>
        public static T GetResult<T>(this ApiResult<T> result, HttpStatusCode expectedStatusCode) {
            if (result == null) throw new ArgumentNullException(nameof(result));
            if (result.Code == (int)expectedStatusCode) {
                return result.Data;
            }

            throw new GoRestApiException {Code = result.Code, Errors = result.Errors};
        }
    }
}