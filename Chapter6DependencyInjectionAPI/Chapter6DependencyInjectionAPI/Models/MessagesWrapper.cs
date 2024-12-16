namespace Chapter6DependencyInjectionAPI.Models;

public class MessagesWrapper(IMessageModel messageModel)
{
    public string WriteAll()
        => messageModel.WriteMessage("Parent message");
}
