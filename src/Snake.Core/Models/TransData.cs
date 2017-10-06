using Snake.Core.Enums;

namespace Snake.Core.Models
{
    public class TransData<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public TransData()
        {
            Code = (int)ServiceResultCode.Succeeded;
        }
    }
}