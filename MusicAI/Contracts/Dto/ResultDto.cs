using System.Text.Json;

namespace MusicAI.Contracts.Dto
{
    public class ResultDto
    {
        public ResultDto()
        {

        }
        public ResultDto(bool IsSuccess = false, string Message = null)
        {
            this.IsSuccess = IsSuccess;
            this.Message = Message;
        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        //public string StatusCode { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        //public static ResultDto Success(string lang)
        //{
        //    return new ResultDto
        //    {
        //        IsSuccess = true,
        //        Message = Messages.GetOperationSuccessFullMessage("عملیات", "operation", lang),
        //    };
        //}

    }
    public class ResultDto<T> : ResultDto
    {

        public T Data { get; set; }

        public ResultDto()
        {

        }
        public ResultDto(bool IsSuccess, string Message, T Data)
        {
            this.IsSuccess = IsSuccess;
            this.Message = Message;
            this.Data = Data;
        }

        public static ResultDto<T> Success(T data)
        {
            return new ResultDto<T>
            {
                IsSuccess = true,
                Message = null,
                Data = data
            };
        }
        //public static ResultDto<T> SuccessWithMessage(T data, string lang)
        //{
        //    return new ResultDto<T>
        //    {
        //        IsSuccess = true,
        //        Message = Messages.GetOperationSuccessFullMessage("عملیات", "operation", lang),
        //        Data = data
        //    };
        //}
        //public static ResultDto Success(string lang)
        //{
        //    return new ResultDto
        //    {
        //        IsSuccess = true,
        //        Message = null,
        //    };
        //}
        //public static ResultDto SuccessWithMessage(string lang)
        //{
        //    return new ResultDto
        //    {
        //        IsSuccess = true,
        //        Message = Messages.GetOperationSuccessFullMessage("عملیات", "operation", lang),
        //    };
        //}

        //public static ResultDto<S> SuccessWithUserStates<S>(S userStatusResultDto, string Message) where S : UserStatesResultDto
        //{
        //    var result = new ResultDto<S>();
        //    result.IsSuccess = true;
        //    result.Message = Message;
        //    result.Data = userStatusResultDto;
        //    userStatusResultDto.ReadyToSendToUser();
        //    if (userStatusResultDto.GetType().Equals(typeof(LoginResultDto)))
        //    {
        //        LoginResultDto loginResultDto = userStatusResultDto as LoginResultDto;
        //        loginResultDto.refreshToken = null;
        //        loginResultDto.accessToken = null;
        //    }
        //    return result;
        //}
    }

}
