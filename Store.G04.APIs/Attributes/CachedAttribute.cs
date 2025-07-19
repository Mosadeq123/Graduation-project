using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.G04.Core.Repositories.Contract;

namespace Store.G04.APIs.Attributes
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTime;

        public CachedAttribute(int expireTime)
        {
            _expireTime = expireTime;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // الحصول على خدمة الكاش من الحاوية
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            // التحقق مما إذا كانت الاستجابة مخزنة مسبقًا
            var cachedResponse = await cacheService.GetCacheKeyAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                context.Result = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                return;
            }

            // متابعة التنفيذ إذا لم يتم العثور على البيانات في الكاش
            var executedContext = await next();

            // إذا كانت النتيجة عبارة عن OkObjectResult، قم بتخزين الاستجابة في الكاش
            if (executedContext.Result is OkObjectResult response)
            {
                await cacheService.SetCacheKeyAsync(cacheKey, response.Value, TimeSpan.FromSeconds(_expireTime));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var cacheKeyBuilder = new System.Text.StringBuilder();
            cacheKeyBuilder.Append(request.Path);

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                cacheKeyBuilder.Append($"|{key}-{value}");
            }

            return cacheKeyBuilder.ToString();
        }
    }
}