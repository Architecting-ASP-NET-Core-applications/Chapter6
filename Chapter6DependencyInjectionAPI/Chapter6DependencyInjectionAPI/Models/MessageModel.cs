namespace Chapter6DependencyInjectionAPI.Models;

public class MessageModel : IMessageModel
{
    public string WriteMessage(string message)
        => $"This is the message: {message}";
}
