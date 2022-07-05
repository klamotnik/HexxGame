using Hexx.DTO.Objects;
using System;

namespace Hexx.DTO
{
    public abstract class Message
    {
        public Objects.Hexx WrapMessage()
        {
            Objects.Hexx hexx = new Objects.Hexx();
            hexx.Type = (ActionType)Enum.Parse(typeof(ActionType), GetType().Name.ToString());
            hexx.Action = this;
            return hexx;
        }
    }
}
