namespace MinimalApi.Extensions.Shared.Enums
{
    public class StatusCodeOperation : Enumeration
    {
        public static StatusCodeOperation NoContent { get; } = new StatusCodeOperation(204, nameof(NoContent));
        public static StatusCodeOperation BusinessError { get; } = new StatusCodeOperation(422, nameof(BusinessError));
        public static StatusCodeOperation BadRequest { get; } = new StatusCodeOperation(400, nameof(BadRequest));
        public static StatusCodeOperation NotFound { get; } = new StatusCodeOperation(404, nameof(NotFound));
        public static StatusCodeOperation Post { get; } = new StatusCodeOperation(201, nameof(Post));
        public static StatusCodeOperation Get { get; } = new StatusCodeOperation(200, nameof(Get));
        public static StatusCodeOperation Put { get; } = new StatusCodeOperation(201, nameof(Put));
        public static StatusCodeOperation Patch { get; } = new StatusCodeOperation(200, nameof(Patch));
        public static StatusCodeOperation Delete { get; } = new StatusCodeOperation(200, nameof(Delete));

        public StatusCodeOperation(int id, string name) : base(id, name) { }
    }
}
