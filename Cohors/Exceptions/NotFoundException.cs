using System.Net;

namespace Cohors.Exceptions;

/// <summary>
///   Represents an exception related to an entity not being found.
/// </summary>
/// <param name="entityId"></param>
/// <typeparam name="T"></typeparam>
public class NotFoundException<T>(int entityId) : Exception($"{typeof(T)} with id of {entityId} not found!")
{
    public HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound;
}