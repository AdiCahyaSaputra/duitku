using DuitKu.DTOs;

namespace DuitKu.Services
{
    public class HelperService
    {
        public BaseResponseFilterDto FilterResponseApi(int pageNumber, int limit, int totalRecord)
        {
            return new BaseResponseFilterDto
            {
                isPreviousExists = pageNumber > 1,
                isNextExists = pageNumber * limit < totalRecord,
            };
        }
    }
}