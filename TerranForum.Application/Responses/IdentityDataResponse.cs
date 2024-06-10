using Microsoft.AspNetCore.Identity;

namespace TerranForum.Application.Responses
{
    public class IdentityDataResponse<T>
    {
        public IdentityResult Result { get; set; } = null!;
        public T? Data { get; set; }
    }
}
