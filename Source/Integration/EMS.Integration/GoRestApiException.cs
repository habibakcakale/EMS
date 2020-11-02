namespace EMS.Integration {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Common;

    [Serializable]
    public class GoRestApiException : Exception, ISerializable {
        public IEnumerable<Error> Errors { get; set; }

        public int Code { get; set; }
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public GoRestApiException() { }
        public GoRestApiException(string message) : base(message) { }
        public GoRestApiException(string message, Exception inner) : base(message, inner) { }

        protected GoRestApiException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {
            this.Errors = info.GetValue(nameof(Errors), typeof(List<Error>)) as List<Error>;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(nameof(Errors), Errors, typeof(IEnumerable<Error>));
            base.GetObjectData(info, context);
        }
        public override string ToString() {
            return string.Join(", ", Errors.Select(item => $"{item.Field}: {item.Message}")) + base.ToString();
        }
    }
}