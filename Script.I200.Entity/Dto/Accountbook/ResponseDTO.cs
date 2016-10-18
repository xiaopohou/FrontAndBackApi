namespace Script.I200.Entity.Dto.Accountbook
{

    /// <summary>
    /// 业务逻辑层返回对象
    /// </summary>
    public class ResponseDto
    {
        public ResponseDto()
        {
        }

        public ResponseDto(bool isSuccess, string message, object data)
        {
            IsSuccess = isSuccess;
            Message = message;
            DataObject = data;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object DataObject { get; set; }
    }
}
