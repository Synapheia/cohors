using System.Net;

namespace Cohors.Exceptions;

public class NotFoundException<T>(int id)
    : HttpException($"{typeof(T)} with id of {id} not found!", HttpStatusCode.NotFound);